﻿using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IEmailService
    {
        void SendEmail(EmailResponseDTO message);
    }
}
