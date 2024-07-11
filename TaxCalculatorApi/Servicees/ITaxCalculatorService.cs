using TaxCalculatorApi.Domain;

namespace TaxCalculatorApi.Servicees
{
    public interface ITaxCalculatorService
    {
        Taxes CalculateTaxes(TaxPayer taxPayer);
    }
}
