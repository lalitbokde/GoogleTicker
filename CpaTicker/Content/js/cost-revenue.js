jQuery.validator.addMethod('requiredif', function (value, element, params)
{
    var propertyId = $(element).attr('data-val-requiredif-property');
    //get the element by id
    var ddlValue = $('#' + propertyId).first().val();
    var expected = $(element).attr('data-val-requiredif-expected');
    
    console.log(ddlValue);
    if (ddlValue == expected) {
        return value; // return if value has data
    }
    //return checkboxValue || value;

}, '');

jQuery.validator.unobtrusive.adapters.add('requiredif', {}, function (options) {
    options.rules['requiredif'] = true;
    options.messages['requiredif'] = options.message;
});