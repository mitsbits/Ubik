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


    function Module() {
        var self = this;
        this.FriendlyName = ko.observable('');
        this.Summary = ko.observable('');
        this.shouldShow = ko.observable(false)
        this.ModuleParametersList = ko.observableArray();
    }
    function ModuleParameter() {
        var self = this;
        this.Identifier = ko.observable('');
        this.Value = ko.observable('')
    }




    $('select#FullName').change(function () {
        var url = "/api/backoffice/devices/mod-config/" + $('select#FullName option:selected').val();
   


        $.get(url)
            .fail(function () {
                ko.cleanNode($('#module-parameter-container')[0]);
                var model = new Module();
                ko.applyBindings(model, $('#module-parameter-container')[0]);
            })
            .done(function (data) {
                $(data.Parameters).each(function (index, item) {
                    ko.cleanNode($('#module-parameter-container')[0]);
                    var model = new Module();
                    model.Summary(data.Summary);
                    model.FriendlyName(data.FriendlyName);
                    model.shouldShow(true);
                    $(data.Parameters).each(function (index, item) {
                        var config = new ModuleParameter();
                        config.Identifier(Object.keys(item)[0]);
                        config.Value(item[config.Identifier()]);
                        model.ModuleParametersList().push(config);
                        ko.applyBindings(model, $('#module-parameter-container')[0]);
                    });
                });
            });





    });
});

