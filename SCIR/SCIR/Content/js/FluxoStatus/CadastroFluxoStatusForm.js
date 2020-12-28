$(document).ready(function () {
    GridInit();

    if ($("#Select2_StatusAtual").val() == 0) {
        $("#Botao_Excluir_Cadastro").hide();
    }

    $("#Botao_Excluir_Cadastro").click(function () {
        LimparAlertas();
        var id = $("#Select2_StatusAtual").val();
        var nome = $("#Nome").val();
        var ativo = $("#Ativo").is(':checked');
        var cancelamento = $("#Cancelamento").is(':checked');
        var msg = "";
        var entidade = {
            Id: id,
            Nome: nome,
            Ativo: ativo,
            Cancelamento: cancelamento
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
                    msg += ("Confirma Exclusão do Registro " + $("#Select2_StatusAtual").val() + "?");
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

    $("#Botao_Salvar").click(function (e) {
        LimparAlertas();
        e.preventDefault();
        var id = $("#Select2_StatusAtual").val();
        var nome = $("#Nome").val();
        var ativo = $("#Ativo").is(':checked');
        var cancelamento = $("#Cancelamento").is(':checked');
        var msg = "";
        var entidade = {
            Id: id,
            Nome: nome,
            Ativo: ativo,
            Cancelamento: cancelamento
        };
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
    
});

function ConfirmarExclusao() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroFluxoStatus");
    var idEntidade = $("#Select2_StatusAtual");
    idEntidade.attr("disabled", false);
    formulario.attr("action", "/FluxoStatus/Excluir");
    formulario.submit();
    LimparAlertas();
}

function ConfirmarSalvar() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroFluxoStatus");
    var idEntidade = $("#Select2_StatusAtual");
    idEntidade.attr("disabled", false);
    formulario.submit();
    LimparAlertas()

}

function buttonCancelar() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    LimparAlertas();
}

function DesabilitarId() {
    $("#Select2_StatusAtual").prop('disabled', true);
}


function GridDelete(id) {
    LimparAlertas();
    var msg = "";
    var entidade = {
        Id: id
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
                msg += ("Confirma Exclusão do Registro " + id + "?");
                addNotification(msg, 2, "ConfirmarExclusao", "LimparAlertas", [id]);
                $(".blockConfirmation").prop('disabled', true);
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });

};

function GridEdit(id) {
    window.location.href = "/FluxoStatus/Form?id=" + id
};

function ConfirmarExclusao(id) {
    LimparAlertas();
    var msg = "";
    var entidade = {
        Id: id
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

    var select2ProximoStatus = '<div class="form-group col-md-12"><select class="js-example-basic-single select2" id="Select2_ProximoStatus" name="fluxoStatus.StatusProximoId"></select>';

    
    var comandos = $("#Comandos")
    comandos.html(select2ProximoStatus +
                  '<input style="margin: 8px 8px 8px 8px;" id="AdicionarStatusProximo" class="btn bg-success botao_cor btn-sm blockConfirmation" type="button" value="Adicionar Próximo"></div>');



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


$(document).on('click', '#AdicionarStatusProximo', function () {
    var statusAtual = $("#Select2_StatusAtual").val();
    var tipoRequerimento = $("#Select2_TipoRequerimento").val();

    var entidade = {
        StatusAtualId: statusAtual,
        TipoRequerimentoId: tipoRequerimento
    };

    $("#ModalProximoStatus").load("/FluxoStatus/AdicionarProximoStatus/", {StatusAtualId: statusAtual, TipoRequerimentoId: tipoRequerimento }, function () {
        $("#ModalEditar").modal("show");
    });
});

