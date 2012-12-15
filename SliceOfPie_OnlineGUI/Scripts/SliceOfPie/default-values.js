$.extend({
    default_values: function () {
        jQuery(function ($) {
            $('textarea, input').each(function () {
                var defaultValue = $(this).attr('data-default-value');
                $(this).change(function (e) {
                    if ($(this).val() != "" && $(this).val() != defaultValue) $(this).removeClass('default-value');
                    else {
                        $(this).addClass('default-value');
                        $(this).val(defaultValue);
                    }
                });
                $(this).change();
            });

            $(".default-value").formDefaults({
                activeColor: '#000000',
                inactiveColor: '#808080'
            });
        });
    }
});