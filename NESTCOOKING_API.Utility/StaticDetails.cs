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
		public enum ActionStatus
		{
			PENDING, ACCEPTED, REJECTED
		}
		public enum AdminAction
		{
			Accept, Reject
		}
		public enum ReportType
		{
			user, comment, recipe

		}
		public enum ReactionType
		{
			haha, favorite, like
		}

		public const string ReportType_USER = "user";
		public const string ReportType_COMMENT = "comment";
		public const string ReportType_RECIPE = "recipe";

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
	}
}
