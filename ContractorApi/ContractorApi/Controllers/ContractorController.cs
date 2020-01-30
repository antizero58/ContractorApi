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
    [Route("api/contractors")]
    public class ContractorController : ControllerBase
    {
        private const string _liteDbName = "MyData.db";
        private const string _collectionName = "contractors";

        private void InitializeDb()
        {
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(_liteDbName))
            {
                // Предварительное заполнение БД
                var col = db.GetCollection<Contractor>(_collectionName);

                if (col.Count() == 0)
                {
                    List<Contractor> contractors = new List<Contractor>
                    {
                        new Contractor
                        {
                            Id = 1,
                            Inn = "123890",
                            Kpp = "12345",
                            Name = "Coantractor_1",
                            Type = ContractorType.Legal
                        },

                        new Contractor
                        {
                            Id = 2,
                            Inn = "7707083893",
                            Kpp = "12345",
                            Name = "Сбербанк",
                            Type = ContractorType.Legal
                        },

                        new Contractor
                        {
                            Id = 3,
                            Inn = "7702070139",
                            Kpp = "12345",
                            Name = "ВТБ",
                            Type = ContractorType.Legal
                        },

                        new Contractor
                        {
                            Id = 4,
                            Inn = "132808730606",
                            Kpp = "12345",
                            Name = "ИП",
                            Type = ContractorType.Individual
                        }
                    };

                    // Create unique index in Name field
                    col.EnsureIndex(x => x.Id, true);

                    foreach (var contractor in contractors)
                        Create(contractor);
                }
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Contractor>> Get()
        {
            using (var db = new LiteDatabase(_liteDbName))
            {
                var contractors = db.GetCollection<Contractor>(_collectionName);
                return contractors.Find(r => true).ToList();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Contractor> Get(int id)
        {
            using (var db = new LiteDatabase(_liteDbName))
            {
                var contractors = db.GetCollection<Contractor>(_collectionName);
                var contractor = contractors.FindOne(r => r.Id == id);

                if (contractor == null)
                    return NotFound();

                return contractor;
            }
        }

        [HttpPost]
        public ActionResult Create(Contractor contractor)
        {
            if (contractor == null)
                return BadRequest();

            // name, type, inn не могут быть пустыми
            if (contractor.Name.IsNullOrEmpty() || !contractor.Type.InList(ContractorType.Legal, ContractorType.Individual) || contractor.Inn.IsNullOrEmpty())
                return BadRequest();

            // kpp не может быть пусто у Юр.лица
            if (contractor.Type == ContractorType.Legal && contractor.Kpp.IsNullOrEmpty())
                return BadRequest();

            // Проверка на существование элемента
            using (var db = new LiteDatabase(_liteDbName))
            {
                var contractors = db.GetCollection<Contractor>(_collectionName);
                if (contractors.Exists(r => r.Id == contractor.Id))
                    return BadRequest();
            }

            // При создании контрагента проверять его наличие в ЕГРЮЛ по полям inn kpp для Юр.лица и по inn для ИП, через сервис dadata.ru(https://dadata.ru/api/find-party/),
            // если организация с указанными inn kpp или ИП с указанным inn не существует, выдавать ошибку
            DadataClient dadataClient = new DadataClient();
            DadataResponse response = dadataClient.GetSuggestions(contractor.Inn, contractor.Kpp, contractor.Type);
            if (response == null)
                return BadRequest();

            // Ответов может быть несколько, берём первый
            contractor.FullName = response.Suggestions[0].Data.Name.FullWithOpf;

            using (var db = new LiteDatabase(_liteDbName))
            {
                var contractors = db.GetCollection<Contractor>(_collectionName);
                contractors.Insert(contractor);
            }

            return CreatedAtAction(nameof(Create), new { id = contractor.Id }, contractor);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using (var db = new LiteDatabase(_liteDbName))
            {
                var contractors = db.GetCollection<Contractor>(_collectionName);
                var contractor = contractors.FindOne(r => r.Id == id);

                if (contractor == null)
                    return NotFound();

                contractors.Delete(contractor.Id);
            }

            return new NoContentResult();
        }
    }
}
