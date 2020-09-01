using ContractorApi.Models;

namespace ContractorApi.WebServices.Dadata
{
    public interface IDadataClient
    {
        public DadataResponse GetSuggestions(string inn, string kpp, ContractorType type);
    }
}
