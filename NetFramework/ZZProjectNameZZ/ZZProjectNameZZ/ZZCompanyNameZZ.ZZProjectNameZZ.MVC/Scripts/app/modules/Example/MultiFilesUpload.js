var app;
(function (app) {
    var module;
    (function (module) {
        var Exemple;
        (function (Exemple) {
            $(document).ready(function () {
                $('.multifield').on('click', '.btn-add', function (e) {
                    e.preventDefault();
                    var controlForm = $(this).closest('.multifield'), currentEntry = $(this).parents('.field:first'), newEntry = $($(currentEntry[0]).clone()).appendTo(controlForm);
                    newEntry.find(".ripple-container").remove();
                    newEntry.find('input').val('');
                    controlForm.find('.field:not(:last) .btn-add')
                        .removeClass('btn-add').addClass('btn-remove')
                        .removeClass('btn-success').addClass('btn-danger')
                        .html('<span class="fas fa-trash-alt"></span>');
                    this.blur();
                }).on('click', '.btn-remove', function (e) {
                    $(this).parents('.field:first').remove();
                    e.preventDefault();
                    return false;
                });
            });
        })(Exemple = module.Exemple || (module.Exemple = {}));
    })(module = app.module || (app.module = {}));
})(app || (app = {}));
//# sourceMappingURL=MultiFilesUpload.js.map