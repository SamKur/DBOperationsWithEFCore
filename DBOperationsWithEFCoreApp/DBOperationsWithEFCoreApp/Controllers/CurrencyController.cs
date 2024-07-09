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
        public async Task <IActionResult> GetAllCurrencies()
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
        [HttpGet("{currtype}")]
        public async Task<IActionResult> GetCurrencyByCurrencyTypeAsync([FromRoute] string currtype)
        {
            var result = await appDbContext.Currencies.Where(x=>x.CurrencyType == currtype).FirstOrDefaultAsync();  //linq
                                                                                                                    //var result = await appDbContext.Currencies.Where(x=>x.CurrencyType == currtype).FirstAsync();  
                                                                                                                    //if data is not present => First -> will throw exception, FirstOrDefault -> will return null

            //var result = await appDbContext.Currencies.Where(x=>x.CurrencyType == currtype).SingleOrDefaultAsync();  //linq
            //SingleOrDefaultAsync -> only one entry should be present, else exception,
            //SingleAsync -> one only must be present else exception
            return Ok(result);
        }



    }
}
