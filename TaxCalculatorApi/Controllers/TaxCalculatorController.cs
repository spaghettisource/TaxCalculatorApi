using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TaxCalculatorApi.Domain;
using TaxCalculatorApi.Servicees;

[ApiController]
[Route("[controller]")]
public class TaxCalculatorController : Controller
{
    private readonly ITaxCalculatorService _taxCalculatorService;
    private readonly IMemoryCache _memoryCache;

    public TaxCalculatorController(ITaxCalculatorService taxCalculatorService, IMemoryCache memoryCache)
    {
        _taxCalculatorService = taxCalculatorService;
        _memoryCache = memoryCache;
    }

    [HttpPost("Calculate")]
    public IActionResult Calculate([FromBody] TaxPayer taxPayer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cacheKey = $"{taxPayer.SSN}-{taxPayer.GrossIncome}-{taxPayer.CharitySpent}";
        if (!_memoryCache.TryGetValue(cacheKey, out Taxes taxes))
        {
            taxes = _taxCalculatorService.CalculateTaxes(taxPayer);
            _memoryCache.Set(cacheKey, taxes);
        }

        return Ok(taxes);
    }
}
