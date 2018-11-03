﻿'use strict';
(function () {
    var app = angular.module('LibWebSystem', ['ngRoute', 'mgcrea.ngStrap', 'ui.utils.masks',  'mgcrea.ngStrap.tooltip', 'ngAnimate', 'toastr']);

    app.WebApi = 'http://localhost:56408/';

    app
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/Login', {
                    templateUrl: 'views/Usuario/Login.html',
                    controller: 'LoginController'
                })
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
                .when('/Usuario', {
                    templateUrl: 'views/Pessoa/Usuario.html',
                    controller: 'UsuarioController'
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
        .factory('Auth', function () {
            var user;

            return {
                setUser: function (aUser) {
                    user = aUser;
                },
                isLoggedIn: function () {
                    return (user) ? user : false;
                }
            };
        });


}());