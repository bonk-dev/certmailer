class Job {
    id = ""
    status = {
        participantsParsed: 0,
        certificatesGenerated: 0,
        mailsSent: 0
    }
    errors = []
    rowElement = null

    constructor(id, status = null, errors = null) {
        this.id = id;
        if (status) {
            this.status = status;
        }
        if (errors) {
            this.errors = errors;
        }

        this._fetchJobStatus = async() => {
            const r = await fetch(`${API_BASE}/api/parser/status/${this.id}`);
            return await r.json();
        };
    }

    async update() {
        console.debug(`updating: ${this.id}`);

        const j = await this._fetchJobStatus();
        this.status = {...j['status']};
        this.errors = j['errors'];
        this.setHtml();
    }

    getIsDone() {
        // TODO: Add 'isDone' to API
        return this.status.participantsParsed <= this.status.mailsSent;
    }

    setHtml() {
        if (this._html == null) {
            const thElement = `
            <th scope="row">
                <span class="text-break-all">${this.id}</span>
                <button class="btn btn-outline-danger btn-sm d-none" data-bs-toggle="collapse" 
                        data-bs-target="#${this.id}-errors" aria-expanded="false" 
                        aria-controls="${this.id}-errors">
                    Show errors
                </button>
                <div id="${this.id}-errors" class="collapse">
                    <ul>
                        ${this.errors.map(e => `<li>${e}</li>`).join('')}
                    </ul>
                </div>
            </th>`;

            this.rowElement.innerHTML = `
                ${thElement}
                <td class="parsed">${this.status.participantsParsed}</td>
                <td class="certs">${this.status.certificatesGenerated}</td>
                <td class="sent">${this.status.mailsSent}</td>
            `;

            this._html = {
                showErrorButton: this.rowElement.querySelector('th > button'),
                errorUl: this.rowElement.querySelector('th > .collapse ul'),
                idSpan: this.rowElement.querySelector('th > span'),
                parsedTd: this.rowElement.querySelector('td.parsed'),
                certsTd: this.rowElement.querySelector('td.certs'),
                emailsTd: this.rowElement.querySelector('td.sent')
            };
        }

        this._html.emailsTd.innerText = this.status.mailsSent;
        this._html.certsTd.innerText = this.status.certificatesGenerated;
        this._html.parsedTd.innerText = this.status.participantsParsed;

        const errorsSet = this.rowElement.classList.contains('error');
        if (this.errors.length > 0 && !errorsSet) {
            const errorClassName = ['text-danger', 'text-decoration-line-through'];
            this._html.showErrorButton.classList.remove('d-none');
            this._html.idSpan.classList.add(...errorClassName);
            this._html.parsedTd.classList.add(...errorClassName);
            this._html.certsTd.classList.add(...errorClassName);
            this._html.emailsTd.classList.add(...errorClassName);

            this.rowElement.classList.add('error');

            this._html.errorUl.innerHTML = this.errors.map(e => `<li>${e}</li>`).join('');
        }
    };
}

class JobTable {
    constructor(tableElement) {
        this._jobs = [];
        this._table = tableElement.querySelector('tbody');
        this._autoInterval = null;
        
        this._addToHtml = (job) => {
            const row = document.createElement('tr');
            job.rowElement = row;
            job.setHtml();
            this._table.appendChild(row);
        };
        this._fetchAllJobs = async () => {
            const r = await fetch(`${API_BASE}/api/parser/status/all`);
            return await r.json();
        };
        this._updateLoop = async () => {
            const dtos = await this._fetchAllJobs();
            for (const d of dtos) {
                const localJob = this._jobs.find(j => j.id === d.batchId);
                if (localJob != null) {
                    localJob.status = d['status'];
                    localJob.errors = d['errors'];
                    localJob.setHtml();
                }
                else {
                    const j = new Job(d['batchId'], d['status'], d['errors']);
                    this.addJob(j);
                }
            }

            this._scheludeUpdate();
        };
        this._scheludeUpdate = () => {
            this._autoInterval = setTimeout(() => this._updateLoop(), 1000);
        };
    }

    startUpdating() {
        this._scheludeUpdate();
    }

    addJob(job) {
        this._jobs.push(job);
        this._addToHtml(job);
    }

    async addJobById(id) {
        const job = new Job(id);
        this.addJob(job);
        await job.update();
    }

    async fillTable() {
        const jobsDtos = await this._fetchAllJobs();
        for (const dto of jobsDtos) {
            const j = new Job(dto['batchId'], dto['status'], dto['errors']);
            
            this.addJob(j);
        }
    }
};