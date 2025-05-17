using Newtonsoft.Json;

namespace QuestionBank.Api.Responses;

public abstract class Response
{
    [JsonProperty(Order = 1)] 
    public string Title { get; set; } = null!;

    [JsonProperty(Order = 2)] 
    public int Status { get; set; }
}