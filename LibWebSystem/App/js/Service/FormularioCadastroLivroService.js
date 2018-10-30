'use strict';
var app = angular.module("LibWebSystem");

app.factory('FormularioCadastroLivrosService', ['$http', function ($http) {

    var FormularioCadastroLivrosService = {};


    FormularioCadastroLivrosService.RetornarDadosCamposSeletores = function () {
        return $http.get(app.WebApi + "FormularioCadastroLivro/RetornarCamposFormularioCadastroLivros/");
    };

    return FormularioCadastroLivrosService;

}])
