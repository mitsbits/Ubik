$(function () {
   
    $("#roles-table").dataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true
    });

    $('#copy-role-modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var data = button.data('source');
        var modal = $(this);
        modal.find('span#roleTitleName').text(data);
        modal.find('input#Name').val(data);
        modal.find('input#Target').val('').focus();
    });

    $('#copy-role-form').validate();
    $('#Target').rules('add', {
        required: true,
        messages: {
            required: 'Some custom message for the username required field'
        }
    });
});