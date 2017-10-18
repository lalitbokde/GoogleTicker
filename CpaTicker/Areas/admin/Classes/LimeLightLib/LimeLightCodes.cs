using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes.LimeLightLib
{
    public class LimeLightCodes
    {
        public static Dictionary<string, string> GetCountryCode()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("AX", "Aaland Islands");
            dic.Add("AF", "Afghanistan");
            dic.Add("AL", "Albania");
            dic.Add("DZ", "Algeria");
            dic.Add("AS", "American Samoa");
            dic.Add("AD", "Andorra");
            dic.Add("AO", "Angola");
            dic.Add("AI", "Anguilla");
            dic.Add("AQ", "Antarctica");
            dic.Add("AG", "Antigua and Barbuda");
            dic.Add("AR", "Argentina");
            dic.Add("AM", "Armenia");
            dic.Add("AW", "Aruba");
            dic.Add("AU", "Australia");
            dic.Add("AT", "Austria");
            dic.Add("AZ", "Azerbaijan");
            dic.Add("BS", "Bahamas");
            dic.Add("BH", "Bahrain");
            dic.Add("BD", "Bangladesh");
            dic.Add("BB", "Barbados");
            dic.Add("BY", "Belarus");
            dic.Add("BE", "Belgium");
            dic.Add("BZ", "Belize");
            dic.Add("BJ", "Benin");
            dic.Add("BM", "Bermuda");
            dic.Add("BT", "Bhutan");
            dic.Add("BO", "Bolivia");
            dic.Add("BA", "Bosnia and Herzegowina");
            dic.Add("BW", "Botswana");
            dic.Add("BV", "Bouvet Island");
            dic.Add("BR", "Brazil");
            dic.Add("IO", "British Indian Ocean Territory");
            dic.Add("BN", "Brunei Darussalam");
            dic.Add("BG", "Bulgaria");
            dic.Add("BF", "Burkina Faso");
            dic.Add("BI", "Burundi");
            dic.Add("KH", "Cambodia");
            dic.Add("CM", "Cameroon");
            dic.Add("CA", "Canada");
            dic.Add("CV", "Cape Verde");
            dic.Add("KY", "Cayman Islands");
            dic.Add("CF", "Central African Republic");
            dic.Add("TD", "Chad");
            dic.Add("CL", "Chile");
            dic.Add("CN", "China");
            dic.Add("CX", "Christmas Island");
            dic.Add("CC", "Cocos (Keeling) Islands");
            dic.Add("CO", "Colombia");
            dic.Add("KM", "Comoros");
            dic.Add("CG", "Congo");
            dic.Add("CK", "Cook Islands");
            dic.Add("CR", "Costa Rica");
            dic.Add("CI", "Cote D'Ivoire");
            dic.Add("HR", "Croatia");
            dic.Add("CU", "Cuba");
            dic.Add("CY", "Cyprus");
            dic.Add("CZ", "Czech Republic");
            dic.Add("DK", "Denmark");
            dic.Add("DJ", "Djibouti");
            dic.Add("DM", "Dominica");
            dic.Add("DO", "Dominican Republic");
            dic.Add("TP", "East Timor");
            dic.Add("EC", "Ecuador");
            dic.Add("EG", "Egypt");
            dic.Add("SV", "El Salvador");
            dic.Add("EN", "England");
            dic.Add("GQ", "Equatorial Guinea");
            dic.Add("ER", "Eritrea");
            dic.Add("EE", "Estonia");
            dic.Add("ET", "Ethiopia");
            dic.Add("FK", "Falkland Islands (Malvinas)");
            dic.Add("FO", "Faroe Islands");
            dic.Add("FJ", "Fiji");
            dic.Add("FI", "Finland");
            dic.Add("FR", "France");
            dic.Add("FX", "France Metropolitan");
            dic.Add("GF", "French Guiana");
            dic.Add("PF", "French Polynesia");
            dic.Add("TF", "French Southern Territories");
            dic.Add("GA", "Gabon");
            dic.Add("GM", "Gambia");
            dic.Add("GE", "Georgia");
            dic.Add("DE", "Germany");
            dic.Add("GH", "Ghana");
            dic.Add("GI", "Gibraltar");
            dic.Add("GR", "Greece");
            dic.Add("GL", "Greenland");
            dic.Add("GD", "Grenada");
            dic.Add("GP", "Guadeloupe");
            dic.Add("GU", "Guam");
            dic.Add("GT", "Guatemala");
            //dic.Add("GB", "Guernsey"); conflic with the UK
            dic.Add("GN", "Guinea");
            dic.Add("GW", "Guinea-bissau");
            dic.Add("GY", "Guyana");
            dic.Add("HT", "Haiti");
            dic.Add("HM", "Heard and Mc Donald Islands");
            dic.Add("HN", "Honduras");
            dic.Add("HK", "Hong Kong");
            dic.Add("HU", "Hungary");
            dic.Add("IS", "Iceland");
            dic.Add("IN", "India");
            dic.Add("ID", "Indonesia");
            dic.Add("IR", "Iran (Islamic Republic of)");
            dic.Add("IQ", "Iraq");
            dic.Add("IE", "Ireland");
            dic.Add("IM", "Isle of Man");
            dic.Add("IL", "Israel");
            dic.Add("IT", "Italy");
            dic.Add("JM", "Jamaica");
            dic.Add("JP", "Japan");
            dic.Add("JY", "Jersey");
            dic.Add("JO", "Jordan");
            dic.Add("KZ", "Kazakhstan");
            dic.Add("KE", "Kenya");
            dic.Add("KI", "Kiribati");
            dic.Add("KP", "Korea Democratic People's Republic of");
            dic.Add("KR", "Korea Republic of");
            dic.Add("KW", "Kuwait");
            dic.Add("KG", "Kyrgyzstan");
            dic.Add("LA", "Lao People's Democratic Republic");
            dic.Add("LV", "Latvia");
            dic.Add("LB", "Lebanon");
            dic.Add("LS", "Lesotho");
            dic.Add("LR", "Liberia");
            dic.Add("LY", "Libyan Arab Jamahiriya");
            dic.Add("LI", "Liechtenstein");
            dic.Add("LT", "Lithuania");
            dic.Add("LU", "Luxembourg");
            dic.Add("MO", "Macau");
            dic.Add("MK", "Macedonia The Former Yugoslav Republic of");
            dic.Add("MG", "Madagascar");
            dic.Add("MW", "Malawi");
            dic.Add("MY", "Malaysia");
            dic.Add("MV", "Maldives");
            dic.Add("ML", "Mali");
            dic.Add("MT", "Malta");
            dic.Add("MH", "Marshall Islands");
            dic.Add("MQ", "Martinique");
            dic.Add("MR", "Mauritania");
            dic.Add("MU", "Mauritius");
            dic.Add("YT", "Mayotte");
            dic.Add("MX", "Mexico");
            dic.Add("FM", "Micronesia Federated States of");
            dic.Add("MD", "Moldova Republic of");
            dic.Add("MC", "Monaco");
            dic.Add("MN", "Mongolia");
            dic.Add("MS", "Montserrat");
            dic.Add("MA", "Morocco");
            dic.Add("MZ", "Mozambique");
            dic.Add("MM", "Myanmar");
            dic.Add("NA", "Namibia");
            dic.Add("NR", "Nauru");
            dic.Add("NP", "Nepal");
            dic.Add("NL", "Netherlands");
            dic.Add("AN", "Netherlands Antilles");
            dic.Add("NC", "New Caledonia");
            dic.Add("NZ", "New Zealand");
            dic.Add("NI", "Nicaragua");
            dic.Add("NE", "Niger");
            dic.Add("NG", "Nigeria");
            dic.Add("NU", "Niue");
            dic.Add("NF", "Norfolk Island");
            dic.Add("ND", "Northern Ireland");
            dic.Add("MP", "Northern Mariana Islands");
            dic.Add("NO", "Norway");
            dic.Add("OM", "Oman");
            dic.Add("PK", "Pakistan");
            dic.Add("PW", "Palau");
            dic.Add("PA", "Panama");
            dic.Add("PG", "Papua New Guinea");
            dic.Add("PY", "Paraguay");
            dic.Add("PE", "Peru");
            dic.Add("PH", "Philippines");
            dic.Add("PN", "Pitcairn");
            dic.Add("PL", "Poland");
            dic.Add("PT", "Portugal");
            dic.Add("PR", "Puerto Rico");
            dic.Add("QA", "Qatar");
            dic.Add("RE", "Reunion");
            dic.Add("RO", "Romania");
            dic.Add("RU", "Russian Federation");
            dic.Add("RW", "Rwanda");
            dic.Add("KN", "Saint Kitts and Nevis");
            dic.Add("LC", "Saint Lucia");
            dic.Add("VC", "Saint Vincent and the Grenadines");
            dic.Add("WS", "Samoa");
            dic.Add("SM", "San Marino");
            dic.Add("ST", "Sao Tome and Principe");
            dic.Add("SA", "Saudi Arabia");
            dic.Add("SS", "Scotland");
            dic.Add("SN", "Senegal");
            dic.Add("SC", "Seychelles");
            dic.Add("SL", "Sierra Leone");
            dic.Add("SG", "Singapore");
            dic.Add("SK", "Slovakia (Slovak Republic)");
            dic.Add("SI", "Slovenia");
            dic.Add("SB", "Solomon Islands");
            dic.Add("SO", "Somalia");
            dic.Add("ZA", "South Africa");
            dic.Add("GS", "South Georgia and the South Sandwich Islands");
            dic.Add("ES", "Spain");
            dic.Add("LK", "Sri Lanka");
            dic.Add("SH", "St. Helena");
            dic.Add("PM", "St. Pierre and Miquelon");
            dic.Add("SD", "Sudan");
            dic.Add("SR", "Suriname");
            dic.Add("SJ", "Svalbard and Jan Mayen Islands");
            dic.Add("SZ", "Swaziland");
            dic.Add("SE", "Sweden");
            dic.Add("CH", "Switzerland");
            dic.Add("SY", "Syrian Arab Republic");
            dic.Add("TW", "Taiwan");
            dic.Add("TJ", "Tajikistan");
            dic.Add("TZ", "Tanzania United Republic of");
            dic.Add("TH", "Thailand");
            dic.Add("TG", "Togo");
            dic.Add("TK", "Tokelau");
            dic.Add("TO", "Tonga");
            dic.Add("TT", "Trinidad and Tobago");
            dic.Add("TN", "Tunisia");
            dic.Add("TR", "Turkey");
            dic.Add("TM", "Turkmenistan");
            dic.Add("TC", "Turks and Caicos Islands");
            dic.Add("TV", "Tuvalu");
            dic.Add("UG", "Uganda");
            dic.Add("UA", "Ukraine");
            dic.Add("AE", "United Arab Emirates");
            dic.Add("GB", "United Kingdom");
            dic.Add("US", "United States");
            dic.Add("UM", "United States Minor Outlying Islands");
            dic.Add("UY", "Uruguay");
            dic.Add("UZ", "Uzbekistan");
            dic.Add("VU", "Vanuatu");
            dic.Add("VA", "Vatican City State (Holy See)");
            dic.Add("VE", "Venezuela");
            dic.Add("VN", "Viet Nam");
            dic.Add("VG", "Virgin Islands (British)");
            dic.Add("VI", "Virgin Islands (U.S.)");
            dic.Add("WL", "Wales");
            dic.Add("WF", "Wallis and Futuna Islands");
            dic.Add("EH", "Western Sahara");
            dic.Add("YE", "Yemen");
            dic.Add("YU", "Yugoslavia");
            dic.Add("ZR", "Zaire");
            dic.Add("ZM", "Zambia");
            dic.Add("ZW", "Zimbabwe");


            return dic;
        }

        public static Dictionary<string, Dictionary<string, string>> GetStateCode()
        {
            Dictionary<string, Dictionary<string, string>> pdic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> dic;

            #region Afghanistan
            dic = new Dictionary<string, string>();
            dic.Add("AF-BDS", "Badakhshan");
            dic.Add("AF-BDG", "Badghis");
            dic.Add("AF-BGL", "Baghlan");
            dic.Add("AF-BAL", "Balkh");
            dic.Add("AF-BAM", "Bamian");
            dic.Add("AF-DAY", "Daykondi");
            dic.Add("AF-FRA", "Farah");
            dic.Add("AF-FYB", "Faryab");
            dic.Add("AF-GHA", "Ghazni");
            dic.Add("AF-GHO", "Ghowr");
            dic.Add("AF-HEL", "Helmand");
            dic.Add("AF-HER", "Herat");
            dic.Add("AF-JOW", "Jowzjan");
            dic.Add("AF-KAB", "Kabul");
            dic.Add("AF-KAN", "Kandahar");
            dic.Add("AF-KAP", "Kapisa");
            dic.Add("AF-KHO", "Khowst");
            dic.Add("AF-KNR", "Konar");
            dic.Add("AF-KDZ", "Kondoz");
            dic.Add("AF-LAG", "Laghman");
            dic.Add("AF-LOW", "Lowgar");
            dic.Add("AF-NAN", "Nangrahar");
            dic.Add("AF-NIM", "Nimruz");
            dic.Add("AF-NUR", "Nurestan");
            dic.Add("AF-ORU", "Oruzgan");
            dic.Add("AF-PIA", "Paktia");
            dic.Add("AF-PKA", "Paktika");
            dic.Add("AF-PAN", "Panjshir");
            dic.Add("AF-PAR", "Parwan");
            dic.Add("AF-SAM", "Samangan");
            dic.Add("AF-SAR", "Sar-e Pol");
            dic.Add("AF-TAK", "Takhar");
            dic.Add("AF-WAR", "Wardak");
            dic.Add("AF-ZAB", "Zabol");
            pdic.Add("AF", dic);
            #endregion

            /*=============================================*/

            #region Albania
            dic = new Dictionary<string, string>();
            dic.Add("AL-BR", "Berat");
            dic.Add("AL-BU", "Bulqizë");
            dic.Add("AL-DL", "Delvinë");
            dic.Add("AL-DV", "Devoll");
            dic.Add("AL-DI", "Dibër");
            dic.Add("AL-DR", "Durrës");
            dic.Add("AL-EL", "Elbasan");
            dic.Add("AL-FR", "Fier");
            dic.Add("AL-GJ", "Gjirokastër");
            dic.Add("AL-GR", "Gramsh");
            dic.Add("AL-HA", "Has");
            dic.Add("AL-KA", "Kavajë");
            dic.Add("AL-ER", "Kolonjë");
            dic.Add("AL-KO", "Korçë");
            dic.Add("AL-KR", "Krujë");
            dic.Add("AL-KC", "Kuçovë");
            dic.Add("AL-KU", "Kukës");
            dic.Add("AL-KB", "Kurbin");
            dic.Add("AL-LE", "Lezhë");
            dic.Add("AL-LB", "Librazhd");
            dic.Add("AL-LU", "Lushnjë");
            dic.Add("AL-MM", "Malësi e Madhe");
            dic.Add("AL-MK", "Mallakastër");
            dic.Add("AL-MT", "Mat");
            dic.Add("AL-MR", "Mirditë");
            dic.Add("AL-PQ", "Peqin");
            dic.Add("AL-PR", "Përmet");
            dic.Add("AL-PG", "Pogradec");
            dic.Add("AL-PU", "Pukë");
            dic.Add("AL-SR", "Sarandë");
            dic.Add("AL-SH", "Shkodër");
            dic.Add("AL-SK", "Skrapar");
            dic.Add("AL-TE", "Tepelenë");
            dic.Add("AL-TR", "Tiranë");
            dic.Add("AL-TP", "Tropojë");
            dic.Add("AL-VL", "Vlorë");
            pdic.Add("AL", dic);
            #endregion

            /*=============================================*/

            #region Algeria
            dic = new Dictionary<string, string>();
            dic.Add("DZ-01", "Adrar");
            dic.Add("DZ-44", "Aïn Defla");
            dic.Add("DZ-46", "Aïn Témouchent");
            dic.Add("DZ-16", "Alger");
            dic.Add("DZ-23", "Annaba");
            dic.Add("DZ-05", "Batna");
            dic.Add("DZ-08", "Béchar");
            dic.Add("DZ-06", "Béjaïa");
            dic.Add("DZ-07", "Biskra");
            dic.Add("DZ-09", "Blida");
            dic.Add("DZ-34", "Bordj Bou Arréridj");
            dic.Add("DZ-10", "Bouira");
            dic.Add("DZ-35", "Boumerdès");
            dic.Add("DZ-02", "Chlef");
            dic.Add("DZ-25", "Constantine");
            dic.Add("DZ-17", "Djelfa");
            dic.Add("DZ-32", "El Bayadh");
            dic.Add("DZ-39", "El Oued");
            dic.Add("DZ-36", "El Tarf");
            dic.Add("DZ-47", "Ghardaïa");
            dic.Add("DZ-24", "Guelma");
            dic.Add("DZ-33", "Illizi");
            dic.Add("DZ-18", "Jijel");
            dic.Add("DZ-40", "Khenchela");
            dic.Add("DZ-03", "Laghouat");
            dic.Add("DZ-29", "Mascara");
            dic.Add("DZ-26", "Médéa");
            dic.Add("DZ-43", "Mila");
            dic.Add("DZ-27", "Mostaganem");
            dic.Add("DZ-28", "Msila");
            dic.Add("DZ-45", "Naama");
            dic.Add("DZ-31", "Oran");
            dic.Add("DZ-30", "Ouargla");
            dic.Add("DZ-04", "Oum el Bouaghi");
            dic.Add("DZ-48", "Relizane");
            dic.Add("DZ-20", "Saïda");
            dic.Add("DZ-19", "Sétif");
            dic.Add("DZ-22", "Sidi Bel Abbès");
            dic.Add("DZ-21", "Skikda");
            dic.Add("DZ-41", "Souk Ahras");
            dic.Add("DZ-11", "Tamanghasset");
            dic.Add("DZ-12", "Tébessa");
            dic.Add("DZ-14", "Tiaret");
            dic.Add("DZ-37", "Tindouf");
            dic.Add("DZ-42", "Tipaza");
            dic.Add("DZ-38", "Tissemsilt");
            dic.Add("DZ-15", "Tizi Ouzou");
            dic.Add("DZ-13", "Tlemcen");
            pdic.Add("DZ", dic);
            #endregion

            /*=============================================*/

            #region Andorra
            dic = new Dictionary<string, string>();
            dic.Add("AD-07", "Andorra la Vella");
            dic.Add("AD-02", "Canillo");
            dic.Add("AD-03", "Encamp");
            dic.Add("AD-08", "Escaldes-Engordany");
            dic.Add("AD-04", "La Massana");
            dic.Add("AD-05", "Ordino");
            dic.Add("AD-06", "Sant Julià de Lòria");
            pdic.Add("AD", dic);
            #endregion

            /*=============================================*/

            #region Angola
            dic = new Dictionary<string, string>();
            dic.Add("AO-BGO", "Bengo");
            dic.Add("AO-BGU", "Benguela");
            dic.Add("AO-BIE", "Bié");
            dic.Add("AO-CAB", "Cabinda");
            dic.Add("AO-CCU", "Cuando-Cubango");
            dic.Add("AO-CNO", "Cuanza Norte");
            dic.Add("AO-CUS", "Cuanza Sul");
            dic.Add("AO-CNN", "Cunene");
            dic.Add("AO-HUA", "Huambo");
            dic.Add("AO-HUI", "Huíla");
            dic.Add("AO-LUA", "Luanda");
            dic.Add("AO-LNO", "Lunda Norte");
            dic.Add("AO-LSU", "Lunda Sul");
            dic.Add("AO-MAL", "Malange");
            dic.Add("AO-MOX", "Moxico");
            dic.Add("AO-NAM", "Namibe");
            dic.Add("AO-UIG", "Uíge");
            dic.Add("AO-ZAI", "Zaire");
            pdic.Add("AO", dic);
            #endregion

            /*=============================================*/

            #region Antigua and Barbuda
            dic = new Dictionary<string, string>();
            dic.Add("AG-10", "Barbuda");
            dic.Add("AG-11", "Redonda");
            dic.Add("AG-03", "Saint George");
            dic.Add("AG-04", "Saint John's");
            dic.Add("AG-05", "Saint Mary");
            dic.Add("AG-06", "Saint Paul");
            dic.Add("AG-07", "Saint Peter");
            dic.Add("AG-08", "Saint Philip");
            pdic.Add("AG", dic);
            #endregion

            /*=============================================*/

            #region Argentina
            dic = new Dictionary<string, string>();
            dic.Add("AR-B", "Buenos Aires");
            dic.Add("AR-K", "Catamarca");
            dic.Add("AR-H", "Chaco");
            dic.Add("AR-U", "Chubut");
            dic.Add("AR-C", "Ciudad Autónoma de Buenos Aires");
            dic.Add("AR-X", "Córdoba");
            dic.Add("AR-W", "Corrientes");
            dic.Add("AR-E", "Entre Ríos");
            dic.Add("AR-P", "Formosa");
            dic.Add("AR-Y", "Jujuy");
            dic.Add("AR-L", "La Pampa");
            dic.Add("AR-F", "La Rioja");
            dic.Add("AR-M", "Mendoza");
            dic.Add("AR-N", "Misiones");
            dic.Add("AR-Q", "Neuquén");
            dic.Add("AR-R", "Río Negro");
            dic.Add("AR-A", "Salta");
            dic.Add("AR-J", "San Juan");
            dic.Add("AR-D", "San Luis");
            dic.Add("AR-Z", "Santa Cruz");
            dic.Add("AR-S", "Santa Fe");
            dic.Add("AR-G", "Santiago del Estero");
            dic.Add("AR-V", "Tierra del Fuego");
            dic.Add("AR-T", "Tucumán");
            pdic.Add("AR", dic);
            #endregion

            /*=============================================*/

            #region Armenia
            dic = new Dictionary<string, string>();
            dic.Add("AM-AG", "Aragatsotn");
            dic.Add("AM-AR", "Ararat");
            dic.Add("AM-AV", "Armavir");
            dic.Add("AM-ER", "Erevan");
            dic.Add("AM-GR", "Gegharkunik");
            dic.Add("AM-KT", "Kotayk");
            dic.Add("AM-LO", "Lori");
            dic.Add("AM-SH", "Shirak");
            dic.Add("AM-SU", "Syunik");
            dic.Add("AM-TV", "Tavush");
            dic.Add("AM-VD", "Vayots Dzor");
            pdic.Add("AM", dic);
            #endregion

            /*=============================================*/

            #region Australia
            dic = new Dictionary<string, string>();
            dic.Add("AU-ACT", "Australian Capital Territory");
            dic.Add("AU-NSW", "New South Wales");
            dic.Add("AU-NT", "Northern Territory");
            dic.Add("AU-QLD", "Queensland");
            dic.Add("AU-SA", "South Australia");
            dic.Add("AU-TAS", "Tasmania");
            dic.Add("AU-VIC", "Victoria");
            dic.Add("AU-WA", "Western Australia");
            pdic.Add("AU", dic);
            #endregion

            /*=============================================*/

            #region Austria
            dic = new Dictionary<string, string>();
            dic.Add("AT-1", "Burgenland");
            dic.Add("AT-2", "Kärnten");
            dic.Add("AT-3", "Niederösterreich");
            dic.Add("AT-4", "Oberösterreich");
            dic.Add("AT-5", "Salzburg");
            dic.Add("AT-6", "Steiermark");
            dic.Add("AT-7", "Tirol");
            dic.Add("AT-8", "Vorarlberg");
            dic.Add("AT-9", "Wien");
            pdic.Add("AT", dic);
            #endregion

            /*=============================================*/

            #region Azerbaijan
            dic = new Dictionary<string, string>();
            dic.Add("AZ-ABS", "Abseron");
            dic.Add("AZ-AGC", "Agcabädi");
            dic.Add("AZ-AGM", "Agdam");
            dic.Add("AZ-AGS", "Agdas");
            dic.Add("AZ-AGA", "Agstafa");
            dic.Add("AZ-AGU", "Agsu");
            dic.Add("AZ-AB", "Äli Bayramli");
            dic.Add("AZ-AST", "Astara");
            dic.Add("AZ-BAB", "Babäk");
            dic.Add("AZ-BA", "Baki");
            dic.Add("AZ-BAL", "Balakän");
            dic.Add("AZ-BAR", "Bärdä");
            dic.Add("AZ-BEY", "Beyläqan");
            dic.Add("AZ-BIL", "Biläsuvar");
            dic.Add("AZ-CAB", "Cäbrayil");
            dic.Add("AZ-CAL", "Cälilabab");
            dic.Add("AZ-CUL", "Culfa");
            dic.Add("AZ-DAS", "Daskäsän");
            dic.Add("AZ-DAV", "Däväçi");
            dic.Add("AZ-FUZ", "Füzuli");
            dic.Add("AZ-GAD", "Gädäbäy");
            dic.Add("AZ-GA", "Gäncä");
            dic.Add("AZ-GOR", "Goranboy");
            dic.Add("AZ-GOY", "Göyçay");
            dic.Add("AZ-HAC", "Haciqabul");
            dic.Add("AZ-IMI", "Imisli");
            dic.Add("AZ-ISM", "Ismayilli");
            dic.Add("AZ-KAL", "Kälbäcär");
            dic.Add("AZ-KUR", "Kürdämir");
            dic.Add("AZ-LAC", "Laçin");
            dic.Add("AZ-LAN", "Länkäran");
            dic.Add("AZ-LA", "Länkäran City");
            dic.Add("AZ-LER", "Lerik");
            dic.Add("AZ-MAS", "Masalli");
            dic.Add("AZ-MI", "Mingäçevir");
            dic.Add("AZ-NA", "Naftalan");
            dic.Add("AZ-NX", "Naxçivan");
            dic.Add("AZ-NEF", "Neftçala");
            dic.Add("AZ-OGU", "Oguz");
            dic.Add("AZ-ORD", "Ordubad");
            dic.Add("AZ-QAB", "Qäbälä");
            dic.Add("AZ-QAX", "Qax");
            dic.Add("AZ-QAZ", "Qazax");
            dic.Add("AZ-QOB", "Qobustan");
            dic.Add("AZ-QBA", "Quba");
            dic.Add("AZ-QBI", "Qubadli");
            dic.Add("AZ-QUS", "Qusar");
            dic.Add("AZ-SAT", "Saatli");
            dic.Add("AZ-SAB", "Sabirabad");
            dic.Add("AZ-SAD", "Sädäräk");
            dic.Add("AZ-SAH", "Sahbuz");
            dic.Add("AZ-SAK", "Säki");
            dic.Add("AZ-SA", "Säki City");
            dic.Add("AZ-SAL", "Salyan");
            dic.Add("AZ-SMI", "Samaxi");
            dic.Add("AZ-SKR", "Sämkir");
            dic.Add("AZ-SMX", "Samux");
            dic.Add("AZ-SAR", "Särur");
            dic.Add("AZ-SIY", "Siyäzän");
            dic.Add("AZ-SM", "Sumqayit");
            dic.Add("AZ-SUS", "Susa");
            dic.Add("AZ-SS", "Susa City");
            dic.Add("AZ-TAR", "Tärtär");
            dic.Add("AZ-TOV", "Tovuz");
            dic.Add("AZ-UCA", "Ucar");
            dic.Add("AZ-XAC", "Xaçmaz");
            dic.Add("AZ-XA", "Xankändi");
            dic.Add("AZ-XAN", "Xanlar");
            dic.Add("AZ-XIZ", "Xizi");
            dic.Add("AZ-XCI", "Xocali");
            dic.Add("AZ-XVD", "Xocavänd");
            dic.Add("AZ-YAR", "Yardimli");
            dic.Add("AZ-YEV", "Yevlax");
            dic.Add("AZ-YE", "Yevlax City");
            dic.Add("AZ-ZAN", "Zängilan");
            dic.Add("AZ-ZAQ", "Zaqatala");
            dic.Add("AZ-ZAR", "Zärdab");
            pdic.Add("AZ", dic);
            #endregion

            /*=============================================*/

            #region Bahamas
            dic = new Dictionary<string, string>();
            dic.Add("BS-AK", "Acklins Islands");
            dic.Add("BS-BY", "Berry Islands");
            dic.Add("BS-BI", "Bimini and Cat Cay");
            dic.Add("BS-BP", "Black Point");
            dic.Add("BS-CI", "Cat Island");
            dic.Add("BS-CO", "Central Abaco");
            dic.Add("BS-CS", "Central Andros");
            dic.Add("BS-CE", "Central Eleuthera");
            dic.Add("BS-FP", "City of Freeport");
            dic.Add("BS-CK", "Crooked Island and Long Cay");
            dic.Add("BS-EG", "East Grand Bahama");
            dic.Add("BS-EX", "Exuma");
            dic.Add("BS-GC", "Grand Cay");
            dic.Add("BS-GT", "Green Turtle Cay");
            dic.Add("BS-HI", "Harbour Island");
            dic.Add("BS-HT", "Hope Town");
            dic.Add("BS-IN", "Inagua");
            dic.Add("BS-LI", "Long Island");
            dic.Add("BS-MC", "Mangrove Cay");
            dic.Add("BS-MG", "Mayaguana");
            dic.Add("BS-MI", "Moore's Island");
            dic.Add("BS-NP", "New Providence");
            dic.Add("BS-NO", "North Abaco");
            dic.Add("BS-NS", "North Andros");
            dic.Add("BS-NE", "North Eleuthera");
            dic.Add("BS-RI", "Ragged Island");
            dic.Add("BS-RC", "Rum Cay");
            dic.Add("BS-SS", "San Salvador");
            dic.Add("BS-SO", "South Abaco");
            dic.Add("BS-SA", "South Andros");
            dic.Add("BS-SE", "South Eleuthera");
            dic.Add("BS-SW", "Spanish Wells");
            dic.Add("BS-WG", "West Grand Bahama");
            pdic.Add("BS", dic);
            #endregion

            /*=============================================*/

            #region Bahrain
            dic = new Dictionary<string, string>();
            dic.Add("BH-14", "Al Janubiyah");
            dic.Add("BH-13", "Al Manamah (Al Asimah)");
            dic.Add("BH-15", "Al Muharraq");
            dic.Add("BH-16", "Al Wustá");
            dic.Add("BH-17", "Ash Shamaliyah");
            pdic.Add("BH", dic);
            #endregion

            /*=============================================*/

            #region Bangladesh
            dic = new Dictionary<string, string>();
            dic.Add("BD-05", "Bagerhat zila");
            dic.Add("BD-01", "Bandarban zila");
            dic.Add("BD-02", "Barguna zila");
            dic.Add("BD-06", "Barisal zila");
            dic.Add("BD-07", "Bhola zila");
            dic.Add("BD-03", "Bogra zila");
            dic.Add("BD-04", "Brahmanbaria zila");
            dic.Add("BD-09", "Chandpur zila");
            dic.Add("BD-10", "Chittagong zila");
            dic.Add("BD-12", "Chuadanga zila");
            dic.Add("BD-08", "Comilla zila");
            dic.Add("BD-11", "Cox's Bazar zila");
            dic.Add("BD-13", "Dhaka zila");
            dic.Add("BD-14", "Dinajpur zila");
            dic.Add("BD-15", "Faridpur zila");
            dic.Add("BD-16", "Feni zila");
            dic.Add("BD-19", "Gaibandha zila");
            dic.Add("BD-18", "Gazipur zila");
            dic.Add("BD-17", "Gopalganj zila");
            dic.Add("BD-20", "Habiganj zila");
            dic.Add("BD-24", "Jaipurhat zila");
            dic.Add("BD-21", "Jamalpur zila");
            dic.Add("BD-22", "Jessore zila");
            dic.Add("BD-25", "Jhalakati zila");
            dic.Add("BD-23", "Jhenaidah zila");
            dic.Add("BD-29", "Khagrachari zila");
            dic.Add("BD-27", "Khulna zila");
            dic.Add("BD-26", "Kishoreganj zila");
            dic.Add("BD-28", "Kurigram zila");
            dic.Add("BD-30", "Kushtia zila");
            dic.Add("BD-31", "Lakshmipur zila");
            dic.Add("BD-32", "Lalmonirhat zila");
            dic.Add("BD-36", "Madaripur zila");
            dic.Add("BD-37", "Magura zila");
            dic.Add("BD-33", "Manikganj zila");
            dic.Add("BD-39", "Meherpur zila");
            dic.Add("BD-38", "Moulvibazar zila");
            dic.Add("BD-35", "Munshiganj zila");
            dic.Add("BD-34", "Mymensingh zila");
            dic.Add("BD-48", "Naogaon zila");
            dic.Add("BD-43", "Narail zila");
            dic.Add("BD-40", "Narayanganj zila");
            dic.Add("BD-42", "Narsingdi zila");
            dic.Add("BD-44", "Natore zila");
            dic.Add("BD-45", "Nawabganj zila");
            dic.Add("BD-41", "Netrakona zila");
            dic.Add("BD-46", "Nilphamari zila");
            dic.Add("BD-47", "Noakhali zila");
            dic.Add("BD-49", "Pabna zila");
            dic.Add("BD-52", "Panchagarh zila");
            dic.Add("BD-51", "Patuakhali zila");
            dic.Add("BD-50", "Pirojpur zila");
            dic.Add("BD-53", "Rajbari zila");
            dic.Add("BD-54", "Rajshahi zila");
            dic.Add("BD-56", "Rangamati zila");
            dic.Add("BD-55", "Rangpur zila");
            dic.Add("BD-58", "Satkhira zila");
            dic.Add("BD-62", "Shariatpur zila");
            dic.Add("BD-57", "Sherpur zila");
            dic.Add("BD-59", "Sirajganj zila");
            dic.Add("BD-61", "Sunamganj zila");
            dic.Add("BD-60", "Sylhet zila");
            dic.Add("BD-63", "Tangail zila");
            dic.Add("BD-64", "Thakurgaon zila");
            pdic.Add("BD", dic);
            #endregion

            /*=============================================*/

            #region Barbados
            dic = new Dictionary<string, string>();
            dic.Add("BB-01", "Christ Church");
            dic.Add("BB-02", "Saint Andrew");
            dic.Add("BB-03", "Saint George");
            dic.Add("BB-04", "Saint James");
            dic.Add("BB-05", "Saint John");
            dic.Add("BB-06", "Saint Joseph");
            dic.Add("BB-07", "Saint Lucy");
            dic.Add("BB-08", "Saint Michael");
            dic.Add("BB-09", "Saint Peter");
            dic.Add("BB-10", "Saint Philip");
            dic.Add("BB-11", "Saint Thomas");
            pdic.Add("BB", dic);
            #endregion

            /*=============================================*/

            #region Belarus
            dic = new Dictionary<string, string>();
            dic.Add("BY-BR", "Brestskaya voblasts");
            dic.Add("BY-HO", "Homyel'skaya voblasts");
            dic.Add("BY-HM", "Horad Minsk");
            dic.Add("BY-HR", "Hrodzenskaya voblasts");
            dic.Add("BY-MA", "Mahilyowskaya voblasts");
            dic.Add("BY-MI", "Minskaya voblasts");
            dic.Add("BY-VI", "Vitsyebskaya voblasts");
            pdic.Add("BY", dic);
            #endregion

            /*=============================================*/

            #region Belgium
            dic = new Dictionary<string, string>();
            dic.Add("BE-VAN", "Antwerpen");
            dic.Add("BE-WBR", "Brabant Wallon");
            dic.Add("BE-BRU", "Brussels");
            dic.Add("BE-WHT", "Hainaut");
            dic.Add("BE-WLG", "Liège");
            dic.Add("BE-VLI", "Limburg");
            dic.Add("BE-WLX", "Luxembourg");
            dic.Add("BE-WNA", "Namur");
            dic.Add("BE-VOV", "Oost-Vlaanderen");
            dic.Add("BE-VBR", "Vlaams Brabant");
            dic.Add("BE-VWV", "West-Vlaanderen");
            pdic.Add("BE", dic);
            #endregion

            /*=============================================*/

            #region Belize
            dic = new Dictionary<string, string>();
            dic.Add("BZ-BZ", "Belize");
            dic.Add("BZ-CY", "Cayo");
            dic.Add("BZ-CZL", "Corozal");
            dic.Add("BZ-OW", "Orange Walk");
            dic.Add("BZ-SC", "Stann Creek");
            dic.Add("BZ-TOL", "Toledo");
            pdic.Add("BZ", dic);
            #endregion

            /*=============================================*/

            #region Benin
            dic = new Dictionary<string, string>();
            dic.Add("BJ-AL", "Alibori");
            dic.Add("BJ-AK", "Atakora");
            dic.Add("BJ-AQ", "Atlantique");
            dic.Add("BJ-BO", "Borgou");
            dic.Add("BJ-CO", "Collines");
            dic.Add("BJ-DO", "Donga");
            dic.Add("BJ-KO", "Kouffo");
            dic.Add("BJ-LI", "Littoral");
            dic.Add("BJ-MO", "Mono");
            dic.Add("BJ-OU", "Ouémé");
            dic.Add("BJ-PL", "Plateau");
            dic.Add("BJ-ZO", "Zou");
            pdic.Add("BJ", dic);
            #endregion

            /*=============================================*/

            #region Bhutan
            dic = new Dictionary<string, string>();
            dic.Add("BT-33", "Bumthang");
            dic.Add("BT-12", "Chhukha");
            dic.Add("BT-22", "Dagana");
            dic.Add("BT-GA", "Gasa");
            dic.Add("BT-13", "Ha");
            dic.Add("BT-44", "Lhuentse");
            dic.Add("BT-42", "Monggar");
            dic.Add("BT-11", "Paro");
            dic.Add("BT-43", "Pemagatshel");
            dic.Add("BT-23", "Punakha");
            dic.Add("BT-45", "Samdrup Jongkha");
            dic.Add("BT-14", "Samtse");
            dic.Add("BT-31", "Sarpang");
            dic.Add("BT-15", "Thimphu");
            dic.Add("BT-TY", "Trashi Yangtse");
            dic.Add("BT-41", "Trashigang");
            dic.Add("BT-32", "Trongsa");
            dic.Add("BT-21", "Tsirang");
            dic.Add("BT-24", "Wangdue Phodrang");
            dic.Add("BT-34", "Zhemgang");
            pdic.Add("BT", dic);
            #endregion

            /*=============================================*/

            #region Bolivia
            dic = new Dictionary<string, string>();
            dic.Add("BO-H", "Chuquisaca");
            dic.Add("BO-C", "Cochabamba");
            dic.Add("BO-B", "El Beni");
            dic.Add("BO-L", "La Paz");
            dic.Add("BO-O", "Oruro");
            dic.Add("BO-N", "Pando");
            dic.Add("BO-P", "Potosí");
            dic.Add("BO-S", "Santa Cruz");
            dic.Add("BO-T", "Tarija");
            pdic.Add("BO", dic);
            #endregion

            /*=============================================*/

            #region Botswana
            dic = new Dictionary<string, string>();
            dic.Add("BW-CE", "Central");
            dic.Add("BW-GH", "Ghanzi");
            dic.Add("BW-KG", "Kgalagadi");
            dic.Add("BW-KL", "Kgatleng");
            dic.Add("BW-KW", "Kweneng");
            dic.Add("BW-NE", "North-East");
            dic.Add("BW-NW", "North-West");
            dic.Add("BW-SE", "South-East");
            dic.Add("BW-SO", "Southern");
            pdic.Add("BW", dic);
            #endregion

            /*=============================================*/

            #region Brazil
            dic = new Dictionary<string, string>();
            dic.Add("BR-AC", "Acre");
            dic.Add("BR-AL", "Alagoas");
            dic.Add("BR-AP", "Amapá");
            dic.Add("BR-AM", "Amazonas");
            dic.Add("BR-BA", "Bahia");
            dic.Add("BR-CE", "Ceará");
            dic.Add("BR-DF", "Distrito Federal");
            dic.Add("BR-ES", "Espírito Santo");
            dic.Add("BR-GO", "Goiás");
            dic.Add("BR-MA", "Maranhão");
            dic.Add("BR-MT", "Mato Grosso");
            dic.Add("BR-MS", "Mato Grosso do Sul");
            dic.Add("BR-MG", "Minas Gerais");
            dic.Add("BR-PA", "Pará");
            dic.Add("BR-PB", "Paraíba");
            dic.Add("BR-PR", "Paraná");
            dic.Add("BR-PE", "Pernambuco");
            dic.Add("BR-PI", "Piauí");
            dic.Add("BR-RJ", "Rio de Janeiro");
            dic.Add("BR-RN", "Rio Grande do Norte");
            dic.Add("BR-RS", "Rio Grande do Sul");
            dic.Add("BR-RO", "Rondônia");
            dic.Add("BR-RR", "Roraima");
            dic.Add("BR-SC", "Santa Catarina");
            dic.Add("BR-SP", "São Paulo");
            dic.Add("BR-SE", "Sergipe");
            dic.Add("BR-TO", "Tocantins");
            pdic.Add("BR", dic);
            #endregion

            /*=============================================*/

            #region Brunei Darussalam
            dic = new Dictionary<string, string>();
            dic.Add("BN-BE", "Belait");
            dic.Add("BN-BM", "Brunei-Muara");
            dic.Add("BN-TE", "Temburong");
            dic.Add("BN-TU", "Tutong");
            pdic.Add("BN", dic);
            #endregion

            /*=============================================*/

            #region Bulgaria
            dic = new Dictionary<string, string>();
            dic.Add("BG-01", "Blagoevgrad");
            dic.Add("BG-02", "Burgas");
            dic.Add("BG-08", "Dobrich");
            dic.Add("BG-07", "Gabrovo");
            dic.Add("BG-26", "Haskovo");
            dic.Add("BG-09", "Kardzhali");
            dic.Add("BG-10", "Kjustendil");
            dic.Add("BG-11", "Lovech");
            dic.Add("BG-12", "Montana");
            dic.Add("BG-13", "Pazardzhik");
            dic.Add("BG-14", "Pernik");
            dic.Add("BG-15", "Pleven");
            dic.Add("BG-16", "Plovdiv");
            dic.Add("BG-17", "Razgrad");
            dic.Add("BG-18", "Ruse");
            dic.Add("BG-27", "Shumen");
            dic.Add("BG-19", "Silistra");
            dic.Add("BG-20", "Sliven");
            dic.Add("BG-21", "Smolyan");
            dic.Add("BG-23", "Sofia");
            dic.Add("BG-22", "Sofia-Grad");
            dic.Add("BG-24", "Stara Zagora");
            dic.Add("BG-25", "Targovishte");
            dic.Add("BG-03", "Varna");
            dic.Add("BG-04", "Veliko Tarnovo");
            dic.Add("BG-05", "Vidin");
            dic.Add("BG-06", "Vratsa");
            dic.Add("BG-28", "Yambol");
            pdic.Add("BG", dic);
            #endregion

            /*=============================================*/

            #region Burkina Faso
            dic = new Dictionary<string, string>();
            dic.Add("BF-BAL", "Balé");
            dic.Add("BF-BAM", "Bam");
            dic.Add("BF-BAN", "Banwa");
            dic.Add("BF-BAZ", "Bazèga");
            dic.Add("BF-01", "Boucle du Mouhoun");
            dic.Add("BF-BGR", "Bougouriba");
            dic.Add("BF-BLG", "Boulgou");
            dic.Add("BF-BLK", "Boulkiemdé");
            dic.Add("BF-02", "Cascades");
            dic.Add("BF-03", "Centre");
            dic.Add("BF-04", "Centre-Est");
            dic.Add("BF-05", "Centre-Nord");
            dic.Add("BF-06", "Centre-Ouest");
            dic.Add("BF-07", "Centre-Sud");
            dic.Add("BF-COM", "Comoé");
            dic.Add("BF-08", "Est");
            dic.Add("BF-GAN", "Ganzourgou");
            dic.Add("BF-GNA", "Gnagna");
            dic.Add("BF-GOU", "Gourma");
            dic.Add("BF-09", "Hauts-Bassins");
            dic.Add("BF-HOU", "Houet");
            dic.Add("BF-IOB", "Ioba");
            dic.Add("BF-KAD", "Kadiogo");
            dic.Add("BF-KEN", "Kénédougou");
            dic.Add("BF-KMD", "Komondjari");
            dic.Add("BF-KMP", "Kompienga");
            dic.Add("BF-KOS", "Kossi");
            dic.Add("BF-KOP", "Koulpélogo");
            dic.Add("BF-KOT", "Kouritenga");
            dic.Add("BF-KOW", "Kourwéogo");
            dic.Add("BF-LER", "Léraba");
            dic.Add("BF-LOR", "Loroum");
            dic.Add("BF-MOU", "Mouhoun");
            dic.Add("BF-NAO", "Nahouri");
            dic.Add("BF-NAM", "Namentenga");
            dic.Add("BF-NAY", "Nayala");
            dic.Add("BF-10", "Nord");
            dic.Add("BF-NOU", "Noumbiel");
            dic.Add("BF-OUB", "Oubritenga");
            dic.Add("BF-OUD", "Oudalan");
            dic.Add("BF-PAS", "Passoré");
            dic.Add("BF-11", "Plateau-Central");
            dic.Add("BF-PON", "Poni");
            dic.Add("BF-12", "Sahel");
            dic.Add("BF-SNG", "Sanguié");
            dic.Add("BF-SMT", "Sanmatenga");
            dic.Add("BF-SEN", "Séno");
            dic.Add("BF-SIS", "Sissili");
            dic.Add("BF-SOM", "Soum");
            dic.Add("BF-SOR", "Sourou");
            dic.Add("BF-13", "Sud-Ouest");
            dic.Add("BF-TAP", "Tapoa");
            dic.Add("BF-TUI", "Tui");
            dic.Add("BF-YAG", "Yagha");
            dic.Add("BF-YAT", "Yatenga");
            dic.Add("BF-ZIR", "Ziro");
            dic.Add("BF-ZON", "Zondoma");
            dic.Add("BF-ZOU", "Zoundwéogo");
            pdic.Add("BF", dic);
            #endregion

            /*=============================================*/

            #region Burundi
            dic = new Dictionary<string, string>();
            dic.Add("BI-BB", "Bubanza");
            dic.Add("BI-BM", "Bujumbura Mairie");
            dic.Add("BI-BL", "Bujumbura Rural");
            dic.Add("BI-BR", "Bururi");
            dic.Add("BI-CA", "Cankuzo");
            dic.Add("BI-CI", "Cibitoke");
            dic.Add("BI-GI", "Gitega");
            dic.Add("BI-KR", "Karuzi");
            dic.Add("BI-KY", "Kayanza");
            dic.Add("BI-KI", "Kirundo");
            dic.Add("BI-MA", "Makamba");
            dic.Add("BI-MU", "Muramvya");
            dic.Add("BI-MY", "Muyinga");
            dic.Add("BI-MW", "Mwaro");
            dic.Add("BI-NG", "Ngozi");
            dic.Add("BI-RT", "Rutana");
            dic.Add("BI-RY", "Ruyigi");
            pdic.Add("BI", dic);
            #endregion

            /*=============================================*/

            #region Cambodia
            dic = new Dictionary<string, string>();
            dic.Add("KH-2", "Baat Dambang");
            dic.Add("KH-1", "Banteay Mean Chey");
            dic.Add("KH-3", "Kampong Chaam");
            dic.Add("KH-4", "Kampong Chhnang");
            dic.Add("KH-5", "Kampong Spueu");
            dic.Add("KH-6", "Kampong Thum");
            dic.Add("KH-7", "Kampot");
            dic.Add("KH-8", "Kandaal");
            dic.Add("KH-9", "Kaoh Kong");
            dic.Add("KH-10", "Kracheh");
            dic.Add("KH-23", "Krong Kaeb");
            dic.Add("KH-24", "Krong Pailin");
            dic.Add("KH-18", "Krong Preah Sihanouk");
            dic.Add("KH-11", "Mondol Kiri");
            dic.Add("KH-22", "Otdar Mean Chey");
            dic.Add("KH-12", "Phnom Penh");
            dic.Add("KH-15", "Pousaat");
            dic.Add("KH-13", "Preah Vihear");
            dic.Add("KH-14", "Prey Veaen");
            dic.Add("KH-16", "Rotanak Kiri");
            dic.Add("KH-17", "Siem Reab");
            dic.Add("KH-19", "Stueng Traeng");
            dic.Add("KH-20", "Svaay Rien");
            dic.Add("KH-21", "Taakae");
            pdic.Add("KH", dic);
            #endregion

            /*=============================================*/

            #region Cameroon
            dic = new Dictionary<string, string>();
            dic.Add("CM-AD", "Adamaoua");
            dic.Add("CM-CE", "Centre");
            dic.Add("CM-ES", "East");
            dic.Add("CM-EN", "Far North");
            dic.Add("CM-LT", "Littoral");
            dic.Add("CM-NO", "North");
            dic.Add("CM-NW", "North-West");
            dic.Add("CM-SU", "South");
            dic.Add("CM-SW", "South-West");
            dic.Add("CM-OU", "West");
            pdic.Add("CM", dic);
            #endregion

            /*=============================================*/

            #region Canada
            dic = new Dictionary<string, string>();
            dic.Add("AB", "Alberta");
            dic.Add("BC", "British Columbia");
            dic.Add("MB", "Manitoba");
            dic.Add("NB", "New Brunswick");
            dic.Add("NL", "Newfoundland and Labrador");
            dic.Add("NT", "Northwest Territories");
            dic.Add("NS", "Nova Scotia");
            dic.Add("NU", "Nunavut");
            dic.Add("ON", "Ontario");
            dic.Add("PE", "Prince Edward Island");
            dic.Add("QC", "Quebec");
            dic.Add("SK", "Saskatchewan");
            dic.Add("YT", "Yukon");
            pdic.Add("CA", dic);
            #endregion

            /*=============================================*/

            #region Cape Verde
            dic = new Dictionary<string, string>();
            dic.Add("CV-BV", "Boa Vista");
            dic.Add("CV-BR", "Brava");
            dic.Add("CV-B", "Ilhas de Barlavento");
            dic.Add("CV-S", "Ilhas de Sotavento");
            dic.Add("CV-MA", "Maio");
            dic.Add("CV-MO", "Mosteiros");
            dic.Add("CV-PA", "Paul");
            dic.Add("CV-PN", "Porto Novo");
            dic.Add("CV-PR", "Praia");
            dic.Add("CV-RB", "Ribeira Brava");
            dic.Add("CV-RG", "Ribeira Grande");
            dic.Add("CV-RS", "Ribeira Grande de Santiago");
            dic.Add("CV-CA", "Santa Catarina");
            dic.Add("CV-CF", "Santa Catarina do Fogo");
            dic.Add("CV-CR", "Santa Cruz");
            dic.Add("CV-SD", "São Domingos");
            dic.Add("CV-SF", "São Filipe");
            dic.Add("CV-SL", "São Lourenço dos Órgãos");
            dic.Add("CV-SM", "São Miguel");
            dic.Add("CV-SS", "São Salvador do Mundo");
            dic.Add("CV-SV", "São Vicente");
            dic.Add("CV-TA", "Tarrafal");
            dic.Add("CV-TS", "Tarrafal de São Nicolau");
            pdic.Add("CV", dic);
            #endregion

            /*=============================================*/

            #region Cayman Islands
            dic = new Dictionary<string, string>();
            dic.Add("KY-01", "Bodden Town");
            dic.Add("KY-02", "Cayman Brac");
            dic.Add("KY-03", "East End");
            dic.Add("KY-04", "George Town");
            dic.Add("Ky-05", "Little Cayman");
            dic.Add("KY-06", "North Side");
            dic.Add("KY-07", "West Bay");
            pdic.Add("KY", dic);
            #endregion

            /*=============================================*/

            #region Central African Republic
            dic = new Dictionary<string, string>();
            dic.Add("CF-BB", "Bamingui-Bangoran");
            dic.Add("CF-BGF", "Bangui");
            dic.Add("CF-BK", "Basse-Kotto");
            dic.Add("CF-KB", "Gribingui");
            dic.Add("CF-HM", "Haut-Mbomou");
            dic.Add("CF-HK", "Haute-Kotto");
            dic.Add("CF-HS", "Haute-Sangha / Mambéré-Kadéï");
            dic.Add("CF-KG", "Kémo-Gribingui");
            dic.Add("CF-LB", "Lobaye");
            dic.Add("CF-MB", "Mbomou");
            dic.Add("CF-NM", "Nana-Mambéré");
            dic.Add("CF-MP", "Ombella-Mpoko");
            dic.Add("CF-UK", "Ouaka");
            dic.Add("CF-AC", "Ouham");
            dic.Add("CF-OP", "Ouham-Pendé");
            dic.Add("CF-SE", "Sangha");
            dic.Add("CF-VK", "Vakaga");
            pdic.Add("CF", dic);
            #endregion

            /*=============================================*/

            #region Chile
            dic = new Dictionary<string, string>();
            dic.Add("CL-AI", "Aisén del General Carlos Ibáñez del Campo");
            dic.Add("CL-AN", "Antofagasta");
            dic.Add("CL-AR", "Araucanía");
            dic.Add("CL-AP", "Arica y Parinacota");
            dic.Add("CL-AT", "Atacama");
            dic.Add("CL-BI", "Bío-Bío");
            dic.Add("CL-CO", "Coquimbo");
            dic.Add("CL-LI", "Libertador General Bernardo O'Higgins");
            dic.Add("CL-LL", "Los Lagos");
            dic.Add("CL-LR", "Los Ríos");
            dic.Add("CL-MA", "Magallanes");
            dic.Add("CL-ML", "Maule");
            dic.Add("CL-RM", "Región Metropolitana de Santiago");
            dic.Add("CL-TA", "Tarapacá");
            dic.Add("CL-VS", "Valparaíso");
            pdic.Add("CL", dic);
            #endregion

            /*=============================================*/

            #region China
            dic = new Dictionary<string, string>();
            dic.Add("CN-34", "Anhui");
            dic.Add("CN-92", "Aomen");
            dic.Add("CN-11", "Beijing");
            dic.Add("CN-50", "Chongqing");
            dic.Add("CN-35", "Fujian");
            dic.Add("CN-62", "Gansu");
            dic.Add("CN-44", "Guangdong");
            dic.Add("CN-45", "Guangxi");
            dic.Add("CN-52", "Guizhou");
            dic.Add("CN-46", "Hainan");
            dic.Add("CN-13", "Hebei");
            dic.Add("CN-23", "Heilongjiang");
            dic.Add("CN-41", "Henan");
            dic.Add("CN-42", "Hubei");
            dic.Add("CN-43", "Hunan");
            dic.Add("CN-32", "Jiangsu");
            dic.Add("CN-36", "Jiangxi");
            dic.Add("CN-22", "Jilin");
            dic.Add("CN-21", "Liaoning");
            dic.Add("CN-15", "Nei Mongol");
            dic.Add("CN-64", "Ningxia");
            dic.Add("CN-63", "Qinghai");
            dic.Add("CN-61", "Shaanxi");
            dic.Add("CN-37", "Shandong");
            dic.Add("CN-31", "Shanghai");
            dic.Add("CN-14", "Shanxi");
            dic.Add("CN-51", "Sichuan");
            dic.Add("CN-71", "Taiwan");
            dic.Add("CN-12", "Tianjin");
            dic.Add("CN-91", "Xianggang");
            dic.Add("CN-65", "Xinjiang");
            dic.Add("CN-54", "Xizang");
            dic.Add("CN-53", "Yunnan");
            dic.Add("CN-33", "Zhejiang");
            pdic.Add("CN", dic);
            #endregion

            /*=============================================*/

            #region Colombia
            dic = new Dictionary<string, string>();
            dic.Add("CO-AMA", "Amazonas");
            dic.Add("CO-ANT", "Antioquia");
            dic.Add("CO-ARA", "Arauca");
            dic.Add("CO-ATL", "Atlántico");
            dic.Add("CO-BOL", "Bolívar");
            dic.Add("CO-BOY", "Boyacá");
            dic.Add("CO-CAL", "Caldas");
            dic.Add("CO-CAQ", "Caquetá");
            dic.Add("CO-CAS", "Casanare");
            dic.Add("CO-CAU", "Cauca");
            dic.Add("CO-CES", "Cesar");
            dic.Add("CO-CHO", "Chocó");
            dic.Add("CO-COR", "Córdoba");
            dic.Add("CO-CUN", "Cundinamarca");
            dic.Add("CO-DC", "Distrito Capital de Bogotá");
            dic.Add("CO-GUA", "Guainía");
            dic.Add("CO-GUV", "Guaviare");
            dic.Add("CO-HUI", "Huila");
            dic.Add("CO-LAG", "La Guajira");
            dic.Add("CO-MAG", "Magdalena");
            dic.Add("CO-MET", "Meta");
            dic.Add("CO-NAR", "Nariño");
            dic.Add("CO-NSA", "Norte de Santander");
            dic.Add("CO-PUT", "Putumayo");
            dic.Add("CO-QUI", "Quindío");
            dic.Add("CO-RIS", "Risaralda");
            dic.Add("CO-SAP", "San Andrés, Providencia y Santa Catalina");
            dic.Add("CO-SAN", "Santander");
            dic.Add("CO-SUC", "Sucre");
            dic.Add("CO-TOL", "Tolima");
            dic.Add("CO-VAC", "Valle del Cauca");
            dic.Add("CO-VAU", "Vaupés");
            dic.Add("CO-VID", "Vichada");
            pdic.Add("CO", dic);
            #endregion

            /*=============================================*/

            #region Comoros
            dic = new Dictionary<string, string>();
            dic.Add("KM-G", "Andjazîdja");
            dic.Add("KM-A", "Andjouân");
            dic.Add("KM-M", "Moûhîlî");
            pdic.Add("KM", dic);
            #endregion

            /*=============================================*/

            #region Congo
            dic = new Dictionary<string, string>();
            dic.Add("CG-11", "Bouenza");
            dic.Add("CG-BZV", "Brazzaville");
            dic.Add("CG-8", "Cuvette");
            dic.Add("CG-15", "Cuvette-Ouest");
            dic.Add("CG-5", "Kouilou");
            dic.Add("CG-2", "Lékoumou");
            dic.Add("CG-7", "Likouala");
            dic.Add("CG-9", "Niari");
            dic.Add("CG-14", "Plateaux");
            dic.Add("CG-12", "Pool");
            dic.Add("CG-13", "Sangha");
            pdic.Add("CG", dic);
            #endregion

            /*=============================================*/

            #region Costa Rica
            dic = new Dictionary<string, string>();
            dic.Add("CR-A", "Alajuela");
            dic.Add("CR-C", "Cartago");
            dic.Add("CR-G", "Guanacaste");
            dic.Add("CR-H", "Heredia");
            dic.Add("CR-L", "Limón");
            dic.Add("CR-P", "Puntarenas");
            dic.Add("CR-SJ", "San José");
            pdic.Add("CR", dic);
            #endregion

            /*=============================================*/

            #region Cote D'Ivoire
            dic = new Dictionary<string, string>();
            dic.Add("CI-06", "18 Montagnes");
            dic.Add("CI-16", "Agnébi");
            dic.Add("CI-17", "Bafing");
            dic.Add("CI-09", "Bas-Sassandra");
            dic.Add("CI-10", "Denguélé");
            dic.Add("CI-18", "Fromager");
            dic.Add("CI-02", "Haut-Sassandra");
            dic.Add("CI-07", "Lacs");
            dic.Add("CI-01", "Lagunes");
            dic.Add("CI-12", "Marahoué");
            dic.Add("CI-19", "Moyen-Cavally");
            dic.Add("CI-05", "Moyen-Comoé");
            dic.Add("CI-11", "Nzi-Comoé");
            dic.Add("CI-03", "Savanes");
            dic.Add("CI-15", "Sud-Bandama");
            dic.Add("CI-13", "Sud-Comoé");
            dic.Add("CI-04", "Vallée du Bandama");
            dic.Add("CI-14", "Worodougou");
            dic.Add("CI-08", "Zanzan");
            pdic.Add("CI", dic);
            #endregion

            /*=============================================*/

            #region Cuba
            dic = new Dictionary<string, string>();
            dic.Add("CU-09", "Camagüey");
            dic.Add("CU-08", "Ciego de Ávila");
            dic.Add("CU-06", "Cienfuegos");
            dic.Add("CU-03", "Ciudad de La Habana");
            dic.Add("CU-12", "Granma");
            dic.Add("CU-14", "Guantánamo");
            dic.Add("CU-11", "Holguín");
            dic.Add("CU-99", "Isla de la Juventud");
            dic.Add("CU-02", "La Habana");
            dic.Add("CU-10", "Las Tunas");
            dic.Add("CU-04", "Matanzas");
            dic.Add("CU-01", "Pinar del Río");
            dic.Add("CU-07", "Sancti Spíritus");
            dic.Add("CU-13", "Santiago de Cuba");
            dic.Add("CU-05", "Villa Clara");
            pdic.Add("CU", dic);
            #endregion

            /*=============================================*/

            #region Cyprus
            dic = new Dictionary<string, string>();
            dic.Add("CY-04", "Ammochostos");
            dic.Add("CY-06", "Keryneia");
            dic.Add("CY-03", "Larnaka");
            dic.Add("CY-01", "Lefkosia");
            dic.Add("CY-02", "Lemesos");
            dic.Add("CY-05", "Pafos");
            pdic.Add("CY", dic);
            #endregion

            /*=============================================*/

            #region Czech Republic
            dic = new Dictionary<string, string>();
            dic.Add("CZ-JC", "Jihoceský kraj");
            dic.Add("CZ-JM", "Jihomoravský kraj");
            dic.Add("CZ-KA", "Karlovarský kraj");
            dic.Add("CZ-KR", "Královéhradecký kraj");
            dic.Add("CZ-LI", "Liberecký kraj");
            dic.Add("CZ-MO", "Moravskoslezský kraj");
            dic.Add("CZ-OL", "Olomoucký kraj");
            dic.Add("CZ-PA", "Pardubický kraj");
            dic.Add("CZ-PL", "Plzenský kraj");
            dic.Add("CZ-PR", "Praha, hlavní mesto");
            dic.Add("CZ-ST", "Stredoceský kraj");
            dic.Add("CZ-US", "Ústecký kraj");
            dic.Add("CZ-VY", "Vysocina");
            dic.Add("CZ-ZL", "Zlínský kraj");
            pdic.Add("CZ", dic);
            #endregion

            /*=============================================*/

            #region Denmark
            dic = new Dictionary<string, string>();
            dic.Add("DK-070", "Århus");
            dic.Add("DK-040", "Bornholm");
            dic.Add("DK-84", "Capital");
            dic.Add("DK-82", "Central Jutland");
            dic.Add("DK-147", "Frederiksberg City");
            dic.Add("DK-020", "Frederiksborg");
            dic.Add("DK-042", "Fyn");
            dic.Add("DK-015", "København");
            dic.Add("DK-101", "København City");
            dic.Add("DK-080", "Nordjylland");
            dic.Add("DK-81", "North Jutland");
            dic.Add("DK-055", "Ribe");
            dic.Add("DK-065", "Ringkøbing");
            dic.Add("DK-025", "Roskilde");
            dic.Add("DK-83", "South Denmark");
            dic.Add("DK-035", "Storstrøm");
            dic.Add("DK-050", "Sønderjylland");
            dic.Add("DK-060", "Vejle");
            dic.Add("DK-030", "Vestsjælland");
            dic.Add("DK-076", "Viborg");
            dic.Add("DK-85", "Zeeland");
            pdic.Add("DK", dic);
            #endregion

            /*=============================================*/

            #region Djibouti
            dic = new Dictionary<string, string>();
            dic.Add("DJ-AS", "Ali Sabieh");
            dic.Add("DJ-AR", "Arta");
            dic.Add("DJ-DI", "Dikhil");
            dic.Add("DJ-DJ", "Djibouti");
            dic.Add("DJ-OB", "Obock");
            dic.Add("DJ-TA", "Tadjourah");
            pdic.Add("DJ", dic);
            #endregion

            /*=============================================*/

            #region Dominica
            dic = new Dictionary<string, string>();
            dic.Add("DM-02", "Saint Andrew");
            dic.Add("DM-03", "Saint David");
            dic.Add("DM-04", "Saint George");
            dic.Add("DM-05", "Saint John");
            dic.Add("DM-06", "Saint Joseph");
            dic.Add("DM-07", "Saint Luke");
            dic.Add("DM-08", "Saint Mark");
            dic.Add("DM-09", "Saint Patrick");
            dic.Add("DM-10", "Saint Paul");
            dic.Add("DM-11", "Saint Peter");
            pdic.Add("DM", dic);
            #endregion

            /*=============================================*/

            #region Dominican Republic
            dic = new Dictionary<string, string>();
            dic.Add("DO-02", "Azua");
            dic.Add("DO-03", "Bahoruco");
            dic.Add("DO-04", "Barahona");
            dic.Add("DO-05", "Dajabón");
            dic.Add("DO-01", "Distrito Nacional (Santo Domingo)");
            dic.Add("DO-06", "Duarte");
            dic.Add("DO-08", "El Seybo");
            dic.Add("DO-09", "Espaillat");
            dic.Add("DO-30", "Hato Mayor");
            dic.Add("DO-10", "Independencia");
            dic.Add("DO-11", "La Altagracia");
            dic.Add("DO-07", "La Estrellet");
            dic.Add("DO-12", "La Romana");
            dic.Add("DO-13", "La Vega");
            dic.Add("DO-14", "María Trinidad Sánchez");
            dic.Add("DO-28", "Monseñor Nouel");
            dic.Add("DO-15", "Monte Cristi");
            dic.Add("DO-29", "Monte Plata");
            dic.Add("DO-16", "Pedernales");
            dic.Add("DO-17", "Peravia");
            dic.Add("DO-18", "Puerto Plata");
            dic.Add("DO-19", "Salcedo");
            dic.Add("DO-20", "Samaná");
            dic.Add("DO-21", "San Cristóbal");
            dic.Add("DO-31", "San Jose de Ocoa");
            dic.Add("DO-22", "San Juan");
            dic.Add("DO-23", "San Pedro de Macorís");
            dic.Add("DO-24", "Sánchez Ramírez");
            dic.Add("DO-25", "Santiago");
            dic.Add("DO-26", "Santiago Rodríguez");
            dic.Add("DO-27", "Valverde");
            pdic.Add("DO", dic);
            #endregion

            /*=============================================*/

            #region Ecuador
            dic = new Dictionary<string, string>();
            dic.Add("EC-A", "Azuay");
            dic.Add("EC-B", "Bolívar");
            dic.Add("EC-F", "Cañar");
            dic.Add("EC-C", "Carchi");
            dic.Add("EC-H", "Chimborazo");
            dic.Add("EC-X", "Cotopaxi");
            dic.Add("EC-O", "El Oro");
            dic.Add("EC-E", "Esmeraldas");
            dic.Add("EC-W", "Galápagos");
            dic.Add("EC-G", "Guayas");
            dic.Add("EC-I", "Imbabura");
            dic.Add("EC-L", "Loja");
            dic.Add("EC-R", "Los Ríos");
            dic.Add("EC-M", "Manabí");
            dic.Add("EC-S", "Morona-Santiago");
            dic.Add("EC-N", "Napo");
            dic.Add("EC-D", "Orellana");
            dic.Add("EC-Y", "Pastaza");
            dic.Add("EC-P", "Pichincha");
            dic.Add("EC-SE", "Santa Elena");
            dic.Add("EC-SD", "Santo Domingo de los Tsachilas");
            dic.Add("EC-U", "Sucumbíos");
            dic.Add("EC-T", "Tungurahua");
            dic.Add("EC-Z", "Zamora-Chinchipe");
            pdic.Add("EC", dic);
            #endregion

            /*=============================================*/

            #region El Salvador
            dic = new Dictionary<string, string>();
            dic.Add("SV-AH", "Ahuachapán");
            dic.Add("SV-CA", "Cabañas");
            dic.Add("SV-CH", "Chalatenango");
            dic.Add("SV-CU", "Cuscatlán");
            dic.Add("SV-LI", "La Libertad");
            dic.Add("SV-PA", "La Paz");
            dic.Add("SV-UN", "La Unión");
            dic.Add("SV-MO", "Morazán");
            dic.Add("SV-SM", "San Miguel");
            dic.Add("SV-SS", "San Salvador");
            dic.Add("SV-SV", "San Vicente");
            dic.Add("SV-SA", "Santa Ana");
            dic.Add("SV-SO", "Sonsonate");
            dic.Add("SV-US", "Usulután");
            pdic.Add("SV", dic);
            #endregion

            /*=============================================*/

            #region England
            dic = new Dictionary<string, string>();
            dic.Add("ENG-BNS", "Barnsley");
            dic.Add("ENG-BAS", "Bath and North East Somerset");
            dic.Add("ENG-BBO", "Bedford Borough");
            dic.Add("ENG-BIR", "Birmingham");
            dic.Add("ENG-BBD", "Blackburn with Darwen");
            dic.Add("ENG-BPL", "Blackpool");
            dic.Add("ENG-BOL", "Bolton");
            dic.Add("ENG-BMH", "Bournemouth");
            dic.Add("ENG-BRC", "Bracknell Forest");
            dic.Add("ENG-BRD", "Bradford");
            dic.Add("ENG-BNH", "Brighton and Hove");
            dic.Add("ENG-BST", "Bristol, City of");
            dic.Add("ENG-BUC", "Buckingham");
            dic.Add("ENG-BKM", "Buckinghamshire");
            dic.Add("ENG-BUR", "Bury");
            dic.Add("ENG-CLD", "Calderdale");
            dic.Add("ENG-CAM", "Cambridgeshire");
            dic.Add("ENG-CHE", "Cheshire East");
            dic.Add("ENG-CWC", "Cheshire West and Chester");
            dic.Add("ENG-CON", "Cornwall");
            dic.Add("ENG-COV", "Coventry");
            dic.Add("ENG-CMA", "Cumbria");
            dic.Add("ENG-DAL", "Darlington");
            dic.Add("ENG-DER", "Derby");
            dic.Add("ENG-DBY", "Derbyshire");
            dic.Add("ENG-DEV", "Devon");
            dic.Add("ENG-DNC", "Doncaster");
            dic.Add("ENG-DOR", "Dorset");
            dic.Add("ENG-DUD", "Dudley");
            dic.Add("ENG-ERY", "East Riding of Yorkshire");
            dic.Add("ENG-ESX", "East Sussex");
            dic.Add("ENG-ESS", "Essex");
            dic.Add("ENG-FLN", "Flintshire");
            dic.Add("ENG-GAT", "Gateshead");
            dic.Add("ENG-GLS", "Gloucestershire");
            dic.Add("ENG-GLO", "Greater London");
            dic.Add("ENG-HAL", "Halton");
            dic.Add("ENG-HAM", "Hampshire");
            dic.Add("ENG-HPL", "Hartlepool");
            dic.Add("ENG-HEF", "Herefordshire, County of");
            dic.Add("ENG-HTF", "Hertford");
            dic.Add("ENG-HRT", "Hertfordshire");
            dic.Add("ENG-IOW", "Isle of Wight");
            dic.Add("ENG-IOS", "Isles of Scilly");
            dic.Add("ENG-KEN", "Kent");
            dic.Add("ENG-KHL", "Kingston upon Hull, City of");
            dic.Add("ENG-KIR", "Kirklees");
            dic.Add("ENG-KWL", "Knowsley");
            dic.Add("ENG-LAN", "Lancashire");
            dic.Add("ENG-LDS", "Leeds");
            dic.Add("ENG-LCE", "Leicester");
            dic.Add("ENG-LEC", "Leicestershire");
            dic.Add("ENG-LIN", "Lincolnshire");
            dic.Add("ENG-LIV", "Liverpool");
            dic.Add("ENG-LND", "London, City of");
            dic.Add("ENG-LUT", "Luton");
            dic.Add("ENG-MAN", "Manchester");
            dic.Add("ENG-MDW", "Medway");
            dic.Add("ENG-MDB", "Middlesbrough");
            dic.Add("ENG-MIK", "Milton Keynes");
            dic.Add("ENG-NET", "Newcastle upon Tyne");
            dic.Add("ENG-NFK", "Norfolk");
            dic.Add("ENG-NEL", "North East Lincolnshire");
            dic.Add("ENG-NLN", "North Lincolnshire");
            dic.Add("ENG-NSM", "North Somerset");
            dic.Add("ENG-NTY", "North Tyneside");
            dic.Add("ENG-NYK", "North Yorkshire");
            dic.Add("ENG-NTH", "Northamptonshire");
            dic.Add("ENG-NBL", "Northumberland");
            dic.Add("ENG-NGM", "Nottingham");
            dic.Add("ENG-NTT", "Nottinghamshire");
            dic.Add("ENG-OLD", "Oldham");
            dic.Add("ENG-OXF", "Oxfordshire");
            dic.Add("ENG-PTE", "Peterborough");
            dic.Add("ENG-PLY", "Plymouth");
            dic.Add("ENG-POL", "Poole");
            dic.Add("ENG-POR", "Portsmouth");
            dic.Add("ENG-RDG", "Reading");
            dic.Add("ENG-RCC", "Redcar and Cleveland");
            dic.Add("ENG-RCH", "Rochdale");
            dic.Add("ENG-ROT", "Rotherham");
            dic.Add("ENG-RUT", "Rutland");
            dic.Add("ENG-SLF", "Salford");
            dic.Add("ENG-SAW", "Sandwell");
            dic.Add("ENG-SFT", "Sefton");
            dic.Add("ENG-SHF", "Sheffield");
            dic.Add("ENG-SHR", "Shropshire");
            dic.Add("ENG-SLG", "Slough");
            dic.Add("ENG-SOL", "Solihull");
            dic.Add("ENG-SOM", "Somerset");
            dic.Add("ENG-SGC", "South Gloucestershire");
            dic.Add("ENG-STY", "South Tyneside");
            dic.Add("ENG-SYK", "South Yorkshire");
            dic.Add("ENG-STH", "Southampton");
            dic.Add("ENG-SOS", "Southend-on-Sea");
            dic.Add("ENG-SHN", "St. Helens");
            dic.Add("ENG-STS", "Staffordshire");
            dic.Add("ENG-SKP", "Stockport");
            dic.Add("ENG-STT", "Stockton-on-Tees");
            dic.Add("ENG-STE", "Stoke-on-Trent");
            dic.Add("ENG-SFK", "Suffolk");
            dic.Add("ENG-SND", "Sunderland");
            dic.Add("ENG-SRY", "Surrey");
            dic.Add("ENG-SWD", "Swindon");
            dic.Add("ENG-TAM", "Tameside");
            dic.Add("ENG-TFW", "Telford and Wrekin");
            dic.Add("ENG-THR", "Thurrock");
            dic.Add("ENG-TOB", "Torbay");
            dic.Add("ENG-TRF", "Trafford");
            dic.Add("ENG-WKF", "Wakefield");
            dic.Add("ENG-WLL", "Walsall");
            dic.Add("ENG-WRT", "Warrington");
            dic.Add("ENG-WAR", "Warwickshire");
            dic.Add("ENG-WBK", "West Berkshire");
            dic.Add("ENG-WMD", "West Midlands");
            dic.Add("ENG-WSX", "West Sussex");
            dic.Add("ENG-WYK", "West Yorkshire");
            dic.Add("ENG-WGN", "Wigan");
            dic.Add("ENG-WIL", "Wiltshire");
            dic.Add("ENG-WNM", "Windsor and Maidenhead");
            dic.Add("ENG-WRL", "Wirral");
            dic.Add("ENG-WOK", "Wokingham");
            dic.Add("ENG-WLV", "Wolverhampton");
            dic.Add("ENG-WOC", "Worcester");
            dic.Add("ENG-WOR", "Worcestershire");
            dic.Add("ENG-YOR", "York");
            pdic.Add("EN", dic);
            #endregion

            /*=============================================*/

            #region Equatorial Guinea
            dic = new Dictionary<string, string>();
            dic.Add("GQ-AN", "Annobón");
            dic.Add("GQ-BN", "Bioko Norte");
            dic.Add("GQ-BS", "Bioko Sur");
            dic.Add("GQ-CS", "Centro Sur");
            dic.Add("GQ-KN", "Kie-Ntem");
            dic.Add("GQ-LI", "Litoral");
            dic.Add("GQ-C", "Región Continental");
            dic.Add("GQ-I", "Región Insular");
            dic.Add("GQ-WN", "Wele-Nzás");
            pdic.Add("GQ", dic);
            #endregion

            /*=============================================*/

            #region Eritrea
            dic = new Dictionary<string, string>();
            dic.Add("ER-AN", "Anseba");
            dic.Add("ER-DU", "Debub");
            dic.Add("ER-DK", "Debubawi Keyih Bahari");
            dic.Add("ER-GB", "Gash-Barka");
            dic.Add("ER-MA", "Maakel");
            dic.Add("ER-SK", "Semenawi Keyih Bahari");
            pdic.Add("ER", dic);
            #endregion

            /*=============================================*/

            #region Estonia
            dic = new Dictionary<string, string>();
            dic.Add("EE-37", "Harjumaa");
            dic.Add("EE-39", "Hiiumaa");
            dic.Add("EE-44", "Ida-Virumaa");
            dic.Add("EE-51", "Järvamaa");
            dic.Add("EE-49", "Jõgevamaa");
            dic.Add("EE-59", "Lääne-Virumaa");
            dic.Add("EE-57", "Läänemaa");
            dic.Add("EE-67", "Pärnumaa");
            dic.Add("EE-65", "Põlvamaa");
            dic.Add("EE-70", "Raplamaa");
            dic.Add("EE-74", "Saaremaa");
            dic.Add("EE-78", "Tartumaa");
            dic.Add("EE-82", "Valgamaa");
            dic.Add("EE-84", "Viljandimaa");
            dic.Add("EE-86", "Võrumaa");
            pdic.Add("EE", dic);
            #endregion

            /*=============================================*/

            #region Ethiopia
            dic = new Dictionary<string, string>();
            dic.Add("ET-AA", "Adis Abeba");
            dic.Add("ET-AF", "Afar");
            dic.Add("ET-AM", "Amara");
            dic.Add("ET-BE", "Binshangul Gumuz");
            dic.Add("ET-DD", "Dire Dawa");
            dic.Add("ET-GA", "Gambela Hizboch");
            dic.Add("ET-HA", "Hareri Hizb");
            dic.Add("ET-OR", "Oromiya");
            dic.Add("ET-SO", "Sumale");
            dic.Add("ET-TI", "Tigray");
            dic.Add("ET-SN", "YeDebub Biheroch Bihereseboch na Hizboch");
            pdic.Add("ET", dic);
            #endregion

            /*=============================================*/

            #region Fiji
            dic = new Dictionary<string, string>();
            dic.Add("FJ-C", "Central");
            dic.Add("FJ-E", "Eastern");
            dic.Add("FJ-N", "Northern");
            dic.Add("FJ-R", "Rotuma");
            dic.Add("FJ-W", "Western");
            pdic.Add("FJ", dic);
            #endregion

            /*=============================================*/

            #region Finland
            dic = new Dictionary<string, string>();
            dic.Add("FI-AL", "Ahvenanmaan lääni");
            dic.Add("FI-ES", "Etelä-Suomen lääni");
            dic.Add("FI-IS", "Itä-Suomen lääni");
            dic.Add("FI-LS", "Länsi-Suomen lääni");
            dic.Add("FI-LL", "Lapin lääni");
            dic.Add("FI-OL", "Oulun lääni");
            pdic.Add("FI", dic);
            #endregion

            /*=============================================*/

            #region France
            dic = new Dictionary<string, string>();
            dic.Add("FR-01", "Ain");
            dic.Add("FR-02", "Aisne");
            dic.Add("FR-03", "Allier");
            dic.Add("FR-04", "Alpes-de-Haute-Provence");
            dic.Add("FR-06", "Alpes-Maritimes");
            dic.Add("FR-07", "Ardèche");
            dic.Add("FR-08", "Ardennes");
            dic.Add("FR-09", "Ariège");
            dic.Add("FR-10", "Aube");
            dic.Add("FR-11", "Aude");
            dic.Add("FR-12", "Aveyron");
            dic.Add("FR-67", "Bas-Rhin");
            dic.Add("FR-13", "Bouches-du-Rhône");
            dic.Add("FR-14", "Calvados");
            dic.Add("FR-15", "Cantal");
            dic.Add("FR-16", "Charente");
            dic.Add("FR-17", "Charente-Maritime");
            dic.Add("FR-18", "Cher");
            dic.Add("FR-CP", "Clipperton");
            dic.Add("FR-19", "Corrèze");
            dic.Add("FR-2A", "Corse-du-Sud");
            dic.Add("FR-21", "Côte-d'Or");
            dic.Add("FR-22", "Côtes-d'Armor");
            dic.Add("FR-23", "Creuse");
            dic.Add("FR-79", "Deux-Sèvres");
            dic.Add("FR-24", "Dordogne");
            dic.Add("FR-25", "Doubs");
            dic.Add("FR-26", "Drôme");
            dic.Add("FR-91", "Essonne");
            dic.Add("FR-27", "Eure");
            dic.Add("FR-28", "Eure-et-Loir");
            dic.Add("FR-29", "Finistère");
            dic.Add("FR-30", "Gard");
            dic.Add("FR-32", "Gers");
            dic.Add("FR-33", "Gironde");
            dic.Add("FR-68", "Haut-Rhin");
            dic.Add("FR-2B", "Haute-Corse");
            dic.Add("FR-31", "Haute-Garonne");
            dic.Add("FR-43", "Haute-Loire");
            dic.Add("FR-52", "Haute-Marne");
            dic.Add("FR-70", "Haute-Saône");
            dic.Add("FR-74", "Haute-Savoie");
            dic.Add("FR-87", "Haute-Vienne");
            dic.Add("FR-05", "Hautes-Alpes");
            dic.Add("FR-65", "Hautes-Pyrénées");
            dic.Add("FR-92", "Hauts-de-Seine");
            dic.Add("FR-34", "Hérault");
            dic.Add("FR-35", "Ille-et-Vilaine");
            dic.Add("FR-36", "Indre");
            dic.Add("FR-37", "Indre-et-Loire");
            dic.Add("FR-38", "Isère");
            dic.Add("FR-39", "Jura");
            dic.Add("FR-40", "Landes");
            dic.Add("FR-41", "Loir-et-Cher");
            dic.Add("FR-42", "Loire");
            dic.Add("FR-44", "Loire-Atlantique");
            dic.Add("FR-45", "Loiret");
            dic.Add("FR-46", "Lot");
            dic.Add("FR-47", "Lot-et-Garonne");
            dic.Add("FR-48", "Lozère");
            dic.Add("FR-49", "Maine-et-Loire");
            dic.Add("FR-50", "Manche");
            dic.Add("FR-51", "Marne");
            dic.Add("FR-53", "Mayenne");
            dic.Add("FR-YT", "Mayotte");
            dic.Add("FR-54", "Meurthe-et-Moselle");
            dic.Add("FR-55", "Meuse");
            dic.Add("FR-56", "Morbihan");
            dic.Add("FR-57", "Moselle");
            dic.Add("FR-58", "Nièvre");
            dic.Add("FR-59", "Nord");
            dic.Add("FR-NC", "Nouvelle-Calédonie");
            dic.Add("FR-60", "Oise");
            dic.Add("FR-61", "Orne");
            dic.Add("FR-75", "Paris");
            dic.Add("FR-62", "Pas-de-Calais");
            dic.Add("FR-PF", "Polynésie française");
            dic.Add("FR-63", "Puy-de-Dôme");
            dic.Add("FR-64", "Pyrénées-Atlantiques");
            dic.Add("FR-66", "Pyrénées-Orientales");
            dic.Add("FR-69", "Rhône");
            dic.Add("FR-BL", "Saint-Barthélemy");
            dic.Add("FR-MF", "Saint-Martin");
            dic.Add("FR-PM", "Saint-Pierre-et-Miquelon");
            dic.Add("FR-71", "Saône-et-Loire");
            dic.Add("FR-72", "Sarthe");
            dic.Add("FR-73", "Savoie");
            dic.Add("FR-77", "Seine-et-Marne");
            dic.Add("FR-76", "Seine-Maritime");
            dic.Add("FR-93", "Seine-Saint-Denis");
            dic.Add("FR-80", "Somme");
            dic.Add("FR-81", "Tarn");
            dic.Add("FR-82", "Tarn-et-Garonne");
            dic.Add("FR-TF", "Terres Australes Françaises");
            dic.Add("FR-90", "Territoire de Belfort");
            dic.Add("FR-95", "Val-d'Oise");
            dic.Add("FR-94", "Val-de-Marne");
            dic.Add("FR-83", "Var");
            dic.Add("FR-84", "Vaucluse");
            dic.Add("FR-85", "Vendée");
            dic.Add("FR-86", "Vienne");
            dic.Add("FR-88", "Vosges");
            dic.Add("FR-WF", "Wallis et Futuna");
            dic.Add("FR-89", "Yonne");
            dic.Add("FR-78", "Yvelines");
            pdic.Add("FR", dic);
            #endregion

            /*=============================================*/

            #region French Southern Territories
            dic = new Dictionary<string, string>();
            dic.Add("TF-X2", "Crozet Islands");
            dic.Add("TF-X1", "Ile Saint-Paul et Ile Amsterdam");
            dic.Add("TF-X4", "Iles Eparses");
            dic.Add("TF-X3", "Kerguelen");
            pdic.Add("TF", dic);
            #endregion

            /*=============================================*/

            #region Gabon
            dic = new Dictionary<string, string>();
            dic.Add("GA-1", "Estuaire");
            dic.Add("GA-2", "Haut-Ogooué");
            dic.Add("GA-3", "Moyen-Ogooué");
            dic.Add("GA-4", "Ngounié");
            dic.Add("GA-5", "Nyanga");
            dic.Add("GA-6", "Ogooué-Ivindo");
            dic.Add("GA-7", "Ogooué-Lolo");
            dic.Add("GA-8", "Ogooué-Maritime");
            dic.Add("GA-9", "Woleu-Ntem");
            pdic.Add("GA", dic);
            #endregion

            /*=============================================*/

            #region Gambia
            dic = new Dictionary<string, string>();
            dic.Add("GM-B", "Banjul");
            dic.Add("GM-L", "Lower River");
            dic.Add("GM-M", "MacCarthy Island");
            dic.Add("GM-N", "North Bank");
            dic.Add("GM-U", "Upper River");
            dic.Add("GM-W", "Western");
            pdic.Add("GM", dic);
            #endregion

            /*=============================================*/

            #region Georgia
            dic = new Dictionary<string, string>();
            dic.Add("GE-AB", "Abkhazia");
            dic.Add("GE-AJ", "Ajaria");
            dic.Add("GE-GU", "Guria");
            dic.Add("GE-IM", "Imereti");
            dic.Add("GE-KA", "Kakheti");
            dic.Add("GE-KK", "Kvemo Kartli");
            dic.Add("GE-MM", "Mtskheta-Mtianeti");
            dic.Add("GE-RL", "Racha-Lechkhumi");
            dic.Add("GE-SZ", "Samegrelo-Zemo Svaneti");
            dic.Add("GE-SJ", "Samtskhe-Javakheti");
            dic.Add("GE-SK", "Shida Kartli");
            dic.Add("GE-TB", "Tbilisi");
            pdic.Add("GE", dic);
            #endregion

            /*=============================================*/

            #region Germany
            dic = new Dictionary<string, string>();
            dic.Add("DE-BW", "Baden-Württemberg");
            dic.Add("DE-BY", "Bayern");
            dic.Add("DE-BE", "Berlin");
            dic.Add("DE-BB", "Brandenburg");
            dic.Add("DE-HB", "Bremen");
            dic.Add("DE-HH", "Hamburg");
            dic.Add("DE-HE", "Hessen");
            dic.Add("DE-MV", "Mecklenburg-Vorpommern");
            dic.Add("DE-NI", "Niedersachsen");
            dic.Add("DE-NW", "Nordrhein-Westfalen");
            dic.Add("DE-RP", "Rheinland-Pfalz");
            dic.Add("DE-SL", "Saarland");
            dic.Add("DE-SN", "Sachsen");
            dic.Add("DE-ST", "Sachsen-Anhalt");
            dic.Add("DE-SH", "Schleswig-Holstein");
            dic.Add("DE-TH", "Thüringen");
            pdic.Add("DE", dic);
            #endregion

            /*=============================================*/

            #region Ghana
            dic = new Dictionary<string, string>();
            dic.Add("GH-AH", "Ashanti");
            dic.Add("GH-BA", "Brong-Ahafo");
            dic.Add("GH-CP", "Central");
            dic.Add("GH-EP", "Eastern");
            dic.Add("GH-AA", "Greater Accra");
            dic.Add("GH-NP", "Northern");
            dic.Add("GH-UE", "Upper East");
            dic.Add("GH-UW", "Upper West");
            dic.Add("GH-TV", "Volta");
            dic.Add("GH-WP", "Western");
            pdic.Add("GH", dic);
            #endregion

            /*=============================================*/

            #region Greece
            dic = new Dictionary<string, string>();
            dic.Add("GR-13", "Achaïa");
            dic.Add("GR-69", "Agio Oros");
            dic.Add("GR-01", "Aitolia kai Akarnania");
            dic.Add("GR-11", "Argolida");
            dic.Add("GR-12", "Arkadia");
            dic.Add("GR-31", "Arta");
            dic.Add("GR-A1", "Attiki");
            dic.Add("GR-64", "Chalkidiki");
            dic.Add("GR-94", "Chania");
            dic.Add("GR-85", "Chios");
            dic.Add("GR-81", "Dodekanisos");
            dic.Add("GR-52", "Drama");
            dic.Add("GR-71", "Evros");
            dic.Add("GR-05", "Evrytania");
            dic.Add("GR-04", "Evvoia");
            dic.Add("GR-63", "Florina");
            dic.Add("GR-07", "Fokida");
            dic.Add("GR-06", "Fthiotida");
            dic.Add("GR-51", "Grevena");
            dic.Add("GR-14", "Ileia");
            dic.Add("GR-53", "Imathia");
            dic.Add("GR-33", "Ioannina");
            dic.Add("GR-91", "Irakleio");
            dic.Add("GR-41", "Karditsa");
            dic.Add("GR-56", "Kastoria");
            dic.Add("GR-55", "Kavala");
            dic.Add("GR-23", "Kefallonia");
            dic.Add("GR-22", "Kerkyra");
            dic.Add("GR-57", "Kilkis");
            dic.Add("GR-15", "Korinthia");
            dic.Add("GR-58", "Kozani");
            dic.Add("GR-82", "Kyklades");
            dic.Add("GR-16", "Lakonia");
            dic.Add("GR-42", "Larisa");
            dic.Add("GR-92", "Lasithi");
            dic.Add("GR-24", "Lefkada");
            dic.Add("GR-83", "Lesvos");
            dic.Add("GR-43", "Magnisia");
            dic.Add("GR-17", "Messinia");
            dic.Add("GR-59", "Pella");
            dic.Add("GR-61", "Pieria");
            dic.Add("GR-34", "Preveza");
            dic.Add("GR-93", "Rethymno");
            dic.Add("GR-73", "Rodopi");
            dic.Add("GR-84", "Samos");
            dic.Add("GR-62", "Serres");
            dic.Add("GR-32", "Thesprotia");
            dic.Add("GR-54", "Thessaloniki");
            dic.Add("GR-44", "Trikala");
            dic.Add("GR-03", "Voiotia");
            dic.Add("GR-72", "Xanthi");
            dic.Add("GR-21", "Zakynthos");
            pdic.Add("GR", dic);
            #endregion

            /*=============================================*/

            #region Greenland
            dic = new Dictionary<string, string>();
            dic.Add("GL-KU", "Kommune Kujalleq");
            dic.Add("GL-SM", "Kommuneqarfik Sermersooq");
            dic.Add("GL-QA", "Qaasuitsup Kommunia");
            dic.Add("GL-QE", "Qeqqata Kommunia");
            pdic.Add("GL", dic);
            #endregion

            /*=============================================*/

            #region Grenada
            dic = new Dictionary<string, string>();
            dic.Add("GD-01", "Saint Andrew");
            dic.Add("GD-02", "Saint David");
            dic.Add("GD-03", "Saint George");
            dic.Add("GD-04", "Saint John");
            dic.Add("GD-05", "Saint Mark");
            dic.Add("GD-06", "Saint Patrick");
            dic.Add("GD-10", "Southern Grenadine Islands");
            pdic.Add("GD", dic);
            #endregion

            /*=============================================*/

            #region Guatemala
            dic = new Dictionary<string, string>();
            dic.Add("GT-AV", "Alta Verapaz");
            dic.Add("GT-BV", "Baja Verapaz");
            dic.Add("GT-CM", "Chimaltenango");
            dic.Add("GT-CQ", "Chiquimula");
            dic.Add("GT-PR", "El Progreso");
            dic.Add("GT-ES", "Escuintla");
            dic.Add("GT-GU", "Guatemala");
            dic.Add("GT-HU", "Huehuetenango");
            dic.Add("GT-IZ", "Izabal");
            dic.Add("GT-JA", "Jalapa");
            dic.Add("GT-JU", "Jutiapa");
            dic.Add("GT-PE", "Petén");
            dic.Add("GT-QZ", "Quetzaltenango");
            dic.Add("GT-QC", "Quiché");
            dic.Add("GT-RE", "Retalhuleu");
            dic.Add("GT-SA", "Sacatepéquez");
            dic.Add("GT-SM", "San Marcos");
            dic.Add("GT-SR", "Santa Rosa");
            dic.Add("GT-SO", "Sololá");
            dic.Add("GT-SU", "Suchitepéquez");
            dic.Add("GT-TO", "Totonicapán");
            dic.Add("GT-ZA", "Zacapa");
            pdic.Add("GT", dic);
            #endregion

            /*=============================================*/
            // conflicto con UK el codigo no debe de ser guernsey
            //#region Guernsey
            //dic = new Dictionary<string, string>();
            //dic.Add("GGY-GGY", "Guernsey");
            //pdic.Add("GB", dic);
            //#endregion

            /*=============================================*/

            #region Guinea
            dic = new Dictionary<string, string>();
            dic.Add("GN-BE", "Beyla");
            dic.Add("GN-BF", "Boffa");
            dic.Add("GN-BK", "Boké");
            dic.Add("GN-C", "Conakry");
            dic.Add("GN-CO", "Coyah");
            dic.Add("GN-DB", "Dabola");
            dic.Add("GN-DL", "Dalaba");
            dic.Add("GN-DI", "Dinguiraye");
            dic.Add("GN-DU", "Dubréka");
            dic.Add("GN-FA", "Faranah");
            dic.Add("GN-FO", "Forécariah");
            dic.Add("GN-FR", "Fria");
            dic.Add("GN-GA", "Gaoual");
            dic.Add("GN-GU", "Guékédou");
            dic.Add("GN-KA", "Kankan");
            dic.Add("GN-KE", "Kérouané");
            dic.Add("GN-KD", "Kindia");
            dic.Add("GN-KS", "Kissidougou");
            dic.Add("GN-KB", "Koubia");
            dic.Add("GN-KN", "Koundara");
            dic.Add("GN-KO", "Kouroussa");
            dic.Add("GN-LA", "Labé");
            dic.Add("GN-LE", "Lélouma");
            dic.Add("GN-LO", "Lola");
            dic.Add("GN-MC", "Macenta");
            dic.Add("GN-ML", "Mali");
            dic.Add("GN-MM", "Mamou");
            dic.Add("GN-MD", "Mandiana");
            dic.Add("GN-NZ", "Nzérékoré");
            dic.Add("GN-PI", "Pita");
            dic.Add("GN-SI", "Siguiri");
            dic.Add("GN-TE", "Télimélé");
            dic.Add("GN-TO", "Tougué");
            dic.Add("GN-YO", "Yomou");
            pdic.Add("GN", dic);
            #endregion

            /*=============================================*/

            #region Guinea-bissau
            dic = new Dictionary<string, string>();
            dic.Add("GW-BA", "Bafatá");
            dic.Add("GW-BM", "Biombo");
            dic.Add("GW-BS", "Bissau");
            dic.Add("GW-BL", "Bolama");
            dic.Add("GW-CA", "Cacheu");
            dic.Add("GW-GA", "Gabú");
            dic.Add("GW-OI", "Oio");
            dic.Add("GW-QU", "Quinara");
            dic.Add("GW-TO", "Tombali");
            pdic.Add("GW", dic);
            #endregion

            /*=============================================*/

            #region Guyana
            dic = new Dictionary<string, string>();
            dic.Add("GY-BA", "Barima-Waini");
            dic.Add("GY-CU", "Cuyuni-Mazaruni");
            dic.Add("GY-DE", "Demerara-Mahaica");
            dic.Add("GY-EB", "East Berbice-Corentyne");
            dic.Add("GY-ES", "Essequibo Islands-West Demerara");
            dic.Add("GY-MA", "Mahaica-Berbice");
            dic.Add("GY-PM", "Pomeroon-Supenaam");
            dic.Add("GY-PT", "Potaro-Siparuni");
            dic.Add("GY-UD", "Upper Demerara-Berbice");
            dic.Add("GY-UT", "Upper Takutu-Upper Essequibo");
            pdic.Add("GY", dic);
            #endregion

            /*=============================================*/

            #region Haiti
            dic = new Dictionary<string, string>();
            dic.Add("HT-AR", "Artibonite");
            dic.Add("HT-CE", "Centre");
            dic.Add("HT-GA", "Grande-Anse");
            dic.Add("HT-ND", "Nord");
            dic.Add("HT-NE", "Nord-Est");
            dic.Add("HT-NO", "Nord-Ouest");
            dic.Add("HT-OU", "Ouest");
            dic.Add("HT-SD", "Sud");
            dic.Add("HT-SE", "Sud-Est");
            pdic.Add("HT", dic);
            #endregion

            /*=============================================*/

            #region Honduras
            dic = new Dictionary<string, string>();
            dic.Add("HN-AT", "Atlántida");
            dic.Add("HN-CH", "Choluteca");
            dic.Add("HN-CL", "Colón");
            dic.Add("HN-CM", "Comayagua");
            dic.Add("HN-CP", "Copán");
            dic.Add("HN-CR", "Cortés");
            dic.Add("HN-EP", "El Paraíso");
            dic.Add("HN-FM", "Francisco Morazán");
            dic.Add("HN-GD", "Gracias a Dios");
            dic.Add("HN-IN", "Intibucá");
            dic.Add("HN-IB", "Islas de la Bahía");
            dic.Add("HN-LP", "La Paz");
            dic.Add("HN-LE", "Lempira");
            dic.Add("HN-OC", "Ocotepeque");
            dic.Add("HN-OL", "Olancho");
            dic.Add("HN-SB", "Santa Bárbara");
            dic.Add("HN-VA", "Valle");
            dic.Add("HN-YO", "Yoro");
            pdic.Add("HN", dic);
            #endregion

            /*=============================================*/

            #region Hungary
            dic = new Dictionary<string, string>();
            dic.Add("HU-BK", "Bács-Kiskun");
            dic.Add("HU-BA", "Baranya");
            dic.Add("HU-BE", "Békés");
            dic.Add("HU-BC", "Békéscsaba");
            dic.Add("HU-BZ", "Borsod-Abaúj-Zemplén");
            dic.Add("HU-BU", "Budapest");
            dic.Add("HU-CS", "Csongrád");
            dic.Add("HU-DE", "Debrecen");
            dic.Add("HU-DU", "Dunaújváros");
            dic.Add("HU-EG", "Eger");
            dic.Add("HU-ER", "Erd");
            dic.Add("HU-FE", "Fejér");
            dic.Add("HU-GY", "Gyor");
            dic.Add("HU-GS", "Gyor-Moson-Sopron");
            dic.Add("HU-HB", "Hajdú-Bihar");
            dic.Add("HU-HE", "Heves");
            dic.Add("HU-HV", "Hódmezovásárhely");
            dic.Add("HU-JN", "Jász-Nagykun-Szolnok");
            dic.Add("HU-KV", "Kaposvár");
            dic.Add("HU-KM", "Kecskemét");
            dic.Add("HU-KE", "Komárom-Esztergom");
            dic.Add("HU-MI", "Miskolc");
            dic.Add("HU-NK", "Nagykanizsa");
            dic.Add("HU-NO", "Nógrád");
            dic.Add("HU-NY", "Nyíregyháza");
            dic.Add("HU-PS", "Pécs");
            dic.Add("HU-PE", "Pest");
            dic.Add("HU-ST", "Salgótarján");
            dic.Add("HU-SO", "Somogy");
            dic.Add("HU-SN", "Sopron");
            dic.Add("HU-SZ", "Szabolcs-Szatmár-Bereg");
            dic.Add("HU-SD", "Szeged");
            dic.Add("HU-SF", "Székesfehérvár");
            dic.Add("HU-SS", "Szekszárd");
            dic.Add("HU-SK", "Szolnok");
            dic.Add("HU-SH", "Szombathely");
            dic.Add("HU-TB", "Tatabánya");
            dic.Add("HU-TO", "Tolna");
            dic.Add("HU-VA", "Vas");
            dic.Add("HU-VE", "Veszprém");
            dic.Add("HU-VM", "Veszprém City");
            dic.Add("HU-ZA", "Zala");
            dic.Add("HU-ZE", "Zalaegerszeg");
            pdic.Add("HU", dic);
            #endregion

            /*=============================================*/

            #region Iceland
            dic = new Dictionary<string, string>();
            dic.Add("IS-7", "Austurland");
            dic.Add("IS-1", "Höfuðborgarsvæði utan Reykjavíkur");
            dic.Add("IS-6", "Norðurland eystra");
            dic.Add("IS-5", "Norðurland vestra");
            dic.Add("IS-0", "Reykjavík");
            dic.Add("IS-8", "Suðurland");
            dic.Add("IS-2", "Suðurnes");
            dic.Add("IS-4", "Vestfirðir");
            dic.Add("IS-3", "Vesturland");
            pdic.Add("IS", dic);
            #endregion

            /*=============================================*/

            #region India
            dic = new Dictionary<string, string>();
            dic.Add("IN-AN", "Andaman and Nicobar Islands");
            dic.Add("IN-AP", "Andhra Pradesh");
            dic.Add("IN-AR", "Arunachal Pradesh");
            dic.Add("IN-AS", "Assam");
            dic.Add("IN-BR", "Bihar");
            dic.Add("IN-CH", "Chandigarh");
            dic.Add("IN-CT", "Chhattisgarh");
            dic.Add("IN-DN", "Dadra and Nagar Haveli");
            dic.Add("IN-DD", "Daman and Diu");
            dic.Add("IN-DL", "Delhi");
            dic.Add("IN-GA", "Goa");
            dic.Add("IN-GJ", "Gujarat");
            dic.Add("IN-HR", "Haryana");
            dic.Add("IN-HP", "Himachal Pradesh");
            dic.Add("IN-JK", "Jammu and Kashmir");
            dic.Add("IN-JH", "Jharkhand");
            dic.Add("IN-KA", "Karnataka");
            dic.Add("IN-KL", "Kerala");
            dic.Add("IN-LD", "Lakshadweep");
            dic.Add("IN-MP", "Madhya Pradesh");
            dic.Add("IN-MH", "Maharashtra");
            dic.Add("IN-MN", "Manipur");
            dic.Add("IN-ML", "Meghalaya");
            dic.Add("IN-MZ", "Mizoram");
            dic.Add("IN-NL", "Nagaland");
            dic.Add("IN-OR", "Orissa");
            dic.Add("IN-PY", "Pondicherry");
            dic.Add("IN-PB", "Punjab");
            dic.Add("IN-RJ", "Rajasthan");
            dic.Add("IN-SK", "Sikkim");
            dic.Add("IN-TN", "Tamil Nadu");
            dic.Add("IN-TR", "Tripura");
            dic.Add("IN-UP", "Uttar Pradesh");
            dic.Add("IN-UT", "Uttarakhand");
            dic.Add("IN-UL", "Uttaranchal");
            dic.Add("IN-WB", "West Bengal");
            pdic.Add("IN", dic);
            #endregion

            /*=============================================*/

            #region Indonesia
            dic = new Dictionary<string, string>();
            dic.Add("ID-AC", "Aceh");
            dic.Add("ID-BA", "Bali");
            dic.Add("ID-BB", "Bangka Belitung");
            dic.Add("ID-BT", "Banten");
            dic.Add("ID-BE", "Bengkulu");
            dic.Add("ID-GO", "Gorontalo");
            dic.Add("ID-JK", "Jakarta Raya");
            dic.Add("ID-JA", "Jambi");
            dic.Add("ID-JB", "Jawa Barat");
            dic.Add("ID-JT", "Jawa Tengah");
            dic.Add("ID-JI", "Jawa Timur");
            dic.Add("ID-KB", "Kalimantan Barat");
            dic.Add("ID-KS", "Kalimantan Selatan");
            dic.Add("ID-KT", "Kalimantan Tengah");
            dic.Add("ID-KI", "Kalimantan Timur");
            dic.Add("ID-KR", "Kepulauan Riau");
            dic.Add("ID-LA", "Lampung");
            dic.Add("ID-MA", "Maluku");
            dic.Add("ID-MU", "Maluku Utara");
            dic.Add("ID-NB", "Nusa Tenggara Barat");
            dic.Add("ID-NT", "Nusa Tenggara Timur");
            dic.Add("ID-PA", "Papua");
            dic.Add("ID-PB", "Papua Barat");
            dic.Add("ID-RI", "Riau");
            dic.Add("ID-SR", "Sulawesi Barat");
            dic.Add("ID-SN", "Sulawesi Selatan");
            dic.Add("ID-ST", "Sulawesi Tengah");
            dic.Add("ID-SG", "Sulawesi Tenggara");
            dic.Add("ID-SA", "Sulawesi Utara");
            dic.Add("ID-SB", "Sumatera Barat");
            dic.Add("ID-SS", "Sumatera Selatan");
            dic.Add("ID-SU", "Sumatera Utara");
            dic.Add("ID-YO", "Yogyakarta");
            pdic.Add("ID", dic);
            #endregion

            /*=============================================*/

            #region Iran (Islamic Republic of)
            dic = new Dictionary<string, string>();
            dic.Add("IR-03", "Ardabil");
            dic.Add("IR-02", "Az¯arbayjan-e Gharbi");
            dic.Add("IR-01", "Az¯arbayjan-e Sharqi");
            dic.Add("IR-06", "Bushehr");
            dic.Add("IR-08", "Chahar Mah¸all va Bakhtiari");
            dic.Add("IR-04", "Esfahan");
            dic.Add("IR-14", "Fars");
            dic.Add("IR-19", "Gilan");
            dic.Add("IR-27", "Golestan");
            dic.Add("IR-24", "Hamadan");
            dic.Add("IR-23", "Hormozgan");
            dic.Add("IR-05", "Ilam");
            dic.Add("IR-15", "Kerman");
            dic.Add("IR-17", "Kermanshah");
            dic.Add("IR-09", "Khorasan");
            dic.Add("IR-29", "Khorasan-e Janubi");
            dic.Add("IR-30", "Khorasan-e Razavi");
            dic.Add("IR-31", "Khorasan-e Shemali");
            dic.Add("IR-10", "Khuzestan");
            dic.Add("IR-18", "Kohkiluyeh va Buyer Ahmad");
            dic.Add("IR-16", "Kordestan");
            dic.Add("IR-20", "Lorestan");
            dic.Add("IR-22", "Markazi");
            dic.Add("IR-21", "Mazandaran");
            dic.Add("IR-28", "Qazvin");
            dic.Add("IR-26", "Qom");
            dic.Add("IR-12", "Semnan");
            dic.Add("IR-13", "Sistan va Baluchestan");
            dic.Add("IR-07", "Tehran");
            dic.Add("IR-25", "Yazd");
            dic.Add("IR-11", "Zanjan");
            pdic.Add("IR", dic);
            #endregion

            /*=============================================*/

            #region Iraq
            dic = new Dictionary<string, string>();
            dic.Add("IQ-AN", "Al Anbar");
            dic.Add("IQ-BA", "Al Basrah");
            dic.Add("IQ-MU", "Al Muthanná");
            dic.Add("IQ-QA", "Al Qadisiyah");
            dic.Add("IQ-NA", "An Najaf");
            dic.Add("IQ-AR", "Arbil");
            dic.Add("IQ-SU", "As Sulaymaniyah");
            dic.Add("IQ-TS", "At Ta'mim");
            dic.Add("IQ-BB", "Babil");
            dic.Add("IQ-BG", "Baghdad");
            dic.Add("IQ-DA", "Dahuk");
            dic.Add("IQ-DQ", "Dhi Qar");
            dic.Add("IQ-DI", "Diyalá");
            dic.Add("IQ-KA", "Karbala'");
            dic.Add("IQ-MA", "Maysan");
            dic.Add("IQ-NI", "Ninawá");
            dic.Add("IQ-SD", "Salah ad Din");
            dic.Add("IQ-WA", "Wasit");
            pdic.Add("IQ", dic);
            #endregion

            /*=============================================*/

            #region Ireland
            dic = new Dictionary<string, string>();
            dic.Add("IE-CW", "Carlow");
            dic.Add("IE-CN", "Cavan");
            dic.Add("IE-CE", "Clare");
            dic.Add("IE-C", "Cork");
            dic.Add("IE-DL", "Donegal");
            dic.Add("IE-D", "Dublin");
            dic.Add("IE-G", "Galway");
            dic.Add("IE-KY", "Kerry");
            dic.Add("IE-KE", "Kildare");
            dic.Add("IE-KK", "Kilkenny");
            dic.Add("IE-LS", "Laois");
            dic.Add("IE-LM", "Leitrim");
            dic.Add("IE-LK", "Limerick");
            dic.Add("IE-LD", "Longford");
            dic.Add("IE-LH", "Louth");
            dic.Add("IE-MO", "Mayo");
            dic.Add("IE-MH", "Meath");
            dic.Add("IE-MN", "Monaghan");
            dic.Add("IE-OY", "Offaly");
            dic.Add("IE-RN", "Roscommon");
            dic.Add("IE-SO", "Sligo");
            dic.Add("IE-TA", "Tipperary");
            dic.Add("IE-WD", "Waterford");
            dic.Add("IE-WH", "Westmeath");
            dic.Add("IE-WX", "Wexford");
            dic.Add("IE-WW", "Wicklow");
            pdic.Add("IE", dic);
            #endregion

            /*=============================================*/

            #region Isle of Man
            dic = new Dictionary<string, string>();
            dic.Add("IMN-IOM", "Isle of Man");
            pdic.Add("IM", dic);
            #endregion

            /*=============================================*/

            #region Israel
            dic = new Dictionary<string, string>();
            dic.Add("IL-D", "HaDarom");
            dic.Add("IL-HA", "Haifa");
            dic.Add("IL-M", "HaMerkaz");
            dic.Add("IL-Z", "HaZafon");
            dic.Add("IL-TA", "Tel-Aviv");
            dic.Add("IL-JM", "Yerushalayim");
            pdic.Add("IL", dic);
            #endregion

            /*=============================================*/

            #region Italy
            dic = new Dictionary<string, string>();
            dic.Add("IT-65", "Abruzzo");
            dic.Add("IT-AG", "Agrigento");
            dic.Add("IT-AL", "Alessandria");
            dic.Add("IT-AN", "Ancona");
            dic.Add("IT-AO", "Aosta");
            dic.Add("IT-AR", "Arezzo");
            dic.Add("IT-AP", "Ascoli Piceno");
            dic.Add("IT-AT", "Asti");
            dic.Add("IT-AV", "Avellino");
            dic.Add("IT-BA", "Bari");
            dic.Add("IT-BT", "Barletta-Andria-Trani");
            dic.Add("IT-77", "Basilicata");
            dic.Add("IT-BL", "Belluno");
            dic.Add("IT-BN", "Benevento");
            dic.Add("IT-BG", "Bergamo");
            dic.Add("IT-BI", "Biella");
            dic.Add("IT-BO", "Bologna");
            dic.Add("IT-BZ", "Bolzano");
            dic.Add("IT-BS", "Brescia");
            dic.Add("IT-BR", "Brindisi");
            dic.Add("IT-CA", "Cagliari");
            dic.Add("IT-78", "Calabria");
            dic.Add("IT-CL", "Caltanissetta");
            dic.Add("IT-72", "Campania");
            dic.Add("IT-CB", "Campobasso");
            dic.Add("IT-CI", "Carbonia-Iglesias");
            dic.Add("IT-CE", "Caserta");
            dic.Add("IT-CT", "Catania");
            dic.Add("IT-CZ", "Catanzaro");
            dic.Add("IT-CH", "Chieti");
            dic.Add("IT-CO", "Como");
            dic.Add("IT-CS", "Cosenza");
            dic.Add("IT-CR", "Cremona");
            dic.Add("IT-KR", "Crotone");
            dic.Add("IT-CN", "Cuneo");
            dic.Add("IT-45", "Emilia-Romagna");
            dic.Add("IT-EN", "Enna");
            dic.Add("IT-FM", "Fermo");
            dic.Add("IT-FE", "Ferrara");
            dic.Add("IT-FI", "Firenze");
            dic.Add("IT-FG", "Foggia");
            dic.Add("IT-FC", "Forli-Cesena");
            dic.Add("IT-36", "Friuli-Venezia Giulia");
            dic.Add("IT-FR", "Frosinone");
            dic.Add("IT-GE", "Genova");
            dic.Add("IT-GO", "Gorizia");
            dic.Add("IT-GR", "Grosseto");
            dic.Add("IT-IM", "Imperia");
            dic.Add("IT-IS", "Isernia");
            dic.Add("IT-AQ", "L'Aquila");
            dic.Add("IT-SP", "La Spezia");
            dic.Add("IT-LT", "Latina");
            dic.Add("IT-62", "Lazio");
            dic.Add("IT-LE", "Lecce");
            dic.Add("IT-LC", "Lecco");
            dic.Add("IT-42", "Liguria");
            dic.Add("IT-LI", "Livorno");
            dic.Add("IT-LO", "Lodi");
            dic.Add("IT-25", "Lombardia");
            dic.Add("IT-LU", "Lucca");
            dic.Add("IT-MC", "Macerata");
            dic.Add("IT-MN", "Mantova");
            dic.Add("IT-57", "Marche");
            dic.Add("IT-MS", "Massa-Carrara");
            dic.Add("IT-MT", "Matera");
            dic.Add("IT-VS", "Medio Campidano");
            dic.Add("IT-ME", "Messina");
            dic.Add("IT-MI", "Milano");
            dic.Add("IT-MO", "Modena");
            dic.Add("IT-67", "Molise");
            dic.Add("IT-MB", "Monza e Brianza");
            dic.Add("IT-NA", "Napoli");
            dic.Add("IT-NO", "Novara");
            dic.Add("IT-NU", "Nuoro");
            dic.Add("IT-OG", "Ogliastra");
            dic.Add("IT-OT", "Olbia-Tempio");
            dic.Add("IT-OR", "Oristano");
            dic.Add("IT-PD", "Padova");
            dic.Add("IT-PA", "Palermo");
            dic.Add("IT-PR", "Parma");
            dic.Add("IT-PV", "Pavia");
            dic.Add("IT-PG", "Perugia");
            dic.Add("IT-PU", "Pesaro e Urbino");
            dic.Add("IT-PE", "Pescara");
            dic.Add("IT-PC", "Piacenza");
            dic.Add("IT-21", "Piemonte");
            dic.Add("IT-PI", "Pisa");
            dic.Add("IT-PT", "Pistoia");
            dic.Add("IT-PN", "Pordenone");
            dic.Add("IT-PZ", "Potenza");
            dic.Add("IT-PO", "Prato");
            dic.Add("IT-75", "Puglia");
            dic.Add("IT-RG", "Ragusa");
            dic.Add("IT-RA", "Ravenna");
            dic.Add("IT-RC", "Reggio Calabria");
            dic.Add("IT-RE", "Reggio Emilia");
            dic.Add("IT-RI", "Rieti");
            dic.Add("IT-RN", "Rimini");
            dic.Add("IT-RM", "Roma");
            dic.Add("IT-RO", "Rovigo");
            dic.Add("IT-SA", "Salerno");
            dic.Add("IT-88", "Sardegna");
            dic.Add("IT-SS", "Sassari");
            dic.Add("IT-SV", "Savona");
            dic.Add("IT-82", "Sicilia");
            dic.Add("IT-SI", "Siena");
            dic.Add("IT-SR", "Siracusa");
            dic.Add("IT-SO", "Sondrio");
            dic.Add("IT-TA", "Taranto");
            dic.Add("IT-TE", "Teramo");
            dic.Add("IT-TR", "Terni");
            dic.Add("IT-TO", "Torino");
            dic.Add("IT-52", "Toscana");
            dic.Add("IT-TP", "Trapani");
            dic.Add("IT-32", "Trentino-Alto Adige");
            dic.Add("IT-TN", "Trento");
            dic.Add("IT-TV", "Treviso");
            dic.Add("IT-TS", "Trieste");
            dic.Add("IT-UD", "Udine");
            dic.Add("IT-55", "Umbria");
            dic.Add("IT-23", "Valle d'Aosta");
            dic.Add("IT-VA", "Varese");
            dic.Add("IT-34", "Veneto");
            dic.Add("IT-VE", "Venezia");
            dic.Add("IT-VB", "Verbano-Cusio-Ossola");
            dic.Add("IT-VC", "Vercelli");
            dic.Add("IT-VR", "Verona");
            dic.Add("IT-VV", "Vibo Valentia");
            dic.Add("IT-VI", "Vicenza");
            dic.Add("IT-VT", "Viterbo");
            pdic.Add("IT", dic);
            #endregion

            /*=============================================*/

            #region Jamaica
            dic = new Dictionary<string, string>();
            dic.Add("JM-13", "Clarendon");
            dic.Add("JM-09", "Hanover");
            dic.Add("JM-01", "Kingston");
            dic.Add("JM-12", "Manchester");
            dic.Add("JM-04", "Portland");
            dic.Add("JM-02", "Saint Andrew");
            dic.Add("JM-06", "Saint Ann");
            dic.Add("JM-14", "Saint Catherine");
            dic.Add("JM-11", "Saint Elizabeth");
            dic.Add("JM-08", "Saint James");
            dic.Add("JM-05", "Saint Mary");
            dic.Add("JM-03", "Saint Thomas");
            dic.Add("JM-07", "Trelawny");
            dic.Add("JM-10", "Westmoreland");
            pdic.Add("JM", dic);
            #endregion

            /*=============================================*/

            #region Japan
            dic = new Dictionary<string, string>();
            dic.Add("JP-23", "Aiti");
            dic.Add("JP-05", "Akita");
            dic.Add("JP-02", "Aomori");
            dic.Add("JP-38", "Ehime");
            dic.Add("JP-21", "Gihu");
            dic.Add("JP-10", "Gunma");
            dic.Add("JP-34", "Hirosima");
            dic.Add("JP-01", "Hokkaidô");
            dic.Add("JP-18", "Hukui");
            dic.Add("JP-40", "Hukuoka");
            dic.Add("JP-07", "Hukusima");
            dic.Add("JP-28", "Hyôgo");
            dic.Add("JP-08", "Ibaraki");
            dic.Add("JP-17", "Isikawa");
            dic.Add("JP-03", "Iwate");
            dic.Add("JP-37", "Kagawa");
            dic.Add("JP-46", "Kagosima");
            dic.Add("JP-14", "Kanagawa");
            dic.Add("JP-39", "Kôti");
            dic.Add("JP-43", "Kumamoto");
            dic.Add("JP-26", "Kyôto");
            dic.Add("JP-24", "Mie");
            dic.Add("JP-04", "Miyagi");
            dic.Add("JP-45", "Miyazaki");
            dic.Add("JP-20", "Nagano");
            dic.Add("JP-42", "Nagasaki");
            dic.Add("JP-29", "Nara");
            dic.Add("JP-15", "Niigata");
            dic.Add("JP-44", "Ôita");
            dic.Add("JP-33", "Okayama");
            dic.Add("JP-47", "Okinawa");
            dic.Add("JP-27", "Ôsaka");
            dic.Add("JP-41", "Saga");
            dic.Add("JP-11", "Saitama");
            dic.Add("JP-25", "Siga");
            dic.Add("JP-32", "Simane");
            dic.Add("JP-22", "Sizuoka");
            dic.Add("JP-12", "Tiba");
            dic.Add("JP-36", "Tokusima");
            dic.Add("JP-13", "Tôkyô");
            dic.Add("JP-09", "Totigi");
            dic.Add("JP-31", "Tottori");
            dic.Add("JP-16", "Toyama");
            dic.Add("JP-30", "Wakayama");
            dic.Add("JP-06", "Yamagata");
            dic.Add("JP-35", "Yamaguti");
            dic.Add("JP-19", "Yamanasi");
            pdic.Add("JP", dic);
            #endregion

            /*=============================================*/

            #region Jersey
            dic = new Dictionary<string, string>();
            dic.Add("JEY-JEY", "Jersey");
            pdic.Add("JY", dic);
            #endregion

            /*=============================================*/

            #region Kazakhstan
            dic = new Dictionary<string, string>();
            dic.Add("KZ-ALA", "Almaty");
            dic.Add("KZ-ALM", "Almaty oblysy");
            dic.Add("KZ-AKM", "Aqmola oblysy");
            dic.Add("KZ-AKT", "Aqtöbe oblysy");
            dic.Add("KZ-AST", "Astana");
            dic.Add("KZ-ATY", "Atyrau oblysy");
            dic.Add("KZ-ZAP", "Batys Qazaqstan oblysy");
            dic.Add("KZ-BAY", "Bayqongyr");
            dic.Add("KZ-MAN", "Mangghystau oblysy");
            dic.Add("KZ-YUZ", "Ongtüstik Qazaqstan oblysy");
            dic.Add("KZ-PAV", "Pavlodar oblysy");
            dic.Add("KZ-KAR", "Qaraghandy oblysy");
            dic.Add("KZ-KUS", "Qostanay oblysy");
            dic.Add("KZ-KZY", "Qyzylorda oblysy");
            dic.Add("KZ-VOS", "Shyghys Qazaqstan oblysy");
            dic.Add("KZ-SEV", "Soltüstik Qazaqstan oblysy");
            dic.Add("KZ-ZHA", "Zhambyl oblysy");
            pdic.Add("KZ", dic);
            #endregion

            /*=============================================*/

            #region Kenya
            dic = new Dictionary<string, string>();
            dic.Add("KE-200", "Central");
            dic.Add("KE-300", "Coast");
            dic.Add("KE-400", "Eastern");
            dic.Add("KE-110", "Nairobi");
            dic.Add("KE-500", "North-Eastern");
            dic.Add("KE-600", "Nyanza");
            dic.Add("KE-700", "Rift Valley");
            dic.Add("KE-900", "Western");
            pdic.Add("KE", dic);
            #endregion

            /*=============================================*/

            #region Kiribati
            dic = new Dictionary<string, string>();
            dic.Add("KI-G", "Gilbert Islands");
            dic.Add("KI-L", "Line Islands");
            dic.Add("KI-P", "Phoenix Islands");
            pdic.Add("KI", dic);
            #endregion

            /*=============================================*/

            #region Korea, Democratic People's Republic of
            dic = new Dictionary<string, string>();
            dic.Add("KP-04", "Chagang-do");
            dic.Add("KP-09", "Hamgyong-bukdo");
            dic.Add("KP-08", "Hamgyong-namdo");
            dic.Add("KP-06", "Hwanghae-bukto");
            dic.Add("KP-05", "Hwanghae-namdo");
            dic.Add("KP-07", "Kangwon-do");
            dic.Add("KP-13", "Nason");
            dic.Add("KP-03", "Pyongan-bukdo");
            dic.Add("KP-02", "Pyongan-namdo");
            dic.Add("KP-01", "Pyongyang");
            dic.Add("KP-10", "Yanggang-do");
            pdic.Add("KP", dic);
            #endregion

            /*=============================================*/

            #region Korea, Republic of
            dic = new Dictionary<string, string>();
            dic.Add("KR-26", "Busan Gwang'yeogs");
            dic.Add("KR-43", "Chungcheongbugdo");
            dic.Add("KR-44", "Chungcheongnamdo");
            dic.Add("KR-27", "Daegu Gwang'yeogs");
            dic.Add("KR-30", "Daejeon Gwang'yeog");
            dic.Add("KR-42", "Gang'weondo");
            dic.Add("KR-29", "Gwangju Gwang'yeogs");
            dic.Add("KR-41", "Gyeonggido");
            dic.Add("KR-47", "Gyeongsangbugdo");
            dic.Add("KR-48", "Gyeongsangnamdo");
            dic.Add("KR-28", "Incheon Gwang'yeog");
            dic.Add("KR-49", "Jejudo");
            dic.Add("KR-45", "Jeonrabugdo");
            dic.Add("KR-46", "Jeonranamdo");
            dic.Add("KR-11", "Seoul Teugbyeolsi");
            dic.Add("KR-31", "Ulsan Gwang'yeogs");
            pdic.Add("KR", dic);
            #endregion

            /*=============================================*/

            #region Kuwait
            dic = new Dictionary<string, string>();
            dic.Add("KW-AH", "Al Ahmadi");
            dic.Add("KW-FA", "Al Farwaniyah");
            dic.Add("KW-JA", "Al Jahrah");
            dic.Add("KW-KU", "Al Kuwayt");
            dic.Add("KW-HA", "Hawalli");
            dic.Add("KW-MU", "Mubarak al-Kabir");
            pdic.Add("KW", dic);
            #endregion

            /*=============================================*/

            #region Kyrgyzstan
            dic = new Dictionary<string, string>();
            dic.Add("KG-B", "Batken");
            dic.Add("KG-GB", "Bishkek");
            dic.Add("KG-C", "Chü");
            dic.Add("KG-J", "Jalal-Abad");
            dic.Add("KG-N", "Naryn");
            dic.Add("KG-O", "Osh");
            dic.Add("KG-T", "Talas");
            dic.Add("KG-Y", "Ysyk-Köl");
            pdic.Add("KG", dic);
            #endregion

            /*=============================================*/

            #region Lao People's Democratic Republic
            dic = new Dictionary<string, string>();
            dic.Add("LA-AT", "Attapu");
            dic.Add("LA-BK", "Bokèo");
            dic.Add("LA-BL", "Bolikhamx");
            dic.Add("LA-CH", "Champasak");
            dic.Add("LA-HO", "Houaphan");
            dic.Add("LA-KH", "Khammouan");
            dic.Add("LA-LM", "Louang Namtha");
            dic.Add("LA-LP", "Louangphabang");
            dic.Add("LA-OU", "Oudômxai");
            dic.Add("LA-PH", "Phôngsali");
            dic.Add("LA-SL", "Salavan");
            dic.Add("LA-SV", "Savannakhét");
            dic.Add("LA-VI", "Vientiane");
            dic.Add("LA-VT", "Vientiane Prefecture");
            dic.Add("LA-XA", "Xaignabou");
            dic.Add("LA-XN", "Xaisômboun");
            dic.Add("LA-XE", "Xékong");
            dic.Add("LA-XI", "Xiangkhoang");
            pdic.Add("LA", dic);
            #endregion

            /*=============================================*/

            #region Latvia
            dic = new Dictionary<string, string>();
            dic.Add("LV-AI", "Aizkraukles Aprinkis");
            dic.Add("LV-AL", "Aluksnes Aprinkis");
            dic.Add("LV-BL", "Balvu Aprinkis");
            dic.Add("LV-BU", "Bauskas Aprinkis");
            dic.Add("LV-CE", "Cesu Aprinkis");
            dic.Add("LV-DGV", "Daugavpils");
            dic.Add("LV-DA", "Daugavpils Aprinkis");
            dic.Add("LV-DO", "Dobeles Aprinkis");
            dic.Add("LV-GU", "Gulbenes Aprinkis");
            dic.Add("LV-JK", "Jekabpils Aprinkis");
            dic.Add("LV-JEL", "Jelgava");
            dic.Add("LV-JL", "Jelgavas Aprinkis");
            dic.Add("LV-JUR", "Jurmala");
            dic.Add("LV-KR", "Kraslavas Aprinkis");
            dic.Add("LV-KU", "Kuldigas Aprinkis");
            dic.Add("LV-LPX", "Liepaja");
            dic.Add("LV-LE", "Liepajas Aprinkis");
            dic.Add("LV-LM", "Limbazu Aprinkis");
            dic.Add("LV-LU", "Ludzas Aprinkis");
            dic.Add("LV-MA", "Madonas Aprinkis");
            dic.Add("LV-OG", "Ogres Aprinkis");
            dic.Add("LV-PR", "Preilu Aprinkis");
            dic.Add("LV-REZ", "Rezekne");
            dic.Add("LV-RE", "Rezeknes Aprinkis");
            dic.Add("LV-RIX", "Riga");
            dic.Add("LV-RI", "Rigas Aprinkis");
            dic.Add("LV-SA", "Saldus Aprinkis");
            dic.Add("LV-TA", "Talsu Aprinkis");
            dic.Add("LV-TU", "Tukuma Aprinkis");
            dic.Add("LV-VK", "Valkas Aprinkis");
            dic.Add("LV-VM", "Valmieras Aprinkis");
            dic.Add("LV-VEN", "Ventspils");
            dic.Add("LV-VE", "Ventspils Aprinkis");
            pdic.Add("LV", dic);
            #endregion

            /*=============================================*/

            #region Lebanon
            dic = new Dictionary<string, string>();
            dic.Add("LB-BA", "Beirut");
            dic.Add("LB-BI", "El Béqaa");
            dic.Add("LB-JL", "Jabal Loubnâne");
            dic.Add("LB-AS", "Loubnâne ech Chemâli");
            dic.Add("LB-JA", "Loubnâne ej Jnoûbi");
            dic.Add("LB-NA", "Nabatîyé");
            pdic.Add("LB", dic);
            #endregion

            /*=============================================*/

            #region Lesotho
            dic = new Dictionary<string, string>();
            dic.Add("LS-D", "Berea");
            dic.Add("LS-B", "Butha-Buthe");
            dic.Add("LS-C", "Leribe");
            dic.Add("LS-E", "Mafeteng");
            dic.Add("LS-A", "Maseru");
            dic.Add("LS-F", "Mohale's Hoek");
            dic.Add("LS-J", "Mokhotlong");
            dic.Add("LS-H", "Qacha's Nek");
            dic.Add("LS-G", "Quthing");
            dic.Add("LS-K", "Thaba-Tseka");
            pdic.Add("LS", dic);
            #endregion

            /*=============================================*/

            #region Liberia
            dic = new Dictionary<string, string>();
            dic.Add("LR-BM", "Bomi");
            dic.Add("LR-BG", "Bong");
            dic.Add("LR-X1", "Gbarpolu");
            dic.Add("LR-GB", "Grand Bassa");
            dic.Add("LR-CM", "Grand Cape Mount");
            dic.Add("LR-GG", "Grand Gedeh");
            dic.Add("LR-GK", "Grand Kru");
            dic.Add("LR-LO", "Lofa");
            dic.Add("LR-MG", "Margibi");
            dic.Add("LR-MY", "Maryland");
            dic.Add("LR-MO", "Montserrado");
            dic.Add("LR-NI", "Nimba");
            dic.Add("LR-X2", "River Gee");
            dic.Add("LR-RI", "Rivercess");
            dic.Add("LR-SI", "Sinoe");
            pdic.Add("LR", dic);
            #endregion

            /*=============================================*/

            #region Liechtenstein
            dic = new Dictionary<string, string>();
            dic.Add("LI-01", "Balzers");
            dic.Add("LI-02", "Eschen");
            dic.Add("LI-03", "Gamprin");
            dic.Add("LI-04", "Mauren");
            dic.Add("LI-05", "Planken");
            dic.Add("LI-06", "Ruggell");
            dic.Add("LI-07", "Schaan");
            dic.Add("LI-08", "Schellenberg");
            dic.Add("LI-09", "Triesen");
            dic.Add("LI-10", "Triesenberg");
            dic.Add("LI-11", "Vaduz");
            pdic.Add("LI", dic);
            #endregion

            /*=============================================*/

            #region Luxembourg
            dic = new Dictionary<string, string>();
            dic.Add("LU-D", "Diekirch");
            dic.Add("LU-G", "Grevenmacher");
            dic.Add("LU-L", "Luxembourg");
            pdic.Add("LU", dic);
            #endregion

            /*=============================================*/

            #region Madagascar
            dic = new Dictionary<string, string>();
            dic.Add("MG-T", "Antananarivo");
            dic.Add("MG-D", "Antsiranana");
            dic.Add("MG-F", "Fianarantsoa");
            dic.Add("MG-M", "Mahajanga");
            dic.Add("MG-A", "Toamasina");
            dic.Add("MG-U", "Toliara");
            pdic.Add("MG", dic);
            #endregion

            /*=============================================*/

            #region Malawi
            dic = new Dictionary<string, string>();
            dic.Add("MW-BA", "Balaka");
            dic.Add("MW-BL", "Blantyre");
            dic.Add("MW-C", "Central Region");
            dic.Add("MW-CK", "Chikwawa");
            dic.Add("MW-CR", "Chiradzulu");
            dic.Add("MW-CT", "Chitipa");
            dic.Add("MW-DE", "Dedza");
            dic.Add("MW-DO", "Dowa");
            dic.Add("MW-KR", "Karonga");
            dic.Add("MW-KS", "Kasungu");
            dic.Add("MW-LK", "Likoma");
            dic.Add("MW-LI", "Lilongwe");
            dic.Add("MW-MH", "Machinga");
            dic.Add("MW-MG", "Mangochi");
            dic.Add("MW-MC", "Mchinji");
            dic.Add("MW-MU", "Mulanje");
            dic.Add("MW-MW", "Mwanza");
            dic.Add("MW-MZ", "Mzimba");
            dic.Add("MW-NE", "Neno");
            dic.Add("MW-NB", "Nkhata Bay");
            dic.Add("MW-NK", "Nkhotakota");
            dic.Add("MW-N", "Northern Region");
            dic.Add("MW-NS", "Nsanje");
            dic.Add("MW-NU", "Ntcheu");
            dic.Add("MW-NI", "Ntchisi");
            dic.Add("MW-PH", "Phalombe");
            dic.Add("MW-RU", "Rumphi");
            dic.Add("MW-SA", "Salima");
            dic.Add("MW-S", "Southern Region");
            dic.Add("MW-TH", "Thyolo");
            dic.Add("MW-ZO", "Zomba");
            pdic.Add("MW", dic);
            #endregion

            /*=============================================*/

            #region Malaysia
            dic = new Dictionary<string, string>();
            dic.Add("MY-01", "Johor");
            dic.Add("MY-02", "Kedah");
            dic.Add("MY-03", "Kelantan");
            dic.Add("MY-04", "Melaka");
            dic.Add("MY-05", "Negeri Sembilan");
            dic.Add("MY-06", "Pahang");
            dic.Add("MY-08", "Perak");
            dic.Add("MY-09", "Perlis");
            dic.Add("MY-07", "Pulau Pinang");
            dic.Add("MY-12", "Sabah");
            dic.Add("MY-13", "Sarawak");
            dic.Add("MY-10", "Selangor");
            dic.Add("MY-11", "Terengganu");
            dic.Add("MY-14", "Wilayah Persekutuan Kuala Lumpur");
            dic.Add("MY-15", "Wilayah Persekutuan Labuan");
            dic.Add("MY-16", "Wilayah Persekutuan Putrajaya");
            pdic.Add("MY", dic);
            #endregion

            /*=============================================*/

            #region Maldives
            dic = new Dictionary<string, string>();
            dic.Add("MV-02", "Alif");
            dic.Add("MV-X1", "Alif Dhaal");
            dic.Add("MV-20", "Baa");
            dic.Add("MV-17", "Dhaalu");
            dic.Add("MV-14", "Faafu");
            dic.Add("MV-27", "Gaaf Alif");
            dic.Add("MV-28", "Gaafu Dhaalu");
            dic.Add("MV-29", "Gnaviyani");
            dic.Add("MV-07", "Haa Alif");
            dic.Add("MV-23", "Haa Dhaalu");
            dic.Add("MV-26", "Kaafu");
            dic.Add("MV-05", "Laamu");
            dic.Add("MV-03", "Lhaviyani");
            dic.Add("MV-MLE", "Male");
            dic.Add("MV-12", "Meemu");
            dic.Add("MV-25", "Noonu");
            dic.Add("MV-13", "Raa");
            dic.Add("MV-01", "Seenu");
            dic.Add("MV-24", "Shaviyani");
            dic.Add("MV-08", "Thaa");
            dic.Add("MV-04", "Vaavu");
            pdic.Add("MV", dic);
            #endregion

            /*=============================================*/

            #region Mali
            dic = new Dictionary<string, string>();
            dic.Add("ML-BKO", "Bamako");
            dic.Add("ML-7", "Gao");
            dic.Add("ML-1", "Kayes");
            dic.Add("ML-8", "Kidal");
            dic.Add("ML-2", "Koulikoro");
            dic.Add("ML-5", "Mopti");
            dic.Add("ML-4", "Ségou");
            dic.Add("ML-3", "Sikasso");
            dic.Add("ML-6", "Tombouctou");
            pdic.Add("ML", dic);
            #endregion

            /*=============================================*/

            #region Marshall Islands
            dic = new Dictionary<string, string>();
            dic.Add("MH-ALL", "Ailinglaplap");
            dic.Add("MH-ALK", "Ailuk");
            dic.Add("MH-ARN", "Arno");
            dic.Add("MH-AUR", "Aur");
            dic.Add("MH-EBO", "Ebon");
            dic.Add("MH-ENI", "Enewetak");
            dic.Add("MH-JAB", "Jabat");
            dic.Add("MH-JAL", "Jaluit");
            dic.Add("MH-KIL", "Kili");
            dic.Add("MH-KWA", "Kwajalein");
            dic.Add("MH-LAE", "Lae");
            dic.Add("MH-LIB", "Lib");
            dic.Add("MH-LIK", "Likiep");
            dic.Add("MH-MAJ", "Majuro");
            dic.Add("MH-MAL", "Maloelap");
            dic.Add("MH-MEJ", "Mejit");
            dic.Add("MH-MIL", "Mili");
            dic.Add("MH-NMK", "Namdrik");
            dic.Add("MH-NMU", "Namu");
            dic.Add("MH-RON", "Rongelap");
            dic.Add("MH-UJA", "Ujae");
            dic.Add("MH-UTI", "Utirik");
            dic.Add("MH-WTH", "Wotho");
            dic.Add("MH-WTJ", "Wotje");
            pdic.Add("MH", dic);
            #endregion

            /*=============================================*/

            #region Mauritania
            dic = new Dictionary<string, string>();
            dic.Add("MR-07", "Adrar");
            dic.Add("MR-03", "Assaba");
            dic.Add("MR-05", "Brakna");
            dic.Add("MR-08", "Dakhlet Nouâdhibou");
            dic.Add("MR-04", "Gorgol");
            dic.Add("MR-10", "Guidimaka");
            dic.Add("MR-01", "Hodh ech Chargui");
            dic.Add("MR-02", "Hodh el Gharbi");
            dic.Add("MR-12", "Inchiri");
            dic.Add("MR-NKC", "Nouakchott");
            dic.Add("MR-09", "Tagant");
            dic.Add("MR-11", "Tiris Zemmour");
            dic.Add("MR-06", "Trarza");
            pdic.Add("MR", dic);
            #endregion

            /*=============================================*/

            #region Mauritius
            dic = new Dictionary<string, string>();
            dic.Add("MU-AG", "Agalega Islands");
            dic.Add("MU-BR", "Beau Bassin-Rose Hill");
            dic.Add("MU-BL", "Black River");
            dic.Add("MU-CC", "Cargados Carajos Shoals");
            dic.Add("MU-CU", "Curepipe");
            dic.Add("MU-FL", "Flacq");
            dic.Add("MU-GP", "Grand Port");
            dic.Add("MU-MO", "Moka");
            dic.Add("MU-PA", "Pamplemousses");
            dic.Add("MU-PW", "Plaines Wilhems");
            dic.Add("MU-PL", "Port Louis City");
            dic.Add("MU-PU", "Port Louis District");
            dic.Add("MU-QB", "Quatre Bornes");
            dic.Add("MU-RR", "Rivière du Rempart");
            dic.Add("MU-RO", "Rodrigues Island");
            dic.Add("MU-SA", "Savanne");
            dic.Add("MU-VP", "Vacoas-Phoenix");
            pdic.Add("MU", dic);
            #endregion

            /*=============================================*/

            #region Mexico
            dic = new Dictionary<string, string>();
            dic.Add("MX-AGU", "Aguascalientes");
            dic.Add("MX-BCN", "Baja California");
            dic.Add("MX-BCS", "Baja California Sur");
            dic.Add("MX-CAM", "Campeche");
            dic.Add("MX-CHP", "Chiapas");
            dic.Add("MX-CHH", "Chihuahua");
            dic.Add("MX-COA", "Coahuila");
            dic.Add("MX-COL", "Colima");
            dic.Add("MX-DIF", "Distrito Federal");
            dic.Add("MX-DUR", "Durango");
            dic.Add("MX-GUA", "Guanajuato");
            dic.Add("MX-GRO", "Guerrero");
            dic.Add("MX-HID", "Hidalgo");
            dic.Add("MX-JAL", "Jalisco");
            dic.Add("MX-MEX", "México");
            dic.Add("MX-MIC", "Michoacán");
            dic.Add("MX-MOR", "Morelos");
            dic.Add("MX-NAY", "Nayarit");
            dic.Add("MX-NLE", "Nuevo León");
            dic.Add("MX-OAX", "Oaxaca");
            dic.Add("MX-PUE", "Puebla");
            dic.Add("MX-QUE", "Querétaro");
            dic.Add("MX-ROO", "Quintana Roo");
            dic.Add("MX-SLP", "San Luis Potosí");
            dic.Add("MX-SIN", "Sinaloa");
            dic.Add("MX-SON", "Sonora");
            dic.Add("MX-TAB", "Tabasco");
            dic.Add("MX-TAM", "Tamaulipas");
            dic.Add("MX-TLA", "Tlaxcala");
            dic.Add("MX-VER", "Veracruz");
            dic.Add("MX-YUC", "Yucatán");
            dic.Add("MX-ZAC", "Zacatecas");
            pdic.Add("MX", dic);
            #endregion

            /*=============================================*/

            #region Micronesia, Federated States of
            dic = new Dictionary<string, string>();
            dic.Add("FM-TRK", "Chuuk");
            dic.Add("FM-KSA", "Kosrae");
            dic.Add("FM-PNI", "Pohnpei");
            dic.Add("FM-YAP", "Yap");
            pdic.Add("FM", dic);
            #endregion

            /*=============================================*/

            #region Mongolia
            dic = new Dictionary<string, string>();
            dic.Add("MN-073", "Arhangay");
            dic.Add("MN-071", "Bayan-Ölgiy");
            dic.Add("MN-069", "Bayanhongor");
            dic.Add("MN-067", "Bulgan");
            dic.Add("MN-037", "Darhan uul");
            dic.Add("MN-061", "Dornod");
            dic.Add("MN-063", "Dornogovi");
            dic.Add("MN-059", "Dundgovi");
            dic.Add("MN-057", "Dzavhan");
            dic.Add("MN-065", "Govi-Altay");
            dic.Add("MN-064", "Govi-Sümber");
            dic.Add("MN-039", "Hentiy");
            dic.Add("MN-043", "Hovd");
            dic.Add("MN-041", "Hövsgöl");
            dic.Add("MN-053", "Ömnögovi");
            dic.Add("MN-035", "Orhon");
            dic.Add("MN-055", "Övörhangay");
            dic.Add("MN-049", "Selenge");
            dic.Add("MN-051", "Sühbaatar");
            dic.Add("MN-047", "Töv");
            dic.Add("MN-1", "Ulaanbaatar");
            dic.Add("MN-046", "Uvs");
            pdic.Add("MN", dic);
            #endregion

            /*=============================================*/

            #region Morocco
            dic = new Dictionary<string, string>();
            dic.Add("MA-HAO", "Al Haouz");
            dic.Add("MA-HOC", "Al Hoceïma");
            dic.Add("MA-ASZ", "Assa-Zag");
            dic.Add("MA-AZI", "Azilal");
            dic.Add("MA-BES", "Ben Slimane");
            dic.Add("MA-BEM", "Beni Mellal");
            dic.Add("MA-BER", "Berkane");
            dic.Add("MA-BOD", "Boujdour (EH)");
            dic.Add("MA-BOM", "Boulemane");
            dic.Add("MA-CHE", "Chefchaouen");
            dic.Add("MA-CHI", "Chichaoua");
            dic.Add("MA-CHT", "Chtouka-Ait Baha");
            dic.Add("MA-HAJ", "El Hajeb");
            dic.Add("MA-JDI", "El Jadida");
            dic.Add("MA-ERR", "Errachidia");
            dic.Add("MA-ESM", "Es Smara (EH)");
            dic.Add("MA-ESI", "Essaouira");
            dic.Add("MA-FIG", "Figuig");
            dic.Add("MA-GUE", "Guelmim");
            dic.Add("MA-IFR", "Ifrane");
            dic.Add("MA-JRA", "Jrada");
            dic.Add("MA-KES", "Kelaat es Sraghna");
            dic.Add("MA-KEN", "Kénitra");
            dic.Add("MA-KHE", "Khemisset");
            dic.Add("MA-KHN", "Khenifra");
            dic.Add("MA-KHO", "Khouribga");
            dic.Add("MA-LAA", "Laâyoune");
            dic.Add("MA-X1", "Laayoune-Boujdour-Sakia El Hamra");
            dic.Add("MA-LAR", "Larache");
            dic.Add("MA-MED", "Mediouna");
            dic.Add("MA-MOU", "Moulay Yacoub");
            dic.Add("MA-NAD", "Nador");
            dic.Add("MA-NOU", "Nouaceur");
            dic.Add("MA-OUA", "Ouarzazate");
            dic.Add("MA-OUD", "Oued ed Dahab (EH)");
            dic.Add("MA-SAF", "Safi");
            dic.Add("MA-SEF", "Sefrou");
            dic.Add("MA-SET", "Settat");
            dic.Add("MA-SIK", "Sidi Kacem");
            dic.Add("MA-TNT", "Tan-Tan");
            dic.Add("MA-TAO", "Taounate");
            dic.Add("MA-TAI", "Taourirt");
            dic.Add("MA-TAR", "Taroudant");
            dic.Add("MA-TAT", "Tata");
            dic.Add("MA-TAZ", "Taza");
            dic.Add("MA-TIZ", "Tiznit");
            dic.Add("MA-ZAG", "Zagora");
            pdic.Add("MA", dic);
            #endregion

            /*=============================================*/

            #region Mozambique
            dic = new Dictionary<string, string>();
            dic.Add("MZ-P", "Cabo Delgado");
            dic.Add("MZ-G", "Gaza");
            dic.Add("MZ-I", "Inhambane");
            dic.Add("MZ-B", "Manica");
            dic.Add("MZ-L", "Maputo");
            dic.Add("MZ-MPM", "Maputo City");
            dic.Add("MZ-N", "Nampula");
            dic.Add("MZ-A", "Niassa");
            dic.Add("MZ-S", "Sofala");
            dic.Add("MZ-T", "Tete");
            dic.Add("MZ-Q", "Zambézia");
            pdic.Add("MZ", dic);
            #endregion

            /*=============================================*/

            #region Myanmar
            dic = new Dictionary<string, string>();
            dic.Add("MM-07", "Ayeyarwady");
            dic.Add("MM-02", "Bago");
            dic.Add("MM-14", "Chin");
            dic.Add("MM-11", "Kachin");
            dic.Add("MM-12", "Kayah");
            dic.Add("MM-13", "Kayin");
            dic.Add("MM-03", "Magway");
            dic.Add("MM-04", "Mandalay");
            dic.Add("MM-15", "Mon");
            dic.Add("MM-16", "Rakhine");
            dic.Add("MM-01", "Sagaing");
            dic.Add("MM-17", "Shan");
            dic.Add("MM-05", "Tanintharyi");
            dic.Add("MM-06", "Yangon");
            pdic.Add("MM", dic);
            #endregion

            /*=============================================*/

            #region Namibia
            dic = new Dictionary<string, string>();
            dic.Add("NA-CA", "Caprivi");
            dic.Add("NA-ER", "Erongo");
            dic.Add("NA-HA", "Hardap");
            dic.Add("NA-KA", "Karas");
            dic.Add("NA-KH", "Khomas");
            dic.Add("NA-KU", "Kunene");
            dic.Add("NA-OW", "Ohangwena");
            dic.Add("NA-OK", "Okavango");
            dic.Add("NA-OH", "Omaheke");
            dic.Add("NA-OS", "Omusati");
            dic.Add("NA-ON", "Oshana");
            dic.Add("NA-OT", "Oshikoto");
            dic.Add("NA-OD", "Otjozondjupa");
            pdic.Add("NA", dic);
            #endregion

            /*=============================================*/

            #region Nauru
            dic = new Dictionary<string, string>();
            dic.Add("NR-01", "Aiwo");
            dic.Add("NR-02", "Anabar");
            dic.Add("NR-03", "Anetan");
            dic.Add("NR-04", "Anibare");
            dic.Add("NR-05", "Baiti");
            dic.Add("NR-06", "Boe");
            dic.Add("NR-07", "Buada");
            dic.Add("NR-08", "Denigomodu");
            dic.Add("NR-09", "Ewa");
            dic.Add("NR-10", "Ijuw");
            dic.Add("NR-11", "Meneng");
            dic.Add("NR-12", "Nibok");
            dic.Add("NR-13", "Uaboe");
            dic.Add("NR-14", "Yaren");
            pdic.Add("NR", dic);
            #endregion

            /*=============================================*/

            #region Nepal
            dic = new Dictionary<string, string>();
            dic.Add("NP-BA", "Bagmati");
            dic.Add("NP-BH", "Bheri");
            dic.Add("NP-DH", "Dhawalagiri");
            dic.Add("NP-GA", "Gandaki");
            dic.Add("NP-JA", "Janakpur");
            dic.Add("NP-KA", "Karnali");
            dic.Add("NP-KO", "Kosi");
            dic.Add("NP-LU", "Lumbini");
            dic.Add("NP-MA", "Mahakali");
            dic.Add("NP-ME", "Mechi");
            dic.Add("NP-NA", "Narayani");
            dic.Add("NP-RA", "Rapti");
            dic.Add("NP-SA", "Sagarmatha");
            dic.Add("NP-SE", "Seti");
            pdic.Add("NP", dic);
            #endregion

            /*=============================================*/

            #region Netherlands
            dic = new Dictionary<string, string>();
            dic.Add("NL-DR", "Drenthe");
            dic.Add("NL-FL", "Flevoland");
            dic.Add("NL-FR", "Friesland");
            dic.Add("NL-GE", "Gelderland");
            dic.Add("NL-GR", "Groningen");
            dic.Add("NL-LI", "Limburg");
            dic.Add("NL-NB", "Noord-Brabant");
            dic.Add("NL-NH", "Noord-Holland");
            dic.Add("NL-OV", "Overijssel");
            dic.Add("NL-UT", "Utrecht");
            dic.Add("NL-ZE", "Zeeland");
            dic.Add("NL-ZH", "Zuid-Holland");
            pdic.Add("NL", dic);
            #endregion

            /*=============================================*/

            #region New Zealand
            dic = new Dictionary<string, string>();
            dic.Add("NZ-AUK", "Auckland");
            dic.Add("NZ-BOP", "Bay of Plenty");
            dic.Add("NZ-CAN", "Canterbury");
            dic.Add("NZ-CIT", "Chatham Islands Territory");
            dic.Add("NZ-GIS", "Gisborne District");
            dic.Add("NZ-HKB", "Hawkes's Bay");
            dic.Add("NZ-MWT", "Manawatu-Wanganui");
            dic.Add("NZ-MBH", "Marlborough District");
            dic.Add("NZ-NSN", "Nelson City");
            dic.Add("NZ-N", "North Island");
            dic.Add("NZ-NTL", "Northland");
            dic.Add("NZ-OTA", "Otago");
            dic.Add("NZ-S", "South Island");
            dic.Add("NZ-STL", "Southland");
            dic.Add("NZ-TKI", "Taranaki");
            dic.Add("NZ-TAS", "Tasman District");
            dic.Add("NZ-WKO", "Waikato");
            dic.Add("NZ-WGN", "Wellington");
            dic.Add("NZ-WTC", "West Coast");
            pdic.Add("NZ", dic);
            #endregion

            /*=============================================*/

            #region Nicaragua
            dic = new Dictionary<string, string>();
            dic.Add("NI-AN", "Atlántico Norte");
            dic.Add("NI-AS", "Atlántico Sur");
            dic.Add("NI-BO", "Boaco");
            dic.Add("NI-CA", "Carazo");
            dic.Add("NI-CI", "Chinandega");
            dic.Add("NI-CO", "Chontales");
            dic.Add("NI-ES", "Estelí");
            dic.Add("NI-GR", "Granada");
            dic.Add("NI-JI", "Jinotega");
            dic.Add("NI-LE", "León");
            dic.Add("NI-MD", "Madriz");
            dic.Add("NI-MN", "Managua");
            dic.Add("NI-MS", "Masaya");
            dic.Add("NI-MT", "Matagalpa");
            dic.Add("NI-NS", "Nueva Segovia");
            dic.Add("NI-SJ", "Río San Juan");
            dic.Add("NI-RI", "Rivas");
            pdic.Add("NI", dic);
            #endregion

            /*=============================================*/

            #region Niger
            dic = new Dictionary<string, string>();
            dic.Add("NE-1", "Agadez");
            dic.Add("NE-2", "Diffa");
            dic.Add("NE-3", "Dosso");
            dic.Add("NE-4", "Maradi");
            dic.Add("NE-8", "Niamey");
            dic.Add("NE-5", "Tahoua");
            dic.Add("NE-6", "Tillabéri");
            dic.Add("NE-7", "Zinder");
            pdic.Add("NE", dic);
            #endregion

            /*=============================================*/

            #region Nigeria
            dic = new Dictionary<string, string>();
            dic.Add("NG-AB", "Abia");
            dic.Add("NG-FC", "Abuja Federal Capital Territory");
            dic.Add("NG-AD", "Adamawa");
            dic.Add("NG-AK", "Akwa Ibom");
            dic.Add("NG-AN", "Anambra");
            dic.Add("NG-BA", "Bauchi");
            dic.Add("NG-BY", "Bayelsa");
            dic.Add("NG-BE", "Benue");
            dic.Add("NG-BO", "Borno");
            dic.Add("NG-CR", "Cross River");
            dic.Add("NG-DE", "Delta");
            dic.Add("NG-EB", "Ebonyi");
            dic.Add("NG-ED", "Edo");
            dic.Add("NG-EK", "Ekiti");
            dic.Add("NG-EN", "Enugu");
            dic.Add("NG-GO", "Gombe");
            dic.Add("NG-IM", "Imo");
            dic.Add("NG-JI", "Jigawa");
            dic.Add("NG-KD", "Kaduna");
            dic.Add("NG-KN", "Kano");
            dic.Add("NG-KT", "Katsina");
            dic.Add("NG-KE", "Kebbi");
            dic.Add("NG-KO", "Kogi");
            dic.Add("NG-KW", "Kwara");
            dic.Add("NG-LA", "Lagos");
            dic.Add("NG-NA", "Nassarawa");
            dic.Add("NG-NI", "Niger");
            dic.Add("NG-OG", "Ogun");
            dic.Add("NG-ON", "Ondo");
            dic.Add("NG-OS", "Osun");
            dic.Add("NG-OY", "Oyo");
            dic.Add("NG-PL", "Plateau");
            dic.Add("NG-RI", "Rivers");
            dic.Add("NG-SO", "Sokoto");
            dic.Add("NG-TA", "Taraba");
            dic.Add("NG-YO", "Yobe");
            dic.Add("NG-ZA", "Zamfara");
            pdic.Add("NG", dic);
            #endregion

            /*=============================================*/

            #region Northern Ireland
            dic = new Dictionary<string, string>();
            dic.Add("NIR-ANT", "Antrim");
            dic.Add("NIR-ARD", "Ards");
            dic.Add("NIR-ARM", "Armagh, County of");
            dic.Add("NIR-BLA", "Ballymena");
            dic.Add("NIR-BLY", "Ballymoney");
            dic.Add("NIR-BNB", "Banbridge");
            dic.Add("NIR-BFS", "Belfast");
            dic.Add("NIR-CKF", "Carrickfergus");
            dic.Add("NIR-CSR", "Castlereagh");
            dic.Add("NIR-CLR", "Coleraine");
            dic.Add("NIR-CKT", "Cookstown");
            dic.Add("NIR-CGV", "Craigavon");
            dic.Add("NIR-DRY", "Derry");
            dic.Add("NIR-DOW", "Down");
            dic.Add("NIR-DGN", "Dungannon");
            dic.Add("NIR-FER", "Fermanagh, County of");
            dic.Add("NIR-LRN", "Larne");
            dic.Add("NIR-LMV", "Limavady");
            dic.Add("NIR-LSB", "Lisburn");
            dic.Add("NIR-MFT", "Magherafelt");
            dic.Add("NIR-MYL", "Moyle");
            dic.Add("NIR-NYM", "Newry and Mourne");
            dic.Add("NIR-NTA", "Newtownabbey");
            dic.Add("NIR-NDN", "North Down");
            dic.Add("NIR-OMH", "Omagh");
            dic.Add("NIR-STB", "Strabane");
            pdic.Add("ND", dic);
            #endregion

            /*=============================================*/

            #region Norway
            dic = new Dictionary<string, string>();
            dic.Add("NO-02", "Akershus");
            dic.Add("NO-09", "Aust-Agder");
            dic.Add("NO-06", "Buskerud");
            dic.Add("NO-20", "Finnmark");
            dic.Add("NO-04", "Hedmark");
            dic.Add("NO-12", "Hordaland");
            dic.Add("NO-22", "Jan Mayen");
            dic.Add("NO-15", "Møre og Romsdal");
            dic.Add("NO-17", "Nord-Trøndelag");
            dic.Add("NO-18", "Nordland");
            dic.Add("NO-05", "Oppland");
            dic.Add("NO-03", "Oslo");
            dic.Add("NO-11", "Rogaland");
            dic.Add("NO-14", "Sogn og Fjordane");
            dic.Add("NO-21", "Svalbard");
            dic.Add("NO-16", "Sør-Trøndelag");
            dic.Add("NO-08", "Telemark");
            dic.Add("NO-19", "Troms");
            dic.Add("NO-10", "Vest-Agder");
            dic.Add("NO-07", "Vestfold");
            dic.Add("NO-01", "Østfold");
            pdic.Add("NO", dic);
            #endregion

            /*=============================================*/

            #region Pakistan
            dic = new Dictionary<string, string>();
            dic.Add("PK-JK", "Azad Kashmir");
            dic.Add("PK-BA", "Baluchistan (en)");
            dic.Add("PK-TA", "Federally Administered Tribal Areas");
            dic.Add("PK-IS", "Islamabad");
            dic.Add("PK-NW", "North-West Frontier");
            dic.Add("PK-NA", "Northern Areas");
            dic.Add("PK-PB", "Punjab");
            dic.Add("PK-SD", "Sind (en)");
            pdic.Add("PK", dic);
            #endregion

            /*=============================================*/

            #region Palau
            dic = new Dictionary<string, string>();
            dic.Add("PW-002", "Aimeliik");
            dic.Add("PW-004", "Airai");
            dic.Add("PW-010", "Angaur");
            dic.Add("PW-050", "Hatobohei");
            dic.Add("PW-100", "Kayangel");
            dic.Add("PW-150", "Koror");
            dic.Add("PW-212", "Melekeok");
            dic.Add("PW-214", "Ngaraard");
            dic.Add("PW-218", "Ngarchelong");
            dic.Add("PW-222", "Ngardmau");
            dic.Add("PW-224", "Ngatpang");
            dic.Add("PW-226", "Ngchesar");
            dic.Add("PW-227", "Ngeremlengui");
            dic.Add("PW-228", "Ngiwal");
            dic.Add("PW-350", "Peleliu");
            dic.Add("PW-370", "Sonsorol");
            pdic.Add("PW", dic);
            #endregion

            /*=============================================*/

            #region Panama
            dic = new Dictionary<string, string>();
            dic.Add("PA-1", "Bocas del Toro");
            dic.Add("PA-4", "Chiriquí");
            dic.Add("PA-2", "Coclé");
            dic.Add("PA-3", "Colón");
            dic.Add("PA-5", "Darién");
            dic.Add("PA-EM", "Emberá");
            dic.Add("PA-6", "Herrera");
            dic.Add("PA-KY", "Kuna Yala");
            dic.Add("PA-7", "Los Santos");
            dic.Add("PA-NB", "Ngöbe-Buglé");
            dic.Add("PA-8", "Panamá");
            dic.Add("PA-9", "Veraguas");
            pdic.Add("PA", dic);
            #endregion

            /*=============================================*/

            #region Papua New Guinea
            dic = new Dictionary<string, string>();
            dic.Add("PG-CPM", "Central");
            dic.Add("PG-CPK", "Chimbu");
            dic.Add("PG-EBR", "East New Britain");
            dic.Add("PG-ESW", "East Sepik");
            dic.Add("PG-EHG", "Eastern Highlands");
            dic.Add("PG-EPW", "Enga");
            dic.Add("PG-GPK", "Gulf");
            dic.Add("PG-MPM", "Madang");
            dic.Add("PG-MRL", "Manus");
            dic.Add("PG-MBA", "Milne Bay");
            dic.Add("PG-MPL", "Morobe");
            dic.Add("PG-NCD", "National Capital District (Port Moresby)");
            dic.Add("PG-NIK", "New Ireland");
            dic.Add("PG-NSA", "North Solomons");
            dic.Add("PG-NPP", "Northern");
            dic.Add("PG-SAN", "Sandaun");
            dic.Add("PG-SHM", "Southern Highlands");
            dic.Add("PG-WBK", "West New Britain");
            dic.Add("PG-WPD", "Western");
            dic.Add("PG-WHM", "Western Highlands");
            pdic.Add("PG", dic);
            #endregion

            /*=============================================*/

            #region Paraguay
            dic = new Dictionary<string, string>();
            dic.Add("PY-16", "Alto Paraguay");
            dic.Add("PY-10", "Alto Paraná");
            dic.Add("PY-13", "Amambay");
            dic.Add("PY-ASU", "Asunción");
            dic.Add("PY-19", "Boquerón");
            dic.Add("PY-5", "Caaguazú");
            dic.Add("PY-6", "Caazapá");
            dic.Add("PY-14", "Canindeyú");
            dic.Add("PY-11", "Central");
            dic.Add("PY-1", "Concepción");
            dic.Add("PY-3", "Cordillera");
            dic.Add("PY-4", "Guairá");
            dic.Add("PY-7", "Itapúa");
            dic.Add("PY-8", "Misiones");
            dic.Add("PY-12", "Ñeembucú");
            dic.Add("PY-9", "Paraguarí");
            dic.Add("PY-15", "Presidente Hayes");
            dic.Add("PY-2", "San Pedro");
            pdic.Add("PY", dic);
            #endregion

            /*=============================================*/

            #region Peru
            dic = new Dictionary<string, string>();
            dic.Add("PE-AMA", "Amazonas");
            dic.Add("PE-ANC", "Ancash");
            dic.Add("PE-APU", "Apurímac");
            dic.Add("PE-ARE", "Arequipa");
            dic.Add("PE-AYA", "Ayacucho");
            dic.Add("PE-CAJ", "Cajamarca");
            dic.Add("PE-CUS", "Cusco");
            dic.Add("PE-CAL", "El Callao");
            dic.Add("PE-HUV", "Huancavelica");
            dic.Add("PE-HUC", "Huánuco");
            dic.Add("PE-ICA", "Ica");
            dic.Add("PE-JUN", "Junín");
            dic.Add("PE-LAL", "La Libertad");
            dic.Add("PE-LAM", "Lambayeque");
            dic.Add("PE-LIM", "Lima");
            dic.Add("PE-LOR", "Loreto");
            dic.Add("PE-MDD", "Madre de Dios");
            dic.Add("PE-MOQ", "Moquegua");
            dic.Add("PE-LMA", "Municipalidad Metropolitana de Lima");
            dic.Add("PE-PAS", "Pasco");
            dic.Add("PE-PIU", "Piura");
            dic.Add("PE-PUN", "Puno");
            dic.Add("PE-SAM", "San Martín");
            dic.Add("PE-TAC", "Tacna");
            dic.Add("PE-TUM", "Tumbes");
            dic.Add("PE-UCA", "Ucayali");
            pdic.Add("PE", dic);
            #endregion

            /*=============================================*/

            #region Philippines
            dic = new Dictionary<string, string>();
            dic.Add("PH-ABR", "Abra");
            dic.Add("PH-AGN", "Agusan del Norte");
            dic.Add("PH-AGS", "Agusan del Sur");
            dic.Add("PH-AKL", "Aklan");
            dic.Add("PH-ALB", "Albay");
            dic.Add("PH-ANT", "Antique");
            dic.Add("PH-APA", "Apayao");
            dic.Add("PH-AUR", "Aurora");
            dic.Add("PH-14", "Autonomous Region in Muslim Mindanao (ARMM)");
            dic.Add("PH-BAS", "Basilan");
            dic.Add("PH-BAN", "Bataan");
            dic.Add("PH-BTN", "Batanes");
            dic.Add("PH-BTG", "Batangas");
            dic.Add("PH-BEN", "Benguet");
            dic.Add("PH-05", "Bicol (Region V)");
            dic.Add("PH-BIL", "Biliran");
            dic.Add("PH-BOH", "Bohol");
            dic.Add("PH-BUK", "Bukidnon");
            dic.Add("PH-BUL", "Bulacan");
            dic.Add("PH-CAG", "Cagayan");
            dic.Add("PH-02", "Cagayan Valley (Region II)");
            dic.Add("PH-40", "CALABARZON (Region IV-A)");
            dic.Add("PH-CAN", "Camarines Norte");
            dic.Add("PH-CAS", "Camarines Sur");
            dic.Add("PH-CAM", "Camiguin");
            dic.Add("PH-CAP", "Capiz");
            dic.Add("PH-13", "Caraga (Region XIII)");
            dic.Add("PH-CAT", "Catanduanes");
            dic.Add("PH-CAV", "Cavite");
            dic.Add("PH-CEB", "Cebu");
            dic.Add("PH-03", "Central Luzon (Region III)");
            dic.Add("PH-07", "Central Visayas (Region VII)");
            dic.Add("PH-COM", "Compostela Valley");
            dic.Add("PH-15", "Cordillera Administrative Region (CAR)");
            dic.Add("PH-11", "Davao (Region XI)");
            dic.Add("PH-DAV", "Davao del Norte");
            dic.Add("PH-DAS", "Davao del Sur");
            dic.Add("PH-DAO", "Davao Oriental");
            dic.Add("PH-DIN", "Dinagat Islands");
            dic.Add("PH-EAS", "Eastern Samar");
            dic.Add("PH-08", "Eastern Visayas (Region VIII)");
            dic.Add("PH-GUI", "Guimaras");
            dic.Add("PH-IFU", "Ifugao");
            dic.Add("PH-01", "Ilocos (Region I)");
            dic.Add("PH-ILN", "Ilocos Norte");
            dic.Add("PH-ILS", "Ilocos Sur");
            dic.Add("PH-ILI", "Iloilo");
            dic.Add("PH-ISA", "Isabela");
            dic.Add("PH-KAL", "Kalinga");
            dic.Add("PH-LUN", "La Union");
            dic.Add("PH-LAG", "Laguna");
            dic.Add("PH-LAN", "Lanao del Norte");
            dic.Add("PH-LAS", "Lanao del Sur");
            dic.Add("PH-LEY", "Leyte");
            dic.Add("PH-MAG", "Maguindanao");
            dic.Add("PH-MAD", "Marinduque");
            dic.Add("PH-MAS", "Masbate");
            dic.Add("PH-41", "MIMAROPA (Region IV-B)");
            dic.Add("PH-MDC", "Mindoro Occidental");
            dic.Add("PH-MDR", "Mindoro Oriental");
            dic.Add("PH-MSC", "Misamis Occidental");
            dic.Add("PH-MSR", "Misamis Oriental");
            dic.Add("PH-MOU", "Mountain Province");
            dic.Add("PH-00", "National Capital Region");
            dic.Add("PH-NEC", "Negros Occidental");
            dic.Add("PH-NER", "Negros Oriental");
            dic.Add("PH-NCO", "North Cotabato");
            dic.Add("PH-10", "Northern Mindanao (Region X)");
            dic.Add("PH-NSA", "Northern Samar");
            dic.Add("PH-NUE", "Nueva Ecija");
            dic.Add("PH-NUV", "Nueva Vizcaya");
            dic.Add("PH-PLW", "Palawan");
            dic.Add("PH-PAM", "Pampanga");
            dic.Add("PH-PAN", "Pangasinan");
            dic.Add("PH-QUE", "Quezon");
            dic.Add("PH-QUI", "Quirino");
            dic.Add("PH-RIZ", "Rizal");
            dic.Add("PH-ROM", "Romblon");
            dic.Add("PH-SAR", "Sarangani");
            dic.Add("PH-X2", "Shariff Kabunsuan");
            dic.Add("PH-SIG", "Siquijor");
            dic.Add("PH-12", "Soccsksargen (Region XII)");
            dic.Add("PH-SOR", "Sorsogon");
            dic.Add("PH-SCO", "South Cotabato");
            dic.Add("PH-SLE", "Southern Leyte");
            dic.Add("PH-SUK", "Sultan Kudarat");
            dic.Add("PH-SLU", "Sulu");
            dic.Add("PH-SUN", "Surigao del Norte");
            dic.Add("PH-SUR", "Surigao del Sur");
            dic.Add("PH-TAR", "Tarlac");
            dic.Add("PH-TAW", "Tawi-Tawi");
            dic.Add("PH-WSA", "Western Samar");
            dic.Add("PH-06", "Western Visayas (Region VI)");
            dic.Add("PH-ZMB", "Zambales");
            dic.Add("PH-ZAN", "Zamboanga del Norte");
            dic.Add("PH-ZAS", "Zamboanga del Sur");
            dic.Add("PH-09", "Zamboanga Peninsula (Region IX)");
            dic.Add("PH-ZSI", "Zamboanga Sibugue");
            pdic.Add("PH", dic);
            #endregion

            /*=============================================*/

            #region Poland
            dic = new Dictionary<string, string>();
            dic.Add("PL-DS", "Dolnoslaskie");
            dic.Add("PL-KP", "Kujawsko-pomorskie");
            dic.Add("PL-LD", "Lódzkie");
            dic.Add("PL-LU", "Lubelskie");
            dic.Add("PL-LB", "Lubuskie");
            dic.Add("PL-MA", "Malopolskie");
            dic.Add("PL-MZ", "Mazowieckie");
            dic.Add("PL-OP", "Opolskie");
            dic.Add("PL-PK", "Podkarpackie");
            dic.Add("PL-PD", "Podlaskie");
            dic.Add("PL-PM", "Pomorskie");
            dic.Add("PL-SL", "Slaskie");
            dic.Add("PL-SK", "Swietokrzyskie");
            dic.Add("PL-WN", "Warminsko-mazurskie");
            dic.Add("PL-WP", "Wielkopolskie");
            dic.Add("PL-ZP", "Zachodniopomorskie");
            pdic.Add("PL", dic);
            #endregion

            /*=============================================*/

            #region Portugal
            dic = new Dictionary<string, string>();
            dic.Add("PT-01", "Aveiro");
            dic.Add("PT-02", "Beja");
            dic.Add("PT-03", "Braga");
            dic.Add("PT-04", "Bragança");
            dic.Add("PT-05", "Castelo Branco");
            dic.Add("PT-06", "Coimbra");
            dic.Add("PT-07", "Évora");
            dic.Add("PT-08", "Faro");
            dic.Add("PT-09", "Guarda");
            dic.Add("PT-10", "Leiria");
            dic.Add("PT-11", "Lisboa");
            dic.Add("PT-12", "Portalegre");
            dic.Add("PT-13", "Porto");
            dic.Add("PT-30", "Região Autónoma da Madeira");
            dic.Add("PT-20", "Região Autónoma dos Açores");
            dic.Add("PT-14", "Santarém");
            dic.Add("PT-15", "Setúbal");
            dic.Add("PT-16", "Viana do Castelo");
            dic.Add("PT-17", "Vila Real");
            dic.Add("PT-18", "Viseu");
            pdic.Add("PT", dic);
            #endregion

            /*=============================================*/

            #region Qatar
            dic = new Dictionary<string, string>();
            dic.Add("QA-DA", "Ad Dawhah");
            dic.Add("QA-GH", "Al Ghuwayriyah");
            dic.Add("QA-JU", "Al Jumayliyah");
            dic.Add("QA-KH", "Al Khawr");
            dic.Add("QA-WA", "Al Wakrah");
            dic.Add("QA-RA", "Ar Rayyan");
            dic.Add("QA-JB", "Jariyan al Batnah");
            dic.Add("QA-MS", "Madinat ash Shamal");
            dic.Add("QA-X1", "Umm Sa'id");
            dic.Add("QA-US", "Umm Salal");
            pdic.Add("QA", dic);
            #endregion

            /*=============================================*/

            #region Russian Federation
            dic = new Dictionary<string, string>();
            dic.Add("RU-AD", "Adygeya, Respublika");
            dic.Add("RU-AL", "Altay, Respublika");
            dic.Add("RU-ALT", "Altayskiy kray");
            dic.Add("RU-AMU", "Amurskaya oblast'");
            dic.Add("RU-ARK", "Arkhangel'skaya oblast'");
            dic.Add("RU-AST", "Astrakhanskaya oblast'");
            dic.Add("RU-BA", "Bashkortostan, Respublika");
            dic.Add("RU-BEL", "Belgorodskaya oblast'");
            dic.Add("RU-BRY", "Bryanskaya oblast'");
            dic.Add("RU-BU", "Buryatiya, Respublika");
            dic.Add("RU-CE", "Chechenskaya Respublika");
            dic.Add("RU-CHE", "Chelyabinskaya oblast'");
            dic.Add("RU-CHU", "Chukotskiy avtonomnyy okrug");
            dic.Add("RU-CU", "Chuvashskaya Respublika");
            dic.Add("RU-DA", "Dagestan, Respublika");
            dic.Add("RU-IN", "Ingushskaya Respublika");
            dic.Add("RU-IRK", "Irkutskaya oblast'");
            dic.Add("RU-IVA", "Ivanovskaya oblast'");
            dic.Add("RU-KB", "Kabardino-Balkarskaya Respublika");
            dic.Add("RU-KGD", "Kaliningradskaya oblast'");
            dic.Add("RU-KL", "Kalmykiya, Respublika");
            dic.Add("RU-KLU", "Kaluzhskaya oblast'");
            dic.Add("RU-KAM", "Kamchatskaya oblast'");
            dic.Add("RU-KC", "Karachayevo-Cherkesskaya Respublika");
            dic.Add("RU-KR", "Kareliya, Respublika");
            dic.Add("RU-KEM", "Kemerovskaya oblast'");
            dic.Add("RU-KHA", "Khabarovskiy kray");
            dic.Add("RU-KK", "Khakasiya, Respublika");
            dic.Add("RU-KHM", "Khant");
            dic.Add("RU-KIR", "Kirovskaya oblast'");
            dic.Add("RU-KO", "Komi, Respublika");
            dic.Add("RU-X1", "Komi-Permyak");
            dic.Add("RU-KOS", "Kostromskaya oblast'");
            dic.Add("RU-KDA", "Krasnodarskiy kray");
            dic.Add("RU-KYA", "Krasnoyarskiy kray");
            dic.Add("RU-KGN", "Kurganskaya oblast'");
            dic.Add("RU-KRS", "Kurskaya oblast'");
            dic.Add("RU-LEN", "Leningradskaya oblast'");
            dic.Add("RU-LIP", "Lipetskaya oblast'");
            dic.Add("RU-MAG", "Magadanskaya oblast'");
            dic.Add("RU-ME", "Mariy El, Respublika");
            dic.Add("RU-MO", "Mordoviya, Respublika");
            dic.Add("RU-MOS", "Moskovskaya oblast'");
            dic.Add("RU-MOW", "Moskva");
            dic.Add("RU-MUR", "Murmanskaya oblast'");
            dic.Add("RU-NEN", "Nenetskiy avtonomnyy okrug");
            dic.Add("RU-NIZ", "Nizhegorodskaya oblast'");
            dic.Add("RU-NGR", "Novgorodskaya oblast'");
            dic.Add("RU-NVS", "Novosibirskaya oblast'");
            dic.Add("RU-OMS", "Omskaya oblast'");
            dic.Add("RU-ORE", "Orenburgskaya oblast'");
            dic.Add("RU-ORL", "Orlovskaya oblast'");
            dic.Add("RU-PNZ", "Penzenskaya oblast'");
            dic.Add("RU-PER", "Perm");
            dic.Add("RU-PRI", "Primorskiy kray");
            dic.Add("RU-PSK", "Pskovskaya oblast'");
            dic.Add("RU-ROS", "Rostovskaya oblast'");
            dic.Add("RU-RYA", "Ryazanskaya oblast'");
            dic.Add("RU-SA", "Sakha, R");
            dic.Add("RU-SAK", "Sakhalinskaya oblast'");
            dic.Add("RU-SAM", "Samarskaya oblast'");
            dic.Add("RU-SPE", "Sankt-Peterburg");
            dic.Add("RU-SAR", "Saratovskaya oblast'");
            dic.Add("RU-SE", "Severnaya Osetiya, Respublika");
            dic.Add("RU-SMO", "Smolenskaya oblast'");
            dic.Add("RU-STA", "Stavropol'skiy kray");
            dic.Add("RU-SVE", "Sverdlovskaya oblast'");
            dic.Add("RU-TAM", "Tambovskaya oblast'");
            dic.Add("RU-TA", "Tatarstan, Respublika");
            dic.Add("RU-TOM", "Tomskaya oblast'");
            dic.Add("RU-TUL", "Tul'skaya oblast'");
            dic.Add("RU-TVE", "Tverskaya oblast'");
            dic.Add("RU-TYU", "Tyumenskaya oblast'");
            dic.Add("RU-TY", "Tyva");
            dic.Add("RU-UD", "Udmurtskaya Respublika");
            dic.Add("RU-ULY", "Ul'yanovskaya oblast'");
            dic.Add("RU-VLA", "Vladimirskaya oblast'");
            dic.Add("RU-VGG", "Volgogradskaya oblast'");
            dic.Add("RU-VLG", "Vologodskaya oblast'");
            dic.Add("RU-VOR", "Voronezhskaya oblast'");
            dic.Add("RU-YAN", "Yamalo-Nenetskiy avtonomnyy okrug");
            dic.Add("RU-YAR", "Yaroslavskaya oblast'");
            dic.Add("RU-YEV", "Yevreyskaya avtonomnaya oblast'");
            dic.Add("RU-ZAB", "Zabajkal'skij kraj");
            pdic.Add("RU", dic);
            #endregion

            /*=============================================*/

            #region Rwanda
            dic = new Dictionary<string, string>();
            dic.Add("RW-02", "Est");
            dic.Add("RW-03", "Nord");
            dic.Add("RW-04", "Ouest");
            dic.Add("RW-05", "Sud");
            dic.Add("RW-01", "Ville de Kigali");
            pdic.Add("RW", dic);
            #endregion

            /*=============================================*/

            #region Saint Kitts and Nevis
            dic = new Dictionary<string, string>();
            dic.Add("KN-01", "Christ Church Nichola Town");
            dic.Add("KN-02", "Saint Anne Sandy Point");
            dic.Add("KN-03", "Saint George Basseterre");
            dic.Add("KN-04", "Saint George Gingerland");
            dic.Add("KN-05", "Saint James Windward");
            dic.Add("KN-06", "Saint John Capisterre");
            dic.Add("KN-07", "Saint John Figtree");
            dic.Add("KN-08", "Saint Mary Cayon");
            dic.Add("KN-09", "Saint Paul Capisterre");
            dic.Add("KN-10", "Saint Paul Charlestown");
            dic.Add("KN-11", "Saint Peter Basseterre");
            dic.Add("KN-12", "Saint Thomas Lowland");
            dic.Add("KN-13", "Saint Thomas Middle Island");
            dic.Add("KN-15", "Trinity Palmetto Point");
            pdic.Add("KN", dic);
            #endregion

            /*=============================================*/

            #region Saint Vincent and the Grenadines
            dic = new Dictionary<string, string>();
            dic.Add("VC-01", "Charlotte");
            dic.Add("VC-06", "Grenadines");
            dic.Add("VC-02", "Saint Andrew");
            dic.Add("VC-03", "Saint David");
            dic.Add("VC-04", "Saint George");
            dic.Add("VC-05", "Saint Patrick");
            pdic.Add("VC", dic);
            #endregion

            /*=============================================*/

            #region Samoa
            dic = new Dictionary<string, string>();
            dic.Add("WS-AA", "A'ana");
            dic.Add("WS-AL", "Aiga-i-le-Tai");
            dic.Add("WS-AT", "Atua");
            dic.Add("WS-FA", "Fa'asaleleaga");
            dic.Add("WS-GE", "Gaga'emauga");
            dic.Add("WS-GI", "Gagaifomauga");
            dic.Add("WS-PA", "Palauli");
            dic.Add("WS-SA", "Satupa'itea");
            dic.Add("WS-TU", "Tuamasaga");
            dic.Add("WS-VF", "Va'a-o-Fonoti");
            dic.Add("WS-VS", "Vaisigano");
            pdic.Add("WS", dic);
            #endregion

            /*=============================================*/

            #region San Marino
            dic = new Dictionary<string, string>();
            dic.Add("SM-01", "Acquaviva");
            dic.Add("SM-06", "Borgo Maggiore");
            dic.Add("SM-02", "Chiesanuova");
            dic.Add("SM-03", "Domagnano");
            dic.Add("SM-04", "Faetano");
            dic.Add("SM-05", "Fiorentino");
            dic.Add("SM-08", "Montegiardino");
            dic.Add("SM-07", "San Marino");
            dic.Add("SM-09", "Serravalle");
            pdic.Add("SM", dic);
            #endregion

            /*=============================================*/

            #region Sao Tome and Principe
            dic = new Dictionary<string, string>();
            dic.Add("ST-P", "Príncipe");
            dic.Add("ST-S", "São Tomé");
            pdic.Add("ST", dic);
            #endregion

            /*=============================================*/

            #region Scotland
            dic = new Dictionary<string, string>();
            dic.Add("SCT-ABE", "Aberdeen City");
            dic.Add("SCT-ABD", "Aberdeenshire");
            dic.Add("SCT-ANS", "Angus");
            dic.Add("SCT-AGB", "Argyll and Bute");
            dic.Add("SCT-CLK", "Clackmannanshire");
            dic.Add("SCT-DGY", "Dumfries and Galloway");
            dic.Add("SCT-DND", "Dundee City");
            dic.Add("SCT-EAY", "East Ayrshire");
            dic.Add("SCT-EDU", "East Dunbartonshire");
            dic.Add("SCT-ELN", "East Lothian");
            dic.Add("SCT-ERW", "East Renfrewshire");
            dic.Add("SCT-EDH", "Edinburgh, City of");
            dic.Add("SCT-ELS", "Eilean Siar");
            dic.Add("SCT-FAL", "Falkirk");
            dic.Add("SCT-FIF", "Fife");
            dic.Add("SCT-GLG", "Glasgow City");
            dic.Add("SCT-HLD", "Highland");
            dic.Add("SCT-IVC", "Inverclyde");
            dic.Add("SCT-MLN", "Midlothian");
            dic.Add("SCT-MRY", "Moray");
            dic.Add("SCT-NAY", "North Ayrshire");
            dic.Add("SCT-NLK", "North Lanarkshire");
            dic.Add("SCT-NBL", "Northumberland");
            dic.Add("SCT-ORK", "Orkney Islands");
            dic.Add("SCT-PKN", "Perth and Kinross");
            dic.Add("SCT-RFW", "Renfrewshire");
            dic.Add("SCT-SCB", "Scottish Borders, The");
            dic.Add("SCT-ZET", "Shetland Islands");
            dic.Add("SCT-SAY", "South Ayrshire");
            dic.Add("SCT-SLK", "South Lanarkshire");
            dic.Add("SCT-STG", "Stirling");
            dic.Add("SCT-WDU", "West Dunbartonshire");
            dic.Add("SCT-WLN", "West Lothian");
            pdic.Add("SS", dic);
            #endregion

            /*=============================================*/

            #region Senegal
            dic = new Dictionary<string, string>();
            dic.Add("SN-DK", "Dakar");
            dic.Add("SN-DB", "Diourbel");
            dic.Add("SN-FK", "Fatick");
            dic.Add("SN-KA", "Kaffrine");
            dic.Add("SN-KL", "Kaolack");
            dic.Add("SN-KE", "Kédougou");
            dic.Add("SN-KD", "Kolda");
            dic.Add("SN-LG", "Louga");
            dic.Add("SN-MT", "Matam");
            dic.Add("SN-SL", "Saint-Louis");
            dic.Add("SN-SE", "Sédhiou");
            dic.Add("SN-TC", "Tambacounda");
            dic.Add("SN-TH", "Thiès");
            dic.Add("SN-ZG", "Ziguinchor");
            pdic.Add("SN", dic);
            #endregion

            /*=============================================*/

            #region Seychelles
            dic = new Dictionary<string, string>();
            dic.Add("SC-01", "Anse aux Pins");
            dic.Add("SC-02", "Anse Boileau");
            dic.Add("SC-03", "Anse Étoile");
            dic.Add("SC-05", "Anse Royale");
            dic.Add("SC-04", "Au Cap");
            dic.Add("SC-06", "Baie Lazare");
            dic.Add("SC-07", "Baie Sainte Anne");
            dic.Add("SC-08", "Beau Vallon");
            dic.Add("SC-09", "Bel Air");
            dic.Add("SC-10", "Bel Ombre");
            dic.Add("SC-11", "Cascade");
            dic.Add("SC-16", "English River");
            dic.Add("SC-12", "Glacis");
            dic.Add("SC-13", "Grand Anse Mahe");
            dic.Add("SC-14", "Grand Anse Praslin");
            dic.Add("SC-15", "La Digue");
            dic.Add("SC-24", "Les Mamelles");
            dic.Add("SC-17", "Mont Buxton");
            dic.Add("SC-18", "Mont Fleuri");
            dic.Add("SC-19", "Plaisance");
            dic.Add("SC-20", "Pointe La Rue");
            dic.Add("SC-21", "Port Glaud");
            dic.Add("SC-25", "Roche Caiman");
            dic.Add("SC-22", "Saint Louis");
            dic.Add("SC-23", "Takamaka");
            pdic.Add("SC", dic);
            #endregion

            /*=============================================*/

            #region Sierra Leone
            dic = new Dictionary<string, string>();
            dic.Add("SL-E", "Eastern");
            dic.Add("SL-N", "Northern");
            dic.Add("SL-S", "Southern");
            dic.Add("SL-W", "Western Area (Freetown)");
            pdic.Add("SL", dic);
            #endregion

            /*=============================================*/

            #region Solomon Islands
            dic = new Dictionary<string, string>();
            dic.Add("SB-CT", "Capital Territory (Honiara)");
            dic.Add("SB-CE", "Central");
            dic.Add("SB-CH", "Choiseul");
            dic.Add("SB-GU", "Guadalcanal");
            dic.Add("SB-IS", "Isabel");
            dic.Add("SB-MK", "Makira");
            dic.Add("SB-ML", "Malaita");
            dic.Add("SB-RB", "Rennell and Bellona");
            dic.Add("SB-TE", "Temotu");
            dic.Add("SB-WE", "Western");
            pdic.Add("SB", dic);
            #endregion

            /*=============================================*/

            #region Somalia
            dic = new Dictionary<string, string>();
            dic.Add("SO-AW", "Awdal");
            dic.Add("SO-BK", "Bakool");
            dic.Add("SO-BN", "Banaadir");
            dic.Add("SO-BR", "Bari");
            dic.Add("SO-BY", "Bay");
            dic.Add("SO-GA", "Galguduud");
            dic.Add("SO-GE", "Gedo");
            dic.Add("SO-HI", "Hiiraan");
            dic.Add("SO-JD", "Jubbada Dhexe");
            dic.Add("SO-JH", "Jubbada Hoose");
            dic.Add("SO-MU", "Mudug");
            dic.Add("SO-NU", "Nugaal");
            dic.Add("SO-SA", "Sanaag");
            dic.Add("SO-SD", "Shabeellaha Dhexe");
            dic.Add("SO-SH", "Shabeellaha Hoose");
            dic.Add("SO-SO", "Sool");
            dic.Add("SO-TO", "Togdheer");
            dic.Add("SO-WO", "Woqooyi Galbeed");
            pdic.Add("SO", dic);
            #endregion

            /*=============================================*/

            #region South Africa
            dic = new Dictionary<string, string>();
            dic.Add("ZA-EC", "Eastern Cape");
            dic.Add("ZA-FS", "Free State");
            dic.Add("ZA-GT", "Gauteng");
            dic.Add("ZA-NL", "Kwazulu-Natal");
            dic.Add("ZA-LP", "Limpopo");
            dic.Add("ZA-MP", "Mpumalanga");
            dic.Add("ZA-NW", "North-West");
            dic.Add("ZA-NC", "Northern Cape");
            dic.Add("ZA-WC", "Western Cape");
            pdic.Add("ZA", dic);
            #endregion

            /*=============================================*/

            #region Spain
            dic = new Dictionary<string, string>();
            dic.Add("ES-C", "A Coruña");
            dic.Add("ES-VI", "Álava");
            dic.Add("ES-AB", "Albacete");
            dic.Add("ES-A", "Alicante");
            dic.Add("ES-AL", "Almería");
            dic.Add("ES-O", "Asturias");
            dic.Add("ES-AV", "Ávila");
            dic.Add("ES-BA", "Badajoz");
            dic.Add("ES-PM", "Baleares");
            dic.Add("ES-B", "Barcelona");
            dic.Add("ES-BU", "Burgos");
            dic.Add("ES-CC", "Cáceres");
            dic.Add("ES-CA", "Cádiz");
            dic.Add("ES-S", "Cantabria");
            dic.Add("ES-CS", "Castellón");
            dic.Add("ES-CE", "Ceuta");
            dic.Add("ES-CR", "Ciudad Real");
            dic.Add("ES-CO", "Córdoba");
            dic.Add("ES-CU", "Cuenca");
            dic.Add("ES-GI", "Girona");
            dic.Add("ES-GR", "Granada");
            dic.Add("ES-GU", "Guadalajara");
            dic.Add("ES-SS", "Guipúzcoa");
            dic.Add("ES-H", "Huelva");
            dic.Add("ES-HU", "Huesca");
            dic.Add("ES-J", "Jaén");
            dic.Add("ES-LO", "La Rioja");
            dic.Add("ES-GC", "Las Palmas");
            dic.Add("ES-LE", "León");
            dic.Add("ES-L", "Lleida");
            dic.Add("ES-LU", "Lugo");
            dic.Add("ES-M", "Madrid");
            dic.Add("ES-MA", "Málaga");
            dic.Add("ES-ML", "Melilla");
            dic.Add("ES-MU", "Murcia");
            dic.Add("ES-NA", "Navarra");
            dic.Add("ES-OR", "Ourense");
            dic.Add("ES-P", "Palencia");
            dic.Add("ES-PO", "Pontevedra");
            dic.Add("ES-SA", "Salamanca");
            dic.Add("ES-TF", "Santa Cruz de Tenerife");
            dic.Add("ES-SG", "Segovia");
            dic.Add("ES-SE", "Sevilla");
            dic.Add("ES-SO", "Soria");
            dic.Add("ES-T", "Tarragona");
            dic.Add("ES-TE", "Teruel");
            dic.Add("ES-TO", "Toledo");
            dic.Add("ES-V", "Valencia");
            dic.Add("ES-VA", "Valladolid");
            dic.Add("ES-BI", "Vizcaya");
            dic.Add("ES-ZA", "Zamora");
            dic.Add("ES-Z", "Zaragoza");
            pdic.Add("ES", dic);
            #endregion

            /*=============================================*/

            #region St. Helena
            dic = new Dictionary<string, string>();
            dic.Add("SH-AC", "Ascension");
            dic.Add("SH-SH", "Saint Helena");
            dic.Add("SH-TA", "Tristan da Cunha");
            pdic.Add("SH", dic);
            #endregion

            /*=============================================*/

            #region Suriname
            dic = new Dictionary<string, string>();
            dic.Add("SR-BR", "Brokopondo");
            dic.Add("SR-CM", "Commewijne");
            dic.Add("SR-CR", "Coronie");
            dic.Add("SR-MA", "Marowijne");
            dic.Add("SR-NI", "Nickerie");
            dic.Add("SR-PR", "Para");
            dic.Add("SR-PM", "Paramaribo");
            dic.Add("SR-SA", "Saramacca");
            dic.Add("SR-SI", "Sipaliwini");
            dic.Add("SR-WA", "Wanica");
            pdic.Add("SR", dic);
            #endregion

            /*=============================================*/

            #region Swaziland
            dic = new Dictionary<string, string>();
            dic.Add("SZ-HH", "Hhohho");
            dic.Add("SZ-LU", "Lubombo");
            dic.Add("SZ-MA", "Manzini");
            dic.Add("SZ-SH", "Shiselweni");
            pdic.Add("SZ", dic);
            #endregion

            /*=============================================*/

            #region Sweden
            dic = new Dictionary<string, string>();
            dic.Add("SE-K", "Blekinge län");
            dic.Add("SE-W", "Dalarnas län");
            dic.Add("SE-X", "Gävleborgs län");
            dic.Add("SE-I", "Gotlands län");
            dic.Add("SE-N", "Hallands län");
            dic.Add("SE-Z", "Jämtlands län");
            dic.Add("SE-F", "Jönköpings län");
            dic.Add("SE-H", "Kalmar län");
            dic.Add("SE-G", "Kronobergs län");
            dic.Add("SE-BD", "Norrbottens län");
            dic.Add("SE-T", "Örebro län");
            dic.Add("SE-E", "Östergötlands län");
            dic.Add("SE-M", "Skåne län");
            dic.Add("SE-D", "Södermanlands län");
            dic.Add("SE-AB", "Stockholms län");
            dic.Add("SE-C", "Uppsala län");
            dic.Add("SE-S", "Värmlands län");
            dic.Add("SE-AC", "Västerbottens län");
            dic.Add("SE-Y", "Västernorrlands län");
            dic.Add("SE-U", "Västmanlands län");
            dic.Add("SE-O", "Västra Götalands län");
            pdic.Add("SE", dic);
            #endregion

            /*=============================================*/

            #region Switzerland
            dic = new Dictionary<string, string>();
            dic.Add("CH-AG", "Aargau (de)");
            dic.Add("CH-AR", "Appenzell Ausserrhoden (de)");
            dic.Add("CH-AI", "Appenzell Innerrhoden (de)");
            dic.Add("CH-BL", "Basel-Landschaft (de)");
            dic.Add("CH-BS", "Basel-Stadt (de)");
            dic.Add("CH-BE", "Bern (de)");
            dic.Add("CH-FR", "Fribourg (fr)");
            dic.Add("CH-GE", "Genève (fr)");
            dic.Add("CH-GL", "Glarus (de)");
            dic.Add("CH-GR", "Graubünden (de)");
            dic.Add("CH-JU", "Jura (fr)");
            dic.Add("CH-LU", "Luzern (de)");
            dic.Add("CH-NE", "Neuchâtel (fr)");
            dic.Add("CH-NW", "Nidwalden (de)");
            dic.Add("CH-OW", "Obwalden (de)");
            dic.Add("CH-SG", "Sankt Gallen (de)");
            dic.Add("CH-SH", "Schaffhausen (de)");
            dic.Add("CH-SZ", "Schwyz (de)");
            dic.Add("CH-SO", "Solothurn (de)");
            dic.Add("CH-TG", "Thurgau (de)");
            dic.Add("CH-TI", "Ticino (it)");
            dic.Add("CH-UR", "Uri (de)");
            dic.Add("CH-VS", "Valais (fr)");
            dic.Add("CH-VD", "Vaud (fr)");
            dic.Add("CH-ZG", "Zug (de)");
            dic.Add("CH-ZH", "Zürich (de)");
            pdic.Add("CH", dic);
            #endregion

            /*=============================================*/

            #region Taiwan
            dic = new Dictionary<string, string>();
            dic.Add("TW-CHA", "Changhua");
            dic.Add("TW-CYQ", "Chiayi");
            dic.Add("TW-CYI", "Chiayi Municipality");
            dic.Add("TW-HSQ", "Hsinchu");
            dic.Add("TW-HSZ", "Hsinchu Municipality");
            dic.Add("TW-HUA", "Hualien");
            dic.Add("TW-ILA", "Ilan");
            dic.Add("TW-KHQ", "Kaohsiung");
            dic.Add("TW-KHH", "Kaohsiung Special Municipality");
            dic.Add("TW-KEE", "Keelung Municipality");
            dic.Add("TW-MIA", "Miaoli");
            dic.Add("TW-NAN", "Nantou");
            dic.Add("TW-PEN", "Penghu");
            dic.Add("TW-PIF", "Pingtung");
            dic.Add("TW-TXQ", "Taichung");
            dic.Add("TW-TXG", "Taichung Municipality");
            dic.Add("TW-TNQ", "Tainan");
            dic.Add("TW-TNN", "Tainan Municipality");
            dic.Add("TW-TPQ", "Taipei");
            dic.Add("TW-TPE", "Taipei Special Municipality");
            dic.Add("TW-TTT", "Taitung");
            dic.Add("TW-TAO", "Taoyuan");
            dic.Add("TW-YUN", "Yunlin");
            pdic.Add("TW", dic);
            #endregion

            /*=============================================*/

            #region Tajikistan
            dic = new Dictionary<string, string>();
            dic.Add("TJ-GB", "Gorno-Badakhshan");
            dic.Add("TJ-KT", "Khatlon");
            dic.Add("TJ-SU", "Sughd");
            pdic.Add("TJ", dic);
            #endregion

            /*=============================================*/

            #region Tanzania, United Republic of
            dic = new Dictionary<string, string>();
            dic.Add("TZ-01", "Arusha");
            dic.Add("TZ-02", "Dar es Salaam");
            dic.Add("TZ-03", "Dodoma");
            dic.Add("TZ-04", "Iringa");
            dic.Add("TZ-05", "Kagera");
            dic.Add("TZ-06", "Kaskazini Pemba");
            dic.Add("TZ-07", "Kaskazini Unguja");
            dic.Add("TZ-08", "Kigoma");
            dic.Add("TZ-09", "Kilimanjaro");
            dic.Add("TZ-10", "Kusini Pemba");
            dic.Add("TZ-11", "Kusini Unguja");
            dic.Add("TZ-12", "Lindi");
            dic.Add("TZ-26", "Manyara");
            dic.Add("TZ-13", "Mara");
            dic.Add("TZ-14", "Mbeya");
            dic.Add("TZ-15", "Mjini Magharibi");
            dic.Add("TZ-16", "Morogoro");
            dic.Add("TZ-17", "Mtwara");
            dic.Add("TZ-18", "Mwanza");
            dic.Add("TZ-19", "Pwani");
            dic.Add("TZ-20", "Rukwa");
            dic.Add("TZ-21", "Ruvuma");
            dic.Add("TZ-22", "Shinyanga");
            dic.Add("TZ-23", "Singida");
            dic.Add("TZ-24", "Tabora");
            dic.Add("TZ-25", "Tanga");
            pdic.Add("TZ", dic);
            #endregion

            /*=============================================*/

            #region Thailand
            dic = new Dictionary<string, string>();
            dic.Add("TH-37", "Amnat Charoen");
            dic.Add("TH-15", "Ang Thong");
            dic.Add("TH-31", "Buri Ram");
            dic.Add("TH-24", "Chachoengsao");
            dic.Add("TH-18", "Chai Nat");
            dic.Add("TH-36", "Chaiyaphum");
            dic.Add("TH-22", "Chanthaburi");
            dic.Add("TH-50", "Chiang Mai");
            dic.Add("TH-57", "Chiang Rai");
            dic.Add("TH-20", "Chon Buri");
            dic.Add("TH-86", "Chumphon");
            dic.Add("TH-46", "Kalasin");
            dic.Add("TH-62", "Kamphaeng Phet");
            dic.Add("TH-71", "Kanchanaburi");
            dic.Add("TH-40", "Khon Kaen");
            dic.Add("TH-81", "Krabi");
            dic.Add("TH-10", "Krung T");
            dic.Add("TH-52", "Lampang");
            dic.Add("TH-51", "Lamphun");
            dic.Add("TH-42", "Loei");
            dic.Add("TH-16", "Lop Buri");
            dic.Add("TH-58", "Mae Hong Son");
            dic.Add("TH-44", "Maha Sarakham");
            dic.Add("TH-49", "Mukdahan");
            dic.Add("TH-26", "Nakhon Nayok");
            dic.Add("TH-73", "Nakhon Pathom");
            dic.Add("TH-48", "Nakhon Phanom");
            dic.Add("TH-30", "Nakhon Ratchasima");
            dic.Add("TH-60", "Nakhon Sawan");
            dic.Add("TH-80", "Nakhon Si Thammarat");
            dic.Add("TH-55", "Nan");
            dic.Add("TH-96", "Narathiwat");
            dic.Add("TH-39", "Nong Bua Lam Phu");
            dic.Add("TH-43", "Nong Khai");
            dic.Add("TH-12", "Nonthaburi");
            dic.Add("TH-13", "Pathum Thani");
            dic.Add("TH-94", "Pattani");
            dic.Add("TH-82", "Phangnga");
            dic.Add("TH-93", "Phatthalung");
            dic.Add("TH-S", "Phatthaya");
            dic.Add("TH-56", "Phayao");
            dic.Add("TH-67", "Phetchabun");
            dic.Add("TH-76", "Phetchaburi");
            dic.Add("TH-66", "Phichit");
            dic.Add("TH-65", "Phitsanulok");
            dic.Add("TH-14", "Phra Nakhon Si Ayutthaya");
            dic.Add("TH-54", "Phrae");
            dic.Add("TH-83", "Phuket");
            dic.Add("TH-25", "Prachin Buri");
            dic.Add("TH-77", "Prachuap Khiri Khan");
            dic.Add("TH-85", "Ranong");
            dic.Add("TH-70", "Ratchaburi");
            dic.Add("TH-21", "Rayong");
            dic.Add("TH-45", "Roi Et");
            dic.Add("TH-27", "Sa Kaeo");
            dic.Add("TH-47", "Sakon Nakhon");
            dic.Add("TH-11", "Samut Prakan");
            dic.Add("TH-74", "Samut Sakhon");
            dic.Add("TH-75", "Samut Songkhram");
            dic.Add("TH-19", "Saraburi");
            dic.Add("TH-91", "Satun");
            dic.Add("TH-33", "Si Sa Ket");
            dic.Add("TH-17", "Sing Buri");
            dic.Add("TH-90", "Songkhla");
            dic.Add("TH-64", "Sukhothai");
            dic.Add("TH-72", "Suphan Buri");
            dic.Add("TH-84", "Surat Thani");
            dic.Add("TH-32", "Surin");
            dic.Add("TH-63", "Tak");
            dic.Add("TH-92", "Trang");
            dic.Add("TH-23", "Trat");
            dic.Add("TH-34", "Ubon Ratchathani");
            dic.Add("TH-41", "Udon Thani");
            dic.Add("TH-61", "Uthai Thani");
            dic.Add("TH-53", "Uttaradit");
            dic.Add("TH-95", "Yala");
            dic.Add("TH-35", "Yasothon");
            pdic.Add("TH", dic);
            #endregion

            /*=============================================*/

            #region Togo
            dic = new Dictionary<string, string>();
            dic.Add("TG-C", "Centre");
            dic.Add("TG-K", "Kara");
            dic.Add("TG-M", "Maritime (Région)");
            dic.Add("TG-P", "Plateaux");
            dic.Add("TG-S", "Savannes");
            pdic.Add("TG", dic);
            #endregion

            /*=============================================*/

            #region Tonga
            dic = new Dictionary<string, string>();
            dic.Add("TO-01", "'Eua");
            dic.Add("TO-02", "Ha'apai");
            dic.Add("TO-03", "Niuas");
            dic.Add("TO-04", "Tongatapu");
            dic.Add("TO-05", "Vava'u");
            pdic.Add("TO", dic);
            #endregion

            /*=============================================*/

            #region Trinidad and Tobago
            dic = new Dictionary<string, string>();
            dic.Add("TT-ARI", "Arima");
            dic.Add("TT-CHA", "Chaguanas");
            dic.Add("TT-CTT", "Couva-Tabaquite-Talparo");
            dic.Add("TT-DMN", "Diego Martin");
            dic.Add("TT-ETO", "Eastern Tobago");
            dic.Add("TT-PED", "Penal-Debe");
            dic.Add("TT-PTF", "Point Fortin");
            dic.Add("TT-POS", "Port of Spain");
            dic.Add("TT-PRT", "Princes Town");
            dic.Add("TT-RCM", "Rio Claro-Mayaro");
            dic.Add("TT-SFO", "San Fernando");
            dic.Add("TT-SJL", "San Juan-Laventille");
            dic.Add("TT-SGE", "Sangre Grande");
            dic.Add("TT-SIP", "Siparia");
            dic.Add("TT-TUP", "Tunapuna-Piarco");
            dic.Add("TT-WTO", "Western Tobago");
            pdic.Add("TT", dic);
            #endregion

            /*=============================================*/

            #region Tunisia
            dic = new Dictionary<string, string>();
            dic.Add("TN-31", "Béja");
            dic.Add("TN-13", "Ben Arous");
            dic.Add("TN-23", "Bizerte");
            dic.Add("TN-81", "Gabès");
            dic.Add("TN-71", "Gafsa");
            dic.Add("TN-32", "Jendouba");
            dic.Add("TN-41", "Kairouan");
            dic.Add("TN-42", "Kasserine");
            dic.Add("TN-73", "Kebili");
            dic.Add("TN-12", "L'Ariana");
            dic.Add("TN-14", "La Manouba");
            dic.Add("TN-33", "Le Kef");
            dic.Add("TN-53", "Mahdia");
            dic.Add("TN-82", "Medenine");
            dic.Add("TN-52", "Monastir");
            dic.Add("TN-21", "Nabeul");
            dic.Add("TN-61", "Sfax");
            dic.Add("TN-43", "Sidi Bouzid");
            dic.Add("TN-34", "Siliana");
            dic.Add("TN-51", "Sousse");
            dic.Add("TN-83", "Tataouine");
            dic.Add("TN-72", "Tozeur");
            dic.Add("TN-11", "Tunis");
            dic.Add("TN-22", "Zaghouan");
            pdic.Add("TN", dic);
            #endregion

            /*=============================================*/



            #region Turkey
            dic = new Dictionary<string, string>();
            dic.Add("TR-01", "Adana");
            dic.Add("TR-02", "Adiyaman");
            dic.Add("TR-03", "Afyon");
            dic.Add("TR-04", "Agri");
            dic.Add("TR-68", "Aksaray");
            dic.Add("TR-05", "Amasya");
            dic.Add("TR-06", "Ankara");
            dic.Add("TR-07", "Antalya");
            dic.Add("TR-75", "Ardahan");
            dic.Add("TR-08", "Artvin");
            dic.Add("TR-09", "Aydin");
            dic.Add("TR-10", "Balikesir");
            dic.Add("TR-74", "Bartin");
            dic.Add("TR-72", "Batman");
            dic.Add("TR-69", "Bayburt");
            dic.Add("TR-11", "Bilecik");
            dic.Add("TR-12", "Bingöl");
            dic.Add("TR-13", "Bitlis");
            dic.Add("TR-14", "Bolu");
            dic.Add("TR-15", "Burdur");
            dic.Add("TR-16", "Bursa");
            dic.Add("TR-17", "Çanakkale");
            dic.Add("TR-18", "Çankiri");
            dic.Add("TR-19", "Çorum");
            dic.Add("TR-20", "Denizli");
            dic.Add("TR-21", "Diyarbakir");
            dic.Add("TR-81", "Düzce");
            dic.Add("TR-22", "Edirne");
            dic.Add("TR-23", "Elazig");
            dic.Add("TR-24", "Erzincan");
            dic.Add("TR-25", "Erzurum");
            dic.Add("TR-26", "Eskisehir");
            dic.Add("TR-27", "Gaziantep");
            dic.Add("TR-28", "Giresun");
            dic.Add("TR-29", "Gümüshane");
            dic.Add("TR-30", "Hakkâri");
            dic.Add("TR-31", "Hatay");
            dic.Add("TR-33", "Içel");
            dic.Add("TR-76", "Igdir");
            dic.Add("TR-32", "Isparta");
            dic.Add("TR-34", "Istanbul");
            dic.Add("TR-35", "Izmir");
            dic.Add("TR-46", "Kahramanmaras");
            dic.Add("TR-78", "Karabük");
            dic.Add("TR-70", "Karaman");
            dic.Add("TR-36", "Kars");
            dic.Add("TR-37", "Kastamonu");
            dic.Add("TR-38", "Kayseri");
            dic.Add("TR-79", "Kilis");
            dic.Add("TR-71", "Kirikkale");
            dic.Add("TR-39", "Kirklareli");
            dic.Add("TR-40", "Kirsehir");
            dic.Add("TR-41", "Kocaeli");
            dic.Add("TR-42", "Konya");
            dic.Add("TR-43", "Kütahya");
            dic.Add("TR-44", "Malatya");
            dic.Add("TR-45", "Manisa");
            dic.Add("TR-47", "Mardin");
            dic.Add("TR-48", "Mugla");
            dic.Add("TR-49", "Mus");
            dic.Add("TR-50", "Nevsehir");
            dic.Add("TR-51", "Nigde");
            dic.Add("TR-52", "Ordu");
            dic.Add("TR-80", "Osmaniye");
            dic.Add("TR-53", "Rize");
            dic.Add("TR-54", "Sakarya");
            dic.Add("TR-55", "Samsun");
            dic.Add("TR-63", "Sanliurfa");
            dic.Add("TR-56", "Siirt");
            dic.Add("TR-57", "Sinop");
            dic.Add("TR-73", "Sirnak");
            dic.Add("TR-58", "Sivas");
            dic.Add("TR-59", "Tekirdag");
            dic.Add("TR-60", "Tokat");
            dic.Add("TR-61", "Trabzon");
            dic.Add("TR-62", "Tunceli");
            dic.Add("TR-64", "Usak");
            dic.Add("TR-65", "Van");
            dic.Add("TR-77", "Yalova");
            dic.Add("TR-66", "Yozgat");
            dic.Add("TR-67", "Zonguldak");
            pdic.Add("TR", dic);
            #endregion

            #region Turkmenistan
            dic = new Dictionary<string, string>();
            dic.Add("TM-A", "Ahal");
            dic.Add("TM-B", "Balkan");
            dic.Add("TM-D", "Dasoguz");
            dic.Add("TM-L", "Lebap");
            dic.Add("TM-M", "Mary");
            pdic.Add("TM", dic);
            #endregion

            // Turks and Caicos Islands	TC	(Use free text)

            #region Tuvalu
            dic = new Dictionary<string, string>();
            dic.Add("TV-FUN", "Funafuti");
            dic.Add("TV-NMG", "Nanumanga");
            dic.Add("TV-NMA", "Nanumea");
            dic.Add("TV-NIT", "Niutao");
            dic.Add("TV-NIU", "Nui");
            dic.Add("TV-NKF", "Nukufetau");
            dic.Add("TV-NKL", "Nukulaelae");
            dic.Add("TV-VAI", "Vaitupu");
            pdic.Add("TV", dic);
            #endregion

            #region Uganda
            dic = new Dictionary<string, string>();
            dic.Add("UG-317", "Abim");
            dic.Add("UG-301", "Adjumani");
            dic.Add("UG-314", "Amolatar");
            dic.Add("UG-216", "Amuria");
            dic.Add("UG-319", "Amuru");
            dic.Add("UG-302", "Apac");
            dic.Add("UG-303", "Arua");
            dic.Add("UG-217", "Budaka");
            dic.Add("UG-223", "Bududa");
            dic.Add("UG-201", "Bugiri");
            dic.Add("UG-224", "Bukedea");
            dic.Add("UG-218", "Bukwa");
            dic.Add("UG-419", "Buliisa");
            dic.Add("UG-401", "Bundibugyo");
            dic.Add("UG-402", "Bushenyi");
            dic.Add("UG-202", "Busia");
            dic.Add("UG-219", "Butaleja");
            dic.Add("UG-318", "Dokolo");
            dic.Add("UG-304", "Gulu");
            dic.Add("UG-403", "Hoima");
            dic.Add("UG-416", "Ibanda");
            dic.Add("UG-203", "Iganga");
            dic.Add("UG-417", "Isingiro");
            dic.Add("UG-204", "Jinja");
            dic.Add("UG-315", "Kaabong");
            dic.Add("UG-404", "Kabale");
            dic.Add("UG-405", "Kabarole");
            dic.Add("UG-213", "Kaberamaido");
            dic.Add("UG-101", "Kalangala");
            dic.Add("UG-220", "Kaliro");
            dic.Add("UG-102", "Kampala");
            dic.Add("UG-205", "Kamuli");
            dic.Add("UG-413", "Kamwenge");
            dic.Add("UG-414", "Kanungu");
            dic.Add("UG-206", "Kapchorwa");
            dic.Add("UG-406", "Kasese");
            dic.Add("UG-207", "Katakwi");
            dic.Add("UG-112", "Kayunga");
            dic.Add("UG-407", "Kibaale");
            dic.Add("UG-103", "Kiboga");
            dic.Add("UG-418", "Kiruhura");
            dic.Add("UG-408", "Kisoro");
            dic.Add("UG-305", "Kitgum");
            dic.Add("UG-316", "Koboko");
            dic.Add("UG-306", "Kotido");
            dic.Add("UG-208", "Kumi");
            dic.Add("UG-415", "Kyenjojo");
            dic.Add("UG-307", "Lira");
            dic.Add("UG-104", "Luwero");
            dic.Add("UG-116", "Lyantonde");
            dic.Add("UG-221", "Manafwa");
            dic.Add("UG-320", "Maracha");
            dic.Add("UG-105", "Masaka");
            dic.Add("UG-409", "Masindi");
            dic.Add("UG-214", "Mayuge");
            dic.Add("UG-209", "Mbale");
            dic.Add("UG-410", "Mbarara");
            dic.Add("UG-114", "Mityana");
            dic.Add("UG-308", "Moroto");
            dic.Add("UG-309", "Moyo");
            dic.Add("UG-106", "Mpigi");
            dic.Add("UG-107", "Mubende");
            dic.Add("UG-108", "Mukono");
            dic.Add("UG-311", "Nakapiripirit");
            dic.Add("UG-115", "Nakaseke");
            dic.Add("UG-109", "Nakasongola");
            dic.Add("UG-222", "Namutumba");
            dic.Add("UG-310", "Nebbi");
            dic.Add("UG-411", "Ntungamo");
            dic.Add("UG-321", "Oyam");
            dic.Add("UG-312", "Pader");
            dic.Add("UG-210", "Pallisa");
            dic.Add("UG-110", "Rakai");
            dic.Add("UG-412", "Rukungiri");
            dic.Add("UG-111", "Sembabule");
            dic.Add("UG-215", "Sironko");
            dic.Add("UG-211", "Soroti");
            dic.Add("UG-212", "Tororo");
            dic.Add("UG-113", "Wakiso");
            dic.Add("UG-313", "Yumbe");
            pdic.Add("UG", dic);
            #endregion

            #region Ukraine
            dic = new Dictionary<string, string>();
            dic.Add("UA-71", "Cherkas'ka Oblast'");
            dic.Add("UA-74", "Chernihivs'ka Oblast'");
            dic.Add("UA-77", "Chernivets'ka Oblast'");
            dic.Add("UA-12", "Dnipropetrovs'ka Oblast'");
            dic.Add("UA-14", "Donets'ka Oblast'");
            dic.Add("UA-26", "Ivano-Frankivs'ka Oblast'");
            dic.Add("UA-63", "Kharkivs'ka Oblast'");
            dic.Add("UA-65", "Khersons'ka Oblast'");
            dic.Add("UA-68", "Khmel'nyts'ka Oblast'");
            dic.Add("UA-35", "Kirovohrads'ka Oblast'");
            dic.Add("UA-30", "Kyïv");
            dic.Add("UA-32", "Kyïvs'ka Oblast'");
            dic.Add("UA-46", "L'vivs'ka Oblast'");
            dic.Add("UA-09", "Luhans'ka Oblast'");
            dic.Add("UA-48", "Mykolaïvs'ka Oblast'");
            dic.Add("UA-51", "Odes'ka Oblast'");
            dic.Add("UA-53", "Poltavs'ka Oblast'");
            dic.Add("UA-43", "Respublika Krym");
            dic.Add("UA-56", "Rivnens'ka Oblast'");
            dic.Add("UA-40", "Sevastopol'");
            dic.Add("UA-59", "Sums'ka Oblast'");
            dic.Add("UA-61", "Ternopil's'ka Oblast'");
            dic.Add("UA-05", "Vinnyts'ka Oblast'");
            dic.Add("UA-07", "Volyns'ka Oblast'");
            dic.Add("UA-21", "Zakarpats'ka Oblast'");
            dic.Add("UA-23", "Zaporiz'ka Oblast'");
            dic.Add("UA-18", "Zhytomyrs'ka Oblast'");
            pdic.Add("UA", dic);
            #endregion

            #region United Arab Emirates
            dic = new Dictionary<string, string>();
            dic.Add("AE-AD", "Abu Dhabi");
            dic.Add("AE-AJ", "Ajman");
            dic.Add("AE-FU", "Fujairah");
            dic.Add("AE-SH", "Sharjah");
            dic.Add("AE-DU", "Dubai");
            dic.Add("AE-RK", "Ras al-Khaimah");
            dic.Add("AE-UQ", "Umm al-Qaiwain");
            pdic.Add("AE", dic);
            #endregion

            #region United Kingdom
            dic = new Dictionary<string, string>();
            dic.Add("GB-ABE", "Aberdeen City");
            dic.Add("GB-ABD", "Aberdeenshire");
            dic.Add("GB-ANS", "Angus");
            dic.Add("GB-ANT", "Antrim");
            dic.Add("GB-ARD", "Ards");
            dic.Add("GB-AGB", "Argyll and Bute");
            dic.Add("GB-ARM", "Armagh");
            dic.Add("GB-BLA", "Ballymena");
            dic.Add("GB-BLY", "Ballymoney");
            dic.Add("GB-BNB", "Banbridge");
            dic.Add("GB-BDG", "Barking and Dagenham");
            dic.Add("GB-BNE", "Barnet");
            dic.Add("GB-BNS", "Barnsley");
            dic.Add("GB-BAS", "Bath and North East Somerset");
            dic.Add("GB-BBO", "Bedford Borough");
            dic.Add("GB-BDF", "Bedfordshire");
            dic.Add("GB-BFS", "Belfast");
            dic.Add("GB-BEX", "Bexley");
            dic.Add("GB-BIR", "Birmingham");
            dic.Add("GB-BBD", "Blackburn with Darwen");
            dic.Add("GB-BPL", "Blackpool");
            dic.Add("GB-BGW", "Blaenau Gwent");
            dic.Add("GB-BOL", "Bolton");
            dic.Add("GB-BMH", "Bournemouth");
            dic.Add("GB-BRC", "Bracknell Forest");
            dic.Add("GB-BRD", "Bradford");
            dic.Add("GB-BEN", "Brent");
            dic.Add("GB-BGE", "Bridgend");
            dic.Add("GB-BNH", "Brighton and Hove");
            dic.Add("GB-BST", "Bristol, City of");
            dic.Add("GB-BRY", "Bromley");
            dic.Add("GB-BUC", "Buckingham");
            dic.Add("GB-BKM", "Buckinghamshire");
            dic.Add("GB-BUR", "Bury");
            dic.Add("GB-CAY", "Caerphilly");
            dic.Add("GB-CLD", "Calderdale");
            dic.Add("GB-CAM", "Cambridgeshire");
            dic.Add("GB-CMD", "Camden");
            dic.Add("GB-CRF", "Cardiff");
            dic.Add("GB-CMN", "Carmarthenshire");
            dic.Add("GB-CKF", "Carrickfergus");
            dic.Add("GB-CSR", "Castlereagh");
            dic.Add("GB-CGN", "Ceredigion");
            dic.Add("GB-CHS", "Cheshire");
            dic.Add("GB-CHE", "Cheshire East");
            dic.Add("GB-CWC", "Cheshire West and Chester");
            dic.Add("GB-CLK", "Clackmannanshire");
            dic.Add("GB-CLR", "Coleraine");
            dic.Add("GB-CWY", "Conwy");
            dic.Add("GB-CKT", "Cookstown");
            dic.Add("GB-CON", "Cornwall");
            dic.Add("GB-COV", "Coventry");
            dic.Add("GB-CGV", "Craigavon");
            dic.Add("GB-CRY", "Croydon");
            dic.Add("GB-CMA", "Cumbria");
            dic.Add("GB-DAL", "Darlington");
            dic.Add("GB-DEN", "DenbighshireD");
            dic.Add("GB-DER", "Derby");
            dic.Add("GB-DBY", "Derbyshire");
            dic.Add("GB-DRY", "Derry");
            dic.Add("GB-DEV", "Devon");
            dic.Add("GB-DNC", "Doncaster");
            dic.Add("GB-DOR", "Dorset");
            dic.Add("GB-DOW", "Down");
            dic.Add("GB-DUD", "Dudley");
            dic.Add("GB-DGY", "Dumfries and Galloway");
            dic.Add("GB-DND", "Dundee City");
            dic.Add("GB-DGN", "Dungannon");
            dic.Add("GB-DUR", "Durham");
            dic.Add("GB-EAL", "Ealing");
            dic.Add("GB-EAY", "East Ayrshire");
            dic.Add("GB-EDU", "East Dunbartonshire");
            dic.Add("GB-ELN", "East Lothian");
            dic.Add("GB-ERW", "East Renfrewshire");
            dic.Add("GB-ERY", "East Riding of Yorkshire");
            dic.Add("GB-ESX", "East Sussex");
            dic.Add("GB-EDH", "Edinburgh, City of");
            dic.Add("GB-ELS", "Eilean Siar");
            dic.Add("GB-ENF", "Enfield");
            dic.Add("GB-ESS", "Essex");
            dic.Add("GB-FAL", "Falkirk");
            dic.Add("GB-FER", "Fermanagh");
            dic.Add("GB-FIF", "Fife");
            dic.Add("GB-FLN", "Flintshire");
            dic.Add("GB-GAT", "Gateshead");
            dic.Add("GB-GLG", "Glasgow City");
            dic.Add("GB-GLS", "Gloucestershire");
            dic.Add("GB-GLO", "Greater London");
            dic.Add("GB-GRE", "Greenwich");
            dic.Add("GB-GGY", "Guernsey");
            dic.Add("GB-GWN", "Gwynedd");
            dic.Add("GB-HCK", "Hackney");
            dic.Add("GB-HAL", "Halton");
            dic.Add("GB-HMF", "Hammersmith and Fulham");
            dic.Add("GB-HAM", "Hampshire");
            dic.Add("GB-HRY", "Haringey");
            dic.Add("GB-HRW", "Harrow");
            dic.Add("GB-HPL", "Hartlepool");
            dic.Add("GB-HAV", "Havering");
            dic.Add("GB-HEF", "Herefordshire, County of");
            dic.Add("GB-HTF", "Hertfordshire");
            dic.Add("GB-HRT", "Hertfordshire");
            dic.Add("GB-HLD", "Highland");
            dic.Add("GB-HIL", "Hillingdon");
            dic.Add("GB-HNS", "Hounslow");
            dic.Add("GB-IVC", "Inverclyde");
            dic.Add("GB-AGY", "Isle of Anglesey");
            dic.Add("GB-IOM", "Isle of Man");
            dic.Add("GB-IOW", "Isle of Wight");
            dic.Add("GB-IOS", "Isles of Scilly");
            dic.Add("GB-ISL", "Islington");
            dic.Add("GB-JEY", "Jersey");
            dic.Add("GB-KEC", "Kensington and Chelsea");
            dic.Add("GB-KEN", "Kent");
            dic.Add("GB-KHL", "Kingston upon Hull, City of");
            dic.Add("GB-KTT", "Kingston upon Thames");
            dic.Add("GB-KIR", "Kirklees");
            dic.Add("GB-KWL", "Knowsley");
            dic.Add("GB-LBH", "Lambeth");
            dic.Add("GB-LAN", "Lancashire");
            dic.Add("GB-LRN", "Larne");
            dic.Add("GB-LDS", "Leeds");
            dic.Add("GB-LCE", "Leicester");
            dic.Add("GB-LEC", "Leicestershire");
            dic.Add("GB-LEW", "Lewisham");
            dic.Add("GB-LMV", "Limavady");
            dic.Add("GB-LIN", "Lincolnshire");
            dic.Add("GB-LSB", "Lisburn");
            dic.Add("GB-LIV", "Liverpool");
            dic.Add("GB-LND", "London, City of");
            dic.Add("GB-LUT", "Luton");
            dic.Add("GB-MFT", "Magherafelt");
            dic.Add("GB-MAN", "Manchester");
            dic.Add("GB-MDW", "Medway");
            dic.Add("GB-MTY", "Merthyr Tydfil");
            dic.Add("GB-MRT", "Merton");
            dic.Add("GB-MDB", "Middlesbrough");
            dic.Add("GB-MLN", "Midlothian");
            dic.Add("GB-MIK", "Milton Keynes");
            dic.Add("GB-MON", "Monmouthshire");
            dic.Add("GB-MRY", "Moray");
            dic.Add("GB-MYL", "Moyle");
            dic.Add("GB-NTL", "Neath Port Talbot");
            dic.Add("GB-NET", "Newcastle upon Tyne");
            dic.Add("GB-NWM", "Newham");
            dic.Add("GB-NWP", "Newport");
            dic.Add("GB-NYM", "Newry and Mourne");
            dic.Add("GB-NTA", "Newtownabbey");
            dic.Add("GB-NFK", "Norfolk");
            dic.Add("GB-NAY", "North Ayrshire");
            dic.Add("GB-NDN", "North Down");
            dic.Add("GB-NEL", "North East Lincolnshire");
            dic.Add("GB-NLK", "North Lanarkshire");
            dic.Add("GB-NLN", "North Lincolnshire");
            dic.Add("GB-NSM", "North Somerset");
            dic.Add("GB-NTY", "North Tyneside");
            dic.Add("GB-NYK", "North Yorkshire");
            dic.Add("GB-NTH", "Northamptonshire");
            dic.Add("GB-NBL", "Northumberland");
            dic.Add("GB-NGM", "Nottingham");
            dic.Add("GB-NTT", "Nottinghamshire");
            dic.Add("GB-OLD", "Oldham");
            dic.Add("GB-OMH", "Omagh");
            dic.Add("GB-ORK", "Orkney Islands");
            dic.Add("GB-OXF", "Oxfordshire");
            dic.Add("GB-PEM", "Pembrokeshire");
            dic.Add("GB-PKN", "Perth and Kinross");
            dic.Add("GB-PTE", "Peterborough");
            dic.Add("GB-PLY", "Plymouth");
            dic.Add("GB-POL", "Poole");
            dic.Add("GB-POR", "Portsmouth");
            dic.Add("GB-POW", "Powys");
            dic.Add("GB-RDG", "Reading");
            dic.Add("GB-RDB", "Redbridge");
            dic.Add("GB-RCC", "Redcar and Cleveland");
            dic.Add("GB-RFW", "Renfrewshire");
            dic.Add("GB-RCT", "Rhondda, Cynon, Ta");
            dic.Add("GB-RIC", "Richmond upon Thames");
            dic.Add("GB-RCH", "Rochdale");
            dic.Add("GB-ROT", "Rotherham");
            dic.Add("GB-RUT", "Rutland");
            dic.Add("GB-SLF", "Salford");
            dic.Add("GB-SAW", "Sandwell");
            dic.Add("GB-SCB", "Scottish Borders, The");
            dic.Add("GB-SFT", "Sefton");
            dic.Add("GB-SHF", "Sheffield");
            dic.Add("GB-ZET", "Shetland Islands");
            dic.Add("GB-SHR", "Shropshire");
            dic.Add("GB-SLG", "Slough");
            dic.Add("GB-SOL", "Solihull");
            dic.Add("GB-SOM", "Somerset");
            dic.Add("GB-SAY", "South Ayrshire");
            dic.Add("GB-SGC", "South Gloucestershire");
            dic.Add("GB-SLK", "South Lanarkshire");
            dic.Add("GB-STY", "South Tyneside");
            dic.Add("GB-SYK", "South Yorkshire");
            dic.Add("GB-STH", "Southampton");
            dic.Add("GB-SOS", "Southend-on-Sea");
            dic.Add("GB-SWK", "Southwark");
            dic.Add("GB-SHN", "St. Helens");
            dic.Add("GB-STS", "Staffordshire");
            dic.Add("GB-STG", "Stirling");
            dic.Add("GB-SKP", "Stockport");
            dic.Add("GB-STT", "Stockton-on-Tees");
            dic.Add("GB-STE", "Stoke-on-Trent");
            dic.Add("GB-STB", "Strabane");
            dic.Add("GB-SFK", "Suffolk");
            dic.Add("GB-SND", "Sunderland");
            dic.Add("GB-SRY", "Surrey");
            dic.Add("GB-STN", "Sutton");
            dic.Add("GB-SWA", "Swansea");
            dic.Add("GB-SWD", "Swindon");
            dic.Add("GB-TAM", "Tameside");
            dic.Add("GB-TFW", "Telford and Wrekin");
            dic.Add("GB-THR", "Thurrock");
            dic.Add("GB-TOB", "Torbay");
            dic.Add("GB-TOF", "Torfaen");
            dic.Add("GB-TWH", "Tower Hamlets");
            dic.Add("GB-TRF", "Trafford");
            dic.Add("GB-VGL", "Vale of Glamorgan, T");
            dic.Add("GB-WKF", "Wakefield");
            dic.Add("GB-WLL", "Walsall");
            dic.Add("GB-WFT", "Waltham Forest");
            dic.Add("GB-WND", "Wandsworth");
            dic.Add("GB-WRT", "Warrington");
            dic.Add("GB-WAR", "Warwickshire");
            dic.Add("GB-WBK", "West Berkshire");
            dic.Add("GB-WDU", "West Dunbartonshire");
            dic.Add("GB-WLN", "West Lothian");
            dic.Add("GB-WMD", "West Midlands");
            dic.Add("GB-WSX", "West Sussex");
            dic.Add("GB-WYK", "West Yorkshire");
            dic.Add("GB-WSM", "Westminster");
            dic.Add("GB-WGN", "Wigan");
            dic.Add("GB-WIL", "Wiltshire");
            dic.Add("GB-WNM", "Windsor and Maidenhead");
            dic.Add("GB-WRL", "Wirral");
            dic.Add("GB-WOK", "Wokingham");
            dic.Add("GB-WLV", "Wolverhampton");
            dic.Add("GB-WOC", "Worcester");
            dic.Add("GB-WOR", "Worcestershire");
            dic.Add("GB-WRX", "Wrexham");
            dic.Add("GB-YOR", "York");
            pdic.Add("GB", dic);
            #endregion

            #region US United States
            dic = new Dictionary<string, string>();
            dic.Add("AL", "Alabama");
            dic.Add("AK", "Alaska");
            dic.Add("AS", "American Samoa");
            dic.Add("AZ", "Arizona");
            dic.Add("AR", "Arkansas");
            dic.Add("AE-A", "Armed Forces Africa");
            dic.Add("AA", "Armed Forces Americas");
            dic.Add("AE-C", "Armed Forces Canada");
            dic.Add("AE-E", "Armed Forces Europe");
            dic.Add("AE-M", "Armed Forces Middle East");
            dic.Add("AP", "Armed Forces Pacific");
            dic.Add("CA", "California");
            dic.Add("CO", "Colorado");
            dic.Add("CT", "Connecticut");
            dic.Add("DE", "Delaware");
            dic.Add("DC", "District of Columbia");
            dic.Add("FM", "Federated States of Micronesia");
            dic.Add("FL", "Florida");
            dic.Add("GA", "Georgia");
            dic.Add("GU", "Guam");
            dic.Add("HI", "Hawaii");
            dic.Add("ID", "Idaho");
            dic.Add("IL", "Illinois");
            dic.Add("IN", "Indiana");
            dic.Add("IA", "Iowa");
            dic.Add("KS", "Kansas");
            dic.Add("KY", "Kentucky");
            dic.Add("LA", "Louisiana");
            dic.Add("ME", "Maine");
            dic.Add("MD", "Maryland");
            dic.Add("MA", "Massachusetts");
            dic.Add("MI", "Michigan");
            dic.Add("MN", "Minnesota");
            dic.Add("MS", "Mississippi");
            dic.Add("MO", "Missouri");
            dic.Add("MT", "Montana");
            dic.Add("NE", "Nebraska");
            dic.Add("NV", "Nevada");
            dic.Add("NH", "New Hampshire");
            dic.Add("NJ", "New Jersey");
            dic.Add("NM", "New Mexico");
            dic.Add("NY", "New York");
            dic.Add("NC", "North Carolina");
            dic.Add("ND", "North Dakota");
            dic.Add("MP", "Northern Mariana Islands");
            dic.Add("OH", "Ohio");
            dic.Add("OK", "Oklahoma");
            dic.Add("OR", "Oregon");
            dic.Add("PA", "Pennsylvania");
            dic.Add("PR", "Puerto Rico");
            dic.Add("MH", "Republic of Marshall Islands");
            dic.Add("RI", "Rhode Island");
            dic.Add("SC", "South Carolina");
            dic.Add("SD", "South Dakota");
            dic.Add("TN", "Tennessee");
            dic.Add("TX", "Texas");
            dic.Add("UT", "Utah");
            dic.Add("VT", "Vermont");
            dic.Add("VI", "Virgin Islands of the U.S.");
            dic.Add("VA", "Virginia");
            dic.Add("WA", "Washington");
            dic.Add("WV", "West Virginia");
            dic.Add("WI", "Wisconsin");
            dic.Add("WY", "Wyoming");

            // adding country states to the main dic
            pdic.Add("US", dic);
            #endregion

            // United States Minor Outlying Islands	UM	(Use free text)


            #region UY Uruguay
            dic = new Dictionary<string, string>();
            dic.Add("UY-AR", "Artigas");
            dic.Add("UY-CA", "Canelones");
            dic.Add("UY-CL", "Cerro Largo");
            dic.Add("UY-CO", "Colonia");
            dic.Add("UY-DU", "Durazno");
            dic.Add("UY-FS", "Flores");
            dic.Add("UY-FD", "Florida");
            dic.Add("UY-LA", "Lavalleja");
            dic.Add("UY-MA", "Maldonado");
            dic.Add("UY-MO", "Montevideo");
            dic.Add("UY-PA", "PaysandÃº");
            dic.Add("UY-RN", "RÃ­o Negro");
            dic.Add("UY-RV", "Rivera");
            dic.Add("UY-RO", "Rocha");
            dic.Add("UY-SA", "Salto");
            dic.Add("UY-SJ", "San JosÃ©");
            dic.Add("UY-SO", "Soriano");
            dic.Add("UY-TA", "TacuarembÃ³");
            dic.Add("UY-TT", "Treinta y Tres");
            pdic.Add("UY", dic);
            #endregion

            #region UZ Uzbekistan
            dic = new Dictionary<string, string>();
            dic.Add("UZ-AN", "Andijon");
            dic.Add("UZ-BU", "Buxoro");
            dic.Add("UZ-FA", "Farg'ona");
            dic.Add("UZ-JI", "Jizzax");
            dic.Add("UZ-NG", "Namangan");
            dic.Add("UZ-NW", "Navoiy");
            dic.Add("UZ-QA", "Qashqadaryo");
            dic.Add("UZ-QR", "Qoraqalpog'iston Respublikasi");
            dic.Add("UZ-SA", "Samarqand");
            dic.Add("UZ-SI", "Sirdaryo");
            dic.Add("UZ-SU", "Surxondaryo");
            dic.Add("UZ-TO", "Toshkent");
            dic.Add("UZ-TK", "Toshkent City");
            dic.Add("UZ-XO", "Xorazm");
            pdic.Add("UZ", dic);
            #endregion

            #region Vanuatu
            dic = new Dictionary<string, string>();
            dic.Add("VU-MAP", "Malampa");
            dic.Add("VU-PAM", "Pénama");
            dic.Add("VU-SAM", "Sanma");
            dic.Add("VU-SEE", "Shéfa");
            dic.Add("VU-TAE", "Taféa");
            dic.Add("VU-TOB", "Torba");
            pdic.Add("VU", dic);
            #endregion

            // Vatican City State (Holy See)	VA (Use free text)

            #region Venezuela
            dic = new Dictionary<string, string>();
            dic.Add("VE-Z", "Amazonas");
            dic.Add("VE-B", "Anzoátegui");
            dic.Add("VE-C", "Apure");
            dic.Add("VE-D", "Aragua");
            dic.Add("VE-E", "Barinas");
            dic.Add("VE-F", "Bolívar");
            dic.Add("VE-G", "Carabobo");
            dic.Add("VE-H", "Cojedes");
            dic.Add("VE-Y", "Delta Amacuro");
            dic.Add("VE-W", "Dependencias Federales");
            dic.Add("VE-A", "Distrito Federal");
            dic.Add("VE-I", "Falcón");
            dic.Add("VE-J", "Guárico");
            dic.Add("VE-K", "Lara");
            dic.Add("VE-L", "Mérida");
            dic.Add("VE-M", "Miranda");
            dic.Add("VE-N", "Monagas");
            dic.Add("VE-O", "Nueva Esparta");
            dic.Add("VE-P", "Portuguesa");
            dic.Add("VE-R", "Sucre");
            dic.Add("VE-S", "Táchira");
            dic.Add("VE-T", "Trujillo");
            dic.Add("VE-X", "Vargas");
            dic.Add("VE-U", "Yaracuy");
            dic.Add("VE-V", "Zulia");
            pdic.Add("VE", dic);
            #endregion

            #region Viet Nam
            dic = new Dictionary<string, string>();
            dic.Add("VN-44", "An Giang");
            dic.Add("VN-43", "Ba Ria - Vung Tau");
            dic.Add("VN-53", "Bac Can");
            dic.Add("VN-54", "Bac Giang");
            dic.Add("VN-55", "Bac Lieu");
            dic.Add("VN-56", "Bac Ninh");
            dic.Add("VN-50", "Ben Tre");
            dic.Add("VN-31", "Binh Dinh");
            dic.Add("VN-57", "Binh Duong");
            dic.Add("VN-58", "Binh Phuoc");
            dic.Add("VN-40", "Binh Thuan");
            dic.Add("VN-59", "Ca Mau");
            dic.Add("VN-48", "Can Tho");
            dic.Add("VN-04", "Cao Bang");
            dic.Add("VN-60", "Da Nang, thanh pho");
            dic.Add("VN-33", "Dac Lac");
            dic.Add("VN-72", "Dak Nong");
            dic.Add("VN-71", "Dien Bien");
            dic.Add("VN-39", "Dong Nai");
            dic.Add("VN-45", "Dong Thap");
            dic.Add("VN-30", "Gia Lai");
            dic.Add("VN-03", "Ha Giang");
            dic.Add("VN-63", "Ha Nam");
            dic.Add("VN-64", "Ha Noi, thu do");
            dic.Add("VN-15", "Ha Tay");
            dic.Add("VN-23", "Ha Tinh");
            dic.Add("VN-61", "Hai Duong");
            dic.Add("VN-62", "Hai Phong, thanh pho");
            dic.Add("VN-73", "Hau Giang");
            dic.Add("VN-65", "Ho Chi");
            dic.Add("VN-14", "Hoa Binh");
            dic.Add("VN-66", "Hung Yen");
            dic.Add("VN-34", "Khanh Hoa");
            dic.Add("VN-47", "Kien Giang");
            dic.Add("VN-28", "Kon Tum");
            dic.Add("VN-01", "Lai Chau");
            dic.Add("VN-35", "Lam Dong");
            dic.Add("VN-09", "Lang Son");
            dic.Add("VN-02", "Lao Cai");
            dic.Add("VN-41", "Long An");
            dic.Add("VN-67", "Nam Dinh");
            dic.Add("VN-22", "Nghe An");
            dic.Add("VN-18", "Ninh Binh");
            dic.Add("VN-36", "Ninh Thuan");
            dic.Add("VN-68", "Phu Tho");
            dic.Add("VN-32", "Phu Yen");
            dic.Add("VN-24", "Quang Binh");
            dic.Add("VN-27", "Quang Nam");
            dic.Add("VN-29", "Quang Ngai");
            dic.Add("VN-13", "Quang Ninh");
            dic.Add("VN-25", "Quang Tri");
            dic.Add("VN-52", "Soc Trang");
            dic.Add("VN-05", "Son La");
            dic.Add("VN-37", "Tay Ninh");
            dic.Add("VN-20", "Thai Binh");
            dic.Add("VN-69", "Thai Nguyen");
            dic.Add("VN-21", "Thanh Hoa");
            dic.Add("VN-26", "Thua Thien-Hue");
            dic.Add("VN-46", "Tien Giang");
            dic.Add("VN-51", "Tra Vinh");
            dic.Add("VN-07", "Tuyen Quang");
            dic.Add("VN-49", "Vinh Long");
            dic.Add("VN-70", "Vinh Phuc");
            dic.Add("VN-06", "Yen Bai");
            pdic.Add("VN", dic);
            #endregion

            // Virgin Islands (British)	VG (Use free text)

            // Virgin Islands (U.S.)	VI (Use free text)


            #region Wales
            dic = new Dictionary<string, string>();
            dic.Add("WLS-BGW", "Blaenau Gwent");
            dic.Add("WLS-BGE", "Bridgend");
            dic.Add("WLS-CAY", "Caerphilly");
            dic.Add("WLS-CRF", "Cardiff");
            dic.Add("WLS-CMN", "Carmarthenshire");
            dic.Add("WLS-CGN", "Ceredigion");
            dic.Add("WLS-CWY", "Conwy");
            dic.Add("WLS-DEN", "Denbighshire");
            dic.Add("WLS-GWN", "Gwynedd");
            dic.Add("WLS-AGY", "Isle of Anglesey");
            dic.Add("WLS-MTY", "Merthyr Tydfil");
            dic.Add("WLS-MON", "Monmouthshire");
            dic.Add("WLS-NTL", "Neath Port Talbot");
            dic.Add("WLS-NWP", "Newport");
            dic.Add("WLS-PEM", "Pembrokeshire");
            dic.Add("WLS-POW", "Powys");
            dic.Add("WLS-RCT", "Rhondda Cynon Taff");
            dic.Add("WLS-SWA", "Swansea");
            dic.Add("WLS-TOF", "Torfaen");
            dic.Add("WLS-VGL", "Vale of Glamorgan, The");
            dic.Add("WLS-WRX", "Wrexham");
            pdic.Add("WL", dic);
            #endregion

            // Wallis and Futuna Islands	WF

            #region Western Sahara
            dic = new Dictionary<string, string>();
            dic.Add("EH-BOD", "Boujdour");
            dic.Add("EH-ESM", "Es Semara");
            dic.Add("EH-LAA", "Laayoune");
            dic.Add("EH-OUD", "Oued el Dahab");
            pdic.Add("EH", dic);
            #endregion


            // Yemen	YE
            // Yugoslavia	YU (Use free text)
            // Zaire	ZR	(Use free text)

            #region Zambia
            dic = new Dictionary<string, string>();
            dic.Add("ZM-02", "Central");
            dic.Add("ZM-08", "Copperbelt");
            dic.Add("ZM-03", "Eastern");
            dic.Add("ZM-04", "Luapula");
            dic.Add("ZM-09", "Lusaka");
            dic.Add("ZM-06", "North-Western");
            dic.Add("ZM-05", "Northern");
            dic.Add("ZM-07", "Southern");
            dic.Add("ZM-01", "Western");
            pdic.Add("ZM", dic);
            #endregion

            #region Zimbabwe
            dic = new Dictionary<string, string>();
            dic.Add("ZW-BU", "Bulawayo");
            dic.Add("ZW-HA", "Harare");
            dic.Add("ZW-MA", "Manicaland");
            dic.Add("ZW-MC", "Mashonaland Central");
            dic.Add("ZW-ME", "Mashonaland East");
            dic.Add("ZW-MW", "Mashonaland West");
            dic.Add("ZW-MV", "Masvingo");
            dic.Add("ZW-MN", "Matabeleland North");
            dic.Add("ZW-MS", "Matabeleland South");
            dic.Add("ZW-MI", "Midlands");
            pdic.Add("ZW", dic);
            #endregion


            return pdic;
        }

        public static Dictionary<string, string> GetOrderUpdateResponseCode()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("100", "Success");
            dic.Add("200", "Invalid login credentials");
            dic.Add("340", "Invalid country code");
            dic.Add("341", "Invalid state code");
            dic.Add("342", "Invalid Email Address");
            dic.Add("343", "Data Element Has Same Value As Value Passed No Update done (Information ONLY, but still a success");
            dic.Add("320", "Invalid Product Id");
            dic.Add("344", "Invalid Number Format");
            dic.Add("346", "Invalid date format. Use mm/dd/yyyy");
            dic.Add("347", "Invalid RMA reason");
            dic.Add("348", "Order is already flagged as RMA");
            dic.Add("349", "Order is not flagged as RMA");
            dic.Add("350", "Invalid order Id supplied");
            dic.Add("351", "Invalid status or action supplied");
            dic.Add("352", "Uneven Order/Status/Action Pairing");
            dic.Add("353", "Cannot stop recurring");
            dic.Add("376", "Invalid tracking number");
            dic.Add("377", "Cannot ship pending orders");
            dic.Add("378", "Order already shipped");
            dic.Add("379", "Order is fully refunded/voided");
            dic.Add("380", "Order is not valid for force bill");
            dic.Add("386", "Order has already been returned");
            dic.Add("387", "Invalid return reason");
            dic.Add("701", "Action not permitted by gateway");
            dic.Add("907", "Invalid shipping Id");
            return dic;
        }
    }
}