var DataTablesUtility = {

    dtConvFromJSON: function (data, withTime) {
        if (data == null) return '1/1/1950';
        var r = /\/Date\(([0-9]+)\)\//gi
        var matches = data.match(r);
        if (matches == null) return '1/1/1950';
        var result = matches.toString().substring(6, 19);
        var epochMilliseconds = result.replace(
        /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
        '$1');
        var b = new Date(parseInt(epochMilliseconds));
        var c = new Date(b.toString());
        var curr_date = c.getDate();
        var curr_month = c.getMonth() + 1;
        var curr_year = c.getFullYear();
        var curr_h = c.getHours();
        var curr_m = c.getMinutes();
        var curr_s = c.getSeconds();
        var curr_offset = c.getTimezoneOffset() / 60
        if (withTime) {
            var d = ('0' + curr_date).substring(curr_date.toString().length - 1) + '/' + ('0' + curr_month).substring(curr_month.toString().length - 1) + '/' + curr_year + " " + ('0' + curr_h).substring(curr_h.toString().length - 1) + ':' + ('0' + curr_m).substring(curr_m.toString().length - 1) + ':' + ('0' + curr_s).substring(curr_s.toString().length - 1);
        }
        else {
            var d = ('0' + curr_date).substring(curr_date.toString().length - 1) + '/' + ('0' + curr_month).substring(curr_month.toString().length - 1) + '/' + curr_year;
        }
        return d;

    },

    ColoredCircleConvFromJSON: function (data) {
        if (data == null) {
            return '';
        }

        if (data == true) {
            return "<span class=\"glyphicon glyphicon-one-fine-dot green\"> </span>";
        }
        else if (data == false) {
            return "<span class=\"glyphicon glyphicon-one-fine-dot red\"> </span>";
        }
    },

    AssignUnassignConvFromJSON: function (data) {
        if (data == null) {
            return '';
        }

        if (data == true) {
            return '<img alt="RequestAdd" src="' + Images.UrlRequestAssign + '" />';
        }
        else if (data == false) {
            return '<img alt="RequestLess" src="' + Images.UrlRequestUnassign + '" />';
        }
    },

    MaxColumnLength: function (data, maxLength) {
        return data.length > maxLength ?
       data.substr(0, maxLength) + '…' :
       data;
    },

    ParseColumnsJSON: function (columnsJSON) {
        // First parse JSON content
        var columns = columnsJSON != '' ? JSON.parse(columnsJSON) : null;
        if (columns) {
            // Then iterate on columns to define the mRender property according to value of mRenderMethod
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].mRenderMethod) {
                    columns[i].mRender = DataTablesUtility.MRenderFunctionProvider(columns[i].mRenderMethod);
                }
            }

            return columns
        }
        else {
            return null;
        }
    },

    MRenderFunctionProvider: function (methodName) {
        switch (methodName) {
            case "dtConvFromJSON":
                return function (data, type, full) {
                    return DataTablesUtility.dtConvFromJSON(data);
                };
            case "ColoredCircleConvFromJSON":
                return function (data, type, full) {
                    return DataTablesUtility.ColoredCircleConvFromJSON(data);
                };
            case "AssignUnassignConvFromJSON":
                return function (data, type, full) {
                    return DataTablesUtility.AssignUnassignConvFromJSON(data);
                };
            case "MaxColumnLength":
                return function (data, type, full) {
                    return DataTablesUtility.MaxColumnLength(data, 25);
                };
            default:
                return null;
        }
    },
}