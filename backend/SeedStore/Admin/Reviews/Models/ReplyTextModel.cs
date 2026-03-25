using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Reviews.Models
{
    public class ReplyTextModel
    {
        [Required]
        [MaxLength(2000)]
        public string Text { get; set; } = string.Empty;
    }
}
