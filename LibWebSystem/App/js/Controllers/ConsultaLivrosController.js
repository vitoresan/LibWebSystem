(function () {

    var app = angular.module('LibWebSystem');

    var ConsultaLivrosController = function ($scope, $http, $log, LivrosService, tabelaPadrao) {

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

        $scope.exibirDados = (i, livro) => {
            if (!livro.seExibeDados) {
                $scope.livros.map(x => x.seExibeDados = false)
            }
            livro.seExibeDados = !livro.seExibeDados;
        }

        function listarLivros() {
            LivrosService.RetornarLivros()
                .then(sucessoRetornarLivros, erroRetornarLivros)
        }


        function sucessoRetornarLivros(response) {
            if (response.status === 200) {
                $scope.livros = response.data.Valor;

                tabelaPadrao.gerar({
                    table: $('#tblLivros'),
                    data: $scope.livros
                })


                //$scope.livros.map(x => x.seExibeDados = false)
                //$('#pagination').pagination({
                //    dataSource: $scope.livros,
                //    callback: function (data, pagination) {
                //        // template method of yourself
                //        var html = template(data);
                //        dataContainer.html(html);
                //    }
                //})

            }
            else {
                return;
            }
        }

        function erroRetornarLivros(reason) {
            $log.debug(reason.Message);
            return;
        }
    };

    app.controller('ConsultaLivrosController', ConsultaLivrosController);

}());