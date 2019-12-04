using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractorApi.Models
{
    public enum ContractorType
    {
        // Юр. лицо
        LegalEntity,

        // ИП
        SoleProprietor
    }

    public class Contractor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public ContractorType Type { get; set; }
        public string inn { get; set; }
        public string kpp { get; set; }
    }
}
