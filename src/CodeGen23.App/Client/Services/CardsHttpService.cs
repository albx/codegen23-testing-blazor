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
}
