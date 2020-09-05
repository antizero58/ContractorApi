using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DataMember(Name = "fullName")]
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "type")]
        [Required]
        [JsonProperty(PropertyName = "type")]
        public ContractorType Type { get; set; }

        [DataMember(Name = "inn")]
        [Required]
        [JsonProperty(PropertyName = "inn")]
        public string Inn { get; set; }

        [DataMember(Name = "kpp")]
        [JsonProperty(PropertyName = "kpp")]
        public string Kpp { get; set; }
    }
}
