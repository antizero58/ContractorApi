using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContractorApi.Models;

namespace ContractorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractorController : ControllerBase
    {
        private readonly ILogger<ContractorController> _logger;

        static private readonly List<Contractor> _contractors = new List<Contractor>();

        static ContractorController()
        {
            _contractors = Enumerable.Range(1, 5).Select(index => new Contractor
            {
                Id = index,
                Name = "Name_" + index
            })
            .ToList<Contractor>();
        }

        public ContractorController(ILogger<ContractorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Contractor> Get()
        {
            return _contractors;
        }

        [HttpGet("{id}")]
        public Contractor Get(int id)
        {
            if (_contractors.Exists(r => r.Id == id))
                return null;

            return _contractors.FirstOrDefault(r => r.Id == id);
        }

        [HttpPost]
        public ActionResult Create(Contractor contractor)
        {
            if (contractor == null)
                return BadRequest();

            if (contractor.Name.IsNullOrEmpty() || contractor.Type == 0 || contractor.Inn.IsNullOrEmpty())
                return BadRequest();

            DadataClient dadataClient = new DadataClient();

           // при создании контрагента проверять его наличие в ЕГРЮЛ по полям inn kpp для Юр.лица и по inn для ИП, через сервис dadata.ru(https://dadata.ru/api/find-party/),
           // если организация с указанными inn kpp или ИП с указанным inn не существует, выдавать ошибку
           DadataClient.DadataResponse response = dadataClient.GetSuggestions(contractor.Inn, contractor.Kpp, contractor.Type);
            if (response == null)
                return BadRequest();

            contractor.FullName = response.Suggestions[0].Data.Name.FullWithOpf;

            _contractors.Add(contractor);
            return CreatedAtAction(nameof(Create), new { id = contractor.Id }, contractor);
        }
    }
}
