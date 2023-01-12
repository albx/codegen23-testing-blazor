using System.ComponentModel.DataAnnotations;

namespace CodeGen23.App.Shared;

public class CardDetailModel
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string IssuerName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; }

    public CardStatus Status { get; set; }
}
