using CodeGen23.App.Shared;
using CodeGen23.Core;

namespace CodeGen23.App.Server.Services;

public class CardsService
{
    public CodeGen23Context Context { get; }

    public CardsService(CodeGen23Context context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<CardListItemModel> GetAllCards()
    {
        var cards = Context.Cards
            .OrderBy(c => c.CreationDate)
            .Select(c => new CardListItemModel
            {
                Id = c.Id,
                CreationDate = c.CreationDate,
                Status = (CardStatus)c.Status,
                Title = c.Title
            }).ToArray();

        return cards;
    }

    public CardDetailModel? GetCardDetail(int cardId)
    {
        var card = Context.Cards.SingleOrDefault(c => c.Id == cardId);
        if (card is null)
        {
            return null;
        }

        return new CardDetailModel
        {
            Id = card.Id,
            CreationDate = card.CreationDate,
            Description = card.Description,
            Status = (CardStatus)card.Status,
            Title = card.Title
        };
    }

    public async Task<int> CreateCardAsync(CreateCardModel model)
    {
        var card = new Card
        {
            CreationDate = DateTime.Now,
            Description = model.Description,
            Title = model.Title,
            Status = Card.CardStatus.ToDo
        };

        Context.Cards.Add(card);
        await Context.SaveChangesAsync();

        return card.Id;
    }

    public Task UpdateCardAsync(int cardId, CardDetailModel model)
    {
        var card = Context.Cards.SingleOrDefault(c => c.Id == cardId);
        if (card is null)
        {
            throw new ArgumentOutOfRangeException();
        }

        card.Title = model.Title;
        card.Description = model.Description;
        card.Status = (Card.CardStatus)model.Status;

        return Context.SaveChangesAsync();
    }

    public Task DeleteCardAsync(int cardId)
    {
        var card = Context.Cards.SingleOrDefault(c => c.Id == cardId);
        if (card is null)
        {
            throw new ArgumentOutOfRangeException();
        }

        Context.Cards.Remove(card);
        return Context.SaveChangesAsync();
    }

    public Task ChangeCardStatusAsync(int cardId, CardStatus status)
    {
        var card = Context.Cards.SingleOrDefault(c => c.Id == cardId);
        if (card is null)
        {
            throw new ArgumentOutOfRangeException();
        }

        card.Status = (Card.CardStatus)status;
        return Context.SaveChangesAsync();
    }

    public IEnumerable<CardListItemModel> SearchCardsByQuery(string query)
    {
        var cards = (from c in Context.Cards
                     let description = c.Description
                     where c.Title.Contains(query) || (description != null && description.Contains(query))
                     select new CardListItemModel
                     {
                         Id = c.Id,
                         CreationDate = c.CreationDate,
                         Status = (CardStatus)c.Status,
                         Title = c.Title
                     }).ToArray();

        return cards;
    }
}
