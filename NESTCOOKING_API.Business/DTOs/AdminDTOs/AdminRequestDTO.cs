using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.AdminDTOs
{
    public class AdminRequestDTO
    {
        public string ReportId { get; set; }
        public StaticDetails.AdminAction AdminAction { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
