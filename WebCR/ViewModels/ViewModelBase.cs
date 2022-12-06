using Newtonsoft.Json;
using ReactiveUI;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebCR.ViewModels;

public class ViewModelBase : ReactiveObject
{
    protected static async Task<IEnumerable<T>> AsyncGetAll<T>(string uri)
    {
        IEnumerable<T> entities;
        using (HttpClient client = new())
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            string jsonRespons = await response.Content.ReadAsStringAsync();
            entities = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonRespons);
        }
        return entities;
    }

    protected static async Task<T> AsyncGet<T>(string uri)
    {
        T entity;
        using (HttpClient client = new())
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            string jsonRespons = await response.Content.ReadAsStringAsync();
            entity = JsonConvert.DeserializeObject<T>(jsonRespons);
        }
        return entity;
    }

    protected static async Task AsyncAdd<T>(string uri, T entity)
    {
        var stringEntity = JsonConvert.SerializeObject(entity);
        var httpContent = new StringContent(stringEntity, Encoding.UTF8, "application/json");
        using HttpClient client = new();
        HttpResponseMessage response = await client.PostAsync(uri, httpContent);
    }

    protected static async Task AsyncUpdate<T>(string uri, T entity)
    {
        var stringEntity = JsonConvert.SerializeObject(entity);
        var httpContent = new StringContent(stringEntity, Encoding.UTF8, "application/json");
        using HttpClient client = new();
        HttpResponseMessage response = await client.PutAsync(uri, httpContent);
    }

    protected static async Task AsyncDelete<T>(string uri)
    {
        using HttpClient client = new();
        HttpResponseMessage response = await client.DeleteAsync(uri);
    }
}
