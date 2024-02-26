using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Models
{
    public class RecipeReaction
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public Recipe Recipe { get; set; } = null!;
        public Reaction Reaction { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
