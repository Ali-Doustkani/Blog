using System.ComponentModel.DataAnnotations;

namespace Blog.Domain
{
   public class DomainObjectEntry
   {
      [Required]
      public string Id { get; set; }
   }
}
