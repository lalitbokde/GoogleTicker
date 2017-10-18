﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CpaTicker.Areas.admin.Classes
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(2), Column(TypeName = "varchar")]
        public string CountryAbbreviation { get; set; }
    }

    public class State
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string StateName { get; set; }

        public int? CountryId { get; set; }

        [MaxLength(255), Column(TypeName = "varchar")]
        public string CountryCode { get; set; }

        [MaxLength(255), Column(TypeName = "varchar")]
        public string StateCode { get; set; }
    }

    public class IP2Country
    {
        [Key][DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public long Min { get; set; }
        public long Max { get; set; }
        [MaxLength(2), Column(TypeName = "varchar")]
        public string Code { get; set; } 
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Name { get; set; } 
    }

    public class CampaignCountry
    {
        [Key, Column(Order = 0)]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        [MaxLength(2), Column(Order = 1, TypeName = "varchar"), Key]
        public string Code { get; set; }
    }

    public class Location
    {
        // geoname_id,locale_code,continent_code,continent_name,country_iso_code,country_name,subdivision_1_iso_code,subdivision_1_name,
        // subdivision_2_iso_code,subdivision_2_name,city_name,metro_code,time_zone

        [Key][DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int locId { get; set; }

        [MaxLength(2), Column(TypeName = "varchar")] // en <- English
        public string locale_code { get; set; }

        [MaxLength(2), Column(TypeName = "varchar")]
        public string continent_code { get; set; }

        public string continent_name { get; set; }        

        [MaxLength(2), Column(TypeName = "varchar")]
        public string country_iso_code { get; set; } // country_iso_code

        public string country_name { get; set; }

         [MaxLength(3), Column(TypeName = "varchar")]
        public string subdivision_1_iso_code { get; set; } // it is like city code 

        public string subdivision_1_name { get; set; }

         [MaxLength(3), Column(TypeName = "varchar")]
        public string subdivision_2_iso_code { get; set; } // it is like city code 

        public string subdivision_2_name { get; set; }

        public string city_name { get; set; }

        public string metro_code { get; set; }
        public string time_zone { get; set; } 







        //[MaxLength(2), Column(TypeName = "varchar")]
        //public string region { get; set; }

        //public string city { get; set; } //[postalCode], [latitude], [longitude], [metroCode], [areaCode]

        //public string postalCode { get; set; }

        //public string latitude { get; set; }

        //public string longitude { get; set; }

        //public string metroCode { get; set; }

        //public string areaCode { get; set; }

    }

    public class Block
    {
        // network_start_integer,network_last_integer,geoname_id,registered_country_geoname_id,represented_country_geoname_id,is_anonymous_proxy,is_satellite_provider,postal_code,latitude,longitude

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public long startIpNum { get; set; }

        [Index(IsUnique = true)]
        public long endIpNum { get; set; }

        [ForeignKey("Location")]
        public int? locId { get; set; } // geoname_id
        public virtual Location Location { get; set; }

        [ForeignKey("registered_country_geoname")]
        public int? registered_country_geoname_id { get; set; }
        public virtual Location registered_country_geoname { get; set; }

        [ForeignKey("represented_country_geoname")]
        public int? represented_country_geoname_id { get; set; }
        public virtual Location represented_country_geoname { get; set; }

        public bool is_anonymous_proxy { get; set; }

        public bool is_satellite_provider { get; set; }

        public string postal_code { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }
        
        public Location GetLocation() {

            return this.Location ??
                (this.registered_country_geoname ?? this.represented_country_geoname);
                    
            
        }

    }
}