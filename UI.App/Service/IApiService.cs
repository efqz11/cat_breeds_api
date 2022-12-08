using Newtonsoft.Json;
using System.Text.Json.Serialization;
using UI.App.Models;

namespace UI.App.Service
{
    public interface IApiService
    {
        Task<string> TryLogin(string email, string password);
        Task<List<ResponseEmployment>> SearchById(string token, string id);
        IEnumerable<string> GetErrors();
    }
}
