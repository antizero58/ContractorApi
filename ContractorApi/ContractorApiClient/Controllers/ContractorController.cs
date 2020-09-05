using System.Collections.Generic;
using System.Threading.Tasks;
using ContractorApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContractorApiClient
{
    [Route("api/Contractor")]
    [ApiController]
    public class ContractorController : Controller
    {
        private readonly IContractorClient _contractorClient;

        public IEnumerable<Contractor> Contractors { get; set; }

        public ContractorController(IContractorClient contractorClient)
        {
            _contractorClient = contractorClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Contractors = await _contractorClient.Get();
            return Json(new { data = Contractors });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return await _contractorClient.Delete(id);
        }
    }
}