﻿$(document).ready(function () {
    GridInit();
});

function GridDelete(id) {
    LimparAlertas();
    var msg = "";
    var curso = {
        Id: id
    };
    $.ajax({
        type: "POST",
        url: "/Cursos/ConsisteExcluir/",
        data: JSON.stringify(curso),
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
                addNotification(msg, 2, "ConfirmarExclusao", "LimparAlertas",[id]);
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
    window.location.href = "/Cursos/Form?id=" + id
};

function ConfirmarExclusao(id) {
    LimparAlertas();
    var msg = "";
    var curso = {
        Id: id
    };
    $.ajax({
        type: "POST",
        url: "/Cursos/ExcluirAjax/",
        data: JSON.stringify(curso),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            var consistencia = JSON.parse(resposta);
            if (consistencia.InconsistenciasToString != "") {
                msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                addNotification(msg, 1);
            } else if (consistencia.SucessoToString != ""){
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