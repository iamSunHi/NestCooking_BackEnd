using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.NotificationDTOs
{
    public class NotificationReadDTO
    {
        public string Id { get; set; } = null!;
        public UserShortInfoDTO? Sender{ get; set; }
        public string NotificationType { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsSeen { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
