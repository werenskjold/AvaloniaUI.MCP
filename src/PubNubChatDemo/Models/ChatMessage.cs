namespace PubNubChatDemo.Models;

public class ChatMessage
{
    public string Username { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Avatar { get; set; } = string.Empty;
    public bool IsOwnMessage { get; set; }

    public string TimeFormatted => Timestamp.ToString("HH:mm:ss");
}
