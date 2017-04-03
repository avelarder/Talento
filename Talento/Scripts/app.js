(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
var login = require('./login/login');

var checkLogin = new login("admin", "admin123");
//checkLogin.log();

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
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJTY3JpcHRzL19kZXYvYXBwLmpzIiwiU2NyaXB0cy9fZGV2L2xvZ2luL2xvZ2luLmpzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0FDQUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUNKQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBIiwiZmlsZSI6ImdlbmVyYXRlZC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzQ29udGVudCI6WyIoZnVuY3Rpb24gZSh0LG4scil7ZnVuY3Rpb24gcyhvLHUpe2lmKCFuW29dKXtpZighdFtvXSl7dmFyIGE9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtpZighdSYmYSlyZXR1cm4gYShvLCEwKTtpZihpKXJldHVybiBpKG8sITApO3ZhciBmPW5ldyBFcnJvcihcIkNhbm5vdCBmaW5kIG1vZHVsZSAnXCIrbytcIidcIik7dGhyb3cgZi5jb2RlPVwiTU9EVUxFX05PVF9GT1VORFwiLGZ9dmFyIGw9bltvXT17ZXhwb3J0czp7fX07dFtvXVswXS5jYWxsKGwuZXhwb3J0cyxmdW5jdGlvbihlKXt2YXIgbj10W29dWzFdW2VdO3JldHVybiBzKG4/bjplKX0sbCxsLmV4cG9ydHMsZSx0LG4scil9cmV0dXJuIG5bb10uZXhwb3J0c312YXIgaT10eXBlb2YgcmVxdWlyZT09XCJmdW5jdGlvblwiJiZyZXF1aXJlO2Zvcih2YXIgbz0wO288ci5sZW5ndGg7bysrKXMocltvXSk7cmV0dXJuIHN9KSIsInZhciBsb2dpbiA9IHJlcXVpcmUoJy4vbG9naW4vbG9naW4nKTtcblxudmFyIGNoZWNrTG9naW4gPSBuZXcgbG9naW4oXCJhZG1pblwiLCBcImFkbWluMTIzXCIpO1xuLy9jaGVja0xvZ2luLmxvZygpO1xuIiwidmFyIExvZ2luID0gZnVuY3Rpb24gKHVzZXIsIHBhc3N3b3JkKSB7XG5cbiAgICB0aGlzLnVzZXIgPSB1c2VyO1xuICAgIHRoaXMucGFzc3dvcmQgPSBwYXNzd29yZDtcblxuICAgIHZhciBzZWxmID0gdGhpcztcblxuICAgIHJldHVybiB7XG4gICAgICAgIGxvZzogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgaWYodXNlciA9PSBcImFkbWluXCIgICYmIHBhc3N3b3JkID09IFwiYWRtaW4xMjNcIil7XG4gICAgICAgICAgICAgICAgY29uc29sZS5sb2coXCJXZWxjb21lIEFkbWluXCIpO1xuICAgICAgICAgICAgfVxuICAgICAgICB9LFxuICAgIH07XG59XG5cbm1vZHVsZS5leHBvcnRzID0gTG9naW47Il19
