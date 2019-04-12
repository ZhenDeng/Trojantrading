(function ($) {
    function _hideValidator(thisObj, defTimeOut) {
        defTimeOut = (defTimeOut || 1000);
        setTimeout(function () {
            var obj = $('#' + thisObj.attr('data-validator'));
            //console.log(obj);
            var removeContainer = function () {
                obj.remove();
                thisObj.removeAttr('data-validator');
                thisObj.removeAttr('data-type');
                thisObj.removeAttr('data-direction');
                thisObj.removeAttr('data-distance');
                if (typeof validatorInterval !== 'undefined')
                    clearInterval(validatorInterval);
            };
            var type = thisObj.attr('data-type');
            var direction = thisObj.attr('data-direction');
            var distance = thisObj.attr('data-distance');
            if (type === 'confirm' && (direction === 'left' || direction === 'right'))
                obj.fadeOut(200);
            else
                obj.hide({ effect: 'drop', direction: direction, duration: 500, easing: 'easeInBack', complete: removeContainer, distance: distance });
            $('.modal-background').remove();
        }, defTimeOut);
    }

    $.fn.showValidator = function (message, type, position, isStayOut, isStayAlive, finishTime, confirmCallback, confirmTheme, confirmOptions) {
        if (typeof this !== 'undefined') {
            finishTime = finishTime || 3000;
            var thisObj = $(this);
            type = $.trim((type || 'error').toLowerCase());
            if (type !== 'error' && type !== 'success' && type !== 'warning' && type !== 'confirm')
                type = 'error';
            position = $.trim((position || 'right').toLowerCase());
            if (position !== 'top' && position !== 'right' && position !== 'bottom' && position !== 'left')
                position = 'right';
            isStayOut = (isStayOut || false);
            isStayAlive = (isStayAlive || false);
            if (thisObj.prop('tagName').toLowerCase() !== 'input') {
                isStayAlive = false;
            }
            var direction = 'right';
            var containerName = 'validatorContainer-' + $(this).attr('id');
            var container = $('#' + containerName);
            var htmlConfirm = '';
            if (type === 'confirm') {
                confirmCallback = (confirmCallback || '');
                var callbackFunction = containerName.replace('-', '_') + '_callback';
                window[callbackFunction] = confirmCallback;

                confirmOptions = (confirmOptions || ['Yes', 'No']);
                htmlConfirm = '<br /><div style="text-align:middle; padding-top: 10px;">';
                for (i = 0; i < confirmOptions.length; i++) {
                    htmlConfirm += '<div class="button button-light button-tiny validator-confirm" data-callback="' + callbackFunction + '" data-origin="' + thisObj.attr('id') + '">' + confirmOptions[i] + '</div>&nbsp;&nbsp;';
                }
                htmlConfirm = htmlConfirm.slice(0, -12) + '</div>';
                confirmTheme = (confirmTheme || 'dark');
                if (confirmTheme !== 'dark' && confirmTheme !== 'light')
                    confirmTheme = 'dark';
                modalBackground = $('<div class="modal-background ' + confirmTheme + '"></div>');
                isStayAlive = true;
                $('body').append(modalBackground);
            }
            container = $('<div id="' + containerName + '" class="validator-container"><div class="validator-new">' + message + htmlConfirm + '</div></div');
            //if ($('#' + containerName).length > 0)
            //    $('.validator-container').remove();
            $('body').append(container);
            thisObj.attr('data-validator', containerName);
            var validator = $('#' + containerName + ' > .validator-new');
            switch (type) {
                case 'error':
                    validator.addClass('validator-red');
                    break;
                case 'success':
                    validator.addClass('validator-green');
                    break;
                case 'warning':
                    validator.addClass('validator-orange');
                    break;
                case 'confirm':
                    validator.addClass('validator-red');
                    break;
            }
            var distance = '';
            //$(element).stop(true, true).not(':visible').css('display', 'block').css('opacity', 0).css('left', parseFloat($(element).css('left')) - 10 + 'px').animate({ left: '+=10', opacity: 1 }, 500, 'easeOutExpo', function () {
            //    $(element).delay(2000).fadeOut(200);
            var easing = 'easeOutBounce';
            switch (position) {
                case 'top':
                    direction = 'up';
                    validator.outerWidth($(this).outerWidth());
                    container.css('left', $(this).offset().left);
                    if (isStayOut)
                        container.css('top', $(this).offset().top - container.outerHeight() - 8);
                    else
                        container.css('top', $(this).offset().top - container.outerHeight() + 1);
                    easing = 'easeOutElastic';
                    break;
                case 'bottom':
                    direction = 'down';
                    validator.outerWidth($(this).outerWidth());
                    container.css('left', $(this).offset().left);
                    if (isStayOut)
                        container.css('top', $(this).offset().top + $(this).outerHeight() + 8);
                    else
                        container.css('top', $(this).offset().top + $(this).outerHeight() - 1);
                    distance = '10px';
                    easing = 'easeOutElastic';
                    break;
                case 'left':
                    direction = 'left';
                    validator.outerWidth(container.outerWidth() + 16);
                    if (isStayOut)
                        container.css('left', $(this).offset().left - container.outerWidth() - 8);
                    else
                        container.css('left', $(this).offset().left - container.outerWidth() + 1);
                    //container.css('top', $(this).offset().top);
                    container.css('top', $(this).offset().top + (($(this).outerHeight() - container.outerHeight()) / 2));
                    break;
                default:
                    validator.outerWidth(container.outerWidth() + 16);
                    if (thisObj.attr('type') === 'file')
                        container.css('left', $(this).offset().left + 190);
                    else if (isStayOut)
                        container.css('left', $(this).offset().left + $(this).outerWidth() + 8);
                    else
                        container.css('left', $(this).offset().left + $(this).outerWidth() - 1);
                    container.css('top', $(this).offset().top + (($(this).outerHeight() - container.outerHeight()) / 2));
                    distance = '50px';
            }
            thisObj.attr('data-type', type);
            thisObj.attr('data-direction', direction);
            thisObj.attr('data-distance', distance);
            var removeContainer = function () {
                clearInterval(validatorInterval);
                thisObj.removeAttr('data-validator');
                thisObj.removeAttr('data-type');
                thisObj.removeAttr('data-direction');
                thisObj.removeAttr('data-distance');
                container.remove();
            };
            var timer = finishTime + $('.validator-container:visible').length * 200;

            validator.addClass('validator-' + position);
            if (type === 'confirm' && (direction === 'left' || direction === 'right'))
                if (direction === 'left')
                    container.css('display', 'block').css('left', parseFloat(container.css('left')) - 10 + 'px').animate({ left: '+=10', opacity: 1 }, 500, 'easeOutExpo');
                else
                    container.css('display', 'block').css('left', parseFloat(container.css('left')) + 10 + 'px').animate({ left: '-=10', opacity: 1 }, 500, 'easeOutExpo');
            else
                container.show({ effect: 'drop', direction: direction, duration: 1000, easing: easing, distance: distance });

            var validatorInterval = setInterval(function () {
                // Remove validators, if trigger object is missingc:\ddprojects\ddmainwebsite\dev\dickerdata.domain\common\
                $('.validator-container').each(function () {
                    var target = $(this).attr('id');
                    if (!$('[data-validator="' + target + '"]').is(':visible')) {
                        clearInterval(validatorInterval);
                        $(this).remove();
                    }
                });
            }, 100);
            if (!isStayAlive) {
                setTimeout(function () {
                    if (container.is(':visible'))
                        if (type === 'confirm' && (direction === 'left' || direction === 'right'))
                            container.fadeOut(200);
                        else
                            container.hide({ effect: 'drop', direction: direction, duration: 500, easing: 'easeInBack', complete: removeContainer, distance: distance });
                }, timer);
            }
        }
    };

    $.fn.hideValidator = function (defTimeOut) {
        var thisObj = $(this);
        _hideValidator(thisObj, defTimeOut);
    };

    $(function () {
        $(document).on('click', '.validator-confirm', function () {
            var callback = $(this).attr('data-callback');
            _hideValidator($('#' + $(this).attr('data-origin')), 0);
            window[callback]($(this).text());
        });
        $(document).on('click', 'input[data-validator]', function () {
            _hideValidator($(this));
        });

        $(document).on('keypress', 'input[data-validator]', function () {
            _hideValidator($(this));
        });
    });
})($);