using Doctor.Domain.Abstractions;
using Microsoft.Extensions.Options;
using Shared.Primitives;
using Shared.Result;

namespace Doctor.Infrastructure.Services;

public class UserService(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings) : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly string _authUrl = apiSettings.Value.AuthUrl;
    public async Task<Result<string>> GetUserId(string email)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_authUrl}Auth/get-userId?email={email}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            if(string.IsNullOrWhiteSpace(result))
                return Result.Failure<string>(new Error("invalidUserid", "user id is empty"));
            return result;
        }
        else
        {
            return Result.Failure<string>(new Error("UserIdEndPointFailure","can't call end point"));
        }
    }
}
