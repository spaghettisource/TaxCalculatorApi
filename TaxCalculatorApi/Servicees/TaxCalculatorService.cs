using TaxCalculatorApi.Domain;

namespace TaxCalculatorApi.Servicees
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        public Taxes CalculateTaxes(TaxPayer taxPayer)
        {
            decimal grossIncome = taxPayer.GrossIncome;
            decimal charitySpent = taxPayer.CharitySpent.HasValue ? Math.Min(taxPayer.CharitySpent.Value, grossIncome * 0.10m) : 0;
            decimal taxableIncome = grossIncome - charitySpent;

            decimal incomeTax = 0;
            decimal socialTax = 0;

            if (taxableIncome > 1000)
            {
                incomeTax = (taxableIncome - 1000) * 0.10m;
            }

            if (taxableIncome > 1000)
            {
                decimal socialTaxableIncome = Math.Min(taxableIncome - 1000, 2000);
                socialTax = socialTaxableIncome * 0.15m;
            }

            decimal totalTax = incomeTax + socialTax;
            decimal netIncome = grossIncome - totalTax;

            return new Taxes
            {
                GrossIncome = grossIncome,
                CharitySpent = charitySpent,
                IncomeTax = incomeTax,
                SocialTax = socialTax,
                TotalTax = totalTax,
                NetIncome = netIncome
            };
        }
    }
}
