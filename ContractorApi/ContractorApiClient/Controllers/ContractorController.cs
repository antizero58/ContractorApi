using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContractorApiClient
{
    [Route("api/Contractor")]
    [ApiController]
    public class ContractorController : Controller
    {
        private readonly ContractorClient cc = new ContractorClient();

        public IEnumerable<Contractor> Contractors { get; set; }

        public ContractorController()
        {
            //Init();
        }

        //public async void Init()
        //{
        //    Contractors = await cc.Get();
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Contractors = await cc.Get();
            return Json(new { data = Contractors });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return await cc.Delete(id);
        }
    }
}