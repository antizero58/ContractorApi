using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContractorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContractorApiClient
{
    public class EditModel : PageModel
    {
        private readonly IContractorClient _contractorClient;

        [BindProperty]
        public Contractor ConractorEntity { get; set; }

        public SelectList ContractorTypeSL { get; set; }

        public EditModel(IContractorClient contractorClient)
        {
            _contractorClient = contractorClient;
            ContractorTypeSL = new SelectList(new List<ContractorType> { ContractorType.Individual, ContractorType.Legal });
        }

        public async Task OnGetAsync(int id)
        {
            ConractorEntity = await _contractorClient.Get(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _contractorClient.Update(ConractorEntity);
                }
                catch (Exception e)
                {
                }

                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}