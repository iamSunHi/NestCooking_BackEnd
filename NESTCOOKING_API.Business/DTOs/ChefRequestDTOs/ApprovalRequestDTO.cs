﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ChefRequestDTOs
{
    public class ApprovalRequestDTO
    {
        public string RequestId { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
