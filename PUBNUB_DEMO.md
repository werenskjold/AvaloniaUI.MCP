# PubNub Chat Demo Application

## Overview

This repository now includes a **real-time chat application** built with **Avalonia UI** and **PubNub**, showcasing the integration of modern desktop UI with cloud-based real-time messaging infrastructure.

## What's New

A complete, production-quality chat application has been added to demonstrate:

- **Real-time messaging** using PubNub's pub/sub architecture
- **Cross-platform desktop UI** with Avalonia's XAML-based framework
- **Modern MVVM architecture** using CommunityToolkit.Mvvm
- **User presence tracking** to show who's online
- **Message history** retrieval
- **Beautiful Fluent Design** interface

## Project Location

```
src/PubNubChatDemo/
```

See the [complete README](src/PubNubChatDemo/README.md) for detailed documentation.

## Quick Start

### Running the Demo App

```bash
cd src/PubNubChatDemo
dotnet restore
dotnet build
dotnet run
```

The app will:
1. Auto-connect to PubNub using demo credentials (demo/demo)
2. Assign you a random username (e.g., User1234)
3. Load recent message history
4. Enable real-time chatting with anyone else running the app

### Try It Out

Open **multiple instances** of the app to see real-time messaging in action! Messages appear instantly across all running instances.

## Features Demonstrated

### 1. Real-Time Communication
- Instant message delivery via PubNub pub/sub
- No polling - true push-based updates
- Sub-second latency

### 2. User Presence
- See who's currently online
- Automatic presence updates
- User join/leave notifications

### 3. Message History
- Automatic history loading on connect
- Persistent message storage via PubNub
- Seamless catch-up experience

### 4. Modern UI/UX
- Clean, modern Fluent Design
- Distinct message bubbles for own vs. other messages
- User avatars (emoji-based)
- Timestamps on all messages
- Online user sidebar
- Responsive layout

## Technical Architecture

### Technology Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| UI Framework | Avalonia UI | 11.3.1 |
| Messaging | PubNub PCL | 7.5.0 |
| Runtime | .NET | 9.0 |
| MVVM | CommunityToolkit.Mvvm | 8.4.0 |
| Theme | Fluent Design | 11.3.1 |

### Project Structure

```
PubNubChatDemo/
├── Models/
│   ├── ChatMessage.cs      # Message data model
│   └── UserPresence.cs     # Presence data model
├── Services/
│   └── PubNubChatService.cs # PubNub API wrapper
├── ViewModels/
│   ├── ViewModelBase.cs    # Base ViewModel
│   └── MainViewModel.cs    # Main chat logic
├── Views/
│   └── MainWindow.axaml    # Chat UI
├── App.axaml               # Application definition
└── Program.cs              # Entry point
```

### Key Components

#### PubNubChatService
Wraps the PubNub SDK and provides:
- Connection management
- Message publishing
- Real-time subscriptions
- Presence tracking
- History retrieval

#### MainViewModel
Manages the chat state:
- Message collection (ObservableCollection)
- Online users list
- Connection status
- User input handling
- Event coordination

#### MainWindow (XAML)
Provides the UI:
- Message list with custom styling
- Message input with send button
- Online users sidebar
- Status indicators
- Responsive layout

## PubNub Integration Details

### Configuration

```csharp
var config = new PNConfiguration(new UserId(userId))
{
    SubscribeKey = "demo",
    PublishKey = "demo",
    Secure = true,
    LogVerbosity = PNLogVerbosity.NONE
};
```

### Demo Credentials

The app uses PubNub's public demo keys:
- **Username**: demo
- **Password**: demo
- **Channel**: avalonia-chat-demo

These are free for testing and demonstration purposes.

### Message Flow

1. **User types message** → ViewModel captures input
2. **Send button clicked** → ViewModel calls PubNubChatService
3. **Service publishes** → Message sent to PubNub cloud
4. **PubNub broadcasts** → All subscribers receive message
5. **Callback fires** → MessageReceived event raised
6. **UI updates** → Message added to ObservableCollection

### Presence Flow

1. **User connects** → Subscribe with presence enabled
2. **PubNub tracks** → User added to channel presence
3. **HereNow called** → List of online users retrieved
4. **UI updates** → Online users list populated
5. **User disconnects** → Presence updated automatically

## Avalonia UI Highlights

### XAML Features Used

- **Data binding** - Two-way binding for input, one-way for display
- **ItemsControl** - Virtualized message list
- **Styles** - Reusable styles for message bubbles
- **Value converters** - (Simplified in this version)
- **Commands** - RelayCommand for user actions

### Design Patterns

- **MVVM** - Complete separation of concerns
- **Observable properties** - CommunityToolkit.Mvvm source generators
- **Commands** - ICommand pattern for user actions
- **Events** - Service events for real-time updates
- **Async/await** - Non-blocking UI operations

### UI Thread Management

```csharp
Dispatcher.UIThread.InvokeAsync(() =>
{
    Messages.Add(message);
});
```

Ensures UI updates happen on the correct thread when receiving real-time messages.

## Extending the Demo

The demo can be easily extended with:

### Additional Features
- **Typing indicators** - Show when someone is typing
- **File uploads** - Share images and documents
- **Private messaging** - Direct message between users
- **Message reactions** - Emoji reactions to messages
- **User profiles** - Avatars, status, bio
- **Multiple rooms** - Switch between different channels
- **Message editing** - Edit sent messages
- **Message deletion** - Remove messages
- **Search** - Find messages in history
- **Notifications** - Desktop notifications for new messages

### Production Considerations
- **Authentication** - Integrate proper user auth
- **Custom PubNub keys** - Use your own production keys
- **Error handling** - More robust error recovery
- **Offline support** - Queue messages when offline
- **Message persistence** - Local database for offline access
- **Rate limiting** - Prevent message spam
- **Content filtering** - Moderate inappropriate content
- **Analytics** - Track usage metrics

## Learning Resources

### Avalonia UI
- [Official Docs](https://docs.avaloniaui.net/)
- [Samples](https://github.com/AvaloniaUI/Avalonia.Samples)
- [Community](https://github.com/AvaloniaCommunity)

### PubNub
- [PubNub Docs](https://www.pubnub.com/docs/)
- [C# SDK Docs](https://www.pubnub.com/docs/sdks/c-sharp)
- [Chat SDK](https://www.pubnub.com/docs/chat/overview)

### MVVM
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [MVVM Pattern](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm)

## Use Cases

This demo architecture is applicable to:

- **Team chat applications**
- **Customer support chat**
- **Live collaboration tools**
- **Gaming chat systems**
- **IoT device monitoring dashboards**
- **Real-time notifications**
- **Live data visualization**
- **Multiplayer game lobbies**

## Conclusion

This PubNub chat demo showcases how to build a modern, real-time desktop application using:

✅ Cross-platform UI (Avalonia)
✅ Cloud-based messaging (PubNub)
✅ Modern .NET (9.0)
✅ Clean architecture (MVVM)
✅ Professional design (Fluent)

The code is well-structured, documented, and ready to be extended for production use cases.

---

**Questions or Issues?**

See the [detailed README](src/PubNubChatDemo/README.md) or open an issue on GitHub.
