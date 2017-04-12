var login = require('./login/login');

var checkLogin = new login("admin", "admin123");
//checkLogin.log();

// 
(function ($) {
    // main
    var $pagination = $("#pagination-log");
    var $positionLog = $("#position-log");
    $(document).on("click", "#pagination-log", function (event) {

        event.preventDefault();
        var $child = event.target;
        if ('A' == $child.tagName) {
            var $url = $child.getAttribute("href");
            if ("#" !== $url) {
                document.body.style.cursor = 'wait';
                var $typeClass = "slide-right";
                var $parent = $(event.target.parentElement);
                // Animation Left or Right
                if ($parent.hasClass("pagination-arrow")) {
                    if ($parent.hasClass("pagination-prev")){
                        $typeClass = "slide-left";
                    }
                } else {
                    var nextPage = $child.innerHTML;
                    var currentPage = $("#pagination-log").find(".active a")[0].innerHTML;
                    if (nextPage < currentPage) {
                        $typeClass = "slide-left";
                    }
                }
                
                // Ajax call
                $.ajax({
                    url: $child.getAttribute("href"),
                    data: {clase: $typeClass}

                }).done(function (pagination) {
                    $positionLog.html(pagination);
                    document.body.style.cursor = 'default';
                });
            }
        }
    });

}(jQuery));