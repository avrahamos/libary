using System.ComponentModel.DataAnnotations;

namespace Libray.ViewModel
{
    public class BookVM
    {
        public long Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public required string Ganre { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public required string Title { get; set; }

        [Required]
        public required int High { get; set; }

        [Required]
        public required int Width { get; set; }

        public long SetId { get; set; }
    }
}
