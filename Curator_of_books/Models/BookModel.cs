using System.ComponentModel.DataAnnotations;

namespace Libray.Models
{
    public class BookModel
    {
        public long Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public required string Ganre { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public required string Title { get; set; }

        [Required]
        public required int Hige { get; set; }

        [Required]
        public required int Width { get; set; }

        public SetModel set { get; set; }
        public long SetId { get; set; }
    }
}