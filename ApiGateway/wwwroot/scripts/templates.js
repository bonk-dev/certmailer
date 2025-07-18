const templates = {};
(() => {
    let templateList;

    const updateTemplateOptions = (selectElement, templates, defaultId) => {
        const optionElements = templates.map(t => `<option value="${t.id}" ${defaultId == t.id ? 'selected' : ''}>${t.name} (id: ${t.id})</option>`);
        selectElement.innerHTML = optionElements.join('\n');
    };
    const updateTemplateList = (containerElement, templates) => {
        templateList = containerElement;
        const items = templates.map(t => {
            const editBoxContainerId = `templateId${t.id}`;
            const editBoxId = `${editBoxContainerId}Text`;
            return `
                <li class="list-group-item">
                    <button class="btn w-100 text-start" data-bs-toggle="collapse" data-bs-target="#${editBoxContainerId}" href="#${editBoxContainerId}" role="button" aria-expanded="false">
                        ${t.name} (id: ${t.id})
                    </button>
                    <div id="${editBoxContainerId}" class="collapse">
                        <div class="form-floating">
                            <textarea class="form-control" placeholder="Use placeholders like {{FirstName}}" 
                                id="${editBoxId}" style="height: 300px" 
                                onchange="templates.set(${t.id}, event.target.value);">${new Option(t.template).innerHTML}</textarea>
                            <label for="${editBoxId}">Template</label>
                        </div>
                        <button class="btn btn-primary" onclick="templates.saveTemplate(${t.id})">Save</button>
                    </div>
                </li>`.trim();
        });
        containerElement.innerHTML = items.join('');
    };
    const set = (id, templateText) => {
        const t = getData().templates.find(t => t.id == id);
        t.template = templateText;
    };
    const saveTemplate = async (id) => {
        const t = getData().templates.find(t => t.id == id);
        const r = await fetch(`${API_BASE}/api/notifications/template/${id}`, {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(t)
        });

        if (r.status !== 200) {
            errString = validationErrorsToString(await r.json());
            alert(`Could not save template: \n${errString}`);
        }

        updateTemplateList(templateList, await fetchTemplates());
    };
    const fetchTemplates = async () => {
        const r = await fetch(`${API_BASE}/api/notifications/template/all`);
        return await r.json();
    };

    templates.updateTemplateOptions = updateTemplateOptions;
    templates.updateTemplateList = updateTemplateList;
    templates.set = set;
    templates.saveTemplate = saveTemplate;
    templates.fetchTemplates = fetchTemplates;
})();