using System.ComponentModel.DataAnnotations;

namespace Libray.ViewModel
{
    public class LibaryVM
    {
        public long Id { get; set; }

        public required string Ganre { get; set; }
        
    }
}