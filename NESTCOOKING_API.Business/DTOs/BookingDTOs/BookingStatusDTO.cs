﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
    public class BookingStatusDTO
    {
        public string BookingId { get; set; } = null!; 
        public string Status { get; set; } = null!;
    }
}
