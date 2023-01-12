namespace CodeGen23.App.Shared;

public class CardListItemModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string IssuerName { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }

    public CardStatus Status { get; set; }
}
