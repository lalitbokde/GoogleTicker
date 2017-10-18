
function GetCalculation() {
    var aux = 0;

    if (cr.checked)
        aux = 1;
    if (cpc.checked)
        aux = aux + 2;
    if (rpc.checked)
        aux = aux + 4;

    return aux;
}

function GetFilter() {

    var aux = 0;
    if (affiliatefilter.checked && $("#AffiliateId").val() != '')
        aux = 1;
    if (campaignfilter.checked && $("#CampaignId").val() != '')
        aux = aux + 2;
    if (countriesfilter.checked && $("#countriesfiltertextbox").val() != '')
        aux = aux + 4;
    if (aux == 0) {

        return '';
    }
    var result = aux + ',';
    if (affiliatefilter.checked)
        result += $("#AffiliateId").val() + ',';
    else
        result += ',';
    if (campaignfilter.checked)
        result += $("#CampaignId").val() + ',';
    else
        result += ',';
    if (countriesfilter.checked)
        result += $("#countriesfiltertextbox").val();


    return result;

}

function SetFilter()
{
    if ($('#affiliatefilter').length && affiliatefilter.checked)
    {
        $("#AffiliateId").removeClass();
        $("#AffiliateId").fadeIn(200);
    }
    else $("#AffiliateId").fadeOut(1);

    if (countriesfilter.checked) {

        $("#countriesfiltertextbox").removeClass();
        $("#countriesfiltertextbox").fadeIn(200);
    }
    else $("#countriesfiltertextbox").fadeOut(1);


    if (campaignfilter.checked) {
        $("#CampaignId").removeClass();
        $("#CampaignId").fadeIn(200);
    }
    else $("#CampaignId").fadeOut(1);

    return 1;
}



//SetFilter();

function setbindings() {

    $('#campaignfilter').bind('click', function () {
        SetFilter();
    });

    $('#affiliatefilter').bind('click', function () {
        SetFilter();
    });

    $('#countriesfilter').bind('click', function () {
        SetFilter();
    });
}

