using ContractorApi.Models;
using System.Text.Json;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ContractorApiClient
{
    public class Contractor
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "type")]
        public ContractorType Type { get; set; }

        [Required]
        [JsonProperty(PropertyName = "inn")]
        public string Inn { get; set; }

        [JsonProperty(PropertyName = "kpp")]
        public string Kpp { get; set; }
    }
}
