using CodeGen23.App.Client.Model;
using CodeGen23.App.Shared;

namespace CodeGen23.App.Client.Services;

public interface ICardsService
{
    Task<IEnumerable<CardListItemModel>> GetCardsAsync();

    Task CreateNewCardAsync(CreateCardModel card);

    Task ChangeCardStatusAsync(CardViewModel card, CardStatus status);

    Task DeleteCardAsync(CardViewModel card);
}
