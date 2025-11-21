using Avalonia.Controls;
using PubNubChatDemo.ViewModels;

namespace PubNubChatDemo.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.Disconnect();
        }
        base.OnClosing(e);
    }
}
