$(function () {

    $("#layouts-table").dataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true
    });

    $("#devicesections-table").dataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true
    });
    
});