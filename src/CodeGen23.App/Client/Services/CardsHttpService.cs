using CodeGen23.App.Client.Model;
using CodeGen23.App.Shared;
using System.Net.Http.Json;

namespace CodeGen23.App.Client.Services;

public class CardsHttpService : ICardsService
{
    public HttpClient Client { get; }

    public CardsHttpService(HttpClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task CreateNewCardAsync(CreateCardModel card)
    {
        var response = await Client.PostAsJsonAsync("cards", card);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<CardListItemModel>> GetCardsAsync()
    {
        return await Client.GetFromJsonAsync<IEnumerable<CardListItemModel>>("cards") ?? Enumerable.Empty<CardListItemModel>();
    }

    public async Task ChangeCardStatusAsync(CardViewModel card, CardStatus status)
    {
        var response = await Client.PatchAsJsonAsync($"cards/{card.Id}/status", new ChangeCardStatusModel { Status = status });
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCardAsync(CardViewModel card)
    {
        var response = await Client.DeleteAsync($"cards/{card.Id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<CardListItemModel>> SearchCardsAsync(string query)
    {
        var cards = await Client.GetFromJsonAsync<IEnumerable<CardListItemModel>>(
            $"cards/search?query={System.Web.HttpUtility.UrlEncode(query)}");

        return cards ?? Enumerable.Empty<CardListItemModel>();
    }
}
