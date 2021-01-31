$(document).ready(function () {
    commandoEspecif = true;
    GridInit();
});


function GridDelete(id) {
    LimparAlertas();
    var msg = "";
    var entidade = {
        Id: id
    };
    $.ajax({
        type: "POST",
        url: "/UnidadeCurricular/ConsisteExcluir/",
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
                msg += ("Confirma Exclusão do Registro " + id + "?");
                addNotification(msg, 2, "ConfirmarExclusao", "LimparAlertas", [id]);
                $(".blockConfirmation").prop('disabled', true);
            }
        },
        error: function (json) {
            alert("Erro de conexão com o servidor!");
            Console.log(json);
        }
    });

};

function GridEdit(id) {
    window.location.href = "/UnidadeCurricular/Form?id=" + id
};

function ConfirmarExclusao(id) {
    LimparAlertas();
    var msg = "";
    var entidade = {
        Id: id
    };
    $.ajax({
        type: "POST",
        url: "/UnidadeCurricular/ExcluirAjax/",
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
}


function SetCommandoEspecifGrid(column, row) {
    var divInicio = ' <div>';

    var iconVisualizar = '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye" viewBox="0 0 16 16"> ' +
        '<path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8zM1.173 8a13.133 13.133 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13.133 13.133 0 0 1 14.828 8c-.058.087-.122.183-.195.288-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5c-2.12 0-3.879-1.168-5.168-2.457A13.134 13.134 0 0 1 1.172 8z" /> ' +
        '<path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5zM4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0z" /> ' +
        '</svg>';

    var buttonVisualizar = '<span style="margin: 0px 0px 0px 8px;" class="text-primary ponteiro" id="Botao_Visualizar" onclick="GridVisualizar(' + row.Id + ')">' + iconVisualizar + '</span>';


    var divFinal = '</div>';

    return divInicio + buttonVisualizar + divFinal
}


function GridVisualizar(id) {
    window.location.href = "/Requerimento/VisualizarRequerimento?Id=" + id
};
