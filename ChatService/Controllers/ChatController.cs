namespace ChatService.Controllers
{
    using ChatService.Dto;
    using ChatService.Service.ChatService;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public ActionResult<ChatResponseDto> Chat([FromBody] ChatRequestDto request)
        {
            var response = _chatService.GetResponse(request.Message);
            return Ok(new ChatResponseDto { Response = response });
        }
    }
}
