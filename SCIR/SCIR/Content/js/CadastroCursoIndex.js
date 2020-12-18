$(document).ready(function () {

    $("#Botao_Excluir").click(function (e) {
        e.stopPropagation();
        $("#grid-basic").bootgrid().on("click.rs.jquery.bootgrid", function (e, columns, rows) {
            console.log(rows.Id);
            window.location.href = "#";
        });
    });

    GridInit();
});

