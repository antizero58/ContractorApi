using ContractorApi.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace ContractorApi.LiteDb
{
    public class LiteDbContractorService : ILiteDbContractorService
    {

        private LiteDatabase _liteDb;
        private const string _collectionName = "contractors";

        public LiteDbContractorService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<Contractor> FindAll()
        {
            var result = _liteDb.GetCollection<Contractor>(_collectionName)
                .FindAll();
            return result;
        }

        public Contractor FindOne(int id)
        {
            return _liteDb.GetCollection<Contractor>(_collectionName)
                .Find(x => x.Id == id).FirstOrDefault();
        }

        public int Insert(Contractor forecast)
        {
            return _liteDb.GetCollection<Contractor>(_collectionName)
                .Insert(forecast);
        }

        public bool Update(Contractor forecast)
        {
            return _liteDb.GetCollection<Contractor>(_collectionName)
                .Update(forecast);
        }

        public bool Delete(int id)
        {
            return _liteDb.GetCollection<Contractor>(_collectionName)
                .Delete(id);
        }

        public bool Exists(int id)
        {
            return _liteDb.GetCollection<Contractor>(_collectionName)
                .Exists(r => r.Id == id);
        }
    }
}
