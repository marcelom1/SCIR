$(document).ready(function () {
    commandoEspecif = true;
    GridInit();
    

    if ($("#Select2_StatusAtual").val() == null || $("#Select2_TipoRequerimento").val() == null) {
        $("#Botao_Excluir_Cadastro").hide();
    }

    $("#Botao_Excluir_Cadastro").click(function () {
        LimparAlertas();
        var statusAtualId = $("#Select2_StatusAtual").val();
        var tipoRequerimentoId = $("#Select2_TipoRequerimento").val();
       
        var msg = "";
        var entidade = {
            StatusAtualId: statusAtualId,
            TipoRequerimentoId: tipoRequerimentoId
        };
        $.ajax({
            type: "POST",
            url: "/FluxoStatus/ConsisteExcluirTodosProximos/",
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
                    msg += ("Confirma Exclusão de todos os Registros filhos desse fluxo Status: " + statusAtualId + ", Tipo Requerimento: " + tipoRequerimentoId + "?");
                    addNotification(msg, 2, "ConfirmarExclusaoButton", "buttonCancelar");
                    $(".blockConfirmation").prop('disabled', true);
                }
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });

    });

   

    $('#Select2_StatusAtual').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,
        minimumInputLength: 2,

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

    $('#Select2_TipoRequerimento').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,
        minimumInputLength: 2,

        ajax: {
            url: "/FluxoStatus/GetTipoRequerimento",
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

    addComandosTabela();
    AtualizaGrid()
});

function ConfirmarExclusaoButton() {
    $(".blockConfirmation").prop('disabled', false);
    var formulario = $("#CadastroFluxoStatus");
    formulario.attr("action", "/FluxoStatus/Excluir");
    formulario.submit();
    LimparAlertas();
}

function ConfirmarSalvar() {
    $(".blockConfirmation").prop('disabled', false);
    var formulario = $("#CadastroFluxoStatus");
    var idEntidade = $("#Select2_StatusAtual");
    idEntidade.attr("disabled", false);
    formulario.submit();
    LimparAlertas()

}

function buttonCancelar() {
    $(".blockConfirmation").prop('disabled', false);
    LimparAlertas();
}


function GridDelete(statusProximo) {
    LimparAlertas();
    var statusAtual = $("#Select2_StatusAtual").val();
    var tipoRequerimento = $("#Select2_TipoRequerimento").val()
    var msg = "";
    var entidade = {
        StatusAtualId: statusAtual,
        StatusProximoId: statusProximo,
        TipoRequerimentoId: tipoRequerimento
    };
    $.ajax({
        type: "POST",
        url: "/FluxoStatus/ConsisteExcluir/",
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
                msg += ("Confirma Exclusão do Registro " + statusProximo + "?");
                addNotification(msg, 2, "ConfirmarExclusao", "LimparAlertas", [statusAtual, statusProximo, tipoRequerimento]);
                $(".blockConfirmation").prop('disabled', true);
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });

};


function ConfirmarExclusao(statusAtual, statusProximo, tipoRequerimento) {
    LimparAlertas();
    $(".blockConfirmation").prop('disabled', false);
    var msg = "";
    var entidade = {
        StatusAtualId: statusAtual,
        StatusProximoId: statusProximo,
        TipoRequerimentoId: tipoRequerimento
    };
    $.ajax({
        type: "POST",
        url: "/FluxoStatus/ExcluirAjax/",
        data: JSON.stringify(entidade),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            var consistencia = JSON.parse(resposta);
            if (consistencia.InconsistenciasToString != "") {
                msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
            } else if (consistencia.SucessoToString != "") {
                $("#grid-basic").bootgrid("reload");
                msg += consistencia.SucessoToString.replaceAll("|", "<br>")
                addNotification(msg, 3);
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });
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
        minimumInputLength: 2,

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


$(document).on('click', '#AdicionarStatusProximo', function (e) {
    LimparAlertas();
    e.preventDefault();

    var statusAtual = $("#Select2_StatusAtual").val();
    var tipoRequerimento = $("#Select2_TipoRequerimento").val();
    var proximoStatus = $("#Select2_ProximoStatus").val();

    var entidade = {
        StatusAtualId: statusAtual,
        StatusProximoId: proximoStatus,
        TipoRequerimentoId: tipoRequerimento
    };

    //$("#ModalProximoStatus").load("/FluxoStatus/AdicionarProximoStatus/", {StatusAtualId: statusAtual, TipoRequerimentoId: tipoRequerimento }, function () {
    //    $("#ModalEditar").modal("show");
    //});

    var msg = "";
    
    $.ajax({
        type: "POST",
        url: "/FluxoStatus/ConsisteNovoAtualiza/",
        data: JSON.stringify(entidade),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            var consistencia = JSON.parse(resposta);
            if (consistencia.InconsistenciasToString != "") {
                msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
            } else if (consistencia.AdvertenciasToString != "") {
                msg += consistencia.AdvertenciasToString.replaceAll("|", "<br>")
                msg += ("Confirma Inclusão/Alteração do Registro " + $("#Select2_StatusAtual").val() + "?");
                addNotification(msg, 2, "ConfirmarSalvar", "buttonCancelar");
                $(".blockConfirmation").prop('disabled', true);
            } else {
                ConfirmarSalvar();
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });



});

$("#Select2_StatusAtual").on('select2:close', function () {
    AtualizaGrid();
});

$("#Select2_TipoRequerimento").on('select2:close', function () {
    AtualizaGrid();
});

function AtualizaGrid() {
    var statusAtual = $("#Select2_StatusAtual").val();
    var tipoRequerimento = $("#Select2_TipoRequerimento").val()

    $("#grid-basic").bootgrid("reload");
    if (statusAtual != null && tipoRequerimento != null) {
        $(".blockConfirmationGrid").prop('disabled', false);
        $("#Botao_Excluir_Cadastro").show();
    } else { 
        $(".blockConfirmationGrid").prop('disabled', true);
        $("#Botao_Excluir_Cadastro").hide();
    }

};


function SetCommandoEspecifGrid(column, row) {
    var divInicio = ' <div>';

    var iconDelete = ' <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-trash-fill" fill="currentColor" xmlns="http://www.w3.org/2000/svg">' +
        '    <path fill-rule="evenodd" d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z" />' +
        '</svg>';

    

    var buttonDelete = '<span style="margin: 0px 0px 0px 8px;" class="text-danger ponteiro" id="Botao_Excluir" onclick="GridDelete('+ row.StatusProximoId + ')">' + iconDelete + '</span>';

    var divFinal = '</div>';

    return divInicio + buttonDelete  + divFinal
};