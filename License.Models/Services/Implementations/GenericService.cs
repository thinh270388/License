using License.Models.Helpers;
using License.Models.Services.Constracts;
using System.Net.Http.Json;

namespace License.Models.Services.Implementations
{
    public class GenericService<T> : IGenericService<T>
    {
        private readonly HttpClient _httpClient;
        public GenericService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrlHelper.URL_API);
        }
        public async Task<DtoResult<T>> DeleteAsync(string endpoint, Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{endpoint}/Delete/{id}");
            var result = await response.Content.ReadFromJsonAsync<DtoResult<T>>();
            return result!;
        }

        public async Task<DtoResult<T>> GetAllAsync(string endpoint)
        {
            var result = await _httpClient.GetFromJsonAsync<DtoResult<T>>($"{endpoint}/GetAll");

            return result!;
        }

        public async Task<DtoResult<T>> GetByIdAsync(string endpoint, Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<DtoResult<T>>($"{endpoint}/GetById/{id}");

            return result!;
        }

        public async Task<DtoResult<T>> AddAsync(string endpoint, T item)
        {
            var response = await _httpClient.PostAsJsonAsync($"{endpoint}/Add", item);
            var result = await response.Content.ReadFromJsonAsync<DtoResult<T>>();

            return result!;
        }

        public async Task<DtoResult<T>> UpdateAsync(string endpoint, T item)
        {
            var response = await _httpClient.PutAsJsonAsync($"{endpoint}/Update", item);
            var result = await response.Content.ReadFromJsonAsync<DtoResult<T>>();

            return result!;
        }
    }
}
