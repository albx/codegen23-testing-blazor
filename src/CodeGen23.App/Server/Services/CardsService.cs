using CodeGen23.App.Shared;
using CodeGen23.Core;
using System.Security.Principal;

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
                IssuerName = c.IssuerName,
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
            IssuerName = card.IssuerName,
            Description = card.Description,
            Status = (CardStatus)card.Status,
            Title = card.Title
        };
    }

    public async Task<int> CreateCardAsync(CreateCardModel model, IIdentity identity)
    {
        var issuerName = identity.Name;
        if (string.IsNullOrWhiteSpace(issuerName))
        {
            throw new InvalidOperationException("Empty user");
        }

        var card = new Card
        {
            CreationDate = DateTime.Now,
            Description = model.Description,
            Title = model.Title,
            IssuerName = issuerName,
            Status = Card.CardStatus.ToDo
        };

        Context.Cards.Add(card);
        await Context.SaveChangesAsync();

        return card.Id;
    }

    public Task UpdateCardAsync(int cardId, CardDetailModel model, IIdentity identity)
    {
        var card = Context.Cards.SingleOrDefault(c => c.Id == cardId);
        if (card is null)
        {
            throw new ArgumentOutOfRangeException();
        }

        if (card.IssuerName != identity.Name)
        {
            throw new UnauthorizedAccessException();
        }

        card.Title = model.Title;
        card.Description = model.Description;
        card.Status = (Card.CardStatus)model.Status;

        return Context.SaveChangesAsync();
    }

    public Task DeleteCardAsync(int cardId, IIdentity identity)
    {
        var card = Context.Cards.SingleOrDefault(c => c.Id == cardId);
        if (card is null)
        {
            throw new ArgumentOutOfRangeException();
        }

        if (card.IssuerName != identity.Name)
        {
            throw new UnauthorizedAccessException();
        }

        Context.Cards.Remove(card);
        return Context.SaveChangesAsync();
    }
}
