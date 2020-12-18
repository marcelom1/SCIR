$(document).ready(function () {

    $("#Botao_Salvar").click(function () {
        $("#CadastroCurso").submit();
    });

    if ($("#Curso_id").val() == 0) {
        $("#Botao_Excluir_Cadastro").hide();
    }

    $("#Botao_Excluir_Cadastro").click(function () {
        var id = $("#Curso_id").val();
        var nome = $("#NomeCurso").val();
        var ativo = $("#Ativo").is(':checked');
        var msg = "";
        var curso = {
            Id:  id,
            Nome: nome,
            Ativo: ativo
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
                    msg += ("Confirma Exclusão do Registro " + $("#Curso_id").val() + "?");
                    addNotification(msg, 2, "ConfirmarExclusao","buttonCancelar");
                    $(".blockConfirmation").prop('disabled', true);
                }
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });
        
    });
    
    $("#Botao_Salvar").click(function () {
        var id = $("#Curso_id").val();
        var nome = $("#NomeCurso").val();
        var ativo = $("#Ativo").is(':checked');
        var msg = "";
        var curso = {
            Id: id,
            Nome: nome,
            Ativo: ativo
        };
        $.ajax({
            type: "POST",
            url: "/Cursos/ConsisteNovoAtualiza/",
            data: JSON.stringify(curso),
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (resposta) {
                var consistencia = JSON.parse(resposta);
                if (consistencia.InconsistenciasToString != "") {
                    msg += consistencia.InconsistenciasToString.replaceAll("|", "<br>")
                    addNotification(msg, 1);
                } else if (consistencia.AdvertenciasToString != ""){
                    msg += consistencia.AdvertenciasToString.replaceAll("|", "<br>")
                    msg += ("Confirma Inclusão/Alteração do Registro " + $("#Curso_id").val() + "?");
                    addNotification(msg, 2, "ConfirmarSalvar", "buttonCancelar");
                    $(".blockConfirmation").prop('disabled', true);
                }
            },
            error: function (json) {
                alert("Erro de conexão com o servidor!");
                Console.log(json);
            }
        });

    });
    
});

function ConfirmarExclusao() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroCurso");
    var CursoId = $("#Curso_id");
    CursoId.attr("disabled", false);
    formulario.attr("action", "/Cursos/Excluir");
    formulario.submit();
    FunctionNaoDefinida();
}

function ConfirmarSalvar() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    var formulario = $("#CadastroCurso");
    var CursoId = $("#Curso_id");
    CursoId.attr("disabled", false);
    formulario.submit();
    FunctionNaoDefinida()

}

function buttonCancelar() {
    $(".blockConfirmation").prop('disabled', false);
    DesabilitarId();
    FunctionNaoDefinida();
}

function DesabilitarId() {
    $("#Curso_id").prop('disabled', true);
}



