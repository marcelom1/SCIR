$(document).ready(function () {

    $('#Select2_Atendente').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/Requerimento/GetProximoAtendente",
            datatype: 'json',
            type: 'POST',

            params: {
                contentType: 'application/json; charset=utf-8'
            },
            quietMillis: 100,
            data: function (params) {
                return {
                    searchTerm: params.term,
                    requerimentoId: $("#RequerimentoId").val()
                };
            },

            processResults: function (data, params) {
                return {
                    results: data
                }
            }
        },


    });
    $('#Select2_Status').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/Requerimento/GetProximoStatus",
            datatype: 'json',
            type: 'POST',

            params: {
                contentType: 'application/json; charset=utf-8'
            },
            quietMillis: 100,
            data: function (params) {
                return {
                    searchTerm: params.term,
                    requerimentoId: $("#RequerimentoId").val()
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

$(document).on('click', '#EncaminharRequerimentoModal', function (e) {
    e.preventDefault();
    EncaminharRequerimento();
});

var urlDirect = "/Requerimento?filtro="
var parFiltro = $("#parFiltro").text();
function EncaminharRequerimento() {
    var msg = "";
    var entidade = {
        Id: $("#RequerimentoId").val(),
        UsuarioAtendenteId: $("#Select2_Atendente").val(),
        StatusRequerimentoId: $("#Select2_Status").val(),
        Mensagem: $("#Mensagem").val()
    };
    $.ajax({
        type: "POST",
        url: "/Requerimento/EncaminharRequerimento/",
        data: JSON.stringify(entidade),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            var consistencia = JSON.parse(resposta);
            console.log(consistencia);
            if (consistencia.Consistencia.SucessoToString != "") {
                msg += consistencia.Consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 3);

                window.location.href = urlDirect + parFiltro;
            }
            
            if (consistencia.Consistencia.InconsistenciasToString != "") {
                msg += consistencia.Consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
            } else {
                msg += consistencia.Consistencia.AdvertenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
            }

           
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });
    $('#ModalAlert').modal('hide');
};