using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContractorApi.Models;
using LiteDB;
using ContractorApi.LiteDb;
using ContractorApi.WebServices.Dadata;

namespace ContractorApi.Controllers
{
    [ApiController]
    [Route("api/contractors")]
    public class ContractorController : ControllerBase
    {
        private readonly ILogger<ContractorController> _logger;
        private readonly ILiteDbContractorService _liteDbContractorService;
        private readonly IDadataClient _dadataClient;

        public ContractorController(ILogger<ContractorController> logger,
            ILiteDbContractorService liteDbContractorService,
            IDadataClient dadataClient)
        {
            _logger = logger;
            _liteDbContractorService = liteDbContractorService;
            _dadataClient = dadataClient;
    }

        [HttpGet]
        public IEnumerable<Contractor> Get()
        {
            _logger.LogInformation("HttpGet");
            return _liteDbContractorService.FindAll().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Contractor> Get(int id)
        {
            return _liteDbContractorService.FindOne(id);
        }

        [HttpPost]
        public ActionResult<Contractor> Create(Contractor contractor)
        {
            if (contractor == null)
                return BadRequest();

            // name, type, inn не могут быть пустыми
            if (contractor.Name.IsNullOrEmpty() || !contractor.Type.InList(ContractorType.Legal, ContractorType.Individual) || contractor.Inn.IsNullOrEmpty())
                return BadRequest("Поля name, type, inn не могут быть пустыми");

            // kpp не может быть пусто у Юр.лица
            if (contractor.Type == ContractorType.Legal && contractor.Kpp.IsNullOrEmpty())
                return BadRequest("kpp не может быть пусто у Юр.лица");

            // Проверка на существование элемента
            if (_liteDbContractorService.Exists(contractor.Id))
                return BadRequest($"Id {contractor.Id} has already exists.");

            // При создании контрагента проверять его наличие в ЕГРЮЛ по полям inn kpp для Юр.лица и по inn для ИП, через сервис dadata.ru(https://dadata.ru/api/find-party/),
            // если организация с указанными inn kpp или ИП с указанным inn не существует, выдавать ошибку
            DadataResponse response = _dadataClient.GetSuggestions(contractor.Inn, contractor.Kpp, contractor.Type);
            if (response == null)
                return BadRequest();

            // Ответов может быть несколько, берём первый
            contractor.FullName = response.Suggestions[0].Data.Name.FullWithOpf;

            _liteDbContractorService.Insert(contractor);

            return CreatedAtAction(nameof(Create), new { id = contractor.Id }, contractor);
        }

        [HttpPut]
        public ActionResult<Contractor> Update(Contractor contractor)
        {
            if (contractor == null)
                return BadRequest("Contractor is null");

            var res = _liteDbContractorService.Update(contractor);
            if (res)
                return NoContent();
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult<Contractor> Delete(int id)
        {
            var res = _liteDbContractorService.Delete(id);
            if (res)
                return NoContent();
            else
                return NotFound();
        }
    }
}
