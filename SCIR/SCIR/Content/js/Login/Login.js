$(document).ready(function () {
    $("#PESenha").click(function (e) {
        e.preventDefault();
        
        $.ajax({
            type: "POST",
            url: "/Login/PrimeiroAcessoEsqueciSenha/",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (resposta) {
                ModalAlert("", "", resposta, "", "", "Conta")
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });
       

    });

    $("#AlterarSenha").click(function (e) {
        e.preventDefault();

        $.ajax({
            type: "POST",
            url: "/Login/AlterarSenha/",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (resposta) {
                ModalAlert("", "", resposta, "", "", "Alterar Senha")
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });


    });
});