class Job {
    id = ""
    status = {
        participantsParsed: 0,
        certificatesGenerated: 0,
        mailsSent: 0
    }
    errors = []
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
        this.errors = j['errors'];
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
        let childrenClass = "";
        if (this.errors && this.errors.length > 0) {
            childrenClass += "text-danger text-decoration-line-through";
        }

        this.rowElement.innerHTML = `
        <th scope="row" class="${childrenClass}">${this.id}</th>
        <td class="${childrenClass}">${this.status.participantsParsed}</td>
        <td class="${childrenClass}">${this.status.certificatesGenerated}</td>
        <td class="${childrenClass}">${this.status.mailsSent}</td>
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