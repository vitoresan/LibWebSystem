'use strict';
var app = angular.module("LibWebSystem");

app.factory('LoginService', ['$http', function ($http) {

    var LoginService = {};

    LoginService.Login = function (usuario, senha) {
        return $http.post(app.WebApi + "Token", "userName=" + encodeURIComponent(usuario) +
            "&password=" + encodeURIComponent(senha) +
            "&grant_type=password"
    };

    return LoginService;

}]);