(function () {

    var app = angular.module('LibWebSystem');

    var ConsultaLivrosController = function ($scope, $http, $log, $window, LivrosService, tabelaPadrao, toastr) {

        $scope.inicializar = function () {
            $scope.checkAll = true;
            listarLivros();
        };

        $scope.retornarValorCampo = (campo) => {
            if (!campo)
                campo = "Não Cadastrado."
            return campo
        }

        $scope.retornarValorListaEmString = (objeto) => {
            if (!objeto)
                return "Não Cadastrado."

            return objeto.reduce(function (prevVal, elem) {
                return prevVal + Object.values(elem)[Object.keys(elem).indexOf(Object.keys(elem).filter(x => x.includes("Descr"))[0])] + ', ';
            }, '')
        }

        $scope.btnEditar = function (livro, index) {
            $scope.LivroAntesAlteracao = livro;
            $scope.indiceEdicaoLivro = index;
            LivrosService.RetornarLivroPorID(livro.Id)
                .then(onCompleteListarLivroPorID, onErrorListarLivroPorID)
        };


        $scope.exibirDados = (i, livro) => {
            if (!livro.seExibeDados) {
                $scope.Livros.map(x => x.seExibeDados = false)
            }
            livro.seExibeDados = !livro.seExibeDados;
        }

        function listarLivros() {
            LivrosService.RetornarLivros()
                .then(sucessoRetornarLivros, erroRetornarLivros)
        }


        function sucessoRetornarLivros(response) {
            if (response.status === 200) {
                $scope.Livros = response.data.Valor;

                tabelaPadrao.gerar({
                    table: $('#tblLivros'),
                    data: $scope.Livros
                })
            }
            else {
                return;
            }
        }

        function erroRetornarLivros(reason) {
            $log.debug(reason.Message);
            return;
        }

        function onCompleteListarLivroPorID(success) {
            if (success.data.Status === 0) {
                toastr.warning(success.data.Mensagem);
            }
            else {
                $scope.formLivro = "parte1";
                LivrosService.Livro = success.data.Valor;
                $window.location.href = '#/Livros/Editar'
            }
        };

        function onErrorListarLivroPorID(error) {
            toastr.error('Erro ao buscar livro: ' + error.data.Message + ' Atualize a página e tente novamente.');
        };
    };

    app.controller('ConsultaLivrosController', ConsultaLivrosController);

}());