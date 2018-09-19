function RefreshUserRight(urlBase) {
    $.ajax({
            type: 'POST',
            async: false,
            dataType: 'json',
            url: urlBase,
        });
        location.reload();
}