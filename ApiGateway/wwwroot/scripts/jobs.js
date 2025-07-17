class Job {
    id = ""
    status = {
        participantsParsed: 0,
        certificatesGenerated: 0,
        mailsSent: 0
    }
    rowElement = null

    constructor(id) {
        this.id = id;

        this._fetchJobStatus = async() => {
            const r = await fetch(`${API_BASE}/api/parser/status/${this.id}`);
            return await r.json();
        };
    }

    async update() {
        console.debug(`updating: ${this.id}`);

        const j = await this._fetchJobStatus();
        this.status = {...j['status']};
        this.setHtml();

        // TODO: Add 'isDone' to API
        if (this.status.participantsParsed <= this.status.mailsSent) {
            this._autoInterval = null;
            console.info(`Job ${this.id} done`);
        }
        else {
            this._autoInterval = setTimeout(() => this.update(), 1000);
        }
    }

    setHtml() {
        this.rowElement.innerHTML = `
        <th scope="row">${this.id}</th>
        <td>${this.status.participantsParsed}</td>
        <td>${this.status.certificatesGenerated}</td>
        <td>${this.status.mailsSent}</td>
        `;
    };
}

class JobTable {
    constructor(tableElement) {
        this._jobs = [];
        this._table = tableElement.querySelector('tbody');
        
        this._addToHtml = (job) => {
            const row = document.createElement('tr');
            job.rowElement = row;
            job.setHtml();
            this._table.appendChild(row);
        };
    }

    async addJob(id) {
        const job = new Job(id);
    
        this._jobs.push(job);
        this._addToHtml(job);
        await job.update();
    }
};