using System.ComponentModel.DataAnnotations;

namespace TaxCalculatorApi.Domain
{
    public class TaxPayer
    {
        [Required]
        [RegularExpression(@"^[A-Za-z]+([-'][A-Za-z]+)*\s[A-Za-z]+([-'][A-Za-z]+)*$", ErrorMessage = "FullName must contain only letters and spaces. Should have at least two names")]
        [MinLength(3)]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "GrossIncome must be a valid number.")]
        public decimal GrossIncome { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "CharitySpent must be a valid number.")]
        public decimal? CharitySpent { get; set; }

        [Required]
        [RegularExpression(@"^\d{5,10}$", ErrorMessage = "SSN must be a valid 5 to 10 digit number.")]
        public string SSN { get; set; }
    }

}
