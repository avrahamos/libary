using System.ComponentModel.DataAnnotations;

namespace Libray.Models
{
    public class SetModel
    {
        public long Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public required string SetName { get; set; }

        public ShelfModel? Shelf { get; set; }
        public long? ShelfId { get; set; }

        public List<BookModel> books { get; set; } = [];
    }
}