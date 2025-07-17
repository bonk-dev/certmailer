const templates = {};
(() => {
    const updateTemplateOptions = (selectElement, templates, defaultId) => {
        const optionElements = templates.map(t => `<option value="${t.id}" ${defaultId == t.id ? 'selected' : ''}>${t.name} (id: ${t.id})</option>`);
        selectElement.innerHTML = optionElements.join('\n');
    };
    const updateTemplateList = (containerElement, templates) => {
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
                            <textarea class="form-control" placeholder="Use placeholders like {{FirstName}}" id="${editBoxId}" style="height: 300px">${new Option(t.template).innerHTML}</textarea>
                            <label for="${editBoxId}">Template</label>
                        </div>
                        <button class="btn btn-primary" onclick="saveTemplate(${t.id})">Save</button>
                    </div>
                </li>`.trim();
        });
        containerElement.innerHTML = items.join('');
    };
    const saveTemplate = (id) => {
        console.debug("TODO: save template");
    };
    const fetchTemplates = async () => {
        const r = await fetch(`${API_BASE}/api/notifications/template/all`);
        return await r.json();
    };

    templates.updateTemplateOptions = updateTemplateOptions;
    templates.updateTemplateList = updateTemplateList;
    templates.saveTemplate = saveTemplate;
    templates.fetchTemplates = fetchTemplates;
})();