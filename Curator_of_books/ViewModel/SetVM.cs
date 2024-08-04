using System.ComponentModel.DataAnnotations;

namespace Libray.ViewModel
{
    public class SetVM
    {
        public long Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public required string SetName { get; set; }

    }
}