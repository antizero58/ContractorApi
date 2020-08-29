using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContractorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContractorApiClient
{
    public class CreateModel : PageModel
    {
        private readonly IContractorClient _contractorClient;

        [BindProperty]
        public Contractor ConractorEntity { get; set; }

        public SelectList ContractorTypeSL { get; set; }

        public CreateModel(IContractorClient contractorClient)
        {
            _contractorClient = contractorClient;
            ContractorTypeSL = new SelectList(new List<ContractorType> { ContractorType.Individual, ContractorType.Legal });
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _contractorClient.Create(ConractorEntity);
                }
                catch (Exception e)
                {

                }
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}