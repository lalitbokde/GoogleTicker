(function ($) {
    $.validator.addMethod("nevervalid", function () {
        return false;
    }, "Clientside Should Not Postback");

    $.validator.unobtrusive.adapters.addBool("nevervalid");

    $.validator.addMethod("requirehttpsif", function (value, element, params) {
        var trackingcode = $(params).val();
        if (trackingcode == "HttpsiFrame" || trackingcode == "HttpsImage") {
            var re = /src="https/m;
            //alert(re.test(value));
            return re.test(value);
        }
        return true;
    });


    $.validator.unobtrusive.adapters.add("requirehttpsif", ["otherpropertyname"], function (options) {
        options.rules["requirehttpsif"] = "#" + options.params.otherpropertyname;
        options.messages["requirehttpsif"] = options.message;
    });

})(jQuery);