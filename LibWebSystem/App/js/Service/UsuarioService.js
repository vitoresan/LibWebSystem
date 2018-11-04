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
            Tipo: [],
            tipoUsuario: {}
        }
    }

    UsuarioService.Usuario = {
        Nome: '',
        Telefone: '',
        Email: '',
        CPF: '',
        Se_Ativo: true,
        Tipo: [{
            Id: 0,
            Descr: ''
        }],
        tipoUsuario: {}

    }

    UsuarioService.RetornarTiposUsuario = function () {
        return $http.get(app.WebApi + "Usuario/RetornarTipos/");
    };

    UsuarioService.Cadastrar = function (usuario) {
        return $http.post(app.WebApi + "Usuario/CadastrarUsuario/", usuario);
    };

    UsuarioService.Editar = function (usuario) {
        return $http.put(app.WebApi + "Usuario/EditarUsuario/", usuario);
    };

    UsuarioService.RetornarUsuariosEmprestimo = function () {
        return $http.get(app.WebApi + "Usuario/RetornarUsuariosEmprestimo/");
    };

    UsuarioService.RetornarUsuariosSistema = function () {
        return $http.get(app.WebApi + "Usuario/RetornarUsuariosSistema/");
    };

    UsuarioService.RetornarUsuarioPorID = function (idUsuario) {
        return $http.get(app.WebApi + "Usuario/RetornarUsuarioPorId/" + idUsuario);
    };


    return UsuarioService;

}])
