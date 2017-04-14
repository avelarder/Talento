(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
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
},{"./utilities/pagination":2}],2:[function(require,module,exports){
var Pagination = function ($parent, $innerChild) {

    this.parent = $parent;
    this.innerChild = $innerChild;

    // Global This accesible
    var self = this;

    this.watch = function () {
        $(document).on("click", self.parent, function (event) {
            event.preventDefault();
            // Get Child
            var $child = event.target;
            // Target Element A
            if ('A' == $child.tagName) {
                var $url = $child.getAttribute("href");
                if ("#" !== $url) {
                    var $typeClass = "slide-right";
                    var $parent = $(event.target.parentElement);
                    // Animation Left or Right
                    if ($parent.hasClass("pagination-arrow")) {
                        if ($parent.hasClass("pagination-prev")) {
                            $typeClass = "slide-left";
                        }
                    } else {
                        var nextPage = $child.innerHTML;
                        var currentPage = $("#pagination-log").find(".active a")[0].innerHTML;
                        if (nextPage < currentPage) {
                            $typeClass = "slide-left";
                        }
                    }
                    self.ajaxCall($url, $typeClass);
                }
            }
        });
    }

    // Ajax call
    this.ajaxCall = function ($href, $class) {
        document.body.style.cursor = 'wait';
        $.ajax({
            url: $href,
            data: { clase: $class }

        }).done(function (pagination) {
            self.innerChild.html(pagination);
            document.body.style.cursor = 'default';
        });
    }

    return {
        Create: function () {
            self.watch();
        },
    };
}
// Export Pagination
module.exports = Pagination;

},{}]},{},[1])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJTY3JpcHRzL19kZXYvYXBwLmpzIiwiU2NyaXB0cy9fZGV2L3V0aWxpdGllcy9wYWdpbmF0aW9uLmpzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0FDQUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FDOUJBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EiLCJmaWxlIjoiZ2VuZXJhdGVkLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXNDb250ZW50IjpbIihmdW5jdGlvbiBlKHQsbixyKXtmdW5jdGlvbiBzKG8sdSl7aWYoIW5bb10pe2lmKCF0W29dKXt2YXIgYT10eXBlb2YgcmVxdWlyZT09XCJmdW5jdGlvblwiJiZyZXF1aXJlO2lmKCF1JiZhKXJldHVybiBhKG8sITApO2lmKGkpcmV0dXJuIGkobywhMCk7dmFyIGY9bmV3IEVycm9yKFwiQ2Fubm90IGZpbmQgbW9kdWxlICdcIitvK1wiJ1wiKTt0aHJvdyBmLmNvZGU9XCJNT0RVTEVfTk9UX0ZPVU5EXCIsZn12YXIgbD1uW29dPXtleHBvcnRzOnt9fTt0W29dWzBdLmNhbGwobC5leHBvcnRzLGZ1bmN0aW9uKGUpe3ZhciBuPXRbb11bMV1bZV07cmV0dXJuIHMobj9uOmUpfSxsLGwuZXhwb3J0cyxlLHQsbixyKX1yZXR1cm4gbltvXS5leHBvcnRzfXZhciBpPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7Zm9yKHZhciBvPTA7bzxyLmxlbmd0aDtvKyspcyhyW29dKTtyZXR1cm4gc30pIiwiLy8gRGVtbyB3aXRoIHJlcXVpcmUgY2hlY2sgL2xvZ2luL2xvZ2luLmpzXHJcbi8vdmFyIGxvZ2luID0gcmVxdWlyZSgnLi9sb2dpbi9sb2dpbicpO1xyXG4vL3ZhciBjaGVja0xvZ2luID0gbmV3IGxvZ2luKFwiYWRtaW5cIiwgXCJhZG1pbjEyM1wiKTtcclxuLy9jaGVja0xvZ2luLmxvZygpO1xyXG5cclxuLy8gXHJcbihmdW5jdGlvbiAoJCkge1xyXG4gICAgLy8gUmVxdWlyZXNcclxuICAgIHZhciBQYWdpbmF0aW9uID0gcmVxdWlyZSgnLi91dGlsaXRpZXMvcGFnaW5hdGlvbicpO1xyXG5cclxuICAgIC8vTWFpblxyXG4gICAgJCgnW2RhdGEtdG9nZ2xlPVwidG9vbHRpcFwiXScpLnRvb2x0aXAoKTtcclxuXHJcbiAgICAvLyBQYWdpbmF0aW9uXHJcbiAgICB2YXIgJHBhZ2luYXRpb24gPSAkKFwiI3BhZ2luYXRpb24tbG9nXCIpO1xyXG4gICAgdmFyICRwb3NpdGlvbkxvZyA9ICQoXCIjcG9zaXRpb24tbG9nXCIpO1xyXG4gICAgLy8gQ2hlY2sgZm9yIHBhZ2luYXRpb25cclxuICAgIGlmICgkcGFnaW5hdGlvbi5oYXNDbGFzcyhcInBhZ2luYXRpb24tZW5hYmxlZFwiKSkge1xyXG4gICAgICAgIHZhciBwYWdpbmF0aW9uTG9naW4gPSBuZXcgUGFnaW5hdGlvbigkcGFnaW5hdGlvbiwgJHBvc2l0aW9uTG9nKTtcclxuICAgICAgICAvLyBDcmVhdGUgUGFnaW5hdGlvblxyXG4gICAgICAgIHBhZ2luYXRpb25Mb2dpbi5DcmVhdGUoKTtcclxuICAgIH1cclxuICAgIC8vIExvZ3MgXHJcbiAgICB2YXIgJGNsb3NlQnV0dG9uID0gJCgnLmNvbnRhaW5lci1jbG9zZScpO1xyXG4gICAgJGNsb3NlQnV0dG9uLm9uKFwiY2xpY2tcIiwgZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICQoJyNwb3NpdGlvbi1sb2ctY29udGFpbmVyJykudG9nZ2xlQ2xhc3MoXCJjb250YWluZXItb3BlblwiKTtcclxuICAgIH0pO1xyXG4gICAgLy8gQ2FuZGlkYXRlXHJcbiAgICB2YXIgJGNhbmRpZGF0ZUxpc3QgPSAkKFwiI2NhbmRpZGF0ZXMtbGlzdFwiKTtcclxuXHJcbn0oalF1ZXJ5KSk7IiwidmFyIFBhZ2luYXRpb24gPSBmdW5jdGlvbiAoJHBhcmVudCwgJGlubmVyQ2hpbGQpIHtcclxuXHJcbiAgICB0aGlzLnBhcmVudCA9ICRwYXJlbnQ7XHJcbiAgICB0aGlzLmlubmVyQ2hpbGQgPSAkaW5uZXJDaGlsZDtcclxuXHJcbiAgICAvLyBHbG9iYWwgVGhpcyBhY2Nlc2libGVcclxuICAgIHZhciBzZWxmID0gdGhpcztcclxuXHJcbiAgICB0aGlzLndhdGNoID0gZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICQoZG9jdW1lbnQpLm9uKFwiY2xpY2tcIiwgc2VsZi5wYXJlbnQsIGZ1bmN0aW9uIChldmVudCkge1xyXG4gICAgICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xyXG4gICAgICAgICAgICAvLyBHZXQgQ2hpbGRcclxuICAgICAgICAgICAgdmFyICRjaGlsZCA9IGV2ZW50LnRhcmdldDtcclxuICAgICAgICAgICAgLy8gVGFyZ2V0IEVsZW1lbnQgQVxyXG4gICAgICAgICAgICBpZiAoJ0EnID09ICRjaGlsZC50YWdOYW1lKSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgJHVybCA9ICRjaGlsZC5nZXRBdHRyaWJ1dGUoXCJocmVmXCIpO1xyXG4gICAgICAgICAgICAgICAgaWYgKFwiI1wiICE9PSAkdXJsKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdmFyICR0eXBlQ2xhc3MgPSBcInNsaWRlLXJpZ2h0XCI7XHJcbiAgICAgICAgICAgICAgICAgICAgdmFyICRwYXJlbnQgPSAkKGV2ZW50LnRhcmdldC5wYXJlbnRFbGVtZW50KTtcclxuICAgICAgICAgICAgICAgICAgICAvLyBBbmltYXRpb24gTGVmdCBvciBSaWdodFxyXG4gICAgICAgICAgICAgICAgICAgIGlmICgkcGFyZW50Lmhhc0NsYXNzKFwicGFnaW5hdGlvbi1hcnJvd1wiKSkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoJHBhcmVudC5oYXNDbGFzcyhcInBhZ2luYXRpb24tcHJldlwiKSkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgJHR5cGVDbGFzcyA9IFwic2xpZGUtbGVmdFwiO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIG5leHRQYWdlID0gJGNoaWxkLmlubmVySFRNTDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIGN1cnJlbnRQYWdlID0gJChcIiNwYWdpbmF0aW9uLWxvZ1wiKS5maW5kKFwiLmFjdGl2ZSBhXCIpWzBdLmlubmVySFRNTDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgaWYgKG5leHRQYWdlIDwgY3VycmVudFBhZ2UpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICR0eXBlQ2xhc3MgPSBcInNsaWRlLWxlZnRcIjtcclxuICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICBzZWxmLmFqYXhDYWxsKCR1cmwsICR0eXBlQ2xhc3MpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgLy8gQWpheCBjYWxsXHJcbiAgICB0aGlzLmFqYXhDYWxsID0gZnVuY3Rpb24gKCRocmVmLCAkY2xhc3MpIHtcclxuICAgICAgICBkb2N1bWVudC5ib2R5LnN0eWxlLmN1cnNvciA9ICd3YWl0JztcclxuICAgICAgICAkLmFqYXgoe1xyXG4gICAgICAgICAgICB1cmw6ICRocmVmLFxyXG4gICAgICAgICAgICBkYXRhOiB7IGNsYXNlOiAkY2xhc3MgfVxyXG5cclxuICAgICAgICB9KS5kb25lKGZ1bmN0aW9uIChwYWdpbmF0aW9uKSB7XHJcbiAgICAgICAgICAgIHNlbGYuaW5uZXJDaGlsZC5odG1sKHBhZ2luYXRpb24pO1xyXG4gICAgICAgICAgICBkb2N1bWVudC5ib2R5LnN0eWxlLmN1cnNvciA9ICdkZWZhdWx0JztcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4ge1xyXG4gICAgICAgIENyZWF0ZTogZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICBzZWxmLndhdGNoKCk7XHJcbiAgICAgICAgfSxcclxuICAgIH07XHJcbn1cclxuLy8gRXhwb3J0IFBhZ2luYXRpb25cclxubW9kdWxlLmV4cG9ydHMgPSBQYWdpbmF0aW9uO1xyXG4iXX0=
