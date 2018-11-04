(function () {

    var app = angular.module('LibWebSystem');

    var UsuarioController = function ($scope, $http, $log, $window, UsuarioService, toastr) {

        $scope.inicializar = function () {
            if ($window.location.hash == '#/Usuario/Editar')
                novoObjetoUsuario();
            else {
                limparUsuario();
                popularTiposUsuario();
            }
        };

        $scope.cancelarEdicao = function () {
            limparUsuario();
            $window.location.href = '#/Usuario/Consulta';
        }

        $scope.cadastrarUsuario = function (usuario) {
            if (dadosValidos(usuario)) {
                UsuarioService.Cadastrar(usuario).then(sucessoCadastrarUsuario, erroCadastrarUsuario)
            }
        }

        $scope.editarUsuario = function (usuario) {
            if (dadosValidos(usuario)) {
                UsuarioService.Editar(usuario).then(sucessoEditarUsuario, erroEditarUsuario)
            }
        }

        $scope.adicionarTipoUsuario = function (tipo) {
            if ($scope.Usuario.Tipo.filter(x => x.Id === tipo.Id).length === 0) {
                $scope.Usuario.Tipo.push(tipo);
            }
        };

        $scope.removerTipoUsuario = function (tipo) {
            $scope.Usuario.Tipo.remove(x => x.Id === tipo.Id);
        };

        $scope.cancelarCadastro = function (){
            limparUsuario();
        }

        function sucessoEditarUsuario(response) {
            if (response.data.Status === 1) {
                toastr.success(response.data.Mensagem);
                limparUsuario();
                novoObjetoUsuario();
                $window.location.href = '#/Usuario/Consulta'
            }
            else {
                toastr.warning(response.data.Mensagem);
            }
        }

        function erroEditarUsuario(reason) {
            $scope.statusRetornoCadastroLivro = false;
            toastr.warning("Ocorreram erros ao realizar edição: " + reason.Message);
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

        function dadosValidos(usuario) {
            var dadosValidos = true;

            if (!usuario.Nome || usuario.Nome == "") {
                dadosValidos = false;
                toastr.info('O campo Nome é obrigatório')
            }

            if (!usuario.CPF || usuario.CPF == "") {
                dadosValidos = false;
                toastr.info('O campo CPF é obrigatório')
            }

            if (!usuario.Email || usuario.Email == "") {
                dadosValidos = false;
                toastr.info('O campo Email é obrigatório')
            }

            if (!usuario.Telefone || usuario.Telefone == "") {
                dadosValidos = false;
                toastr.info('O campo Telefone é obrigatório')
            }

            if (usuario.Tipo.length === 0) {
                dadosValidos = false;
                toastr.info('Ao menos um tipo de usuário é obrigatório.')
            }

            return dadosValidos;
        }

        function sucessoCadastrarUsuario(response) {
            if (response.data.Status === 1) {
                limparUsuario();
                toastr.success(response.data.Mensagem)
                novoObjetoUsuario();
            }
            else {
                toastr.warning(response.data.Mensagem)
            }
        }

        function erroCadastrarUsuario(reason) {
            toastr.warning("Erros ao cadastrar usuário, " + reason.Message)
        }

        function limparUsuario() {
            $scope.tipoUsuario = {};
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