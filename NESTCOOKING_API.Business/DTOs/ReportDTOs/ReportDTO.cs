using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
    public class ReportDTO
    {
        public string TargetId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImagesURL { get; set; }
    }
}
