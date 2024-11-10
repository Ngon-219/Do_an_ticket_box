function Validator(formSelector) {
    var _this = this;
    var formRules = {};
    function getParent(element, selector) {
        while (element.parentElement) {
            if (element.parentElement.matches(selector)) {
                return element.parentElement;
            }
            element = element.parentElement;
        }
    }



    var validatorRules = {
        required: function (value) {
            return value ? undefined : "Vui lòng nhập trường này!";
        },
        email: function (value) {
            var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            return regex.test(value) ? undefined : 'Vui lòng nhập đầy đủ email!';
        },
        min: function (min) {
            return function (value) {
                return value.length >= min ? undefined : `Vui lòng nhập ít nhất ${min} ký tự!`;
            }
        }
    }



    var formElement = document.querySelector(formSelector);


    if (formElement) {
        var inputs = formElement.querySelectorAll('[name][rules]');
        for (var input of inputs) {
            var rules = input.getAttribute('rules').split('|');
            for (var rule of rules) {
                var ruleInfor;
                var isRuleHasValue = rule.includes(':');
                if (isRuleHasValue) {
                    ruleInfor = rule.split(':');
                    rule = ruleInfor[0];
                }
                var ruleFunc = validatorRules[rule];
                if (isRuleHasValue) {
                    ruleFunc = ruleFunc(ruleInfor[1]);
                }
                if (Array.isArray(formRules[input.name])) {
                    formRules[input.name].push(ruleFunc);
                }
                else {
                    formRules[input.name] = [ruleFunc];
                }
            }

            input.onblur = handleValidate;
            input.oninput = handleClearError;
            function handleValidate(event) {
                var rules = formRules[event.target.name];
                var errorMessage;
                // for (var rule of rules) {
                //     errorMessage =  rule(event.target.value);
                //     if (errorMessage) break;
                // }
                rules.some(function (rule) {
                    errorMessage = rule(event.target.value);
                    return errorMessage;
                });
                if (errorMessage) {
                    var formGroup = getParent(event.target, '.auth-form__group');

                    if (formGroup) {
                        formGroup.classList.add('invalid');
                        var formMessage = formGroup.querySelector('.form-message');
                        if (formMessage) {
                            formMessage.textContent = errorMessage;

                        }
                    }
                }
                return errorMessage;
            }
            function handleClearError(event) {
                var formGroup = getParent(event.target, '.auth-form__group');
                if (formGroup.classList.contains('invalid')) {
                    formGroup.classList.remove('invalid');
                    var formMessage = formGroup.querySelector('.form-message');
                    if (formMessage) {
                        formMessage.textContent = '';
                    }
                }
            }
        }
        //Xử lý hành vi submit form
        formElement.onsubmit = (event) => {
            event.preventDefault(event);
            var inputs = formElement.querySelectorAll('[name][rules]');
            var isFormValid = true;
            for (var input of inputs) {
                if (handleValidate({ target: input })) {
                    isFormValid = false;
                }
            }
            //Khi khong co loi
            if (isFormValid) {
                if (typeof _this.onSubmit === 'function') {
                    var enableInput = formElement.querySelectorAll('[name]');
                    var formValues = Array.from(enableInput).reduce(function (values, input) {

                        switch (input.type) {
                            case 'checkbox':
                                if (!input.matches(':checked')) {
                                    values[input.name] = '';
                                    return values;
                                }
                                if (!Array.isArray(values[input.name])) {
                                    values[input.name] = [];
                                }
                                values[input.name].push(input.value);
                                break;
                            case 'radio':
                                values[input.name] = formElement.querySelector('input[name="' + input.name + '"]:checked').value;
                                break
                            case 'file':
                                values[input.name] = input.files;
                                break;
                            default:
                                values[input.name] = input.value;
                                break;
                        }
                        return values;
                    }, {})
                    _this.onSubmit(formValues);
                } else {
                    formElement.submit();
                }
            }
        }
    }
}