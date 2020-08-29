using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContractorApiClient
{
    public interface IContractorClient
    {
        public Task<IEnumerable<Contractor>> Get();

        public Task<Contractor> Get(int id);

        public Task<IActionResult> Create(Contractor c);

        public Task<IActionResult> Update(Contractor c);

        public Task<IActionResult> Delete(int id);
    }
}
