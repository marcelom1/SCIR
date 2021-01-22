var formData = new FormData();
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

    IniciarGridAnexo();
    var keyArquivo = 0;
    $("#fileUpload").on("change", function () {
        var formDataTemp = new FormData();
        var j = 0;
        for (var pair of formData.entries()) {
            j++;
            formDataTemp.append(j, pair[1])
        }

        var files = $(this).get(0).files;
        for (var i = 0; i < files.length; i++) {
            j++;
            formDataTemp.append(j, files[i]);
        }
        formData = formDataTemp;
        AtualizarGridArquivo();
       
    })

});

function AtualizarGridArquivo() {
    for (var pair of formData.entries()) {
        var JSONINFO = { "rows": [{ "Sequencia": pair[0], "Nome": pair[1].name }] };   
        $("#grid-basic").bootgrid("append", JSONINFO.rows);
    }   
}

function RemoverArquivo(posicao) {
    formData.delete(posicao)
    var formDataTemp = new FormData();
    var j = 0;
    for (var pair of formData.entries()) {
        j++;
        formDataTemp.append(j, pair[1])
    }
    formData = formDataTemp;
    $("#grid-basic").bootgrid("clear");
    AtualizarGridArquivo()
}


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
        Motivo: motivo,
        Arquivo: formData
    };

    formData.append("TipoRequerimentoId", tipoRequerimento)
    formData.append("TipoValidacaoCurricularId", tipoValidacao)
    formData.append("UnidadeCurricularId", unidadeCurricular)
    formData.append("Motivo", motivo)
  
    $.ajax({
        type: "POST",
        url: "/FormularioValidacaoUC/Salvar/",
        data: formData,
        processData: false,
        contentType: false,
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

    headerTabela.addClass("row justify-content-between")

    headerTabela.prepend('<div id="Comandos" class="actions btn-group"><div class="row container-fluid"></div></div>')

    var icon = '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-files" viewBox="0 0 16 16"> ' +
                    '<path d="M13 0H6a2 2 0 0 0-2 2 2 2 0 0 0-2 2v10a2 2 0 0 0 2 2h7a2 2 0 0 0 2-2 2 2 0 0 0 2-2V2a2 2 0 0 0-2-2zm0 13V4a2 2 0 0 0-2-2H5a1 1 0 0 1 1-1h7a1 1 0 0 1 1 1v10a1 1 0 0 1-1 1zM3 4a1 1 0 0 1 1-1h7a1 1 0 0 1 1 1v10a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4z" /> ' +
               '</svg> ';

    var comandos = $("#Comandos")

    
    var inputButton = '<input type="file" multiple id="fileUpload" />'
    var inputfile = '<label style="margin: 8px 8px 8px 8px;" for="fileUpload" id="AnexarArquivos" class="btn bg-success botao_cor btn-sm blockConfirmation blockConfirmationGrid">Anexar Arquivo ' + icon + '</label>';


    var divFileContainer = '<div class="file-upload-container">' + inputfile + inputButton + '</div></div>'

    comandos.html(divFileContainer);
}

function IniciarGridAnexo() {
    $("#grid-basic").bootgrid(
        {
            ajax: false,
            labels:
            {
                search: "Pesquisar",
                infos: "Mostrando {{ctx.start}} a {{ctx.end}} de {{ctx.total}} resultados",
                all: "Tudo",
                loading: "Carregando...",
                noResults: "Nenhum arquivo carregado!",
                refresh: "Atualizar"
            },
            formatters: {
                "commands": function (column, row) {


                    var divInicio = ' <div>';

                    var iconDelete = ' <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-trash-fill" fill="currentColor" xmlns="http://www.w3.org/2000/svg">' +
                        '    <path fill-rule="evenodd" d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z" />' +
                        '</svg>';

                   

                    var buttonDelete = '<span style="margin: 0px 0px 0px 8px;" class="text-danger ponteiro" id="Botao_Excluir" onclick="RemoverArquivo(' + row.Sequencia + ')">' + iconDelete + '</span>';

                    var divFinal = '</div>';

                    return divInicio + buttonDelete +divFinal

                }
            },
            searchSettings: {
                delay: 100,
                characters: 3
            },

        });

    $("#grid-basic-header").removeClass("container-fluid");
    $("#grid-basic").addClass("table-responsive-md");

    addComandosTabela();
}

function GridDelete(id) {
    console.log(id);
}

