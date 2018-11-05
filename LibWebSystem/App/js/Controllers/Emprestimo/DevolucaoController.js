(function () {

    var app = angular.module('LibWebSystem');

    var DevolucaoController = function ($scope, $http, $log, $window, UsuarioService, tabelaPadrao, toastr) {

        $scope.inicializar = function () {
            $scope.devolucao = "teste"
        };
    }

    app.controller('DevolucaoController', DevolucaoController);
}());