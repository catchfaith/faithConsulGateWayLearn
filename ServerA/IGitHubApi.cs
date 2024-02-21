using Refit;

namespace ServerB;

public interface IGitHubApi
{
    [Get("/getperson")]
    public Task<List<Person>> getPerson();
}