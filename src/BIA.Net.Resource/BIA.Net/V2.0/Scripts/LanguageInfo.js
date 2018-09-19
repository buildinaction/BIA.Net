function SetLanguageInfo(urlSetLanguageInfo, languageCode) {
    if (languageCode != null && languageCode != "") {
        $.ajax({
            type: 'POST',
            async: false,
            dataType: 'json',
            data: { code: JSON.stringify(languageCode) },
            url: urlSetLanguageInfo,
        });
        location.reload();
    }
}