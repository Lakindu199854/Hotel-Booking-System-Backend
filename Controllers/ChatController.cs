namespace Hotel_Booking_App.Controllers
{
    using Hotel_Booking_App.Dto;
    using Hotel_Booking_App.Service.ChatService;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public ActionResult<ChatResponseDto> Chat([FromBody] ChatRequestDto request)
        {
            var responseText = _chatService.GetResponse(request.Message);
            return Ok(new ChatResponseDto
            {
                Response = responseText
            });
        }
    }

}
