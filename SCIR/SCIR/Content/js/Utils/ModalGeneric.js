function ModalAlert(TextoBotao1, TextoBotao2, frase, acaobotao1, acaobotao2, titulo) {
    if (TextoBotao1 == '')
        $('#Botao1').hide();
    if (TextoBotao2 == '')
        $('#Botao2').hide();

    var botao1 = $('#Botao1');
    var botao2 = $('#Botao2');

    botao1.text(TextoBotao1).click(function () {
        $(window.document.location).attr('href', acaobotao1);
    });
    botao2.text(TextoBotao2).click(function () {
        $(window.document.location).attr('href', acaobotao2);
    });

    $('.modal-title').text(titulo);
    if (frase != null)
        $('#conteudoModal').html(frase);



    $("#ModalAlert").modal();

};

function ModalAlertFechar() {
    $("#ModalAlert").modal('hide');
};