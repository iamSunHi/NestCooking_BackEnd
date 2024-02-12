namespace NESTCOOKING_API.Utility
{
	public class StaticDetails
	{
		// For Local
		// public const string FE_URL = "http://localhost:3000";

		// For Production
		public const string FE_URL = "https://nest-cooking.onrender.com";
		public enum ApiType
		{
			GET, POST, PUT, DELETE
		}

		public enum ProviderLogin {
			FACEBOOK, GOOGLE
		}
		public const string Role_Admin = "admin";
		public const string Role_User = "user";
		public const string Role_Chef = "chef";

		public const string ActionStatus_PENDING = "PENDING";
		public const string ActionStatus_ACCEPTED = "ACCEPTED";
		public const string ActionStatus_REJECTED = "REJECTED";

		public const string PaymentType_DEPOSIT = "DEPOSIT";
		public const string PaymentType_WITHDRAW = "WITHDRAW";

		public const string Currency_VND = "VND";
		public const string Currency_USD = "USD";

		public const string NotificationType_REACTION = "REACTION";
		public const string NotificationType_COMMENT = "COMMENT";
		public const string NotificationType_REPORT = "REPORT";
		public const string NotificationType_REQUEST = "REQUEST";

		public const string AvatarFolderPath = "images/avatar";
		public static readonly Dictionary<string, string> FolderPath = new Dictionary<string, string>
		{
			{ "avatar", "images/avatar" }
		};
		public enum RequestStatus
		{
			Pending,
			Approved,
			Rejected,
			Completed
		}

		public const string RequestStatus_Pending = "Pending";
		public const string RequestStatus_Approved = "Approved";
		public const string RequestStatus_Rejected = "Rejected";
		public const string RequestStatus_Completed = "Completed";
	}
}
