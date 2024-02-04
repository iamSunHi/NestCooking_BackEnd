using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
    public class UpdateReportDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImagesURL { get; set; }
    }
}
