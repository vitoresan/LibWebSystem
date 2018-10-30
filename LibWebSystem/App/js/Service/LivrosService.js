'use strict';
var app = angular.module("LibWebSystem");

app.factory('LivrosService', ['$http', function ($http) {

    var LivrosService = {};

    LivrosService.Livro = function () {
        return {
            titulo: '',
            num_paginas: 0,
            autor: '',
            endereco: '',
            ano_Publicacao: '',
            cdd: '',
            qtd_Exemplares: '',
            local_Publicacao: '',
            camposLivro: {
                Cursos: [],
                Biografias_Tipo: [],
                Esquemas_Codificacao: [],
                Formas_Catalogacao_Descritiva: [],
                Formas_Item: [],
                Formas_Literaria: [],
                Formas_Material: [],
                Ilustracoes_Tipo: [],
                Naturezas_Conteudo: [],
                Niveis_Bibliograficos: [],
                Niveis_Codificacao: [],
                Niveis_Varias_Partes: [],
                Publicacao_Governamental_Tipos: [],
                Publicos_Alvos: [],
                Status_Registro: [],
                Tipos_Controle: [],
                Tipos_Registro: []
            }
        }
    }

    LivrosService.RetornarLivros = function () {
        return $http.get(app.WebApi + "Livros/RetornarLivros/");
    };

    LivrosService.CadastrarLivro = function (livro) {
        return $http.post(app.WebApi + "Livros/CadastrarLivro/", livro);
    };

    return LivrosService;

}])
