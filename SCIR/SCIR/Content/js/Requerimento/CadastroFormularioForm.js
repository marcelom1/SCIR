$(document).ready(function () {

    $('#Select2_TipoRequerimento').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/Requerimento/GetTipoRequerimento",
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

    var requerimentoId = $("#requerimentoId").text();
    if (requerimentoId != 0) {
        $('#Select2_TipoRequerimento').prop('disabled', true);
        CarregarFormulario();
    }

});

var AntesTipoRequerimentoSelect2 = 0;
$("#Select2_TipoRequerimento").on('select2:close', function () {
    CarregarFormulario()
});

function CarregarFormulario() {
    var tipoRequerimentoID = $("#Select2_TipoRequerimento").val();
    var requerimentoID = $("#requerimentoId").text();

    if ((tipoRequerimentoID != AntesTipoRequerimentoSelect2) && (tipoRequerimentoID != null)) {
        $("#Formulario").load("/Requerimento/CarregarFormulario/", { tipoRequerimentoID, requerimentoID }, function () {

        });
        AntesTipoRequerimentoSelect2 = tipoRequerimentoID;
    }

    if (tipoRequerimentoID == null) {
        AntesTipoRequerimentoSelect2 = tipoRequerimentoID;
        $("#Formulario").html("");
    }
}
