using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContractorApi.Models;
using LiteDB;

namespace ContractorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractorController : ControllerBase
    {
        private readonly ILogger<ContractorController> _logger;

        //static private readonly List<Contractor> _contractors = new List<Contractor>();

        static ContractorController()
        {
            // Open database (or create if doesn't exist)
            //using (var db = new LiteDatabase(@"MyData.db"))
            //{
            //    // Get customer collection
            //    var col = db.GetCollection<Contractor>("contractors");

            //    //var maxId = col.Find(r => r.Id > 0).OrderByDescending(r => r.Id).First().Id;

            //    // Create your new customer instance
            //    var customer = new Contractor
            //    {
            //        Id = 3,
            //        Inn = "123890",
            //        Kpp = "12345",
            //        Name = "Coantractor_3"
            //    };

            //    // Create unique index in Name field
            //    col.EnsureIndex(x => x.Id, true);

            //    // Insert new customer document (Id will be auto-incremented)
            //    col.Insert(customer);
            //}
        }

        public ContractorController(ILogger<ContractorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Contractor> Get()
        {
            using (var db = new LiteDatabase("MyData.db"))
            {
                var contractors = db.GetCollection<Contractor>("contractors");
                return contractors.Find(r => r.Id > 0);
            }
        }

        [HttpGet("{id}")]
        public Contractor Get(int id)
        {
            using (var db = new LiteDatabase("MyData.db"))
            {
                var contractors = db.GetCollection<Contractor>("contractors");
                return contractors.FindOne(r => r.Id >id);
            }
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

            using (var db = new LiteDatabase("MyData.db"))
            {
                var contractors = db.GetCollection<Contractor>("contractors");
                contractors.Insert(contractor);
            }

            return CreatedAtAction(nameof(Create), new { id = contractor.Id }, contractor);
        }
    }
}
