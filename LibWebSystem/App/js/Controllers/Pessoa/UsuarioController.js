(function () {

    var app = angular.module('LibWebSystem');

    var UsuarioController = function ($scope, $http, $log, $window, UsuarioService, tabelaPadrao, toastr) {

        $scope.inicializar = function () {
            if ($window.location.hash == '#/Usuario/Editar')
                novoObjetoUsuario();
            else {
                limparUsuario();
                popularTiposUsuario();
            }
        };

        function popularTiposUsuario() {
            UsuarioService.RetornarTiposUsuario().then(sucessoRetornarTiposUsuario, erroRetornarTiposUsuario)
        }

        function sucessoRetornarTiposUsuario(response) {
            if (response.data.Status === 1) {
                $scope.tiposUsuarios = response.data.Valor;
            }
            else {
                toastr.warning("Erros ao listar tipos de usuário, " + response.data.Mensagem)
            }
        }

        function erroRetornarTiposUsuario(reason) {
            toastr.warning("Erros ao listar tipos de usuário, " + reason.Message )
        }

        $scope.cadastrarUsuario = function (usuario) {
            if (dadosValidos(usuario)) {
                usuario.Tipo.push($scope.tipoUsuario);
                UsuarioService.Cadastrar(usuario).then(sucessoCadastrarUsuario, erroCadastrarUsuario)
            }
        }

        function dadosValidos(usuario) {
            return true;
        }

        function sucessoCadastrarUsuario(response) {
            if (response.data.Status === 1) {
                toastr.success(response.data.Mensagem)
            }
            else {
                toastr.warning(response.data.Mensagem)
            }
        }

        function erroCadastrarUsuario(reason) {
            toastr.warning("Erros ao cadastrar usuário, " + reason.Message)
        }

        function limparUsuario() {
            $scope.Usuario = UsuarioService.NovoUsuario();
            UsuarioService.Usuario = UsuarioService.NovoUsuario();
        }

        function novoObjetoUsuario() {
            $scope.Usuario = UsuarioService.Usuario;

            if ($scope.Usuario === undefined) {
                $scope.Usuario = UsuarioService.NovoUsuario();
                return;
            }
        }
    }

    app.controller('UsuarioController', UsuarioController);
}());