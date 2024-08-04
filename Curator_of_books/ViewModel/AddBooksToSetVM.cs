using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Libray.ViewModel
{
    
        public class AddBooksToSetVM
        {
            [Required]
            public required long SetId { get; set; }

            [Required]
            public long ShelfId { get; set; }

            [Required]
            public IEnumerable<long> SelectedBookIds { get; set; } = new List<long>();

            public IEnumerable<SelectListItem> Sets { get; set; }
            public IEnumerable<SelectListItem> Shelfes { get; set; }
            public IEnumerable<SelectListItem> Books { get; set; }
        }
    }


