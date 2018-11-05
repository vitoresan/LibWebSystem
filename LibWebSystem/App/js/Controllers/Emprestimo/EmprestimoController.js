(function () {

    var app = angular.module('LibWebSystem');

    var EmprestimoController = function ($scope, $http, $log, $window, UsuarioService, LivrosService, tabelaPadrao, toastr) {

        $scope.inicializar = function () {
            $scope.emprestimo = {};
            listarUsuarios();
            listarLivros();
        };

        $scope.retornarNome = function (usuario) {
            return usuario.Nome
        }

        $scope.retornarTitulo = function (livro) {
            return livro.Titulo
        }

        $scope.retornarDataSaida = function () {
            return "04/11/2018"
        }

        $scope.retornarDataDevolucao = function (dias) {
            return dias
        }

        function listarUsuarios() {
            UsuarioService.RetornarUsuariosEmprestimo()
                .then(sucessoRetornarUsuarios, erroRetornarUsuarios)
        }

        function sucessoRetornarUsuarios(response) {
            if (response.data.Status === 1) {
                $scope.TodosUsuarios = response.data.Valor;
            }
            else {
                toastr.warning('Erros ao listar usuários. ' + reason.Message);
                $log.debug(reason.Message);
            }
        }

        function erroRetornarUsuarios(reason) {
            toastr.warning('Erros ao listar usuários. ' + reason.Message);
            $log.debug(reason.Message);
            return;
        }

        function listarLivros() {
            LivrosService.RetornarLivros()
                .then(sucessoRetornarLivros, erroRetornarLivros)
        }

        function sucessoRetornarLivros(response) {
            if (response.data.Status === 1) {
                $scope.TodosLivros = response.data.Valor;
            }
            else {
                toastr.warning('Erros ao listar livros. ' + reason.Message);
                $log.debug(reason.Message);
            }
        }

        function erroRetornarLivros(reason) {
            toastr.warning('Erros ao listar livros. ' + reason.Message);
            $log.debug(reason.Message);
            return;
        }
    }

    app.controller('EmprestimoController', EmprestimoController);
}());