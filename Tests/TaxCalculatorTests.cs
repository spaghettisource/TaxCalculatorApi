using TaxCalculatorApi.Domain;
using TaxCalculatorApi.Servicees;

namespace Tests
{
    public class TaxCalculatorTests
    {

        private readonly ITaxCalculatorService _taxCalculatorService;

        public TaxCalculatorTests()
        {
            _taxCalculatorService = new TaxCalculatorService();
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeBelowThreshold_NoTaxes()
        {
            var taxpayer = new TaxPayer { GrossIncome = 980 };
            var taxes = _taxCalculatorService.CalculateTaxes(taxpayer);

            Assert.Equal(980, taxes.NetIncome);
            Assert.Equal(0, taxes.TotalTax);
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeAboveThreshold_CorrectTaxes()
        {
            var taxpayer = new TaxPayer { GrossIncome = 3400 };
            var taxes = _taxCalculatorService.CalculateTaxes(taxpayer);

            Assert.Equal(2860, taxes.NetIncome);
            Assert.Equal(540, taxes.TotalTax);
        }

        // Additional tests for charity, different income levels, etc.
    }
}
