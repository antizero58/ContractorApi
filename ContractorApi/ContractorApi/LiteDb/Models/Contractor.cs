using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ContractorApi.Models
{
    public enum ContractorType
    {
        // Юр. лицо
        Legal= 1,

        // ИП
        Individual
    }

    [DataContract]
    public class Contractor
    {
        //[DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "type")]
        public ContractorType Type { get; set; }

        [DataMember(Name = "inn")]
        public string Inn { get; set; }

        [DataMember(Name = "kpp")]
        public string Kpp { get; set; }
    }
}
