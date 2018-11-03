(function () {

    var app = angular.module('LibWebSystem');

    var LoginController = function ($scope, $http, $log, $window, Auth, LoginService, toastr) {

        $scope.login = function (usuario, senha) {
            LoginService.Login(usuario, senha)
                .then(onCompleteLogin, onError);
        };

        function onCompleteLogin(success) {
            if (success.data.Status === 0) {
                toastr.warning(success.data.Mensagem);
            }
            else {
                Auth.setUser(success.data); 
                $window.location.href = '/Index';
            }
        }

        function onError(error) {
            toastr.error('Erro ao realizar login: ' + error.data.Message);
        }

        app.controller('LoginController', LoginController);

    };
});