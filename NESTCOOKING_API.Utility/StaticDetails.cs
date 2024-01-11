namespace NESTCOOKING_API.Utility
{
	public class StaticDetails
	{
		public enum ApiType
		{
			GET, POST, PUT, DELETE
		}

		public const string Role_Admin = "Admin";
		public const string Role_User = "User";
		public const string Role_Chef = "Chef";

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
	}
}
