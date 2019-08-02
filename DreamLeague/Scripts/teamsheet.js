$(function () {
    $('.teamsheet-player-label').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Player/AutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }));
                }
            });
        },
        select: function (event, ui) {
            $(this).closest('.form-group').find('.teamsheet-player-id').val(ui.item.val).trigger('change');
        },
        messages: {
            noResults: "", results: ""
        }
    });
});

$('.teamsheet-player-id').change(function () {

    var managerId = $(this).closest('.form-horizontal').find('.managerTeamId').val();

    var playerIds = [];
    var playerSubs = [];

    $(this).closest('.form-horizontal').find('.teamsheet-player-id').each(function (i) {

        playerIds[i] = $(this).val();

        if ($(this).closest('.form-group').find('.teamsheet-player-substitute').is(':checked')) {
            playerSubs.push($(this).val());
        }
    });

    $.ajax({
        type: "POST",
        url: "/TeamSheet/EditPlayer",
        data: { managerId: managerId, playerIds: playerIds, playerSubs: playerSubs },
        traditional: true,
        success: function () {
            $('#save-confirmation').fadeIn(2000);
            $('#save-confirmation').fadeOut(2000);
        }
    });
});

$('.teamsheet-player-label').change(function () {
    if ($(this).val() == '') {
        $(this).closest('.form-group').find('.teamsheet-player-id').val(0).trigger('change');
    }
});

$('.teamsheet-player-substitute').change(function () {
    $(this).closest('.form-group').find('.teamsheet-player-id').trigger('change');
});

$(function () {
    $('.teamsheet-goalkeeper-label').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Team/AutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                }
            })
        },
        select: function (event, ui) {
            $(this).closest('.form-group').find('.teamsheet-goalkeeper-id').val(ui.item.val).trigger('change');
        },
        messages: {
            noResults: "", results: ""
        }
    });
});

$('.teamsheet-goalkeeper-id').change(function () {

    var managerId = $(this).closest('.form-horizontal').find('.managerTeamId').val();

    var teamIds = [];
    var teamSubs = [];

    $(this).closest('.form-horizontal').find('.teamsheet-goalkeeper-id').each(function (i) {

        teamIds[i] = $(this).val();

        if ($(this).closest('.form-group').find('.teamsheet-goalkeeper-substitute').is(':checked')) {
            teamSubs.push($(this).val());
        }
    });

    $.ajax({
        type: "POST",
        url: "/TeamSheet/EditGoalKeeper",
        data: { managerId: managerId, teamIds: teamIds, teamSubs: teamSubs },
        traditional: true,
        success: function () {
            $('#save-confirmation').fadeIn(2000);
            $('#save-confirmation').fadeOut(2000);
        }
    });
});

$('.teamsheet-goalkeeper-label').change(function () {
    if ($(this).val() == '') {
        $(this).closest('.form-group').find('.teamsheet-goalkeeper-id').val(0).trigger('change');
    }
});

$('.teamsheet-goalkeeper-substitute').change(function () {
    $(this).closest('.form-group').find('.teamsheet-goalkeeper-id').trigger('change');
});

$(document).on('change', ':file', function () {
    var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
    input.trigger('fileselect', [numFiles, label]);
});

$(document).ready(function () {
    $(':file').on('fileselect', function (event, numFiles, label) {

        var input = $(this).parents('.input-group').find(':text'),
            log = numFiles > 1 ? numFiles + ' files selected' : label;

        if (input.length) {
            input.val(log);
        } else {
            if (log) alert(log);
        }

    });
});
