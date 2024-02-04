using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ResponseDTOs
{
    public class AdminResponseDTO
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
