using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.IntegrationTests.Model;
using AccountReceivableSystem.Web.Models;
using FluentAssertions;
using Newtonsoft.Json;

namespace AccountReceivableSystem.IntegrationTests.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse> GetReadResponseAsync<TResponse>(this HttpClient client, string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TResponse>(responseString);
    }

    public static async Task<ErrorResponseModel> GetReadErrorResponseAsync(this HttpClient client, string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync(requestUri, cancellationToken);
        response.IsSuccessStatusCode.Should().BeFalse();

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        var model = JsonConvert.DeserializeObject<ErrorModel>(responseString);

        return new ErrorResponseModel(model, response.StatusCode);
    }

    public static async Task<TResponse> PostAsJsonReadResponseAsync<TRequest, TResponse>(this HttpClient client, string requestUri, TRequest value, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(requestUri, value, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TResponse>(responseString);
    }

    public static async Task<ErrorResponseModel> PostAsJsonReadErrorResponseAsync<TRequest>(this HttpClient client, string requestUri, TRequest value, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(requestUri, value, cancellationToken);
        response.IsSuccessStatusCode.Should().BeFalse();
        
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        var model = JsonConvert.DeserializeObject<ErrorModel>(responseString);

        return new ErrorResponseModel(model, response.StatusCode);
    }

    public static async Task<TResponse> PutAsJsonReadResponseAsync<TRequest, TResponse>(this HttpClient client, string requestUri, TRequest value, CancellationToken cancellationToken = default)
    {
        var response = await client.PutAsJsonAsync(requestUri, value, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TResponse>(responseString);
    }

    public static async Task<ErrorResponseModel> PutAsJsonReadErrorResponseAsync<TRequest>(this HttpClient client, string requestUri, TRequest value, CancellationToken cancellationToken = default)
    {
        var response = await client.PutAsJsonAsync(requestUri, value, cancellationToken);
        response.IsSuccessStatusCode.Should().BeFalse();

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        var model = JsonConvert.DeserializeObject<ErrorModel>(responseString);

        return new ErrorResponseModel(model, response.StatusCode);
    }
}