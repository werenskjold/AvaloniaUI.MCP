# PubNub Chat Demo - Avalonia UI

A beautiful real-time chat application built with **Avalonia UI** and **PubNub**, demonstrating the power of real-time messaging in a modern cross-platform desktop application.

## Features

- 🚀 **Real-time messaging** - Instant message delivery powered by PubNub
- 👥 **Online presence** - See who's currently in the chat room
- 📜 **Message history** - Load past messages when you join
- 🎨 **Modern UI** - Beautiful Fluent Design interface
- 💻 **Cross-platform** - Runs on Windows, macOS, and Linux
- 🔒 **Demo credentials** - Uses PubNub's demo API (demo/demo)

## Technology Stack

- **Avalonia UI 11.3.1** - Cross-platform XAML UI framework
- **PubNub SDK 7.5.0** - Real-time messaging infrastructure
- **.NET 9.0** - Modern .NET runtime
- **CommunityToolkit.Mvvm** - MVVM pattern helpers
- **Fluent Theme** - Modern Windows 11-style design

## How It Works

### Architecture

The application follows the MVVM (Model-View-ViewModel) pattern:

```
Models/
├── ChatMessage.cs       - Message data model
└── UserPresence.cs      - User presence data model

Services/
└── PubNubChatService.cs - PubNub integration wrapper

ViewModels/
├── ViewModelBase.cs     - Base ViewModel class
└── MainViewModel.cs     - Main chat logic

Views/
└── MainWindow.axaml     - Chat UI
```

### PubNub Integration

The `PubNubChatService` wraps the PubNub SDK and provides:

1. **Publish/Subscribe** - Real-time message exchange
2. **Presence** - Track online users
3. **History** - Fetch past messages
4. **Auto-reconnect** - Handles connection drops

### Demo Credentials

The app uses PubNub's publicly available demo keys:
- **Publish Key**: demo
- **Subscribe Key**: demo

These are perfect for testing and demonstrations. For production use, you should create your own PubNub account and use your own keys.

## Running the Application

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022, VS Code, or JetBrains Rider (optional)

### Build and Run

```bash
# Navigate to the project directory
cd src/PubNubChatDemo

# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## Using the Chat

1. **Launch the app** - The application automatically connects to PubNub
2. **Random username** - You're assigned a random username (e.g., User1234)
3. **Type a message** - Use the text box at the bottom
4. **Press Enter or click Send** - Your message is instantly sent
5. **See online users** - Check the right panel for who's online
6. **Open multiple instances** - Test real-time messaging by opening multiple windows

## Key Code Highlights

### PubNub Configuration

```csharp
var config = new PNConfiguration(new UserId(userId))
{
    SubscribeKey = "demo",
    PublishKey = "demo",
    Secure = true
};
```

### Publishing Messages

```csharp
await _pubnub.Publish()
    .Channel(_channelName)
    .Message(messageJson)
    .Execute();
```

### Subscribing to Messages

```csharp
_pubnub.Subscribe<string>()
    .Channels(new[] { _channelName })
    .WithPresence()
    .Execute();
```

### Real-time UI Updates

```csharp
_chatService.MessageReceived += OnMessageReceived;

private void OnMessageReceived(object? sender, ChatMessage message)
{
    Dispatcher.UIThread.InvokeAsync(() =>
    {
        Messages.Add(message);
    });
}
```

## UI Features

- **Message bubbles** - Different styles for your messages vs. others
- **User avatars** - Random emoji avatars for each user
- **Timestamps** - See when each message was sent
- **Online indicators** - Green dots show who's online
- **Responsive layout** - Adapts to different window sizes
- **Fluent Design** - Modern, clean interface

## Customization

### Change the Channel

Edit `PubNubChatService.cs`:

```csharp
private readonly string _channelName = "your-custom-channel";
```

### Use Your Own PubNub Keys

Replace the demo keys in `PubNubChatService.cs`:

```csharp
var config = new PNConfiguration(new UserId(userId))
{
    SubscribeKey = "your-subscribe-key",
    PublishKey = "your-publish-key",
    Secure = true
};
```

Get your free keys at: https://www.pubnub.com/

## Next Steps

This demo can be extended with:

- **Typing indicators** - Show when someone is typing
- **File sharing** - Send images and files
- **Private messaging** - One-on-one conversations
- **Message reactions** - Like/react to messages
- **Push notifications** - Alert users of new messages
- **User authentication** - Secure login system
- **Chat rooms** - Multiple channels to join
- **Message moderation** - Content filtering

## Learn More

- **Avalonia UI**: https://avaloniaui.net/
- **PubNub**: https://www.pubnub.com/docs/
- **PubNub C# SDK**: https://www.pubnub.com/docs/sdks/c-sharp
- **.NET 9.0**: https://dotnet.microsoft.com/

## License

This demo is provided as-is for educational and demonstration purposes.

---

**Built with ❤️ using Avalonia UI and PubNub**
