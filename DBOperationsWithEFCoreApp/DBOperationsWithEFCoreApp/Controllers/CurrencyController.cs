using DBOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationsWithEFCoreApp.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public CurrencyController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /*//action method  - synchronous way
        [HttpGet("")]
        public IActionResult GetAllCurrencies()
        {
            //var result = this.appDbContext.Currencies.ToList(); //1way with linq
            var result = (from currencies in appDbContext.Currencies
                         select currencies).ToList();
            return Ok(result);
        }*/


        //async for above - very fast for handling many operations

        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            //var result = await appDbContext.Currencies.ToListAsync();  //linq
            var result = await (from currencies in appDbContext.Currencies
                                select currencies).ToListAsync();
            return Ok(result);
        }



        //Get data by primary key (here using async but synch also possible)
        //[HttpGet("{id")] //below to handle numeric params only in url
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyByIdAsync([FromRoute] int id)
        {
            var result = await appDbContext.Currencies.FindAsync(id);  //linq, (id=2)->hardcoding

            //if (result == null) { return NoContent(); }
            //below is not available for find by pk
            //var result = await (from currencies in appDbContext.Currencies select currencies).ToListAsync();
            return Ok(result);
        }


        //Get data by other field except primary key //everything also can be done via synchr programming
        /*commenting below for FromQuery
        [HttpGet("{currtype}")]
        public async Task<IActionResult> GetCurrencyByCurrencyTypeAsync([FromRoute] string currtype)
        {
            var result = await appDbContext.Currencies.Where(x=>x.CurrencyType == currtype).FirstOrDefaultAsync();  //linq
                                                                                                                    //var result = await appDbContext.Currencies.Where(x=>x.CurrencyType == currtype).FirstAsync();  
                                                                                                                    //if data is not present => First -> will throw exception, FirstOrDefault -> will return null

            //var result = await appDbContext.Currencies.Where(x=>x.CurrencyType == currtype).SingleOrDefaultAsync();  //linq
            //SingleOrDefaultAsync -> only one entry should be present, multiple entry- exception, not present - no error
            //SingleAsync -> one only must be present else exception

            //for First or FirstOrDefault use below for BETTER PERFORMANCE as it will return as soon as first entry is fetched
            //var result = await appDbContext.Currencies.FirstOrDefaultAsync(x => x.CurrencyType == currtype);  //linq
            //above we can also use for pk, as only 1 entry possible
            return Ok(result);
        }*/

        /*[HttpGet("{currtype}/{description}")]
        public async Task<IActionResult> GetCurrencyByCurrencyTypeAndDescriptionAsync([FromRoute] string currtype, [FromRoute] string description)
        {
            var result = await appDbContext.Currencies.Where(x => x.CurrencyType == currtype && x.Description == description).FirstOrDefaultAsync();  
            return Ok(result);
        }*/

        /*[HttpGet("{currtype}")]
        public async Task<IActionResult> GetCurrencyByCurrencyTypeAndFromQueryAsync([FromRoute] string currtype, [FromQuery] string? description)
        {
            //handling null
            //send QUERY by => https://localhost:7104/api/currencies/INR?description=Bharatiya Mudra
            // more by adding &q2=
            var result = await appDbContext.Currencies.FirstOrDefaultAsync(x =>
            x.CurrencyType == currtype && (x.Description == description || string.IsNullOrEmpty(description)));
            return Ok(result);
        }*/

        //above was giving only 1 record, and top most for all records. NOW,
        //Get all records using multiple parameters (Optional and Required)

        [HttpGet("{currtype}")]
        public async Task<IActionResult> GetCurrencyByCurrencyTypeAndFromQueryAsync([FromRoute] string currtype, [FromQuery] string? description)
        {
            var result = await appDbContext.Currencies.Where(x =>
            x.CurrencyType == currtype && (x.Description == description || string.IsNullOrEmpty(description))).ToListAsync();
            return Ok(result);
        }


        /*[HttpGet("all")]
        public async Task<IActionResult> GetCurrenciesByIdsAsync()
        {
            var ids = new List<int> {1,3,2 };  //generally dynamically will come from frontend
            var result = await appDbContext.Currencies.
                Where(x => ids.Contains(x.Id)).
                ToListAsync();
            return Ok(result);
        }*/

        //if we want to keep list directly on route -> in asp.net core USE custom model binder
        //we can use from header or body or query

        /*[HttpPost("all")]  //here using POST to pass list via BODY 
        //public async Task<IActionResult> PostCurrenciesByIdsAsync([FromBody] List<int> ids)
        public async Task<IActionResult> PostCurrenciesByIdsAsync([FromBody] List<int> ids)
        {
            //in list if element provided is not in DB, it will just ignore
            var result = await appDbContext.Currencies.
                Where(x => ids.Contains(x.Id)).
                ToListAsync();
            return Ok(result);
        }*/

        [HttpGet("all")]  //GET method doesnt accept BODY
        //use below https://localhost:7104/api/currencies/all?ids=2&ids=99&ids=6
        public async Task<IActionResult> GetCurrenciesByIdsAsync([FromQuery] List<int> ids)
        {
            //in list if element provided is not in DB, it will just ignore
            var result = await appDbContext.Currencies.
                Where(x => ids.Contains(x.Id)).
                ToListAsync();
            return Ok(result);

            //var result = await appDbContext.Currencies.FirstOrDefaultAsync(x =>
            //x.CurrencyType == currtype && (x.Description == description || string.IsNullOrEmpty(description)));
            //return Ok(result);
        }

        //FindAsync && ToListAsync -> fetches the whole table for data, which gives bad performance

        [HttpGet("all_optimized")]  //GET method doesnt accept BODY
        //use below https://localhost:7104/api/currencies/all?ids=2&ids=99&ids=6
        public async Task<IActionResult> GetCurrenciesByIdsOptimizedAsync([FromQuery] List<int> ids)
        {
            var result = await appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(x => new Currency()   //we have mapped only 2 cols, desciption will be ignored
                                              //that colm response will come as null
                {
                    Id = x.Id,
                    CurrencyType = x.CurrencyType,
                })
                .ToListAsync();
            return Ok(result);
        }

        //by creating anonymous obj
        [HttpGet("all_optimized_anonobj")]  //GET method doesnt accept BODY
        //use below https://localhost:7104/api/currencies/all?ids=2&ids=99&ids=6
        public async Task<IActionResult> GetCurrenciesByIdsOptimizedAnonAsync([FromQuery] List<int> ids)
        {
            var result = await appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(x => new              //no classname defined
                                              //that colm response will come as null
                {
                    currencyid = x.Id,
                    name = x.CurrencyType,
                })
                .ToListAsync();
            return Ok(result);
            // result will come in two above mentioned alias form
        }

    }
}
