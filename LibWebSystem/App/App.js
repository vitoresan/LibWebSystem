'use strict';
(function () {
    var app = angular.module('LibWebSystem', ['ngRoute', 'mgcrea.ngStrap', 'mgcrea.ngStrap.tooltip', 'ngAnimate']);

    app.WebApi = 'http://localhost:56408/';

    app
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/Livros/Cadastro', {
                    templateUrl: 'views/Livros/cadastro.html',
                    controller: 'CadastroLivrosController'
                })
                .when('/Livros/Consulta', {
                    templateUrl: 'views/Livros/consulta.html',
                    controller: 'ConsultaLivrosController'
                })
                .otherwise({
                    templateUrl: 'views/home.html',
                    //controller: 'homeController'
                });
        }])
        .config(function ($typeaheadProvider) {
            angular.extend($typeaheadProvider.defaults, {
                animation: 'am-flip-x',
                minLength: 1,
                limit: 8
            });
        })
}());