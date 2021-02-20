$(document).ready(function () {
    $("#PEModalEnviar").click(function (e) {
        e.preventDefault();
        var entidade = {
            email: $("#emailModal").val()
        };
        $.ajax({
            type: "POST",
            url: "/Login/EnviarPESenha/",
            data: JSON.stringify(entidade),
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (resposta) {
                console.log(resposta);
                
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });


    });
});