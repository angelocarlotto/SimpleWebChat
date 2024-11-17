namespace SimpleWebChat.Models;

public class ChatMessage
{
    public string UserName { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

