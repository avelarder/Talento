
$(document).ready(function () {
    updatelist();
    $(".btn-modal").on("click", function () {
        $("#Position_Id").val(($(this).attr("name")).split("-")[1]);
    });

    $("#Files").on("change", function () {
        addFile();
    });
    $("#fileListDiv").on("click", ".delete-file", function () {
        deleteFile($(this).attr('data-file'));
    });
});

$('#complete-dialog').on('hidden.bs.modal', function () {
    $("body").removeClass("modal-open");
    $.ajax({
        type: "POST",
        url: '/File/EmptyList',
        dataType: 'json',
        success: function (response) {
            buildFileList(response);
        },
        error: function (error) {
            //alert("holi2");
        }
    });
    buildFileList();
    setTimeout(stopUpdate, 2000);
});