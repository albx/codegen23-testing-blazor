namespace CodeGen23.Core;

public class Card
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; }

    public CardStatus Status { get; set; }

    public enum CardStatus
    {
        ToDo,
        InProgress,
        Done
    }
}
