using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PubNubChatDemo.Models;
using PubNubChatDemo.Services;
using Avalonia.Threading;

namespace PubNubChatDemo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly PubNubChatService _chatService;

    [ObservableProperty]
    private string _currentMessage = string.Empty;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private bool _isConnected = false;

    [ObservableProperty]
    private string _statusMessage = "Not connected";

    public ObservableCollection<ChatMessage> Messages { get; } = new();
    public ObservableCollection<UserPresence> OnlineUsers { get; } = new();

    public MainViewModel()
    {
        // Generate a random username for demo
        Username = $"User{new Random().Next(1000, 9999)}";
        _chatService = new PubNubChatService(Username);

        // Subscribe to events
        _chatService.MessageReceived += OnMessageReceived;
        _chatService.PresenceUpdated += OnPresenceUpdated;

        // Auto-connect
        _ = ConnectAsync();
    }

    [RelayCommand]
    private async Task ConnectAsync()
    {
        try
        {
            StatusMessage = "Connecting...";
            await _chatService.ConnectAsync();

            // Load message history
            var history = await _chatService.GetHistoryAsync(50);
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Messages.Clear();
                foreach (var msg in history)
                {
                    Messages.Add(msg);
                }
            });

            IsConnected = true;
            StatusMessage = $"Connected as {Username}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Connection failed: {ex.Message}";
        }
    }

    [RelayCommand(CanExecute = nameof(CanSendMessage))]
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentMessage))
            return;

        try
        {
            await _chatService.SendMessageAsync(CurrentMessage);
            CurrentMessage = string.Empty;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Send failed: {ex.Message}";
        }
    }

    private bool CanSendMessage()
    {
        return IsConnected && !string.IsNullOrWhiteSpace(CurrentMessage);
    }

    partial void OnCurrentMessageChanged(string value)
    {
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    partial void OnIsConnectedChanged(bool value)
    {
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    private void OnMessageReceived(object? sender, ChatMessage message)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Messages.Add(message);
        });
    }

    private void OnPresenceUpdated(object? sender, System.Collections.Generic.List<UserPresence> users)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            OnlineUsers.Clear();
            foreach (var user in users)
            {
                OnlineUsers.Add(user);
            }
        });
    }

    public void Disconnect()
    {
        _chatService.Disconnect();
        IsConnected = false;
        StatusMessage = "Disconnected";
    }
}
