
$(function () {

    // Reset button in filter fieldsets
    $('form .form-reset').each(function () {
        $(this).click(function () {
            var form = $(this).parents('form');

            $('input[type="text"]', form).each(function () {
                $(this).val('');
            });

            $('input[type="checkbox"]', form).each(function () {
                $(this).removeAttr('checked');
            });

            $('select', form).each(function () {
                $(this).children('option').each(function () {
                    $(this).removeAttr('selected');
                });

                $('option:first-child').attr('selected', 'selected');
            });

            form.submit();
        });
    });

    $('[autofocus]:first').focus();

    $("#CreateStart.datepicker").datepicker({
        minDate: 0,
        dateFormat: "dd-mm-yy",
        onSelect: function (selected) {
            $("#CreateEnd").datepicker("option", "minDate", selected);
            $("#CreateRegistrationClosedAt").datepicker("option", "maxDate", selected);
        }
    });

    $("#CreateRegistrationClosedAt.datepicker").datepicker({ dateFormat: "dd-mm-yy" });

    $(".datepicker").datepicker({ dateFormat: "dd-mm-yy", showButtonPanel: true });

    if ($.validator != null)
        $.validator.addMethod('date',
        function (value, element) {
            if (this.optional(element)) {
                return true;
            }

            var ok = true;
            try {
                $.datepicker.parseDate('dd-mm-yy', value);
            }
            catch (err) {
                ok = false;
            }
            return ok;
        });
});

$(function () {
    $(".JQueryButton")
      .button()
});


function showFancybox(data, afterClose) {

    $.fancybox({
        'content': $(data),
        minWidth: '400px',
        minHeight: '400px',
        afterClose: afterClose,
        helpers: {
            overlay: {
                speedIn: 0,
                speedOut: 300,
                opacity: 0.8,
                css: {
                    cursor: 'default'
                },
                closeClick: false
            }
        },
    });

    return true;
}

function hideFancybox() {
    parent.$.fancybox.close();
}




