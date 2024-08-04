using System.ComponentModel.DataAnnotations;

namespace Libray.Models
{
    public class ShelfModel
    {
        public long Id { get; set; }

        public required int High { get; set; }

        public required int Width { get; set; }

        public LibaryModel Libary { get; set; }
        public long LibaryId { get; set; }

        public List<SetModel> sets { get; set; } = [];
    }
}