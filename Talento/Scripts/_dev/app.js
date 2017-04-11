var login = require('./login/login');

var checkLogin = new login("admin", "admin123");
//checkLogin.log();

// 
(function ($) {
    // main
    var $pagination = $("#pagination-log");
    var $positionLog = $("#position-log");
    console.log("mierdaKeny");
    $(document).on("click","#pagination-log", function (event) {

        event.preventDefault();
        var $child = event.target;
        if ('A' == $child.tagName) {
            var $url = $child.getAttribute("href");
            if ("#" !== $url) {
                document.body.style.cursor = 'wait';
                $.ajax({
                    url: $child.getAttribute("href"),

                }).done(function (pagination) {
                    $positionLog.html(pagination);
                    document.body.style.cursor = 'default';
                });
            }
        }
    });

}(jQuery));