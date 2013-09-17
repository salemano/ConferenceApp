$(function () {

    $('.register-in-session').click(function () {

        $.ajax({    
            url: this.href,
            cache: false,
            success: function (data) {
                if (data == '')
                    hideFancybox();
                else
                    showFancybox(data);
            }
        });

        return false;
    });

    $('.viewSession').click(function () {

        $.ajax({
            url: this.href,
            cache: false,
            success: function (data) {
                if (data == '')
                    hideFancybox();
                else
                    showFancybox(data);
            }
        });

        return false;
    });
});

