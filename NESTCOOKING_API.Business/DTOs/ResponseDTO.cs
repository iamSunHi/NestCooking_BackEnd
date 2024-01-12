using System.Net;

namespace NESTCOOKING_API.Business.DTOs
{
	public class ResponseDTO
	{
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
		public Object Result { get; set; }
	}
}
