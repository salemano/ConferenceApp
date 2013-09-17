$(function () {
    $('.accept-session').click(function () {
        // set acepted status
        // refresh the page
        $.ajax({
            url: this.href,
            cache: false,
            success: function (data) {
                if (data == '')
                    window.location.reload();
            }
        });

        return false;
    });

    $('.reject-session').click(function () {
        // display dialog to record reason 

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

    $('.delete-session').click(function (event) {

        // close dropdown 
        $('.btn-group').removeClass('open');

        var target = event.target;
        var usersCount = $(target).attr('users');
        var message = "Are you sure you would like to Delete this session? ";
        if (usersCount > 0) {
            message += "</br>Session has " + usersCount + " registered users.";
        }

        var proceedToDelete = confirm(message);
        if (!proceedToDelete) {
                return false;
        }
    });
});
