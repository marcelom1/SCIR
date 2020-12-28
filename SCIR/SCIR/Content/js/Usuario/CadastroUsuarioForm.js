﻿$(document).ready(function () {

    if ($("#Usuario_id").val() == 0) {
        $("#Botao_Excluir_Cadastro").hide();
    }

    $("#Botao_Excluir_Cadastro").click(function () {
        LimparAlertas();
        var id = $("#Usuario_id").val();
        var nome = $("#Nome").val();
        var papel = $("#Papel").val();
        var ativo = $("#Ativo").is(':checked');
        var msg = "";
        var entidade = {
            Id: id,
            Nome: nome,
            Papel: papel,
            Ativo: ativo
        };
        $.ajax({
            type: "POST",
            url: "/Usuario/ConsisteExcluir/",
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
                    msg += ("Confirma Exclusão do Registro " + $("#Usuario_id").val() + "?");
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
        var id = $("#Usuario_id").val();
        var nome = $("#Nome").val();
        var papel = $("#Papel").val();
        var ativo = $("#Ativo").is(':checked');
        var msg = "";
        var entidade = {
            Id: id,
            Nome: nome,
            Papel: papel,
            Ativo: ativo
        };
        $.ajax({
            type: "POST",
            url: "/Usuario/ConsisteNovoAtualiza/",
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
                    msg += ("Confirma Inclusão/Alteração do Registro " + $("#Usuario_id").val() + "?");
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

    $('#Select2_Papel').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,
        minimumInputLength: 2,

        ajax: {
            url: "/Usuario/GetPapel",
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


});

function ConfirmarExclusao() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroUsuario");
    var idEntidade = $("#Usuario_id");
    idEntidade.attr("disabled", false);
    formulario.attr("action", "/Usuario/Excluir");
    formulario.submit();
    LimparAlertas();
}

function ConfirmarSalvar() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroUsuario");
    var idEntidade = $("#Usuario_id");
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
    $("#Usuario_id").prop('disabled', true);
}
