using System.ComponentModel.DataAnnotations;

namespace Magazine.Models
{
    //Батьківський клас для Customer 
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }

        // Discriminator для визначення типу класу
        [Required]
        public PersonType CustomerType { get; set; }
    }
}