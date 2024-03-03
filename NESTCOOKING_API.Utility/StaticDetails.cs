﻿namespace NESTCOOKING_API.Utility
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

		public enum AdminAction
		{
			ACCEPT, REJECT
		}

		public enum ReactionType
		{
			haha, favorite, like
		}

		public const string ReportType_USER = "user";
		public const string ReportType_COMMENT = "comment";
		public const string ReportType_RECIPE = "recipe";

		public const string CommentType_RECIPE = "Recipe";
		public const string CommentType_COMMENTCHILD = "Comment";

		public enum ProviderLogin {
			FACEBOOK, GOOGLE
		}

		public const string Role_Admin = "admin";
		public const string Role_User = "user";
		public const string Role_Chef = "chef";

		public const string ActionStatus_PENDING = "PENDING";
		public const string ActionStatus_ACCEPTED = "ACCEPTED";
		public const string ActionStatus_REJECTED = "REJECTED";
		public const string ActionStatus_COMPLETED = "COMPLETED";

		public const string PaymentType_DEPOSIT = "DEPOSIT";
		public const string PaymentType_WITHDRAW = "WITHDRAW";

		public const string Currency_VND = "VND";
		public const string Currency_USD = "USD";

		public const string NotificationType_REACTION = "REACTION";
		public const string NotificationType_COMMENT = "COMMENT";
		public const string NotificationType_REPORT = "REPORT";
		public const string NotificationType_REQUEST = "REQUEST";

		public const string TransactionFe_URL= "https://nest-cooking.onrender.com/payment/transaction";
		public const string IpAddress = "127.0.0.1";


        public const string AvatarFolderPath = "images/avatar";
		public static readonly Dictionary<string, string> FolderPath = new Dictionary<string, string>
		{
			{ "avatar", "images/avatar" }
		};

		public const string TargetType_RECIPE = "recipe";
		public const string TargetType_COMMENT = "comment";
	}
}
