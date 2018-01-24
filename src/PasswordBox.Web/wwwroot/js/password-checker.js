
$(function () {

    initEyeClick();

})


function initEyeClick() {

    $(".eye").click(function () {
        
        var span = $(this).find("span");
        var clickStateAttr = "data-click-state";
        var isClicked = $(span).attr(clickStateAttr) == 1;
        var passwordTxt = $(this).prev();

        if (isClicked) {

            $(span).attr(clickStateAttr, 0);
            $(span).removeClass('fa fa-eye-slash').addClass('fa fa-eye');
            $(passwordTxt).attr('type', 'password');

        }
        else {

            $(span).attr(clickStateAttr, 1);
            $(span).removeClass('fa fa-eye').addClass('fa fa-eye-slash');
            $(passwordTxt).attr('type', 'text');


        }
    });
}
