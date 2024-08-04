using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Libray.Models
{
    [Index(nameof(Ganre), IsUnique = true)]
    public class LibaryModel
    {
        public long Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public required string Ganre { get; set; }

        public List<ShelfModel> Shelfes { get; set; } = [];
    }
}