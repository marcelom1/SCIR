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
                    searchTerm: params.term
                };
            },

            processResults: function (data, params) {
                return {
                    results: data
                }
            }
        },


    });
    console.log($("#RequerimentoId").val());
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