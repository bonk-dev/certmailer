const validationErrorsToString = (apiResponse) => {
    errors = apiResponse['errors'];
    let errString = "";
    if (errors) {
        for (const eKey of Object.keys(errors)) {
            errString += eKey + ':';
            errString += errors[eKey].join(', ') + '\n';
        }
    }

    return errString;
};