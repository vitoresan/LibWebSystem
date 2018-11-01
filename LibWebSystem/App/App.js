'use strict';
(function () {
    var app = angular.module('LibWebSystem', ['ngRoute', 'mgcrea.ngStrap', 'ui.utils.masks',  'mgcrea.ngStrap.tooltip', 'ngAnimate', 'toastr']);

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
                .when('/Livros/Editar', {
                    templateUrl: 'views/Livros/edicao.html',
                    controller: 'CadastroLivrosController'
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
        .config(['toastrConfig', function (toastrConfig) {
            angular.extend(toastrConfig, {
                allowHtml: false,
                closeButton: true,
                templates: {
                    toast: 'views/Toast/toast.html',
                }
            });
        }])
}());