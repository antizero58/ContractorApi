using ContractorApi.Models;
using System.Collections.Generic;

namespace ContractorApi.LiteDb
{
    public interface ILiteDbContractorService
    {
        IEnumerable<Contractor> FindAll();
        Contractor FindOne(int id);
        int Insert(Contractor forecast);
        bool Update(Contractor forecast);
        bool Delete(int id);
        bool Exists(int id);
    }
}