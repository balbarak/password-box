﻿$(function () {

    setupConfirm();
    setupResetForms();
    initICheck();
})


function setMetronicActiveLink() {

    var currentPath = location.pathname.toLocaleLowerCase();

    var selectedTag = '<span class="selected"></span>';

    $('.nav-item a').each(function (i, n) {

        var href = $(n).attr("href");

        if (href) {
            href = href.toLowerCase();

            if (href.endsWith(currentPath)) {

                var anchor = $(n);

                var navToggle = anchor.parents('.nav-item');


                navToggle.addClass('active');
                navToggle.addClass('open');
                navToggle.find("a").append(selectedTag);

                $(n).closest("li").addClass('active');
                $(n).append(selectedTag);
            }
        }
    });
}


//========= Start Ajax Setting =========

function block(element) {

    var html = '<div class="loading-message loading-message-boxed"><img src="/images/loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp; جاري التحميل ... </span></div>';
    var options = {};


    var el = $(element);

    if (el.height() <= ($(window).height())) {
        options.cenrerY = true;
    }

    el.block({
        message: html,
        baseZ: 1000,
        centerY: options.cenrerY !== undefined ? options.cenrerY : false,
        css: {
            top: '10%',
            border: '0',
            padding: '0',
            backgroundColor: 'none'
        },
        overlayCSS: {
            backgroundColor: '#555',
            opacity: 0.05,
            cursor: 'wait'
        }
    });
}

function unblock(element) {

    $(element).unblock();
}

function onAjaxBegin(blockDiv) {

    block(blockDiv);
}

function onAjaxFailed(xhr, status, error, alertDiv, formId) {

    var data = xhr.responseJSON;

    if (!data) {
        data = xhr;
    }

    //$(formId).data("form-status", 'Edit');
}

function onAjaxSuccess(xhr, status, modalToHide) {
    if (modalToHide) {
        $(modalToHide).modal('hide');
    }
}

function onAjaxComplete(xhr, status, blockDiv, alertDiv, divToReplace, formId) {

    var data = xhr.responseJSON;

    if (!data) {
        data = xhr;
    }

    if (data) {

        if (data.isRedirect)
            window.location.href = data.redirectUrl;

        if (data.success) {
            $(divToReplace).html(data.partialViewHtml);
            
        }

        showAlert(data.alert, alertDiv);

        //setFormStatus(formId);
    }

    if (formId)
        refreshUnobtrusiveValidation(formId);

    initICheck();
    unblock(blockDiv);
}

//========= End Ajax Setting =========


//========= BEGIN FORM STUFF =========
function clearFromData(formId) {

    $(':input', formId)
        .not(':button, :submit,:checkbox, :radio, :reset, :hidden')
        .removeAttr('checked')
        .removeAttr('selected')
        .val('')

    $(':input', formId).each(function (index) {

        var hasFileUpload = $(this).data("fileinput");

        if (hasFileUpload)
            $(this).fileinput('clear');
    });

    clearICheck();
}

function clearFormValidation(formId) {

    var form = $(formId);

    //Clear validation summary
    form.find("[data-valmsg-summary=true]")
        .removeClass("validation-summary-errors")
        .addClass("validation-summary-valid")
        .find("ul").empty();

    //reset unobtrusive field level, if it exists
    form.find("[data-valmsg-replace]")
        .removeClass("field-validation-error")
        .addClass("field-validation-valid")
        .empty();

    form.find("[data-val=true]")
        .removeClass("text-danger")

    form.find(".form-group")
        .removeClass("has-error")
        .removeClass("has-success");

    form.find('[data-alert]').empty();
}

function setupResetForms() {

    var selector = '[data-reset-form*="true"]';

    $(selector).on('show.bs.modal', function (event) {

        var modal = $(event.target);
        var form = modal.find('form');

        if (form) {
            clearFormValidation(form);
        }
    });

    $(selector).on('shown.bs.modal', function (event) {

        var modal = $(event.target);
        var form = modal.find('form');

        if (form) {
            clearFromData(form);
        }
    });
}
//========= END FORM STUFF =========

function setupConfirm() {

    $('#confirm-modal').on('show.bs.modal', function (event) {

        var button = $(event.relatedTarget);

        var action = button.data('action');

        $("#confirm-form").attr("action", action);
    });

    $('#confirm-ajax-modal').on('show.bs.modal', function (event) {

        var button = $(event.relatedTarget);

        var action = button.data('action');

        $("#confirm-ajax-form").attr("action", action);
    });
}


function updateContainer(url, divToUpdate) {

    var result = $.Deferred();


    var foundPage = $("[data-page]", divToUpdate);

    if (foundPage) {

        var page = foundPage.attr("data-page");

        if (url.includes('?'))
            url += "&PageNumber=" + page;

        else if (typeof (page) != 'undefined')
            url += "?PageNumber=" + page;
    }


    $.ajax(url,
        {
            method: "GET",
            beforeSend: function () {
                block(divToUpdate);
            },
            complete: function (xhr, status) {
                unblock(divToUpdate);
            },
            success: function (data, status, xhr) {

                if (typeof data === 'object') {
                    $(divToUpdate).html(data.partialViewHtml);
                } else {
                    $(divToUpdate).html(data);
                }

                result.resolve(data);
            },
            error: function (xhr, status, error) {
                result.reject;
            }
        });

    return result.promise();
}

function showModal(modal) {
    $(modal).modal('show');
}

function hideModal(modal) {
    $(modal).modal('hide');
}

function refreshUnobtrusiveValidation(formSelector) {
    var form = $(formSelector);
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);

    $(formSelector).validateBootstrap(true);
}
