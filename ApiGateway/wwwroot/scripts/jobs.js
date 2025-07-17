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
        let errorClass = "";
        if (this.errors && this.errors.length > 0) {
            errorClass += "text-danger text-decoration-line-through";
        }

        let thElement;
        if (this.errors.length > 0) {
            thElement = `
            <th scope="row">
                <span class="${errorClass} text-break-all">${this.id}</span>
                <button class="btn btn-outline-danger btn-sm" data-bs-toggle="collapse" 
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
        }
        else if (!this.getIsDone()) {
            // TODO: add spinner maybe
            thElement = `
            <th scope="row" class="${errorClass} text-break-all">
                <span>${this.id} (in progress)</span>
            </th>`;
        }
        else {
            thElement = `
            <th scope="row" class="${errorClass} text-break-all">
                <span>${this.id}</span>
            </th>`;
        }

        this.rowElement.innerHTML = `
        ${thElement}
        <td class="${errorClass}">${this.status.participantsParsed}</td>
        <td class="${errorClass}">${this.status.certificatesGenerated}</td>
        <td class="${errorClass}">${this.status.mailsSent}</td>
        `;
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