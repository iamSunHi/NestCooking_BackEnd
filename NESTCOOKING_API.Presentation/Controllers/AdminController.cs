using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ResponseDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Presentation.Controllers
{
 

    [Route("api/admin/")]
    [ApiController]
    [Authorize(Role_Admin)]
    public class AdminController : ControllerBase

    {
        private readonly IResponseService _responseService;

        public AdminController(IResponseService reponseService)
        {
            _responseService = reponseService;
        }
        [HttpPost("{reportId}")]
        public async Task<IActionResult> HandleReport(string reportId, AdminAction adminAction,string title,string content )
        {

            try
            {
                var result = await _responseService.AdminHandleReportAsync(reportId, adminAction, title, content);
               if (result != null)
                {
                    return Ok(ResponseDTO.Accept(result:result));
                }
                else
                {
                    return BadRequest(ResponseDTO.BadRequest());
                }
            }catch (Exception ex)
            {
                return  BadRequest(ResponseDTO.Create(System.Net.HttpStatusCode.BadRequest, ex.Message));
            }
           
        }
    }
}
