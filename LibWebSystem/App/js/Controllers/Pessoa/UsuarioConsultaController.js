(function () {

    var app = angular.module('LibWebSystem');

    var UsuarioConsultaController = function ($scope, $http, $log, $window, UsuarioService, tabelaPadrao, toastr) {

        $scope.inicializar = function () {
            popularTiposUsuario();
        };

        $scope.btnPesquisar = function (filtro) {
            if (filtro.tipoUsuario.Id === 1) {
                UsuarioService.RetornarUsuariosEmprestimo()
                    .then(sucessoRetornarUsuarios, erroRetornarUsuarios)
            }
            else {
                UsuarioService.RetornarUsuariosSistema()
                    .then(sucessoRetornarUsuarios, erroRetornarUsuarios)
            }
        }

        $scope.btnEditar = function (usuario, index) {
            UsuarioService.RetornarUsuarioPorID(usuario.Id)
                .then(onCompleteRetornarUsuarioPorID, onErrorRetornarUsuarioPorID)
        };

        function onCompleteRetornarUsuarioPorID(success) {
            if (success.data.Status === 0) {
                toastr.warning(success.data.Mensagem);
            }
            else {
                UsuarioService.Usuario = success.data.Valor;
                $window.location.href = '#/Usuario/Editar'
            }
        };

        function onErrorRetornarUsuarioPorID(error) {
            toastr.error('Erro ao buscar usuário: ' + error.data.Message + ' Atualize a página e tente novamente.');
        };

        function sucessoRetornarUsuarios(response) {
            if (response.data.Status === 1) {
                $scope.Usuarios = response.data.Valor;

                tabelaPadrao.gerar({
                    table: $('#tblUsuarios'),
                    data: $scope.Usuarios
                })
            }
            else {
                toastr.warning('Erros ao consultar usuários. ' + reason.Message);
            }
        }

        function erroRetornarUsuarios(reason) {
            toastr.warning('Erros ao consultar usuários. ' + reason.Message);
            $log.debug(reason.Message);
            return;
        }

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
            toastr.warning("Erros ao listar tipos de usuário, " + reason.Message)
        }

    }

    app.controller('UsuarioConsultaController', UsuarioConsultaController);
}());