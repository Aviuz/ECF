using ECF;
using ECF.Utilities;
using System.Net.Http.Json;
using System.Text.Json;

namespace Example.Commands;

[Command("get-posts", "posts")]
public class FetchPostsCommand : AsyncCommandBase
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly HttpClient httpClient;

    public FetchPostsCommand(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
        httpClient = httpClientFactory.CreateClient();
    }

    [Parameter("-id --post-id", Description = "If specified it will return only post with specified id.")]
    public int? PostId { get; set; }

    [Flag("-f", "--format", Description = "If set it will format output json with indentation")]
    public bool FormatJson { get; set; }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if(PostId != default)
        {
            Post? post = await httpClient.GetFromJsonAsync<Post>($"https://jsonplaceholder.typicode.com/posts/{PostId}");
            WriteToConsole(post, FormatJson);
        }
        else
        {
            Post[]? posts = await httpClient.GetFromJsonAsync<Post[]>($"https://jsonplaceholder.typicode.com/posts");
            WriteToConsole(posts, FormatJson);
        }
    }

    public void WriteToConsole<T>(T value, bool indented) 
        => ColorConsole.WriteLine(JsonSerializer.Serialize(value, options: new() { WriteIndented = indented }), ConsoleColor.Blue);

    private class Post
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Body { get; set; }
    }
}

