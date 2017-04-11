(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
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
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJTY3JpcHRzL19kZXYvYXBwLmpzIiwiU2NyaXB0cy9fZGV2L2xvZ2luL2xvZ2luLmpzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0FDQUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FDOUJBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EiLCJmaWxlIjoiZ2VuZXJhdGVkLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXNDb250ZW50IjpbIihmdW5jdGlvbiBlKHQsbixyKXtmdW5jdGlvbiBzKG8sdSl7aWYoIW5bb10pe2lmKCF0W29dKXt2YXIgYT10eXBlb2YgcmVxdWlyZT09XCJmdW5jdGlvblwiJiZyZXF1aXJlO2lmKCF1JiZhKXJldHVybiBhKG8sITApO2lmKGkpcmV0dXJuIGkobywhMCk7dmFyIGY9bmV3IEVycm9yKFwiQ2Fubm90IGZpbmQgbW9kdWxlICdcIitvK1wiJ1wiKTt0aHJvdyBmLmNvZGU9XCJNT0RVTEVfTk9UX0ZPVU5EXCIsZn12YXIgbD1uW29dPXtleHBvcnRzOnt9fTt0W29dWzBdLmNhbGwobC5leHBvcnRzLGZ1bmN0aW9uKGUpe3ZhciBuPXRbb11bMV1bZV07cmV0dXJuIHMobj9uOmUpfSxsLGwuZXhwb3J0cyxlLHQsbixyKX1yZXR1cm4gbltvXS5leHBvcnRzfXZhciBpPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7Zm9yKHZhciBvPTA7bzxyLmxlbmd0aDtvKyspcyhyW29dKTtyZXR1cm4gc30pIiwidmFyIGxvZ2luID0gcmVxdWlyZSgnLi9sb2dpbi9sb2dpbicpO1xyXG5cclxudmFyIGNoZWNrTG9naW4gPSBuZXcgbG9naW4oXCJhZG1pblwiLCBcImFkbWluMTIzXCIpO1xyXG4vL2NoZWNrTG9naW4ubG9nKCk7XHJcblxyXG4vLyBcclxuKGZ1bmN0aW9uICgkKSB7XHJcbiAgICAvLyBtYWluXHJcbiAgICB2YXIgJHBhZ2luYXRpb24gPSAkKFwiI3BhZ2luYXRpb24tbG9nXCIpO1xyXG4gICAgdmFyICRwb3NpdGlvbkxvZyA9ICQoXCIjcG9zaXRpb24tbG9nXCIpO1xyXG4gICAgY29uc29sZS5sb2coXCJtaWVyZGFLZW55XCIpO1xyXG4gICAgJChkb2N1bWVudCkub24oXCJjbGlja1wiLFwiI3BhZ2luYXRpb24tbG9nXCIsIGZ1bmN0aW9uIChldmVudCkge1xyXG5cclxuICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xyXG4gICAgICAgIHZhciAkY2hpbGQgPSBldmVudC50YXJnZXQ7XHJcbiAgICAgICAgaWYgKCdBJyA9PSAkY2hpbGQudGFnTmFtZSkge1xyXG4gICAgICAgICAgICB2YXIgJHVybCA9ICRjaGlsZC5nZXRBdHRyaWJ1dGUoXCJocmVmXCIpO1xyXG4gICAgICAgICAgICBpZiAoXCIjXCIgIT09ICR1cmwpIHtcclxuICAgICAgICAgICAgICAgIGRvY3VtZW50LmJvZHkuc3R5bGUuY3Vyc29yID0gJ3dhaXQnO1xyXG4gICAgICAgICAgICAgICAgJC5hamF4KHtcclxuICAgICAgICAgICAgICAgICAgICB1cmw6ICRjaGlsZC5nZXRBdHRyaWJ1dGUoXCJocmVmXCIpLFxyXG5cclxuICAgICAgICAgICAgICAgIH0pLmRvbmUoZnVuY3Rpb24gKHBhZ2luYXRpb24pIHtcclxuICAgICAgICAgICAgICAgICAgICAkcG9zaXRpb25Mb2cuaHRtbChwYWdpbmF0aW9uKTtcclxuICAgICAgICAgICAgICAgICAgICBkb2N1bWVudC5ib2R5LnN0eWxlLmN1cnNvciA9ICdkZWZhdWx0JztcclxuICAgICAgICAgICAgICAgIH0pO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSk7XHJcblxyXG59KGpRdWVyeSkpOyIsInZhciBMb2dpbiA9IGZ1bmN0aW9uICh1c2VyLCBwYXNzd29yZCkge1xyXG5cclxuICAgIHRoaXMudXNlciA9IHVzZXI7XHJcbiAgICB0aGlzLnBhc3N3b3JkID0gcGFzc3dvcmQ7XHJcblxyXG4gICAgdmFyIHNlbGYgPSB0aGlzO1xyXG5cclxuICAgIHJldHVybiB7XHJcbiAgICAgICAgbG9nOiBmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgIGlmKHVzZXIgPT0gXCJhZG1pblwiICAmJiBwYXNzd29yZCA9PSBcImFkbWluMTIzXCIpe1xyXG4gICAgICAgICAgICAgICAgY29uc29sZS5sb2coXCJXZWxjb21lIEFkbWluXCIpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSxcclxuICAgIH07XHJcbn1cclxuXHJcbm1vZHVsZS5leHBvcnRzID0gTG9naW47Il19
