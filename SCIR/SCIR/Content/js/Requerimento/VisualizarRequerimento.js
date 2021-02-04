$(document).ready(function () {
    var requerimentoID = $("#Id").val();
    $("#AncoraCamposExtras").load("/Requerimento/CarregarRequerimentoCamposExtras/", { requerimentoID }, function () {

    });
    commandoEspecif = true;
    rowCountEspecif = true;
    GridInit();
    $("#Encaminhar").click(function (e) {
        e.preventDefault();
        var entidade = {
            requerimentoId: $("#Id").val(),
            chamadoOrigem: $("#Origem").text()
        };
        console.log(entidade);
        $.ajax({
            type: "POST",
            url: "/Requerimento/ModalEncaminhar/",
            data: JSON.stringify(entidade),
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (resposta) {
                ModalAlert("", "", resposta,"","","Encaminhar requerimento")
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });

    });

});


function DesabilitarId() {
    $("#UnidadeCurricular_id").prop('disabled', true);
}


function addComandosTabela() {

    var headerTabela = $(".bootgrid-header");

    //headerTabela.removeClass("row");
    headerTabela.addClass("row justify-content-between")

    headerTabela.prepend('<div id="Comandos" class="actions btn-group"><div class="row container-fluid"></div></div>')

    //var select2ProximoStatus = '<div class="form-group col-md-3"> ' +
    //                            '  <div class="form-group col-md-12"> ' +
    //                            '      <label for="ProximoStatus">Proximo Status</label> ' +
    //                            '      <select class="js-example-basic-single select2" id="Select2_ProximoStatus" name="fluxoStatus.StatusProximoId"> ' +
    //                            '       </select> ' +
    //                            '  </div> ' +
    //    '</div> ';

    var select2ProximoStatus = '<div class="form-group col-md-12"><select class="js-example-basic-single select2 blockConfirmationGrid" id="Select2_ProximoStatus" name="fluxoStatus.StatusProximoId"></select>';


    var comandos = $("#Comandos")
    comandos.html(select2ProximoStatus +
        '<input style="margin: 8px 8px 8px 8px;" id="AdicionarStatusProximo" class="btn bg-success botao_cor btn-sm blockConfirmation blockConfirmationGrid" type="button" value="Adicionar Próximo"></div>');



    $('#Select2_ProximoStatus').select2({

        language: "pt-BR",
        id: function (e) { return e.Id; },
        placeholder: "",
        allowClear: true,

        ajax: {
            url: "/FluxoStatus/GetStatusAtual",
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
}


function SetRowCountEspecifGrid() {
    return [5, 10, 25, 50, -1];
}

function SetCommandoEspecifGrid(column, row) {
    var divInicio = ' <div>';

    var iconDownload = '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-download" viewBox="0 0 16 16"> ' +
        '<path d="M.5 9.9a.5.5 0 0 1 .5.5v2.5a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1v-2.5a.5.5 0 0 1 1 0v2.5a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2v-2.5a.5.5 0 0 1 .5-.5z" /> ' +
        ' <path d="M7.646 11.854a.5.5 0 0 0 .708 0l3-3a.5.5 0 0 0-.708-.708L8.5 10.293V1.5a.5.5 0 0 0-1 0v8.793L5.354 8.146a.5.5 0 1 0-.708.708l3 3z" />' +
        ' </svg>';

    var buttonDownload = '<span style="margin: 0px 0px 0px 8px;" class="text-primary ponteiro" id="Botao_Visualizar" onclick="GridDownload(' + row.Id + ')">' + iconDownload + '</span>';


    var divFinal = '</div>';

    return divInicio + buttonDownload + divFinal
}

function GridDownload(id) {
    var idRequerimento = $("#Id").val();
    $("#iframe").attr("src", "/Requerimento/Download?file=" + id + "&" + "requerimentoId=" + idRequerimento);
};
