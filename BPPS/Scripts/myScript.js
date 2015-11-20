$(function () {

    $('.answerResult').each(function () {
        if ($(this).text()==1) {
            $(this).css({ "color": "red" });
        }
        else if ($(this).text() == 2) {
            $(this).css({ "color": "orange" });
        }
        else {
            $(this).css({ "color": "green" });
        }
    });

    $('[data-submenu]').submenupicker();

});