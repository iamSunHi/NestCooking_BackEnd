using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Exceptions
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException() : base(AppString.UserNotFoundMessage) { }
	}
}
