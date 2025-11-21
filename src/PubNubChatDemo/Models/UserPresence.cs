namespace PubNubChatDemo.Models;

public class UserPresence
{
    public string Username { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public DateTime LastSeen { get; set; }
}
