This is an implementation of a tax calculator. The soultion includes unit tests and docker support.

The caching can be done using redis using the following steps:

//Install nuget packages

dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
dotnet add package StackExchange.Redis

//Configure redis 
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

//Use redis in the controller
public class CalculatorController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private readonly TaxCalculatorService _taxCalculatorService;

    public CalculatorController(IDistributedCache cache, TaxCalculatorService taxCalculatorService)
    {
        _cache = cache;
        _taxCalculatorService = taxCalculatorService;
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] TaxPayer taxpayer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string cacheKey = taxpayer.SSN;
        string cachedResult = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedResult))
        {
            var result = JsonConvert.DeserializeObject<TaxResult>(cachedResult);
            return Ok(result);
        }

        var taxResult = _taxCalculatorService.CalculateTaxes(taxpayer);

        var serializedResult = JsonConvert.SerializeObject(taxResult);
        await _cache.SetStringAsync(cacheKey, serializedResult);

        return Ok(taxResult);
    }

//Check that redis container is running
    docker run -d -p 6379:6379 --name redis redis


