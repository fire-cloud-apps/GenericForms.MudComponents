using Blazor.Shared.FormGenerator.Conversions;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FinanceGoal.Helper;

public interface IGoalHttpUtility
{
    Task<string> Insert(GoalModel? goal, string jwt);
}
public class GoalHttpUtility : IGoalHttpUtility
{
    private readonly HttpClient _http;
    private readonly string _anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNxZmt2eXBwenJ3anJpa3lhcXN2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQ2Mjc5ODUsImV4cCI6MjA2MDIwMzk4NX0.mGgQm5SEUtDvfP8d3itpGsJqIC-5-4g2Gfnivw0ALPc";

    public GoalHttpUtility(HttpClient http) => _http = http;

    #region Goal HTTP Actions
    public async Task<string> Insert(GoalModel? goal, string jwt)
    {
        Console.WriteLine("Inserting Goal - Iteration - 1");
        var req = new HttpRequestMessage(HttpMethod.Post, "Goal?select=*")
        {
            Content = JsonContent.Create (goal)

            //Content = JsonContent.Create(new { 
            //    goal.GoalName,
            //    goal.ImageSVG,
            //    goal.OwnedBy,
            //    goal.InvestmentCategory, 
            //    goal.TotalAmountInvested,
            //    goal.TargetAmount,                
            //    goal.TargetYear, 
            //    goal.StartedYear } )
        };
        req.Headers.Add("apikey", _anonKey);
        req.Headers.Add("Authorization", "Bearer " + jwt);
        req.Headers.Add("Prefer", "return=representation");
        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<string>();
    }

    #endregion
}

public class GoalModel
{
    [JsonIgnore]
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonPropertyName("GoalName")]
    public string GoalName { get; set; }
    [JsonPropertyName("OwnedBy")]
    public string OwnedBy { get; set; }
    [JsonPropertyName("InvestmentCategory")]
    public string InvestmentCategory { get; set; }
    [JsonPropertyName("TotalAmountInvested")]
    public string TotalAmountInvested { get; set; }

    [JsonPropertyName("TargetAmount")]
    public string TargetAmount { get; set; }

    #region Custom Convertion

    [JsonConverter(typeof(CustomDateTimeConverter))]
    [JsonPropertyName("StartedYear")]
    public DateTime StartedYear { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    [JsonPropertyName("TargetDate")]
    public DateTime TargetYear { get; set; }

    #endregion


    [JsonPropertyName("ImageSVG")]
    public string ImageSVG { get; set; }

}

