function addNotification(mensagem, tipo) {
    if (tipo == 1) {
        $("#alert").append('<div class="form-control-feedback glyphicon alert alert-danger" role="alert" id="erro">' + mensagem + '</div>');
    } else if (tipo == 2) {
        $("#alert").append('<div class="form-control-feedback glyphicon alert alert-primary" role="alert" id="confirmation">' + mensagem +
                            '<p>'+                    
                                '<button type="button" class="m-1 btn btn-primary btn-sm">Primary</button>' + 
                                '<button type="button" class="m-1 btn btn-secondary btn-sm">Secondary</button>' +
                            '</p>'+
                            '</div > ');
                    }
}