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