$(document).ready(function () {
    $("#PEModalEnviar").click(function (e) {
        e.preventDefault();
        $(".blockConfirmation").prop('disabled', true);
        var form = $("#formulario");

        var form_data = new FormData(form[0]);

        $.ajax({
            type: "POST",
            url: "/Login/EnviarPESenha/",
            processData: false,
            contentType: false,
            data: form_data,
            dataType: "html",
            success: function (resposta) {
                $("#conteudoModal").html(resposta);
                
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });


    });
});