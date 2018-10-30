'use strict';
var app = angular.module("LibWebSystem");

app.factory('CursosService', ['$http', function ($http) {

    var CursosService = {};

    CursosService.Curso = function () {
        return {
            Id: 0,
            Descr: ''
        }
    };

    CursosService.RetornarCursos = function () {
        return $http.get(app.WebApi + "Cursos/RetornarCursos/");
    };

    return CursosService;

}])
