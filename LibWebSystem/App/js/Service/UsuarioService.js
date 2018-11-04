'use strict';
var app = angular.module("LibWebSystem");

app.factory('UsuarioService', ['$http', function ($http) {

    var UsuarioService = {};

    UsuarioService.NovoUsuario = function () {
        return {
            Nome: '',
            Telefone: '',
            Email: '',
            CPF: '',
            Se_Ativo: true,
            Tipo: []
        }
    }

    UsuarioService.Usuario = {
        Nome: '',
        Telefone: '',
        Email: '',
        CPF: '',
        Se_Ativo: true,
        Tipo: []
    }

    UsuarioService.RetornarTiposUsuario = function () {
        return $http.get(app.WebApi + "Usuario/RetornarTipos/");
    };

    UsuarioService.Cadastrar = function (usuario) {
        return $http.post(app.WebApi + "Usuario/CadastrarUsuario/", usuario);
    };

    return UsuarioService;

}])
