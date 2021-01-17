$(document).ready(function () {

    $("#Botao_Excluir_Cadastro").click(function () {
        LimparAlertas();
        var id = $("#UnidadeCurricular_id").val();
        var nome = $("#Nome").val();
        var curso = $("#Select2_Curso").val();
        var ativo = $("#Ativo").is(':checked');
        var msg = "";
        var entidade = {
            Id: id,
            Nome: nome,
            Curso: curso,
            Ativo: ativo
        };
        $.ajax({
            type: "POST",
            url: "/UnidadeCurricular/ConsisteExcluir/",
            data: JSON.stringify(entidade),
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (resposta) {
                var consistencia = JSON.parse(resposta);
                if (consistencia.InconsistenciasToString != "") {
                    msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                    addNotification(msg, 1);
                } else {
                    msg += consistencia.AdvertenciasToString.replaceAll("|", "<br>")
                    msg += ("Confirma Exclusão do Registro " + $("#UnidadeCurricular_id").val() + "?");
                    addNotification(msg, 2, "ConfirmarExclusao", "buttonCancelar");
                    $(".blockConfirmation").prop('disabled', true);
                }
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });

    });

    

    $('#Select2_TipoValidacao').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/FormularioValidacaoUC/GetTipoValidacaoCurricular",
            datatype: 'json',
            type: 'POST',

            params: {
                contentType: 'application/json; charset=utf-8'
            },
            quietMillis: 100,
            data: function (params) {
                return {
                    searchTerm: params.term
                };
            },

            processResults: function (data, params) {
                return {
                    results: data
                }
            }
        },


    });

    $('#Select2_Curso').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/Cursos/GetCursos",
            datatype: 'json',
            type: 'POST',

            params: {
                contentType: 'application/json; charset=utf-8'
            },
            quietMillis: 100,
            data: function (params) {
                return {
                    searchTerm: params.term
                };
            },

            processResults: function (data, params) {
                return {
                    results: data
                }
            }
        },


    });

    $('#Select2_UnidadeCurricular').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/UnidadeCurricular/GetUnidadeCurricularFilterCurso",
            datatype: 'json',
            type: 'POST',

            params: {
                contentType: 'application/json; charset=utf-8'
            },
            quietMillis: 100,
            data: function (params) {
                return {
                    searchTerm: params.term,
                    cursoId: $("#Select2_Curso").val()
                };
            },

            processResults: function (data, params) {
                return {
                    results: data
                }
            }
        },


    });

});


$(document).on('click', '#Botao_Salvar', function () {
    console.log("Botao");
    LimparAlertas();
    var tipoRequerimento = $("#Select2_TipoRequerimento").val();
    var tipoValidacao = $("#Select2_TipoValidacao").val();
    var unidadeCurricular = $("#Select2_UnidadeCurricular").val();
    var motivo = $("#Motivo").val();
    var msg = "";

    var entidade = {
        TipoRequerimentoId: tipoRequerimento,
        TipoValidacaoCurricularId: tipoValidacao,
        UnidadeCurricularId: unidadeCurricular,
        Motivo: motivo
    };
    $.ajax({
        type: "POST",
        url: "/FormularioValidacaoUC/ConsisteNovoAtualiza/",
        data: JSON.stringify(entidade),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            console.log("Resposta");
            var consistencia = JSON.parse(resposta);
            if (consistencia.InconsistenciasToString != "") {
                msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
                console.log("RespostaIf1" + msg);
            } else if (consistencia.AdvertenciasToString != "") {
                msg += consistencia.AdvertenciasToString.replaceAll("|", "<br>")
                msg += ("Confirma Inclusão/Alteração do Registro " + $("#UnidadeCurricular_id").val() + "?");
                addNotification(msg, 2, "ConfirmarSalvar", "buttonCancelar");
                $(".blockConfirmation").prop('disabled', true);
                console.log("RespostaIf2");
            } else {
                ConfirmarSalvar();
                console.log("ConsisteOK");
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });

});

function ConfirmarExclusao() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroUnidadeCurricular");
    var idEntidade = $("#UnidadeCurricular_id");
    idEntidade.attr("disabled", false);
    formulario.attr("action", "/UnidadeCurricular/Excluir");
    formulario.submit();
    LimparAlertas();
}

function ConfirmarSalvar() {
    console.log("ConfirmaSalvarInicio")
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    LimparAlertas();

    var tipoRequerimento = $("#Select2_TipoRequerimento").val();
    var tipoValidacao = $("#Select2_TipoValidacao").val();
    var unidadeCurricular = $("#Select2_UnidadeCurricular").val();
    var motivo = $("#Motivo").val();

    var entidade = {
        TipoRequerimentoId: tipoRequerimento,
        TipoValidacaoCurricularId: tipoValidacao,
        UnidadeCurricularId: unidadeCurricular,
        Motivo: motivo
    };
    $.ajax({
        type: "POST",
        url: "/FormularioValidacaoUC/Salvar/",
        data: JSON.stringify(entidade),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            $("#Formulario").html(resposta);
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });
    console.log("ConfirmaSalvarFim")
}

function buttonCancelar() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    LimparAlertas();
}

function DesabilitarId() {
    $("#UnidadeCurricular_id").prop('disabled', true);
}


function addComandosTabela() {

    var headerTabela = $(".bootgrid-header");

    //headerTabela.removeClass("row");
    headerTabela.addClass("row justify-content-between")

    headerTabela.prepend('<div id="Comandos" class="actions btn-group"><div class="row container-fluid"></div></div>')

    //var select2ProximoStatus = '<div class="form-group col-md-3"> ' +
    //                            '  <div class="form-group col-md-12"> ' +
    //                            '      <label for="ProximoStatus">Proximo Status</label> ' +
    //                            '      <select class="js-example-basic-single select2" id="Select2_ProximoStatus" name="fluxoStatus.StatusProximoId"> ' +
    //                            '       </select> ' +
    //                            '  </div> ' +
    //    '</div> ';

    var select2ProximoStatus = '<div class="form-group col-md-12"><select class="js-example-basic-single select2 blockConfirmationGrid" id="Select2_ProximoStatus" name="fluxoStatus.StatusProximoId"></select>';


    var comandos = $("#Comandos")
    comandos.html(select2ProximoStatus +
        '<input style="margin: 8px 8px 8px 8px;" id="AdicionarStatusProximo" class="btn bg-success botao_cor btn-sm blockConfirmation blockConfirmationGrid" type="button" value="Adicionar Próximo"></div>');



    $('#Select2_ProximoStatus').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/FluxoStatus/GetStatusAtual",
            datatype: 'json',
            type: 'POST',

            params: {
                contentType: 'application/json; charset=utf-8'
            },
            quietMillis: 100,
            data: function (params) {
                return {
                    searchTerm: params.term
                };
            },

            processResults: function (data, params) {
                return {
                    results: data
                }
            }
        },


    });
}
