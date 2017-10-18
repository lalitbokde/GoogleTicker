using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Models
{
    public class EditModel
    {
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Select Default TimeZone")]
        public string TimeZone { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string Zip { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        [CreditCard]
        public string CreditCardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration month")]
        public string CreditCardExpMonth { get; set; }

        [Required]
        [Display(Name = "Expiration year")]
        public string CreditCardExpYear { get; set; }
        
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "The e-mail adress isn't in a correct format")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [RemoteWithServerSideAttribute("CheckEmailExistsNotMine", "Validation", "admin", ErrorMessage = "This email is already taken.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        //[Display(Name = "Plan Type")]
        //public Level Level { get; set; }

        private List<SelectListItem> months;

        [Display(Name = "Plan Type")]
        public Level Level { get; set; }

        public bool HasAPIKey { get; set; }

        public bool SendEmail { get; set; }

        [Display(Name = "TransactionId Type")]
        public TransactionIdGenerationType TransactionIdType { get; set; }

        public IEnumerable<SelectListItem> MonthList
        {
            get
            {
                if (months == null)
                {
                    months = new List<SelectListItem>();
                    months.Add(new SelectListItem() { Text = "January", Value = "01" });
                    months.Add(new SelectListItem() { Text = "February", Value = "02" });
                    months.Add(new SelectListItem() { Text = "March", Value = "03" });
                    months.Add(new SelectListItem() { Text = "April", Value = "04" });
                    months.Add(new SelectListItem() { Text = "May", Value = "05" });
                    months.Add(new SelectListItem() { Text = "June", Value = "06" });
                    months.Add(new SelectListItem() { Text = "July", Value = "07" });
                    months.Add(new SelectListItem() { Text = "August", Value = "08" });
                    months.Add(new SelectListItem() { Text = "September", Value = "09" });
                    months.Add(new SelectListItem() { Text = "October", Value = "10" });
                    months.Add(new SelectListItem() { Text = "November", Value = "11" });
                    months.Add(new SelectListItem() { Text = "December", Value = "12" });
                }
                return months;
            }
        }

        private List<SelectListItem> years;

        public IEnumerable<SelectListItem> YearList
        {
            get
            {
                if (years == null)
                {
                    int currentyear = System.DateTime.Now.Year % 100;
                    int iy;
                    years = new List<SelectListItem>();
                    for (int i = 0; i < 30; i++)
                    {
                        iy = (currentyear + i) % 100;
                        years.Add(new SelectListItem() { Text = iy.ToString("D2"), Value = iy.ToString("D2") });
                    }
                }
                return years;
            }
        }

        [Required]
        [Display(Name = "State")]
        public string SelectedState { get; set; }

        public IEnumerable<SelectListItem> StateList
        {
            get
            {
                var db = new CpaTickerDb();
                return db.States.Where(s => s.CountryId == SelectedCountry).OrderBy(p => p.StateName).AsEnumerable().Select(p => new SelectListItem { Value = p.StateCode, Text = p.StateName });
            }
        }

        private IEnumerable<SelectListItem> countries;

        [Display(Name = "Country")]
        public int SelectedCountry { get; set; }

        public IEnumerable<SelectListItem> CountryList
        {
            get
            {
                if (this.countries == null)
                {
                    var db = new CpaTickerDb();
                    countries = db.Countries.OrderBy(p => p.Name).AsEnumerable().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name });
                }
                return countries;
            }
        }
    }
}