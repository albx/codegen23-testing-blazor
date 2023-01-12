using System.ComponentModel.DataAnnotations;

namespace CodeGen23.App.Shared;

public class CreateCardModel
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
}
