using NESTCOOKING_API.Business.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
    public class ReportResponsDTO
    {
        public string Id { get; set; }
        public UserDTO User { get; set; }
        public string Target { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
