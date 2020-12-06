function addNotification(mensagem, tipo, ClickConfirmation = "FunctionNaoDefinida", ClickCancel = "FunctionNaoDefinida") {
    if (tipo == 1) {
        $("#alert").append('<div class="form-control-feedback glyphicon alert alert-danger" role="alert" id="alertConfirmation">' + mensagem + '</div>');
    } else if (tipo == 2) {
        $("#alert").append('<div class="form-control-feedback glyphicon alert alert-primary" role="alert" id="alertConfirmation">' + mensagem +
                            '<p>'+                    
                                '<button onclick="' + ClickConfirmation+'();" type="button" class="m-1 btn btn-primary btn-sm">Confirmar</button>' + 
                                '<button onclick="' + ClickCancel+'();" type="button" class="m-1 btn btn-secondary btn-sm">Cancelar</button>' +
                            '</p>'+
                            '</div > ');
                    }
}

function FunctionNaoDefinida() {
    $("#alertConfirmation").remove();
}