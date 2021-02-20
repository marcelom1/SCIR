
$("#SalvarNovaSenha").click(function () {
    var SenhaAtual = $("#SenhaAtual").val();
    var NovaSenha = $("#NovaSenha").val();
    var ConfirmaSenha = $("#ConfirmaSenha").val();
    $("#MsgServidor").text("");
    ValidaSenhaEmBranco(SenhaAtual, NovaSenha, ConfirmaSenha);

});

function ValidaSenhaEmBranco(SenhaAtual, NovaSenha, ConfirmaSenha) {
    if ((NovaSenha.split(/\s+/).length > 1) || (NovaSenha == "")) {
        $(".Erro_ConfirmaSenha").text("Campo senha não pode conter espaços ou ficar em branco!").show();
        return true;
    } else {
        $(".Erro_ConfirmaSenha").hide();
        ValidaSenhaConfirmação(SenhaAtual, NovaSenha, ConfirmaSenha)
        return false;
    }
};

function ValidaSenhaConfirmação(SenhaAtual, NovaSenha, ConfirmaSenha) {

    if (NovaSenha != ConfirmaSenha) {
        $(".Erro_ConfirmaSenha").text("As senhas não coincidem!").show()
        return true;
    } else {
        $("#Erro_ConfirmaSenha").hide();
        EnviarSolicitacao(SenhaAtual, NovaSenha, ConfirmaSenha)
        return false;
    }
}


function EnviarSolicitacao(SenhaAtual, NovaSenha, ConfirmaSenha) {
    $.ajax({
        type: "POST",
        url: "/Login/AlterarSenhaUsuario/",
        data: JSON.stringify({ senhaAtual: SenhaAtual, novaSenha: NovaSenha, confirmacaoSenha: ConfirmaSenha }),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (temp) {
            var resposta = JSON.parse(temp);
            if (resposta == "Senha alterada com sucesso!") {
                $("#MsgServidor").removeClass("erro").addClass("sucess");
                $("#MsgServidor").text(resposta);
                $("#ModalSenha").html($("#MsgServidor"));

            }
            $("#MsgServidor").text(resposta);
            console.log(resposta)

        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");

            Console.log(json);
        }
    });
};
