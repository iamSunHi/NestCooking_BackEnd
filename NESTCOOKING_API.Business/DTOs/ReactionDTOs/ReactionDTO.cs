using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ReactionDTOs
{
    public class ReactionDTO
    {
        public StaticDetails.ReactionType ReactionType { get; set; }
        public string TargetID { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
