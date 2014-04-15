$(function() {
    var client = new WindowsAzure.MobileServiceClient('[[MOBILE_SERVICES_URL]]', '[[SHARED_SECRET]]'),
        warningTable = client.getTable('warning');

    // Read current data and rebuild UI.
    function refreshWarnings() {
        var query = warningTable;

        query.read().then(function(warnings) {
            var listWarnings = $.map(warnings, function (warning) {
                return $('<li>')
                    .attr('data-warning-id', warning.id)
                    .append($('<button class="warning-delete">Delete</button>'))
                    .append($('<div>')
                    .append($('<input class="warningText">').val(warning.englishWarningText))
                    .append($('<input class="warningText">').val(warning.afrikaansWarningText)));
            });

            $('#warnings').empty().append(listWarnings).toggle(listWarnings.length > 0);
            $('#summary').html('<strong>' + listWarnings.length + '</strong> warning(s)');
        }, handleError);
    }

    function handleError(error) {
        var text = error + (error.request ? ' - ' + error.request.status : '');
        $('#errorlog').append($('<li>').text(text));
    }

    function getWarningId(formElement) {
        return $(formElement).closest('li').attr('data-warning-id');
    }

    // Handle insert
    $('#add-warning').submit(function(evt) {
        var englishTextbox = $('#warning_msg_english'),
            englishWarningText = englishTextbox.val();
        var afrikaansTextbox = $('#warning_msg_afrikaans'),
            afrikaansWarningText = afrikaansTextbox.val();
        var regions = new Array();
        AddIfChecked(regions, '#Western_Cape');
        AddIfChecked(regions, '#Eastern_Cape');
        AddIfChecked(regions, '#Northern_Cape');
        AddIfChecked(regions, '#Free_State'); 
        AddIfChecked(regions, '#KwaZulu-Natal');
        AddIfChecked(regions, '#Mpumalanga');
        AddIfChecked(regions, '#Gauteng');
        AddIfChecked(regions, '#Limpopo_Province');
        AddIfChecked(regions, '#North_West');
        var warnings = new Array();
        AddIfChecked(warnings, '#Snow');
        AddIfChecked(warnings, '#Heavy_Rain');
        AddIfChecked(warnings, '#Gails');
        AddIfChecked(warnings, '#Big_Waves');

        if ((englishWarningText !== '')&& (afrikaansWarningText !== '')) {
            warningTable.insert({
                englishWarningText: englishWarningText,
                afrikaansWarningText: afrikaansWarningText,
                regions: regions.toString(),
                warnings: warnings.toString()
            }).then(refreshWarnings, handleError);
        }
        englishTextbox.val('').focus();
        afrikaansTextbox.val('').focus();
        evt.preventDefault();
    });

    function AddIfChecked(target, cbId) {
        var checkBox = $(cbId);
        if (checkBox.prop('checked')) {
            checkBoxText = checkBox.val();
            target.push(checkBoxText);
        }
    }

    // Handle delete
    $(document.body).on('click', '.warning-delete', function () {
        warningTable.del({ id: getWarningId(this) }).then(refreshWarnings, handleError);
    });

    // On initial load, start by fetching the current data
    refreshWarnings();
});