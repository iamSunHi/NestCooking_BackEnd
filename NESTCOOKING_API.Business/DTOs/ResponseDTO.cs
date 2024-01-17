using System.Net;

namespace NESTCOOKING_API.Business.DTOs
{
	public class ResponseDTO
	{
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
		public Object Result { get; set; }
		public static ResponseDTO Accept(string message = "Success", object result = null)
		{
			return new ResponseDTO
			{
				StatusCode = HttpStatusCode.OK,
				Message = message,
				Result = result
			};
		}

		public static ResponseDTO BadRequest(string message = "Error")
		{
			return new ResponseDTO
			{
				StatusCode = HttpStatusCode.BadRequest,
				Message = message,
			};
		}

		public static ResponseDTO Create(HttpStatusCode statusCode, string message = "", object result = null)
		{
			return new ResponseDTO
			{
				StatusCode = statusCode,
				Message = message,
				Result = result
			};
		}
	}
}
