(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
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
},{"./login/login":2}],2:[function(require,module,exports){
var Login = function (user, password) {

    this.user = user;
    this.password = password;

    var self = this;

    return {
        log: function () {
            if(user == "admin"  && password == "admin123"){
                console.log("Welcome Admin");
            }
        },
    };
}

module.exports = Login;
},{}]},{},[1])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJTY3JpcHRzL19kZXYvYXBwLmpzIiwiU2NyaXB0cy9fZGV2L2xvZ2luL2xvZ2luLmpzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0FDQUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUM5Q0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uIGUodCxuLHIpe2Z1bmN0aW9uIHMobyx1KXtpZighbltvXSl7aWYoIXRbb10pe3ZhciBhPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7aWYoIXUmJmEpcmV0dXJuIGEobywhMCk7aWYoaSlyZXR1cm4gaShvLCEwKTt2YXIgZj1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK28rXCInXCIpO3Rocm93IGYuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixmfXZhciBsPW5bb109e2V4cG9ydHM6e319O3Rbb11bMF0uY2FsbChsLmV4cG9ydHMsZnVuY3Rpb24oZSl7dmFyIG49dFtvXVsxXVtlXTtyZXR1cm4gcyhuP246ZSl9LGwsbC5leHBvcnRzLGUsdCxuLHIpfXJldHVybiBuW29dLmV4cG9ydHN9dmFyIGk9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtmb3IodmFyIG89MDtvPHIubGVuZ3RoO28rKylzKHJbb10pO3JldHVybiBzfSkiLCJ2YXIgbG9naW4gPSByZXF1aXJlKCcuL2xvZ2luL2xvZ2luJyk7XHJcblxyXG52YXIgY2hlY2tMb2dpbiA9IG5ldyBsb2dpbihcImFkbWluXCIsIFwiYWRtaW4xMjNcIik7XHJcbi8vY2hlY2tMb2dpbi5sb2coKTtcclxuXHJcbi8vIFxyXG4oZnVuY3Rpb24gKCQpIHtcclxuICAgIC8vIG1haW5cclxuICAgIHZhciAkcGFnaW5hdGlvbiA9ICQoXCIjcGFnaW5hdGlvbi1sb2dcIik7XHJcbiAgICB2YXIgJHBvc2l0aW9uTG9nID0gJChcIiNwb3NpdGlvbi1sb2dcIik7XHJcbiAgICAkKGRvY3VtZW50KS5vbihcImNsaWNrXCIsIFwiI3BhZ2luYXRpb24tbG9nXCIsIGZ1bmN0aW9uIChldmVudCkge1xyXG5cclxuICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xyXG4gICAgICAgIHZhciAkY2hpbGQgPSBldmVudC50YXJnZXQ7XHJcbiAgICAgICAgaWYgKCdBJyA9PSAkY2hpbGQudGFnTmFtZSkge1xyXG4gICAgICAgICAgICB2YXIgJHVybCA9ICRjaGlsZC5nZXRBdHRyaWJ1dGUoXCJocmVmXCIpO1xyXG4gICAgICAgICAgICBpZiAoXCIjXCIgIT09ICR1cmwpIHtcclxuICAgICAgICAgICAgICAgIGRvY3VtZW50LmJvZHkuc3R5bGUuY3Vyc29yID0gJ3dhaXQnO1xyXG4gICAgICAgICAgICAgICAgdmFyICR0eXBlQ2xhc3MgPSBcInNsaWRlLXJpZ2h0XCI7XHJcbiAgICAgICAgICAgICAgICB2YXIgJHBhcmVudCA9ICQoZXZlbnQudGFyZ2V0LnBhcmVudEVsZW1lbnQpO1xyXG4gICAgICAgICAgICAgICAgLy8gQW5pbWF0aW9uIExlZnQgb3IgUmlnaHRcclxuICAgICAgICAgICAgICAgIGlmICgkcGFyZW50Lmhhc0NsYXNzKFwicGFnaW5hdGlvbi1hcnJvd1wiKSkge1xyXG4gICAgICAgICAgICAgICAgICAgIGlmICgkcGFyZW50Lmhhc0NsYXNzKFwicGFnaW5hdGlvbi1wcmV2XCIpKXtcclxuICAgICAgICAgICAgICAgICAgICAgICAgJHR5cGVDbGFzcyA9IFwic2xpZGUtbGVmdFwiO1xyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdmFyIG5leHRQYWdlID0gJGNoaWxkLmlubmVySFRNTDtcclxuICAgICAgICAgICAgICAgICAgICB2YXIgY3VycmVudFBhZ2UgPSAkKFwiI3BhZ2luYXRpb24tbG9nXCIpLmZpbmQoXCIuYWN0aXZlIGFcIilbMF0uaW5uZXJIVE1MO1xyXG4gICAgICAgICAgICAgICAgICAgIGlmIChuZXh0UGFnZSA8IGN1cnJlbnRQYWdlKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICR0eXBlQ2xhc3MgPSBcInNsaWRlLWxlZnRcIjtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICBcclxuICAgICAgICAgICAgICAgIC8vIEFqYXggY2FsbFxyXG4gICAgICAgICAgICAgICAgJC5hamF4KHtcclxuICAgICAgICAgICAgICAgICAgICB1cmw6ICRjaGlsZC5nZXRBdHRyaWJ1dGUoXCJocmVmXCIpLFxyXG4gICAgICAgICAgICAgICAgICAgIGRhdGE6IHtjbGFzZTogJHR5cGVDbGFzc31cclxuXHJcbiAgICAgICAgICAgICAgICB9KS5kb25lKGZ1bmN0aW9uIChwYWdpbmF0aW9uKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgJHBvc2l0aW9uTG9nLmh0bWwocGFnaW5hdGlvbik7XHJcbiAgICAgICAgICAgICAgICAgICAgZG9jdW1lbnQuYm9keS5zdHlsZS5jdXJzb3IgPSAnZGVmYXVsdCc7XHJcbiAgICAgICAgICAgICAgICB9KTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0pO1xyXG5cclxufShqUXVlcnkpKTsiLCJ2YXIgTG9naW4gPSBmdW5jdGlvbiAodXNlciwgcGFzc3dvcmQpIHtcclxuXHJcbiAgICB0aGlzLnVzZXIgPSB1c2VyO1xyXG4gICAgdGhpcy5wYXNzd29yZCA9IHBhc3N3b3JkO1xyXG5cclxuICAgIHZhciBzZWxmID0gdGhpcztcclxuXHJcbiAgICByZXR1cm4ge1xyXG4gICAgICAgIGxvZzogZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICBpZih1c2VyID09IFwiYWRtaW5cIiAgJiYgcGFzc3dvcmQgPT0gXCJhZG1pbjEyM1wiKXtcclxuICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKFwiV2VsY29tZSBBZG1pblwiKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0sXHJcbiAgICB9O1xyXG59XHJcblxyXG5tb2R1bGUuZXhwb3J0cyA9IExvZ2luOyJdfQ==
