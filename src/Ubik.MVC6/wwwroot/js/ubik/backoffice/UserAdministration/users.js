$(function () {
    $("#users-table").dataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true
    });

    $('input.option-role').on('ifChanged', function (event) {
        setSelectedClaims();
    });

    function getSelectedRoles() {
        var bucket = new Array();
        $('input.option-role').each(function (index, element) {
            if ($(element).prop('checked')) {
                bucket.push($(element).data("id"));
            }
        });
        return bucket.join(";");
    }

    function setSelectedClaims() {
        var url = "/api/backoffice/users/claimsforroles/" + getSelectedRoles();
        $.get(url, function (data) {
            $('input.option-claim').iCheck('uncheck');
            $(data).each(function(index, element) {
                $('input.option-claim').filter('[data-id="' + element + '"]').iCheck('check');
            });
        });
    }

    setSelectedClaims();
});