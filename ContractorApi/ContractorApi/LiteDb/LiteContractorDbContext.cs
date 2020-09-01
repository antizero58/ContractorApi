using LiteDB;
using Microsoft.Extensions.Options;

namespace ContractorApi.LiteDb
{
    public class LitetContractorDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public LitetContractorDbContext(IOptions<LiteDbOptions> options)
        {
            Database = new LiteDatabase(options.Value.DatabaseLocation);
        }
    }
}
