'use strict';
var app = angular.module("LibWebSystem");

app.factory('LivrosService', ['$http', function ($http) {

    var LivrosService = {};

    LivrosService.NovoLivro = function () {
        return {
            Titulo: '',
            Num_paginas: 0,
            Autor: '',
            Endereco: '',
            Ano_Publicacao: '',
            Cdd: '',
            Qtd_Exemplares: '',
            Local_Publicacao: '',
            CamposLivro: {
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

    LivrosService.Livro = {
        Titulo: '',
        Num_paginas: 0,
        Autor: '',
        Endereco: '',
        Ano_Publicacao: '',
        Cdd: '',
        Qtd_Exemplares: '',
        Local_Publicacao: '',
        CamposLivro: {
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

    LivrosService.RetornarLivros = function () {
        return $http.get(app.WebApi + "Livros/RetornarLivros/");
    };

    LivrosService.RetornarLivroPorID = function (idLivro) {
        return $http.get(app.WebApi + "Livros/RetornarLivroPorID/" + idLivro);
    };

    LivrosService.CadastrarLivro = function (livro) {
        return $http.post(app.WebApi + "Livros/CadastrarLivro/", livro);
    };

    LivrosService.EditarLivro = function (livro) {
        return $http.post(app.WebApi + "Livros/EditarLivro/", livro);
    };

    return LivrosService;

}])
