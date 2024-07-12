using Microsoft.AspNetCore.Mvc;
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

        [Fact]
        public void CalculateTaxes_GrossIncomeCharity_CorrectTaxes()
        {
            var taxpayer = new TaxPayer { GrossIncome = 2500, CharitySpent = 150 };
            var taxes = _taxCalculatorService.CalculateTaxes(taxpayer);

            Assert.Equal(2162.5m, taxes.NetIncome);
            Assert.Equal(337.5m, taxes.TotalTax);
        }

        [Fact]
        public void Calculate_ZeroIncome_ReturnsZeroNetIncome()
        {
            var taxpayer = new TaxPayer
            {
                FullName = "John Doe",
                SSN = "12345",
                GrossIncome = 0,
                CharitySpent = 0
            };

            var result = _taxCalculatorService.CalculateTaxes(taxpayer);

            Assert.Equal(0, result.NetIncome);
        }

        [Fact]
        public void Calculate_BelowTaxableThreshold_ReturnsGrossIncomeAsNetIncome()
        {
            var taxpayer = new TaxPayer
            {
                FullName = "Jane Doe",
                SSN = "54321",
                GrossIncome = 800,
                CharitySpent = 0
            };

            var result = _taxCalculatorService.CalculateTaxes(taxpayer);

            Assert.Equal(800, result.NetIncome);
        } 

        [Fact]
        public void Calculate_AboveTaxableThresholdWithCharity_ReturnsCorrectTaxes()
        {
            var taxpayer = new TaxPayer
            {
                FullName = "Bob Brown",
                SSN = "11223",
                GrossIncome = 2500,
                CharitySpent = 200
            };

            var result = _taxCalculatorService.CalculateTaxes(taxpayer);

            Assert.Equal(130, result.IncomeTax);
            Assert.Equal(195, result.SocialTax);
            Assert.Equal(325, result.TotalTax); 
            Assert.Equal(2175, result.NetIncome);
        }

        [Fact]
        public void Calculate_CharityExceedingTenPercent_ReturnsCorrectTaxes()
        {
            var taxpayer = new TaxPayer
            {
                FullName = "Charlie Green",
                SSN = "33445",
                GrossIncome = 3000,
                CharitySpent = 500 
            };

            var result = _taxCalculatorService.CalculateTaxes(taxpayer);

            Assert.Equal(170, result.IncomeTax); 
            Assert.Equal(255, result.SocialTax); 
            Assert.Equal(425, result.TotalTax); 
            Assert.Equal(2575, result.NetIncome);
        }
    }
}
