$(document).ready(function () {

    if ($("#StatusRequerimento_id").val() == 0) {
        $("#Botao_Excluir_Cadastro").hide();
    }

    $("#Botao_Excluir_Cadastro").click(function () {
        LimparAlertas();
        var id = $("#StatusRequerimento_id").val();
        var nome = $("#Nome").val();
        var ativo = $("#Ativo").is(':checked');
        var cancelamento = $("#Cancelamento").is(':checked');
        var codigoInterno = $("#CodigoInterno").val();
        var msg = "";
        var entidade = {
            Id: id,
            Nome: nome,
            Ativo: ativo,
            Cancelamento: cancelamento,
            CodigoInterno: codigoInterno
        };
        $.ajax({
            type: "POST",
            url: "/StatusRequerimento/ConsisteExcluir/",
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
                    msg += ("Confirma Exclusão do Registro " + $("#StatusRequerimento_id").val() + "?");
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
        var id = $("#StatusRequerimento_id").val();
        var nome = $("#Nome").val();
        var ativo = $("#Ativo").is(':checked');
        var cancelamento = $("#Cancelamento").is(':checked');
        var codigoInterno = $("#CodigoInterno").val();
        var msg = "";
        var entidade = {
            Id: id,
            Nome: nome,
            Ativo: ativo,
            Cancelamento: cancelamento,
            CodigoInterno: codigoInterno
        };
        $.ajax({
            type: "POST",
            url: "/StatusRequerimento/ConsisteNovoAtualiza/",
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
                    msg += ("Confirma Inclusão/Alteração do Registro " + $("#StatusRequerimento_id").val() + "?");
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
    
});

function ConfirmarExclusao() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroStatusRequerimento");
    var idEntidade = $("#StatusRequerimento_id");
    idEntidade.attr("disabled", false);
    formulario.attr("action", "/StatusRequerimento/Excluir");
    formulario.submit();
    LimparAlertas();
}

function ConfirmarSalvar() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroStatusRequerimento");
    var idEntidade = $("#StatusRequerimento_id");
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
    $("#StatusRequerimento_id").prop('disabled', true);
}

