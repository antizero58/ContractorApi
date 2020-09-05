using System.Collections.Generic;
using System.Threading.Tasks;
using ContractorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContractorApiClient
{
    public class IndexModel : PageModel
    {
        private readonly IContractorClient _contractorClient;

        public IndexModel(IContractorClient contractorClient)
        {
            _contractorClient = contractorClient;
        }

        public IEnumerable<Contractor> Contractors { get; set; }

        public async Task OnGetAsync()
        {
            Contractors = await _contractorClient.Get();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _contractorClient.Delete(id);

            return RedirectToPage("Index");
        }
    }
}