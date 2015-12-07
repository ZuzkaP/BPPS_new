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

    $('.rq').click(function () {
 
        $('.rq').parent().css({ "color": "black" });

        if ($(this).val() == "1") {
            $(this).parent().css({ "color": "red" });
        }
        else if ($(this).val() == "2") {
            $(this).parent().css({ "color": "orange" });
        }
        else {
            $(this).parent().css({ "color": "green" });
        }

        $('.rq').parent().css({ "font-weight": "normal" });
        $(this).parent().css({"font-weight":"bold"});
    });
    

    $('.radio-inline > input').change(function () {
        var val = $(this).val();
        var comment = $(this).closest('.form-group').find('textarea.form-control');
        if (val < 3) {
            comment.attr('required', 'required');
        }
        else {
            comment.removeAttr('required');
        }
    });

    $('input[name="checked_all"]').change(function () {
        if ($('input[name = "checked_all"]').is(':checked')) {
            $('input[name="partners"]').prop('checked', true);
        }
        else {
            $('input[name="partners"]').prop('checked', false);
        }
    })

    if ($('.parrr').is(':visible') && (!$('#partners').is(':visible'))) {
        $('#submitFeedback').slideUp('slow');
    }

});