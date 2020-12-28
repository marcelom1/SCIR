$(document).ready(function () {
    commandoEspecif = true;
    GridInit();
});


function GridDelete(statusAtualId, tipoRequerimentoId) {
    LimparAlertas();
    var msg = "";
    var entidade = {
        StatusAtualId: statusAtualId,
        TipoRequerimentoId: tipoRequerimentoId
    };
    $.ajax({
        type: "POST",
        url: "/FluxoStatus/ConsisteExcluirTodosProximos/",
        data: JSON.stringify(entidade),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            var consistencia = JSON.parse(resposta);
            if (consistencia.InconsistenciasToString != "") {
                msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
            } else {
                msg += consistencia.AdvertenciasToString.replaceAll("|", "<br>")
                msg += ("Confirma Exclusão de todos os Registros filhos desse fluxo Status: " + statusAtualId + ", Tipo Requerimento: " + tipoRequerimentoId+"?");
                addNotification(msg, 2, "ConfirmarExclusao", "LimparAlertas", [statusAtualId, tipoRequerimentoId]);
                $(".blockConfirmation").prop('disabled', true);
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });

};

function GridEdit(statusAtualId, tipoRequerimentoId) {
    window.location.href = "/FluxoStatus/Form?statusAtual=" + statusAtualId + "&tipoRequerimento=" + tipoRequerimentoId
};

function ConfirmarExclusao(statusAtualId, tipoRequerimentoId) {
    LimparAlertas();
    var msg = "";
    var entidade = {
        StatusAtualId: statusAtualId,
        TipoRequerimentoId: tipoRequerimentoId
    };
    $.ajax({
        type: "POST",
        url: "/FluxoStatus/ExcluirAjaxTodosProximos/",
        data: JSON.stringify(entidade),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            var consistencia = JSON.parse(resposta);
            if (consistencia.InconsistenciasToString != "") {
                msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
            } else if (consistencia.SucessoToString != "") {
                $("#grid-basic").bootgrid("reload");
                msg += consistencia.SucessoToString.replaceAll("|", "<br>")
                addNotification(msg, 3);
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });
};

function SetCommandoEspecifGrid(column, row) {
    var divInicio = ' <div>';

    var iconDelete = ' <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-trash-fill" fill="currentColor" xmlns="http://www.w3.org/2000/svg">' +
        '    <path fill-rule="evenodd" d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z" />' +
        '</svg>';


    var buttonDelete = '<span style="margin: 0px 0px 0px 8px;" class="text-danger ponteiro" id="Botao_Excluir" onclick="GridDelete(' + row.StatusAtualId + ',' + row.TipoRequerimentoId+')">' + iconDelete + '</span>';


    var iconEdit = '<svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-pencil-fill" fill="currentColor" xmlns="http://www.w3.org/2000/svg">' +
        '     <path fill-rule="evenodd" d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708l-3-3zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207l6.5-6.5zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.499.499 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11l.178-.178z" />' +
        ' </svg>'


    var buttonEdit = '<span style="margin: 0px 0px 0px 8px;" class="text-primary ponteiro" onclick="GridEdit(' + row.StatusAtualId + ',' + row.TipoRequerimentoId +')">' + iconEdit + '</span>';

    var divFinal = '</div>';

    return divInicio + buttonDelete + buttonEdit + divFinal
}