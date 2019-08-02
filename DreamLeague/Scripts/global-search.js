$(function () {
    $('#global-search').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/AutoComplete",
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
            window.location.href = ui.item.val;
        },
        messages: {
            noResults: "", results: ""
        }
    });
});