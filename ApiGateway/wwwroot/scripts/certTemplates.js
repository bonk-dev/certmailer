const certTemplates = {};
(() => {
    let templateList;

    const updateTemplateOptions = (selectElement, templates, defaultId) => {
        const optionElements = templates.map(t => `<option value="${t.id}" ${defaultId == t.id ? 'selected' : ''}>${t.name} (id: ${t.id})</option>`);
        selectElement.innerHTML = optionElements.join('\n');
    };
    const updateTemplateList = (containerElement, templates) => {
        templateList = containerElement;
        const items = templates.map(t => {
            const editBoxContainerId = `certTemplateId${t.id}`;
            const editBoxId = `${editBoxContainerId}Text`;
            const editBoxFormId = `${editBoxContainerId}Form`;

            let backgroundAnchor = `<a href='#'>Not set</a>`;
            if (t.backgroundUri) {
                backgroundAnchor = `<a href='${API_BASE}/api/certificates/templates/${t.id}/background'>Download current background</a>`;
            }

            return `
                <li class="list-group-item">
                    <button class="btn w-100 text-start" data-bs-toggle="collapse" data-bs-target="#${editBoxContainerId}" href="#${editBoxContainerId}" role="button" aria-expanded="false">
                        ${t.name} (id: ${t.id})
                    </button>

                    <div id="${editBoxContainerId}" class="collapse">

                        <form id="${editBoxFormId}">
                            <div class="mb-3">
                                <label class="form-label" for="name">Name</label>
                                <input class="form-control" name="name" value="${t.name}"
                                    onchange="certTemplates.set(${t.id}, 'name', event.target.value)">
                            </div>
                            <div class="mb-3">
                                <label class="form-label" for="title">Title</label>
                                <input class="form-control" name="title" value="${t.title}"
                                    onchange="certTemplates.set(${t.id}, 'title', event.target.value)">
                            </div>
                            <div class="mb-3">
                                <label class="form-label" for="subtitle">Subtitle</label>
                                <input class="form-control" name="subtitle" value="${t.subtitle}"
                                    onchange="certTemplates.set(${t.id}, 'subtitle', event.target.value)">
                            </div>
                            <div class="mb-3">
                                <label class="form-label" for="description">Description</label>
                                <input class="form-control" name="description" value="${t.description}"
                                    onchange="certTemplates.set(${t.id}, 'description', event.target.value)">
                            </div>
                            <div class="mb-3">
                                <label for="backgroundFile" class="form-label">Background file</label>
                                <input class="form-control" type="file" name="backgroundFile">
                                ${backgroundAnchor}
                            </div>

                            <button class="btn btn-primary" type="button" onclick="certTemplates.saveTemplate(${t.id})">Save</button>
                        </form>
                    </div>
                </li>`.trim();
        });
        containerElement.innerHTML = items.join('');
    };
    const set = (id, prop, value) => {
        const t = getData().certTemplates.find(t => t.id == id);
        t[prop] = value;
    };
    const saveTemplate = async (id) => {
        const form = document.getElementById(`certTemplateId${id}Form`);
        const data = new FormData(form);
        const r = await fetch(`${API_BASE}/api/certificates/templates/${id}`, {
            method: "PUT",
            body: data
        });

        if (r.status !== 200) {
            errString = validationErrorsToString(await r.json());
            alert(`Could not save template: \n${errString}`);
        }

        updateTemplateList(templateList, await fetchTemplates());
    };
    const fetchTemplates = async () => {
        const r = await fetch(`${API_BASE}/api/certificates/templates/all`);
        return await r.json();
    };

    certTemplates.updateTemplateOptions = updateTemplateOptions;
    certTemplates.updateTemplateList = updateTemplateList;
    certTemplates.set = set;
    certTemplates.saveTemplate = saveTemplate;
    certTemplates.fetchTemplates = fetchTemplates;
})();