using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignUp.Model
{
    public class Announcement
    {
 public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = "";

    [Required]
    public string Text { get; set; } = "";

    public string ForWhoRaw { get; set; } = "";

    [NotMapped]
    public List<string> ForWho
    {
        get => string.IsNullOrEmpty(ForWhoRaw)
                ? new List<string>()
                : ForWhoRaw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        set => ForWhoRaw = string.Join(',', value);
    }

    public DateTime PublishDate { get; set; }
    public string? ImageUrl { get; set; }
}}
