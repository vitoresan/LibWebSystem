(function () {

    var app = angular.module('LibWebSystem');

    var CadastroLivrosController = function ($scope, $window, $http, $log, FormularioCadastroLivrosService, toastr, LivrosService) {

        $scope.inicializar = function () {
            $scope.infoLivro = {};
            if ($window.location.hash == '#/Livros/Editar')
                novoObjetoLivro();
            else
                limparLivro();

            popularCamposSeletores();
            $scope.formLivro = "parte1";
        };

        $scope.cadastrarLivro = function () {
            if (camposObrigatoriosvalidos($scope.Livro))
                LivrosService.CadastrarLivro($scope.Livro)
                    .then(sucessoCadastroLivro, erroCadastroLivro)
        }

        $scope.cancelar_cadastro = function () {
            novoObjetoLivro();
        }

        $scope.alterarTabsFormLivro = function (tab) {
            $scope.formLivro = tab;
        }

        $scope.editarLivro = function () {
            if (camposObrigatoriosvalidos($scope.Livro))
                LivrosService.EditarLivro($scope.Livro)
                    .then(sucessoEditarLivro, erroEditarLivro)
        }

        $scope.cancelarAlteracao = function () {
            limparLivro();
            $window.location.href = '#/Livros/Consulta'
        }

        $scope.adicionarCursoRelacionado = function () {

            if (!$scope.Livro.CursoRelacionado) {
                toastr.warning("Favor selecionar um curso da lista. Comece digitando alguma letra do curso desejado.");
                return;
            } else if (!$scope.Livro.CursoRelacionado.Descr) {
                toastr.warning("Favor selecionar um curso da lista. Comece digitando alguma letra do curso desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.Cursos, $scope.Livro.CursoRelacionado)) {
                toastr.warning(`O livro já possui o curso de ${$scope.Livro.CursoRelacionado.Descr}.`);
                return;
            }

            $scope.Livro.CamposLivro.Cursos.push($scope.Livro.CursoRelacionado)
            $scope.infoLivro.cursos = retornarListaEmString($scope.Livro.CamposLivro.Cursos.map(x => x.Descr));
            $scope.Livro.CursoRelacionado = "";
        }

        $scope.adicionarBiografia = function () {

            if (!$scope.Livro.BiografiaTipo) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (!$scope.Livro.BiografiaTipo.Descr_Bibliografia_Tipo) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.Biografias_Tipo, $scope.Livro.BiografiaTipo)) {
                toastr.warning(`O livro já possui o tipo de biografia: ${$scope.Livro.BiografiaTipo.Descr_Bibliografia_Tipo}.`);
                return;
            }

            $scope.Livro.CamposLivro.Biografias_Tipo.push($scope.Livro.BiografiaTipo)
            $scope.infoLivro.biografias = retornarListaEmString($scope.Livro.CamposLivro.Biografias_Tipo.map(x => x.Descr_Bibliografia_Tipo));
            $scope.Livro.BiografiaTipo = "";
        }

        $scope.adicionarEsquemaCodificacao = function () {

            if (!$scope.Livro.EsquemaCodificacao) {
                toastr.warning("Favor selecionar um esquema da lista. Comece digitando alguma letra do esquema desejado.");
                return;
            } else if (!$scope.Livro.EsquemaCodificacao.Descr_Esquema_Codificacao) {
                toastr.warning("Favor selecionar um esquema da lista. Comece digitando alguma letra do esquema desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.Esquemas_Codificacao, $scope.Livro.EsquemaCodificacao)) {
                toastr.warning(`O livro já possui o esquema de codificação: ${$scope.Livro.EsquemaCodificacao.Descr_Esquema_Codificacao}.`);
                return;
            }

            $scope.Livro.CamposLivro.Esquemas_Codificacao.push($scope.Livro.EsquemaCodificacao)
            $scope.infoLivro.esquemasCodificacao = retornarListaEmString($scope.Livro.CamposLivro.Esquemas_Codificacao.map(x => x.Descr_Esquema_Codificacao));
            $scope.Livro.EsquemaCodificacao = "";
        }

        $scope.adicionarFormasCatalogacaoDescritiva = function () {

            if (!$scope.Livro.FormaCatalogacaoDescritiva) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (!$scope.Livro.FormaCatalogacaoDescritiva.Descr_Forma_Catalogacao_Descritiva) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.formas_Catalogacao_Descritiva, $scope.Livro.FormaCatalogacaoDescritiva)) {
                toastr.warning(`O livro já possui a forma de catalogação descritiva: ${$scope.Livro.FormaCatalogacaoDescritiva.Descr_Forma_Catalogacao_Descritiva}.`);
                return;
            }

            $scope.Livro.CamposLivro.Formas_Catalogacao_Descritiva.push($scope.Livro.FormaCatalogacaoDescritiva)
            $scope.infoLivro.formasCatalogacaoDescritiva = retornarListaEmString($scope.Livro.CamposLivro.Formas_Catalogacao_Descritiva.map(x => x.Descr_Forma_Catalogacao_Descritiva));
            $scope.Livro.FormaCatalogacaoDescritiva = "";
        }

        $scope.adicionarFormasItem = function () {

            if (!$scope.Livro.formaItem) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (!$scope.Livro.formaItem.Descr_Forma_Item) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.formas_Item, $scope.Livro.formaItem)) {
                toastr.warning(`O livro já possui a forma de item: ${$scope.Livro.formaItem.Descr_Forma_Item}.`);
                return;
            }

            $scope.Livro.CamposLivro.formas_Item.push($scope.Livro.formaItem)
            $scope.infoLivro.formasItem = retornarListaEmString($scope.Livro.CamposLivro.formas_Item.map(x => x.Descr_Forma_Item));
            $scope.Livro.formaItem = "";
        }

        $scope.adicionarFormaLiteraria = function () {

            if (!$scope.Livro.formaLiteraria) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (!$scope.Livro.formaLiteraria.Descr_Forma_Literaria) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.formas_Literaria, $scope.Livro.formaLiteraria)) {
                toastr.warning(`O livro já possui a forma literária: ${$scope.Livro.formaLiteraria.Descr_Forma_Literaria}.`);
                return;
            }

            $scope.Livro.CamposLivro.formas_Literaria.push($scope.Livro.formaLiteraria)
            $scope.infoLivro.formasLiterarias = retornarListaEmString($scope.Livro.CamposLivro.formas_Literaria.map(x => x.Descr_Forma_Literaria));
            $scope.Livro.formaLiteraria = "";
        }

        $scope.adicionarFormaMaterial = function () {

            if (!$scope.Livro.formaMaterial) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (!$scope.Livro.formaMaterial.Descr_Forma_Material) {
                toastr.warning("Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.formas_Material, $scope.Livro.formaMaterial)) {
                toastr.warning(`O livro já possui a forma material: ${$scope.Livro.formaMaterial.Descr_Forma_Material}.`);
                return;
            }

            $scope.Livro.CamposLivro.formas_Material.push($scope.Livro.formaMaterial)
            $scope.infoLivro.formasMaterial = retornarListaEmString($scope.Livro.CamposLivro.formas_Material.map(x => x.Descr_Forma_Material));
            $scope.Livro.formaMaterial = "";
        }

        $scope.adicionarIlustracaoTipo = function () {

            if (!$scope.Livro.IlustracaoTipo) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (!$scope.Livro.IlustracaoTipo.Descr_Ilustracao_Tipo) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.Ilustracoes_Tipo, $scope.Livro.IlustracaoTipo)) {
                toastr.warning(`O livro já possui o tipo de ilustração: ${$scope.Livro.IlustracaoTipo.Descr_Ilustracao_Tipo}.`);
                return;
            }

            $scope.Livro.CamposLivro.Ilustracoes_Tipo.push($scope.Livro.IlustracaoTipo)
            $scope.infoLivro.ilustracoes = retornarListaEmString($scope.Livro.CamposLivro.Ilustracoes_Tipo.map(x => x.Descr_Ilustracao_Tipo));
            $scope.Livro.IlustracaoTipo = "";
        }

        $scope.adicionarNaturezaConteudo = function () {

            if (!$scope.Livro.naturezaConteudo) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (!$scope.Livro.naturezaConteudo.Descr_Natureza_Conteudo) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.naturezas_Conteudo, $scope.Livro.naturezaConteudo)) {
                toastr.warning(`O livro já possui a natureza do conteúdo: ${$scope.Livro.naturezaConteudo.Descr_Natureza_Conteudo}.`);
                return;
            }

            $scope.Livro.CamposLivro.naturezas_Conteudo.push($scope.Livro.naturezaConteudo)
            $scope.infoLivro.naturezas = retornarListaEmString($scope.Livro.CamposLivro.naturezas_Conteudo.map(x => x.Descr_Natureza_Conteudo));
            $scope.Livro.naturezaConteudo = "";
        }

        $scope.adicionarNivelBibliografico = function () {

            if (!$scope.Livro.nivelBibliografico) {
                toastr.warning("Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.");
                return;
            } else if (!$scope.Livro.nivelBibliografico.Descr_Nivel_Bibliografico) {
                toastr.warning("Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.niveis_Bibliograficos, $scope.Livro.nivelBibliografico)) {
                toastr.warning(`O livro já possui o nível bibliográfico: ${$scope.Livro.nivelBibliografico.Descr_Nivel_Bibliografico}.`);
                return;
            }

            $scope.Livro.CamposLivro.niveis_Bibliograficos.push($scope.Livro.nivelBibliografico)
            $scope.infoLivro.niveisBibliograficos = retornarListaEmString($scope.Livro.CamposLivro.niveis_Bibliograficos.map(x => x.Descr_Nivel_Bibliografico));
            $scope.Livro.nivelBibliografico = "";
        }

        $scope.adicionarNivelCodificacao = function () {

            if (!$scope.Livro.nivelCodificacao) {
                toastr.warning("Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.");
                return;
            } else if (!$scope.Livro.nivelCodificacao.Descr_Nivel_Codificacao) {
                toastr.warning("Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.niveis_Codificacao, $scope.Livro.nivelCodificacao)) {
                toastr.warning(`O livro já possui o nível de codificação: ${$scope.Livro.nivelCodificacao.Descr_Nivel_Codificacao}.`);
                return;
            }

            $scope.Livro.CamposLivro.niveis_Codificacao.push($scope.Livro.nivelCodificacao)
            $scope.infoLivro.niveisCodificacao = retornarListaEmString($scope.Livro.CamposLivro.niveis_Codificacao.map(x => x.Descr_Nivel_Codificacao));
            $scope.Livro.nivelCodificacao = "";
        }

        $scope.adicionarNivelVariasPartes = function () {

            if (!$scope.Livro.nivelVariasPartes) {
                toastr.warning("Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.");
                return;
            } else if (!$scope.Livro.nivelVariasPartes.Descr_Nivel_Varias_Partes) {
                toastr.warning("Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.niveis_Varias_Partes, $scope.Livro.nivelVariasPartes)) {
                toastr.warning(`O livro já possui o nível de várias partes: ${$scope.Livro.nivelVariasPartes.Descr_Nivel_Varias_Partes}.`);
                return;
            }

            $scope.Livro.CamposLivro.niveis_Varias_Partes.push($scope.Livro.nivelVariasPartes)
            $scope.infoLivro.niveisVariasPartes = retornarListaEmString($scope.Livro.CamposLivro.niveis_Varias_Partes.map(x => x.Descr_Nivel_Varias_Partes));
            $scope.Livro.nivelVariasPartes = "";
        }

        $scope.adicionarPublicacaoGovernamentalTipo = function () {

            if (!$scope.Livro.publicacaoGovernamentalTipo) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (!$scope.Livro.publicacaoGovernamentalTipo.Descr_Publicacao_Governamental_Tipo) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.publicacao_Governamental_Tipos, $scope.Livro.publicacaoGovernamentalTipo)) {
                toastr.warning(`O livro já possui o tipo de publicação governamental: ${$scope.Livro.publicacaoGovernamentalTipo.Descr_Publicacao_Governamental_Tipo}.`);
                return;
            }

            $scope.Livro.CamposLivro.publicacao_Governamental_Tipos.push($scope.Livro.publicacaoGovernamentalTipo)
            $scope.infoLivro.publicacoesGovernamentais = retornarListaEmString($scope.Livro.CamposLivro.publicacao_Governamental_Tipos.map(x => x.Descr_Publicacao_Governamental_Tipo));
            $scope.Livro.publicacaoGovernamentalTipo = "";
        }

        $scope.adicionarPublicoAlvo = function () {

            if (!$scope.Livro.PublicoAlvo) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (!$scope.Livro.PublicoAlvo.Descr_Publico_Alvo) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.Publicos_Alvos, $scope.Livro.PublicoAlvo)) {
                toastr.warning(`O livro já possui público alvo: ${$scope.Livro.publicoAlvo.Descr_Publico_Alvo}.`);
                return;
            }

            $scope.Livro.CamposLivro.Publicos_Alvos.push($scope.Livro.PublicoAlvo)
            $scope.infoLivro.publicos = retornarListaEmString($scope.Livro.CamposLivro.Publicos_Alvos.map(x => x.Descr_Publico_Alvo));
            $scope.Livro.PublicoAlvo = "";
        }

        $scope.adicionarStatusRegistro = function () {

            if (!$scope.Livro.statusRegistro) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (!$scope.Livro.statusRegistro.Descr_Status_Registro) {
                toastr.warning("Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.status_Registro, $scope.Livro.statusRegistro)) {
                toastr.warning(`O livro já possui o status: ${$scope.Livro.statusRegistro.Descr_Status_Registro}.`)
                return;
            }

            $scope.Livro.CamposLivro.status_Registro.push($scope.Livro.statusRegistro)
            $scope.infoLivro.status = retornarListaEmString($scope.Livro.CamposLivro.status_Registro.map(x => x.Descr_Status_Registro));
            $scope.Livro.statusRegistro = "";
        }

        $scope.adicionarTipoControle = function () {

            if (!$scope.Livro.tipoControle) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (!$scope.Livro.tipoControle.Descr_Tipo_Controle) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.tipos_Controle, $scope.Livro.tipoControle)) {
                toastr.warning(`O livro já possui o tipo de controle: ${$scope.Livro.tipoControle.Descr_Tipo_Controle}.`)
                return;
            }

            $scope.Livro.CamposLivro.tipos_Controle.push($scope.Livro.tipoControle)
            $scope.infoLivro.tiposControle = retornarListaEmString($scope.Livro.CamposLivro.tipos_Controle.map(x => x.Descr_Tipo_Controle));
            $scope.Livro.tipoControle = "";
        }

        $scope.adicionarTipoRegistro = function () {

            if (!$scope.Livro.tipoRegistro) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (!$scope.Livro.tipoRegistro.Descr_Tipo_Registro) {
                toastr.warning("Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.");
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.Livro.CamposLivro.tipos_Registro, $scope.Livro.tipoRegistro)) {
                toastr.warning(`O livro já possui o tipo de registro: ${$scope.Livro.tipoControle.Descr_Tipo_Controle}.`)
                return;
            }

            $scope.Livro.CamposLivro.tipos_Registro.push($scope.Livro.tipoRegistro)
            $scope.infoLivro.tiposRegistro = retornarListaEmString($scope.Livro.CamposLivro.tipos_Registro.map(x => x.Descr_Tipo_Registro));
            $scope.Livro.tipoRegistro = "";
        }

        function sucessoCadastroLivro(response) {
            if (response.data.Status === 1) {
                $scope.statusRetornoCadastroLivro = true;
                limparLivro();
                toastr.success(response.data.Mensagem);
                novoObjetoLivro();
            }
            else {
                $scope.statusRetornoCadastroLivro = false;
            }
        }

        function erroCadastroLivro(reason) {
            $scope.statusRetornoCadastroLivro = false;
            toastr.warning("Ocorreram erros ao realizar cadastro: " + reason.Message);
            return;
        }

        function sucessoEditarLivro(response) {
            if (response.data.Status === 1) {
                toastr.success(response.data.Mensagem);
                limparLivro();
                novoObjetoLivro();
                $window.location.href = '#/Livros/Consulta'
            }
            else {
                $scope.statusRetornoCadastroLivro = false;
            }
        }

        function erroEditarLivro(reason) {
            $scope.statusRetornoCadastroLivro = false;
            toastr.warning("Ocorreram erros ao realizar edição: " + reason.Message);
            return;
        }


        function limparLivro() {
            $scope.Livro = LivrosService.NovoLivro();
            LivrosService.Livro = LivrosService.NovoLivro();
        }

        function novoObjetoLivro() {
            $scope.Livro = LivrosService.Livro;

            if ($scope.Livro === undefined) {
                $scope.Livro = LivrosService.NovoLivro();
                return;
            }

            popularInfoLivro($scope.Livro.CamposLivro);
        }

        function popularInfoLivro(camposLivro) {
            $scope.infoLivro.cursos = retornarListaEmString($scope.Livro.CamposLivro.Cursos.map(x => x.Descr));
            $scope.infoLivro.ilustracoes = retornarListaEmString($scope.Livro.CamposLivro.Ilustracoes_Tipo.map(x => x.Descr_Ilustracao_Tipo));
            $scope.infoLivro.biografias = retornarListaEmString($scope.Livro.CamposLivro.Biografias_Tipo.map(x => x.Descr_Bibliografia_Tipo));
            $scope.infoLivro.publicos = retornarListaEmString($scope.Livro.CamposLivro.Publicos_Alvos.map(x => x.Descr_Publico_Alvo));
        }

        function popularCamposSeletores() {
            FormularioCadastroLivrosService.RetornarDadosCamposSeletores()
                .then(sucessoRetornarDadosCamposSeletores, erroRetornarDadosCamposSeletores)
        }

        function sucessoRetornarDadosCamposSeletores(response) {
            if (response.status === 200) {
                $scope.camposSeletores = response.data.Valor;
            }
            else {
                return;
            }
        }

        function erroRetornarDadosCamposSeletores(reason) {
            $log.debug(reason.Message);
            return;
        }

        function verificarSeObjetoJaPossuiItem(objeto, item) {
            return objeto.filter(function (object) {
                return object === item
            })[0]
        }

        function retornarListaEmString(lista) {
            var texto = "";
            lista.map(x => texto += x + ', ');
            return texto;
        }

        function camposObrigatoriosvalidos(livro) {
            var seCamposValidos = true;

            if (!livro.Titulo) {
                seCamposValidos = invalidarCadastroLivro();
                toastr.warning("O campo Titúlo do livro é obrigatório!");
                return;
            }

            if (!livro.Autor) {
                seCamposValidos = invalidarCadastroLivro();
                toastr.warning("O campo Autor do livro é obrigatório!");
                return;
            }

            if (!livro.Ano_Publicacao) {
                seCamposValidos = invalidarCadastroLivro();
                toastr.warning("O campo Ano de Publicação do livro é obrigatório!");
                return;
            }

            if (!livro.Local_Publicacao) {
                seCamposValidos = invalidarCadastroLivro();
                toastr.warning("O campo Local de Publicação do livro é obrigatório!");
            }

            return seCamposValidos;
        }

        function invalidarCadastroLivro() {
            $scope.statusRetornoCadastroLivro = false;
            toastr.warning("Favor preencher todos os campos obrigatórios.")
            return $scope.statusRetornoCadastroLivro;
        }
    };

    app.controller('CadastroLivrosController', CadastroLivrosController);

}());