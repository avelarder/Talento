function addFile() {
    var formData = new FormData();
    formData.append('image', $('input[type=file]')[0].files[0]);
    $.ajax({
        type: "POST",
        url: '/File/Add',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {

        },
        error: function (error) {
            //Error is good enough because the result is always empty
            updatelist();
        }
    });
}
function deleteFile(filename) {
    var formData = new FormData();
    $.ajax({
        type: "POST",
        url: '/File/Delete',
        data: "filename=" + filename,
        success: function (response) {
            updatelist();
        },
        error: function (error) {
            updatelist();
        }

    });
}

function buildFileList(filelist) {

    $("#fileListDiv").empty();
    for (i = 0; i < filelist.length; i++) {
        $("#fileListDiv").append("<p id='file_" + filelist[i] + "'>" +
            "<i class='material-icons'>insert_drive_file</i>" +
            "<span>" + filelist[i].FileName + "</span>" +
            "<i class='delete-file material-icons' data-file='" + filelist[i].FileName + "'>delete</i>" +
            "</p>");
    }
}

function updatelist() {
    $.ajax({
        type: "POST",
        url: '/File/ListCandidateFiles/',
        data: {candidateId : @Model.Id}, 
        dataType: 'json', 
        success: function (response) {
            buildFileList(response);
        },
        error: function (error) {
            //alert("holi2");
        }
    });
}