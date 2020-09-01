using LiteDB;

namespace ContractorApi.LiteDb
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}