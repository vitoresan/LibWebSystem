(function () {

    var app = angular.module('LibWebSystem');

    var CadastroLivrosController = function ($scope, $http, $log, FormularioCadastroLivrosService, LivrosService) {

        $scope.inicializar = function () {
            novoObjetoLivro();
            popularCamposSeletores();
            $scope.formLivro = "parte1";

        };

        $scope.cadastrarLivro = function () {
            if (camposObrigatoriosvalidos($scope.livro))
                LivrosService.CadastrarLivro($scope.livro)
                    .then(sucessoCadastroLivro, erroCadastroLivro)
        }

        $scope.limparMensagensValidacao = () => {
            $scope.mensagem = {};
        }

        $scope.cancelar_cadastro = function () {
            novoObjetoLivro();
        }

        $scope.alterarTabsFormLivro = function (tab) {
            $scope.formLivro = tab;
        }

        $scope.adicionarCursoRelacionado = function () {
            $scope.mensagem.ValidacaoCursos = "";

            if (!$scope.livro.cursoRelacionado) {
                $scope.mensagem.ValidacaoCursos = "Favor selecionar um curso da lista. Comece digitando alguma letra do curso desejado.";
                return;
            } else if (!$scope.livro.cursoRelacionado.Descr) {
                $scope.mensagem.ValidacaoCursos = "Favor selecionar um curso da lista. Comece digitando alguma letra do curso desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Cursos, $scope.livro.cursoRelacionado)) {
                $scope.mensagem.ValidacaoCursos = `O livro já possui o curso de ${$scope.livro.cursoRelacionado.Descr}.`
                return;
            }

            $scope.livro.camposLivro.Cursos.push($scope.livro.cursoRelacionado)
            $scope.infoLivro.cursos = retornarListaEmString($scope.livro.camposLivro.Cursos.map(x => x.Descr));
            $scope.livro.cursoRelacionado = "";
        }

        $scope.adicionarBiografia = function () {
            $scope.mensagem.ValidacaoBiografia = "";

            if (!$scope.livro.biografiaTipo) {
                $scope.mensagem.ValidacaoBiografia = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (!$scope.livro.biografiaTipo.Descr_Bibliografia_Tipo) {
                $scope.mensagem.ValidacaoBiografia = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Biografias_Tipo, $scope.livro.biografiaTipo)) {
                $scope.mensagem.ValidacaoBiografia = `O livro já possui o tipo de biografia: ${$scope.livro.biografiaTipo.Descr_Bibliografia_Tipo}.`
                return;
            }

            $scope.livro.camposLivro.Biografias_Tipo.push($scope.livro.biografiaTipo)
            $scope.infoLivro.biografias = retornarListaEmString($scope.livro.camposLivro.Biografias_Tipo.map(x => x.Descr_Bibliografia_Tipo));
            $scope.livro.biografiaTipo = "";
        }

        $scope.adicionarEsquemaCodificacao = function () {
            $scope.mensagem.ValidacaoEsquemaCodificacao = "";

            if (!$scope.livro.esquemaCodificacao) {
                $scope.mensagem.ValidacaoEsquemaCodificacao = "Favor selecionar um esquema da lista. Comece digitando alguma letra do esquema desejado.";
                return;
            } else if (!$scope.livro.esquemaCodificacao.Descr_Esquema_Codificacao) {
                $scope.mensagem.ValidacaoEsquemaCodificacao = "Favor selecionar um esquema da lista. Comece digitando alguma letra do esquema desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Esquemas_Codificacao, $scope.livro.esquemaCodificacao)) {
                $scope.mensagem.ValidacaoEsquemaCodificacao = `O livro já possui o esquema de codificação: ${$scope.livro.esquemaCodificacao.Descr_Esquema_Codificacao}.`
                return;
            }

            $scope.livro.camposLivro.Esquemas_Codificacao.push($scope.livro.esquemaCodificacao)
            $scope.infoLivro.esquemasCodificacao = retornarListaEmString($scope.livro.camposLivro.Esquemas_Codificacao.map(x => x.Descr_Esquema_Codificacao));
            $scope.livro.esquemaCodificacao = "";
        }

        $scope.adicionarFormasCatalogacaoDescritiva = function () {
            $scope.mensagem.ValidacaoFormaCatalogacaoDescritiva = "";

            if (!$scope.livro.formaCatalogacaoDescritiva) {
                $scope.mensagem.ValidacaoFormaCatalogacaoDescritiva = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (!$scope.livro.formaCatalogacaoDescritiva.Descr_Forma_Catalogacao_Descritiva) {
                $scope.mensagem.ValidacaoFormaCatalogacaoDescritiva = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Formas_Catalogacao_Descritiva, $scope.livro.formaCatalogacaoDescritiva)) {
                $scope.mensagem.ValidacaoFormaCatalogacaoDescritiva = `O livro já possui a forma de catalogação descritiva: ${$scope.livro.formaCatalogacaoDescritiva.Descr_Forma_Catalogacao_Descritiva}.`
                return;
            }

            $scope.livro.camposLivro.Formas_Catalogacao_Descritiva.push($scope.livro.formaCatalogacaoDescritiva)
            $scope.infoLivro.formasCatalogacaoDescritiva = retornarListaEmString($scope.livro.camposLivro.Formas_Catalogacao_Descritiva.map(x => x.Descr_Forma_Catalogacao_Descritiva));
            $scope.livro.formaCatalogacaoDescritiva = "";
        }

        $scope.adicionarFormasItem = function () {
            $scope.mensagem.ValidacaoFormaItem = "";

            if (!$scope.livro.formaItem) {
                $scope.mensagem.ValidacaoFormaItem = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (!$scope.livro.formaItem.Descr_Forma_Item) {
                $scope.mensagem.ValidacaoFormaItem = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Formas_Item, $scope.livro.formaItem)) {
                $scope.mensagem.ValidacaoFormaItem = `O livro já possui a forma de item: ${$scope.livro.formaItem.Descr_Forma_Item}.`
                return;
            }

            $scope.livro.camposLivro.Formas_Item.push($scope.livro.formaItem)
            $scope.infoLivro.formasItem = retornarListaEmString($scope.livro.camposLivro.Formas_Item.map(x => x.Descr_Forma_Item));
            $scope.livro.formaItem = "";
        }

        $scope.adicionarFormaLiteraria = function () {
            $scope.mensagem.ValidacaoFormaLiteraria = "";

            if (!$scope.livro.formaLiteraria) {
                $scope.mensagem.ValidacaoFormaLiteraria = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (!$scope.livro.formaLiteraria.Descr_Forma_Literaria) {
                $scope.mensagem.ValidacaoFormaLiteraria = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Formas_Literaria, $scope.livro.formaLiteraria)) {
                $scope.mensagem.ValidacaoFormaLiteraria = `O livro já possui a forma literária: ${$scope.livro.formaLiteraria.Descr_Forma_Literaria}.`
                return;
            }

            $scope.livro.camposLivro.Formas_Literaria.push($scope.livro.formaLiteraria)
            $scope.infoLivro.formasLiterarias = retornarListaEmString($scope.livro.camposLivro.Formas_Literaria.map(x => x.Descr_Forma_Literaria));
            $scope.livro.formaLiteraria = "";
        }

        $scope.adicionarFormaMaterial = function () {
            $scope.mensagem.ValidacaoFormaMaterial = "";
            if (!$scope.livro.formaMaterial) {
                $scope.mensagem.ValidacaoFormaMaterial = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (!$scope.livro.formaMaterial.Descr_Forma_Material) {
                $scope.mensagem.ValidacaoFormaMaterial = "Favor selecionar uma forma da lista. Comece digitando alguma letra da forma desejada.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Formas_Material, $scope.livro.formaMaterial)) {
                $scope.mensagem.ValidacaoFormaMaterial = `O livro já possui a forma material: ${$scope.livro.formaMaterial.Descr_Forma_Material}.`
                return;
            }

            $scope.livro.camposLivro.Formas_Material.push($scope.livro.formaMaterial)
            $scope.infoLivro.formasMaterial = retornarListaEmString($scope.livro.camposLivro.Formas_Material.map(x => x.Descr_Forma_Material));
            $scope.livro.formaMaterial = "";
        }

        $scope.adicionarIlustracaoTipo = function () {
            $scope.mensagem.ValidacaoIlustracaoTipo = "";

            if (!$scope.livro.ilustracaoTipo) {
                $scope.mensagem.ValidacaoIlustracaoTipo = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (!$scope.livro.ilustracaoTipo.Descr_Ilustracao_Tipo) {
                $scope.mensagem.ValidacaoIlustracaoTipo = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Ilustracoes_Tipo, $scope.livro.ilustracaoTipo)) {
                $scope.mensagem.ValidacaoIlustracaoTipo = `O livro já possui o tipo de ilustração: ${$scope.livro.ilustracaoTipo.Descr_Ilustracao_Tipo}.`
                return;
            }

            $scope.livro.camposLivro.Ilustracoes_Tipo.push($scope.livro.ilustracaoTipo)
            $scope.infoLivro.ilustracoes = retornarListaEmString($scope.livro.camposLivro.Ilustracoes_Tipo.map(x => x.Descr_Ilustracao_Tipo));
            $scope.livro.ilustracaoTipo = "";
        }

        $scope.adicionarNaturezaConteudo = function () {
            $scope.mensagem.ValidacaoNaturezaConteudo = "";

            if (!$scope.livro.naturezaConteudo) {
                $scope.mensagem.ValidacaoNaturezaConteudo = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (!$scope.livro.naturezaConteudo.Descr_Natureza_Conteudo) {
                $scope.mensagem.ValidacaoNaturezaConteudo = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Naturezas_Conteudo, $scope.livro.naturezaConteudo)) {
                $scope.mensagem.ValidacaoNaturezaConteudo = `O livro já possui a natureza do conteúdo: ${$scope.livro.naturezaConteudo.Descr_Natureza_Conteudo}.`
                return;
            }

            $scope.livro.camposLivro.Naturezas_Conteudo.push($scope.livro.naturezaConteudo)
            $scope.infoLivro.naturezas = retornarListaEmString($scope.livro.camposLivro.Naturezas_Conteudo.map(x => x.Descr_Natureza_Conteudo));
            $scope.livro.naturezaConteudo = "";
        }

        $scope.adicionarNivelBibliografico = function () {
            $scope.mensagem.ValidacaoNivelBibliografico = "";

            if (!$scope.livro.nivelBibliografico) {
                $scope.mensagem.ValidacaoNivelBibliografico = "Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.";
                return;
            } else if (!$scope.livro.nivelBibliografico.Descr_Nivel_Bibliografico) {
                $scope.mensagem.ValidacaoNivelBibliografico = "Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Niveis_Bibliograficos, $scope.livro.nivelBibliografico)) {
                $scope.mensagem.ValidacaoNivelBibliografico = `O livro já possui o nível bibliográfico: ${$scope.livro.nivelBibliografico.Descr_Nivel_Bibliografico}.`
                return;
            }

            $scope.livro.camposLivro.Niveis_Bibliograficos.push($scope.livro.nivelBibliografico)
            $scope.infoLivro.niveisBibliograficos = retornarListaEmString($scope.livro.camposLivro.Niveis_Bibliograficos.map(x => x.Descr_Nivel_Bibliografico));
            $scope.livro.nivelBibliografico = "";
        }

        $scope.adicionarNivelCodificacao = function () {
            $scope.mensagem.ValidacaoNivelCodificacao = "";

            if (!$scope.livro.nivelCodificacao) {
                $scope.mensagem.ValidacaoNivelCodificacao = "Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.";
                return;
            } else if (!$scope.livro.nivelCodificacao.Descr_Nivel_Codificacao) {
                $scope.mensagem.ValidacaoNivelCodificacao = "Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Niveis_Codificacao, $scope.livro.nivelCodificacao)) {
                $scope.mensagem.ValidacaoNivelCodificacao = `O livro já possui o nível de codificação: ${$scope.livro.nivelCodificacao.Descr_Nivel_Codificacao}.`
                return;
            }

            $scope.livro.camposLivro.Niveis_Codificacao.push($scope.livro.nivelCodificacao)
            $scope.infoLivro.niveisCodificacao = retornarListaEmString($scope.livro.camposLivro.Niveis_Codificacao.map(x => x.Descr_Nivel_Codificacao));
            $scope.livro.nivelCodificacao = "";
        }

        $scope.adicionarNivelVariasPartes = function () {
            $scope.mensagem.ValidacaoNivelVariasPartes = "";

            if (!$scope.livro.nivelVariasPartes) {
                $scope.mensagem.ValidacaoNivelVariasPartes = "Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.";
                return;
            } else if (!$scope.livro.nivelVariasPartes.Descr_Nivel_Varias_Partes) {
                $scope.mensagem.ValidacaoNivelVariasPartes = "Favor selecionar um nível da lista. Comece digitando alguma letra do nível desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Niveis_Varias_Partes, $scope.livro.nivelVariasPartes)) {
                $scope.mensagem.ValidacaoNivelVariasPartes = `O livro já possui o nível de várias partes: ${$scope.livro.nivelVariasPartes.Descr_Nivel_Varias_Partes}.`
                return;
            }

            $scope.livro.camposLivro.Niveis_Varias_Partes.push($scope.livro.nivelVariasPartes)
            $scope.infoLivro.niveisVariasPartes = retornarListaEmString($scope.livro.camposLivro.Niveis_Varias_Partes.map(x => x.Descr_Nivel_Varias_Partes));
            $scope.livro.nivelVariasPartes = "";
        }

        $scope.adicionarPublicacaoGovernamentalTipo = function () {
            $scope.mensagem.ValidacaoPublicacaoGovernamentalTipo = "";

            if (!$scope.livro.publicacaoGovernamentalTipo) {
                $scope.mensagem.ValidacaoPublicacaoGovernamentalTipo = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (!$scope.livro.publicacaoGovernamentalTipo.Descr_Publicacao_Governamental_Tipo) {
                $scope.mensagem.ValidacaoPublicacaoGovernamentalTipo = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Publicacao_Governamental_Tipos, $scope.livro.publicacaoGovernamentalTipo)) {
                $scope.mensagem.ValidacaoPublicacaoGovernamentalTipo = `O livro já possui o tipo de publicação governamental: ${$scope.livro.publicacaoGovernamentalTipo.Descr_Publicacao_Governamental_Tipo}.`
                return;
            }

            $scope.livro.camposLivro.Publicacao_Governamental_Tipos.push($scope.livro.publicacaoGovernamentalTipo)
            $scope.infoLivro.publicacoesGovernamentais = retornarListaEmString($scope.livro.camposLivro.Publicacao_Governamental_Tipos.map(x => x.Descr_Publicacao_Governamental_Tipo));
            $scope.livro.publicacaoGovernamentalTipo = "";
        }

        $scope.adicionarPublicoAlvo = function () {
            $scope.mensagem.ValidacaoPublicoAlvo = "";

            if (!$scope.livro.publicoAlvo) {
                $scope.mensagem.ValidacaoPublicoAlvo = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (!$scope.livro.publicoAlvo.Descr_Publico_Alvo) {
                $scope.mensagem.ValidacaoPublicoAlvo = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Publicos_Alvos, $scope.livro.publicoAlvo)) {
                $scope.mensagem.ValidacaoPublicoAlvo = `O livro já possui público alvo: ${$scope.livro.publicoAlvo.Descr_Publico_Alvo}.`
                return;
            }

            $scope.livro.camposLivro.Publicos_Alvos.push($scope.livro.publicoAlvo)
            $scope.infoLivro.publicos = retornarListaEmString($scope.livro.camposLivro.Publicos_Alvos.map(x => x.Descr_Publico_Alvo));
            $scope.livro.publicoAlvo = "";
        }

        $scope.adicionarStatusRegistro = function () {
            $scope.mensagem.ValidacaoStatusRegistro = "";

            if (!$scope.livro.statusRegistro) {
                $scope.mensagem.ValidacaoStatusRegistro = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (!$scope.livro.statusRegistro.Descr_Status_Registro) {
                $scope.mensagem.ValidacaoStatusRegistro = "Favor selecionar um item da lista. Comece digitando alguma letra do item desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Status_Registro, $scope.livro.statusRegistro)) {
                $scope.mensagem.ValidacaoStatusRegistro = `O livro já possui o status: ${$scope.livro.statusRegistro.Descr_Status_Registro}.`
                return;
            }

            $scope.livro.camposLivro.Status_Registro.push($scope.livro.statusRegistro)
            $scope.infoLivro.status = retornarListaEmString($scope.livro.camposLivro.Status_Registro.map(x => x.Descr_Status_Registro));
            $scope.livro.statusRegistro = "";
        }

        $scope.adicionarTipoControle = function () {
            $scope.mensagem.ValidacaoTipoControle = "";

            if (!$scope.livro.tipoControle) {
                $scope.mensagem.ValidacaoTipoControle = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (!$scope.livro.tipoControle.Descr_Tipo_Controle) {
                $scope.mensagem.ValidacaoTipoControle = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Tipos_Controle, $scope.livro.tipoControle)) {
                $scope.mensagem.ValidacaoTipoControle = `O livro já possui o tipo de controle: ${$scope.livro.tipoControle.Descr_Tipo_Controle}.`
                return;
            }

            $scope.livro.camposLivro.Tipos_Controle.push($scope.livro.tipoControle)
            $scope.infoLivro.tiposControle = retornarListaEmString($scope.livro.camposLivro.Tipos_Controle.map(x => x.Descr_Tipo_Controle));
            $scope.livro.tipoControle = "";
        }

        $scope.adicionarTipoRegistro = function () {
            $scope.mensagem.ValidacaoTipoRegistro = "";

            if (!$scope.livro.tipoRegistro) {
                $scope.mensagem.ValidacaoTipoRegistro = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (!$scope.livro.tipoRegistro.Descr_Tipo_Registro) {
                $scope.mensagem.ValidacaoTipoRegistro = "Favor selecionar um tipo da lista. Comece digitando alguma letra do tipo desejado.";
                return;
            } else if (verificarSeObjetoJaPossuiItem($scope.livro.camposLivro.Tipos_Registro, $scope.livro.tipoRegistro)) {
                $scope.mensagem.ValidacaoTipoRegistro = `O livro já possui o tipo de registro: ${$scope.livro.tipoRegistro.Descr_Tipo_Registro}.`
                return;
            }

            $scope.livro.camposLivro.Tipos_Registro.push($scope.livro.tipoRegistro)
            $scope.infoLivro.tiposRegistro = retornarListaEmString($scope.livro.camposLivro.Tipos_Registro.map(x => x.Descr_Tipo_Registro));
            $scope.livro.tipoRegistro = "";
        }

        function sucessoCadastroLivro(response) {
            if (response.data.Status === 1) {
                $scope.statusRetornoCadastroLivro = true;
                novoObjetoLivro();
            }
            else {
                $scope.statusRetornoCadastroLivro = false;
            }
            $scope.mensagem_retorno_cadastro = response.data.Mensagem;
        }

        function erroCadastroLivro(reason) {
            $scope.statusRetornoCadastroLivro = false;
            $scope.mensagem_retorno_cadastro = "Ocorreram erros ao realizar cadastro: " + reason.Message;
            return;
        }

        function novoObjetoLivro() {
            $scope.livro = LivrosService.Livro();
            $scope.infoLivro = {};
            $scope.limparMensagensValidacao();
            return;
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
            $scope.limparMensagensValidacao();
            var seCamposValidos = true;

            if (!livro.titulo) {
                seCamposValidos = invalidarCadastroLivro();
                $scope.mensagem.ValidacaoTitulo = "O campo Titúlo do livro é obrigatório!"
            }

            if (!livro.autor) {
                seCamposValidos = invalidarCadastroLivro();
                $scope.mensagem.ValidacaoAutor = "O campo Autor do livro é obrigatório!"
            }

            if (!livro.ano_Publicacao) {
                seCamposValidos = invalidarCadastroLivro();
                $scope.mensagem.ValidacaoAnoPublicacao = "O campo Ano de Publicação do livro é obrigatório!"
            }

            if (!livro.local_Publicacao) {
                seCamposValidos = invalidarCadastroLivro();
                $scope.mensagem.ValidacaoLocalPublicacao = "O campo Local de Publicação do livro é obrigatório!"
            }

            return seCamposValidos;
        }

        function invalidarCadastroLivro() {
            $scope.statusRetornoCadastroLivro = false;
            $scope.mensagem_retorno_cadastro = "Favor preencher todos os campos obrigatórios."
            return $scope.statusRetornoCadastroLivro;
        }
    };

    app.controller('CadastroLivrosController', CadastroLivrosController);

}());