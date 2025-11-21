using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PubnubApi;
using PubNubChatDemo.Models;
using System.Text.Json;

namespace PubNubChatDemo.Services;

public class PubNubChatService
{
    private readonly Pubnub _pubnub;
    private readonly string _channelName = "avalonia-chat-demo";
    private readonly string _currentUserId;

    public event EventHandler<ChatMessage>? MessageReceived;
    public event EventHandler<List<UserPresence>>? PresenceUpdated;

    public PubNubChatService(string userId)
    {
        _currentUserId = userId;

        // Configure PubNub with demo credentials
        var config = new PNConfiguration(new UserId(userId))
        {
            SubscribeKey = "demo",
            PublishKey = "demo",
            Secure = true,
            LogVerbosity = PNLogVerbosity.NONE
        };

        _pubnub = new Pubnub(config);

        // Set up event listeners
        var listener = new SubscribeCallback();
        listener.Message += (sender, e) =>
        {
            if (e.Channel == _channelName)
            {
                try
                {
                    var chatMessage = JsonSerializer.Deserialize<ChatMessage>(e.Message.ToString());
                    if (chatMessage != null)
                    {
                        chatMessage.IsOwnMessage = chatMessage.Username == _currentUserId;
                        MessageReceived?.Invoke(this, chatMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing message: {ex.Message}");
                }
            }
        };

        listener.Presence += (sender, e) =>
        {
            // Handle presence events
            Console.WriteLine($"Presence: {e.Event} - {e.Uuid}");
            _ = UpdatePresenceAsync();
        };

        listener.Status += (sender, e) =>
        {
            Console.WriteLine($"Status: {e.Category}");
            if (e.Category == PNStatusCategory.PNConnectedCategory)
            {
                _ = UpdatePresenceAsync();
            }
        };

        _pubnub.AddListener(listener);
    }

    public async Task ConnectAsync()
    {
        await Task.Run(() =>
        {
            _pubnub.Subscribe<string>()
                .Channels(new[] { _channelName })
                .WithPresence()
                .Execute();
        });

        // Send join message
        await SendJoinMessageAsync();
    }

    public async Task SendMessageAsync(string message)
    {
        var chatMessage = new ChatMessage
        {
            Username = _currentUserId,
            Message = message,
            Timestamp = DateTime.Now,
            Avatar = GetAvatarForUser(_currentUserId)
        };

        var messageJson = JsonSerializer.Serialize(chatMessage);

        try
        {
            var result = await _pubnub.Publish()
                .Channel(_channelName)
                .Message(messageJson)
                .ExecuteAsync();

            if (!result.Status.Error)
            {
                Console.WriteLine($"Message published at {result.Result.Timetoken}");
            }
            else
            {
                Console.WriteLine($"Publish error: {result.Status.ErrorData?.Information ?? "Unknown error"}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    private async Task SendJoinMessageAsync()
    {
        var joinMessage = new ChatMessage
        {
            Username = "System",
            Message = $"{_currentUserId} joined the chat",
            Timestamp = DateTime.Now,
            Avatar = "🤖",
            IsOwnMessage = false
        };

        var messageJson = JsonSerializer.Serialize(joinMessage);

        try
        {
            await _pubnub.Publish()
                .Channel(_channelName)
                .Message(messageJson)
                .ExecuteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending join message: {ex.Message}");
        }
    }

    private async Task UpdatePresenceAsync()
    {
        try
        {
            var result = await _pubnub.HereNow()
                .Channels(new[] { _channelName })
                .IncludeUUIDs(true)
                .ExecuteAsync();

            if (!result.Status.Error && result.Result != null)
            {
                var users = new List<UserPresence>();

                if (result.Result.Channels.TryGetValue(_channelName, out var channelData))
                {
                    foreach (var occupant in channelData.Occupants)
                    {
                        var userId = occupant.Uuid;
                        users.Add(new UserPresence
                        {
                            Username = userId,
                            Avatar = GetAvatarForUser(userId),
                            IsOnline = true,
                            LastSeen = DateTime.Now
                        });
                    }
                }

                PresenceUpdated?.Invoke(this, users);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating presence: {ex.Message}");
        }
    }

    public async Task<List<ChatMessage>> GetHistoryAsync(int count = 50)
    {
        var messages = new List<ChatMessage>();

        try
        {
            var result = await _pubnub.FetchMessages()
                .Channels(new[] { _channelName })
                .MaximumPerChannel(count)
                .ExecuteAsync();

            if (!result.Status.Error && result.Result != null)
            {
                if (result.Result.Channels.TryGetValue(_channelName, out var channelMessages))
                {
                    foreach (var msg in channelMessages)
                    {
                        try
                        {
                            var chatMessage = JsonSerializer.Deserialize<ChatMessage>(msg.Message.ToString());
                            if (chatMessage != null)
                            {
                                chatMessage.IsOwnMessage = chatMessage.Username == _currentUserId;
                                messages.Add(chatMessage);
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching history: {ex.Message}");
        }

        return messages;
    }

    public void Disconnect()
    {
        _pubnub.Unsubscribe<string>()
            .Channels(new[] { _channelName })
            .Execute();
    }

    private string GetAvatarForUser(string username)
    {
        // Simple avatar assignment based on username hash
        var avatars = new[] { "👨", "👩", "🧑", "👦", "👧", "🧔", "👴", "👵", "🦸", "🧙" };
        var hash = Math.Abs(username.GetHashCode());
        return avatars[hash % avatars.Length];
    }
}
