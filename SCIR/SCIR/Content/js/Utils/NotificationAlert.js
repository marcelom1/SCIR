function addNotification(mensagem, tipo, ClickConfirmation = "LimparAlertas", ClickCancel = "LimparAlertas",  ParameterConfirmation = [], ParameterCancel = []) {

    if (tipo == 1) {
        $("#alert").append('<div class="form-control-feedback glyphicon alert alert-danger" role="alert" id="alertConfirmation">' + mensagem + '</div>');
    } else if (tipo == 2) {
        $("#alert").append('<div class="form-control-feedback glyphicon alert alert-warning" role="alert" id="alertConfirmation">' + mensagem +
                            '<p>'+                    
            '<button onclick="' + ClickConfirmation + '(' + TratarParametrosArrayToString(ParameterConfirmation)+');" type="button" class="m-1 btn btn-primary btn-sm">Confirmar</button>' + 
            '<button onclick="' + ClickCancel + '(' + TratarParametrosArrayToString(ParameterCancel) +');" type="button" class="m-1 btn btn-secondary btn-sm">Cancelar</button>' +
                            '</p>'+
                            '</div > ');
    } else if (tipo == 3) {
        $("#alert").append('<div class="form-control-feedback glyphicon alert alert-success" role="alert" id="alertConfirmation">' + mensagem + '</div>');
    }
}

function LimparAlertas() {
    $("#alertConfirmation").remove();
}

function VerificarConsistencia(mensagem, tipo, ClickConfirmation, ClickCancel) {
    if (mensagem != "") {
        var msg = TratarMsgConsistencia(mensagem);
        addNotification(msg, tipo,ClickConfirmation , ClickCancel)
    }
}

function TratarMsgConsistencia(mensagem) {
    return mensagem.replaceAll("|", "<br>")
}

function TratarParametrosArrayToString(ArrayParametros) {
    var response = "";
    $.each(ArrayParametros, function (index, value) {
        if (response == ""){
            response = value;
        } else {
            response += "," + value;
        } 
    });

    return response;

}