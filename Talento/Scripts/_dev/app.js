// Demo with require check /login/login.js
//var login = require('./login/login');
//var checkLogin = new login("admin", "admin123");
//checkLogin.log();

// 
(function ($) {
    // Requires
    var Pagination = require('./utilities/pagination');

    //Main
    $('[data-toggle="tooltip"]').tooltip();

    // Pagination
    var $pagination = $("#pagination-log");
    var $positionLog = $("#position-log");
    // Check for pagination
    if ($pagination.hasClass("pagination-enabled")) {
        var paginationLogin = new Pagination($pagination, $positionLog);
        // Create Pagination
        paginationLogin.Create();
    }
    // Logs 
    var $closeButton = $('.container-close');
    $closeButton.on("click", function () {
        $('#position-log-container').toggleClass("container-open");
    });
    // Candidate
    var $candidateList = $("#candidates-list");

}(jQuery));