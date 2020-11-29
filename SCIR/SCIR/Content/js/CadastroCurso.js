$(document).ready(function () {

    $("#Botao_Salvar").click(function () {
        $("#CadastroCurso").submit();
    });

    if ($("#Curso_id").val() == 0) {
        $("#Botao_Excluir_Cadastro").hide();

    }

    $("#Botao_Excluir_Cadastro").click(function () {
        if (confirm("Confirma Exclusão do Registro " + $("#Curso_id").val() + "?")) {
            var formulario = $("#CadastroCurso");
            var CursoId = $("#Curso_id");
            CursoId.attr("disabled", false);
            formulario.attr("action", "/Cursos/Excluir");
            formulario.submit();
        }
    });

    $("#Botao_Salvar").click(function () {
        var formulario = $("#CadastroCurso");
        var CursoId = $("#Curso_id");
        CursoId.attr("disabled", false);
        formulario.submit()

    });


});






