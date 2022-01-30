using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Campul Name este obligatoriu.")]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Required(ErrorMessage ="Campul Display Order este obligatoriu.")]
        [Range(1,100,ErrorMessage ="Display Order trebuie sa fie intre 1 si 100.")]
        public int DisplayOrder { get; set; }


        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
