$(function () {
    function clearResult() {
        $('#result').html('');
    }

    function clearError() {
        var element = $('#error');
        element.html('');
        element.hide();
    }

    function showError(msg) {
        var element = $('#error');
        element.html(msg);
        element.show();
    }

    function clearUrl() {
        $('#url').val('');
    }

    function clearUrl2() {
        $('#url2').val('');
    }

    function setUrl2(url) {
        $('#url2').html(url);
    }

    function getUrl() {
        return prependHttp($('#url').val());
    }

    function dataToWords(data) {
        var result = [];
        for (var i = 0; i < data.length; i++) {
            result.push({ "text": data[i]["Key"], "weight": data[i]["Value"] });
        }
        return result;
    }

    function initWordCloud() {
        var words = [];
        $('#result').jQCloud(words, { width: 640, height: 480 });
    }

    function updateWordCloud(data) {
        var words = dataToWords(data);
        $('#result').jQCloud('update', words);
    }

    function prependHttp(url) {
        if (!/^(f|ht)tps?:\/\//i.test(url)) {
            url = "http://" + url;
        }
        return url;
    }

    // copied from so
    function isUrlValid(url) {
        return /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url);
    }

    function enableBtn() {
        $('#build').removeClass('disabled');
    }

    function disableBtn() {
        $('#build').addClass('disabled');
    }

    $('#url').keyup(function () {
        var url = prependHttp($(this).val());
        if (isUrlValid(url)) {
            enableBtn();
        } else {
            disableBtn();
        }
    });

    $('#build').click(function (event) {
        if ($(this).hasClass('disabled')) {
            event.preventDefault();
            return;
        }
        
        setUrl2('Loading...'); // tbd spinner btn
        clearResult();
        clearError();

        var url = getUrl();

        $.getJSON(
            '/api/Word/Get',
            'url=' + url,
            function (data) {
                setUrl2(url);
                updateWordCloud(data);
                clearUrl();
                disableBtn();
            }
        ).error(function (event, jqxhr, exception) {
            var msg = event.status === 404
                ? 'Domain not found.'
                : 'Error occured. Please, try again later.';
            showError(msg);
        });
    });

    $(window).keydown(function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            $('#build').click();
        }
    });

    initWordCloud();
});