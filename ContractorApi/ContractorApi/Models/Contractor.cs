using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractorApi.Models
{
    public enum ContractorType
    {
        // Юр. лицо
        Legal= 1,

        // ИП
        Individual
    }

    public class Contractor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public ContractorType Type { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
    }
}
