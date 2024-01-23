﻿namespace NESTCOOKING_API.Utility
{
	public class AppString
	{
		public static string NameEmailOwnerDisplay = "NestCooking";

		public static string ResetPasswordSubjectEmail = "Reset Your Password";
		public static string ResetPasswordContentEmail(string resetPasswordLink)
		{
			return $"<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n<title></title>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n<link href=\"https://fonts.cdnfonts.com/css/alamanda\" rel=\"stylesheet\">\r\n<style type=\"text/css\">\r\n@media screen {{\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: normal;\r\nfont-weight: 400;\r\nsrc: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: normal;\r\nfont-weight: 700;\r\nsrc: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: italic;\r\nfont-weight: 400;\r\nsrc: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: italic;\r\nfont-weight: 700;\r\nsrc: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n}}\r\n}}\r\n\r\n/* CLIENT-SPECIFIC STYLES */\r\nbody,\r\ntable,\r\ntd,\r\na {{\r\n-webkit-text-size-adjust: 100%;\r\n-ms-text-size-adjust: 100%;\r\n}}\r\n\r\ntable,\r\ntd {{\r\nmso-table-lspace: 0pt;\r\nmso-table-rspace: 0pt;\r\n}}\r\n\r\nimg {{\r\n-ms-interpolation-mode: bicubic;\r\n}}\r\n\r\n/* RESET STYLES */\r\nimg {{\r\nborder: 0;\r\nheight: auto;\r\nline-height: 100%;\r\noutline: none;\r\ntext-decoration: none;\r\n}}\r\n\r\ntable {{\r\nborder-collapse: collapse !important;\r\n}}\r\n\r\nbody {{\r\nheight: 100% !important;\r\nmargin: 0 !important;\r\npadding: 0 !important;\r\nwidth: 100% !important;\r\n}}\r\n\r\n/* iOS BLUE LINKS */\r\na[x-apple-data-detectors] {{\r\ncolor: inherit !important;\r\ntext-decoration: none !important;\r\nfont-size: inherit !important;\r\nfont-family: inherit !important;\r\nfont-weight: inherit !important;\r\nline-height: inherit !important;\r\n}}\r\n\r\n/* MOBILE STYLES */\r\n@media screen and (max-width:600px) {{\r\nh1 {{\r\nfont-size: 32px !important;\r\nline-height: 32px !important;\r\n}}\r\n}}\r\n\r\n/* ANDROID CENTER FIX */\r\ndiv[style*=\"margin: 16px 0;\"] {{\r\nmargin: 0 !important;\r\n}}\r\n</style>\r\n</head>\r\n\r\n<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n<!-- HIDDEN PREHEADER TEXT -->\r\n<div\r\nstyle=\"display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;\">\r\nWe're thrilled to have you here! Get ready to dive into your new account.\r\n</div>\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n<!-- LOGO -->\r\n<tr>\r\n<td bgcolor=\"#68bcbe\" align=\"center\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#68bcbe\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\"\r\nstyle=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n<h1 style=\"font-size: 48px; font-weight: 400; margin: 2;\">Hi there!</h1> <img\r\nsrc=\" https://img.icons8.com/clouds/100/000000/handshake.png\" width=\"125\"\r\nheight=\"120\" style=\"display: block; border: 0px;\" />\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 80px 10px;\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">We're thrilled to welcome you on board! To initiate the password reset process, kindly verify your account by clicking the button below:</p>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\">\r\n<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 60px 30px;\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n<tr>\r\n<td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#68bcbe\">" +
				$"<a href=\"{resetPasswordLink}\" target=\"_blank\" style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #68bcbe; display: inline-block;\">" +
				$"Reset Password" +
				$"</a></td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">If you have any questions, just reply to this email&mdash;we're\r\nalways happy to help out.</p>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">Cheers,<br>NestCooking</p>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</table>\r\n</body>\r\n</html>";
		}
		public static string ConfirmPasswordMismatchErrorMessage = "The confirm password does not match the new password.";

		public static string SamePasswordErrorMessage = "New password must be different from current password.";
		public static string ChangePasswordSuccessMessage = "Password changed successfully.";

		public static string InvalidPasswordErrorMessage = "Invalid password.";
		public static string InvalidImageErrorMessage = "Please choose a valid image file to upload.";
		public static string UpdateAvatarSuccessMessage = "Change avatar successfully.";
		public static string UpdateAvatarErrorMessage = "Something went error when update the avatar.";
		public static string UpdateInformationSuccessMessage = "Your information changed successfully.";
		public static string UpdateInformationErrorMessage = "Something went wrong when update your information.";
		public static string RegisterSuccessMessage = "Register successfully. Please check your email.";
		public static string RegisterErrorMessage = "Something went wrong when register.";

		public static string EmailConfirmationSubjectEmail = "Confirm Your Account.";
		public static string EmailConfirmationContentEmail(string emailConfirmationLink)
		{
			return $"<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n<title></title>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n<link href=\"https://fonts.cdnfonts.com/css/alamanda\" rel=\"stylesheet\">\r\n<style type=\"text/css\">\r\n@media screen {{\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: normal;\r\nfont-weight: 400;\r\nsrc: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: normal;\r\nfont-weight: 700;\r\nsrc: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: italic;\r\nfont-weight: 400;\r\nsrc: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: italic;\r\nfont-weight: 700;\r\nsrc: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n}}\r\n}}\r\n\r\n/* CLIENT-SPECIFIC STYLES */\r\nbody,\r\ntable,\r\ntd,\r\na {{\r\n-webkit-text-size-adjust: 100%;\r\n-ms-text-size-adjust: 100%;\r\n}}\r\n\r\ntable,\r\ntd {{\r\nmso-table-lspace: 0pt;\r\nmso-table-rspace: 0pt;\r\n}}\r\n\r\nimg {{\r\n-ms-interpolation-mode: bicubic;\r\n}}\r\n\r\n/* RESET STYLES */\r\nimg {{\r\nborder: 0;\r\nheight: auto;\r\nline-height: 100%;\r\noutline: none;\r\ntext-decoration: none;\r\n}}\r\n\r\ntable {{\r\nborder-collapse: collapse !important;\r\n}}\r\n\r\nbody {{\r\nheight: 100% !important;\r\nmargin: 0 !important;\r\npadding: 0 !important;\r\nwidth: 100% !important;\r\n}}\r\n\r\n/* iOS BLUE LINKS */\r\na[x-apple-data-detectors] {{\r\ncolor: inherit !important;\r\ntext-decoration: none !important;\r\nfont-size: inherit !important;\r\nfont-family: inherit !important;\r\nfont-weight: inherit !important;\r\nline-height: inherit !important;\r\n}}\r\n\r\n/* MOBILE STYLES */\r\n@media screen and (max-width:600px) {{\r\nh1 {{\r\nfont-size: 32px !important;\r\nline-height: 32px !important;\r\n}}\r\n}}\r\n\r\n/* ANDROID CENTER FIX */\r\ndiv[style*=\"margin: 16px 0;\"] {{\r\nmargin: 0 !important;\r\n}}\r\n</style>\r\n</head>\r\n\r\n<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n<!-- HIDDEN PREHEADER TEXT -->\r\n<div\r\nstyle=\"display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;\">\r\nWe're thrilled to have you here! Get ready to dive into your new account.\r\n</div>\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n<!-- LOGO -->\r\n<tr>\r\n<td bgcolor=\"#68bcbe\" align=\"center\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#68bcbe\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\"\r\nstyle=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n<h1 style=\"font-size: 48px; font-weight: 400; margin: 2;\">Welcome!</h1> <img\r\nsrc=\" https://img.icons8.com/clouds/100/000000/handshake.png\" width=\"125\"\r\nheight=\"120\" style=\"display: block; border: 0px;\" />\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 80px 10px;\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">We're excited to have you get started. First, you need to confirm your account. Just press the button below:</p>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\">\r\n<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 60px 30px;\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n<tr>\r\n<td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#68bcbe\">" +
				$"<a href=\"{emailConfirmationLink}\" target=\"_blank\" style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #68bcbe; display: inline-block;\">" +
				$"Verify Account" +
				$"</a></td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">If you have any questions, just reply to this email&mdash;we're\r\nalways happy to help out.</p>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">Cheers,<br>NestCooking</p>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</table>\r\n</body>\r\n</html>";
		}

		public static string UserNotFoundMessage = "User not found";
		public static string EmailConfirmationSuccessMessage = "Email confirmation successfully";
		public static string SomethingWrongMessage = "Something went wrong";
		public static string InvalidTokenErrorMessage = "Wrong token";
		public static string NotEmailConfirmedErrorMessage = "Sorry, but you are not confirmed your email. Please check your email again";
		public static string LockoutAccountErrorMessage = "Your account has been locked due to multiple failed login attempts. Please try again later.";
		public static string ResendEmailConfirmationSubjectEmail = "Re-send email confirmation";
		public static string ResendEmailConfirmationContentEmail(string emailConfirmationLink)
		{
			return $"<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n<title></title>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n<link href=\"https://fonts.cdnfonts.com/css/alamanda\" rel=\"stylesheet\">\r\n<style type=\"text/css\">\r\n@media screen {{\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: normal;\r\nfont-weight: 400;\r\nsrc: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: normal;\r\nfont-weight: 700;\r\nsrc: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: italic;\r\nfont-weight: 400;\r\nsrc: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');\r\n}}\r\n\r\n@font-face {{\r\nfont-family: 'Lato';\r\nfont-style: italic;\r\nfont-weight: 700;\r\nsrc: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');\r\n}}\r\n}}\r\n\r\n/* CLIENT-SPECIFIC STYLES */\r\nbody,\r\ntable,\r\ntd,\r\na {{\r\n-webkit-text-size-adjust: 100%;\r\n-ms-text-size-adjust: 100%;\r\n}}\r\n\r\ntable,\r\ntd {{\r\nmso-table-lspace: 0pt;\r\nmso-table-rspace: 0pt;\r\n}}\r\n\r\nimg {{\r\n-ms-interpolation-mode: bicubic;\r\n}}\r\n\r\n/* RESET STYLES */\r\nimg {{\r\nborder: 0;\r\nheight: auto;\r\nline-height: 100%;\r\noutline: none;\r\ntext-decoration: none;\r\n}}\r\n\r\ntable {{\r\nborder-collapse: collapse !important;\r\n}}\r\n\r\nbody {{\r\nheight: 100% !important;\r\nmargin: 0 !important;\r\npadding: 0 !important;\r\nwidth: 100% !important;\r\n}}\r\n\r\n/* iOS BLUE LINKS */\r\na[x-apple-data-detectors] {{\r\ncolor: inherit !important;\r\ntext-decoration: none !important;\r\nfont-size: inherit !important;\r\nfont-family: inherit !important;\r\nfont-weight: inherit !important;\r\nline-height: inherit !important;\r\n}}\r\n\r\n/* MOBILE STYLES */\r\n@media screen and (max-width:600px) {{\r\nh1 {{\r\nfont-size: 32px !important;\r\nline-height: 32px !important;\r\n}}\r\n}}\r\n\r\n/* ANDROID CENTER FIX */\r\ndiv[style*=\"margin: 16px 0;\"] {{\r\nmargin: 0 !important;\r\n}}\r\n</style>\r\n</head>\r\n\r\n<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">\r\n<!-- HIDDEN PREHEADER TEXT -->\r\n<div\r\nstyle=\"display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;\">\r\nWe're thrilled to have you here! Get ready to dive into your new account.\r\n</div>\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n<!-- LOGO -->\r\n<tr>\r\n<td bgcolor=\"#68bcbe\" align=\"center\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"> </td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#68bcbe\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\"\r\nstyle=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">\r\n<h1 style=\"font-size: 48px; font-weight: 400; margin: 2;\">Welcome!</h1> <img\r\nsrc=\" https://img.icons8.com/clouds/100/000000/handshake.png\" width=\"125\"\r\nheight=\"120\" style=\"display: block; border: 0px;\" />\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 80px 10px;\">\r\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">We're excited to get you started, but it looks like you haven't confirmed your account yet. You must do that before you can sign in. Just press the button below:</p>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\">\r\n<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 60px 30px;\">\r\n<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n<tr>\r\n<td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#68bcbe\">" +
				$"<a href=\"{emailConfirmationLink}\" target=\"_blank\" style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #68bcbe; display: inline-block;\">" +
				$"Verify Account" +
				$"</a></td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">If you have any questions, just reply to this email&mdash;we're\r\nalways happy to help out.</p>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td bgcolor=\"#ffffff\" align=\"left\"\r\nstyle=\"padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">\r\n<p style=\"margin: 0;\">Cheers,<br>NestCooking</p>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</table>\r\n</body>\r\n</html>";
		}
		public static string InvalidEmailErrorMessage = "Invalid email";
		public static string IncorrectCredentialsLoginErrorMessage = "Username or password is incorrect!";
		public static string AccountLockedOutLoginErrorMessage = "This account is locked out!";
		public static string RequestErrorMessage = "Error in request !";
	}
}
