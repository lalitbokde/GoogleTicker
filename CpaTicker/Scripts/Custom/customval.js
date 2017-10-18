(function ($) {
 
    $.validator.addMethod("requirehttpsif", function (value, element, params)
    {
        var trackingcode = $(params).val();
        //console.log(trackingcode);
        if (trackingcode == "HttpsiFrame" || trackingcode == "HttpsImage")
        {
            //var re = /src="https/m;
            var re = /https/m;
            
            //console.log(value);
            //console.log(re.test(value));

            return re.test(value);

        }
        return true;
    });


    $.validator.unobtrusive.adapters.add("requirehttpsif", ["otherpropertyname"], function (options) {
        options.rules["requirehttpsif"] = "#" + options.params.otherpropertyname;
        options.messages["requirehttpsif"] = options.message;
    });

    $.validator.addMethod("requiredarray", function (value, element, params) {
        var selector = 'select[name=' + $(element).attr('name') + '] option:selected';
        return $(selector).length > 0;
    }, 'Clientside Should Not Postback');

    $.validator.unobtrusive.adapters.addBool("requiredarray");
    
})(jQuery);

