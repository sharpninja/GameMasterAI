using Avalonia.Controls;
using Avalonia.Interactivity;
using GMAI.XPlat.ViewModels;

namespace GMAI.XPlat.Views;

public partial class MainView : UserControl
{
    protected MainViewModel? ViewModel 
        => DataContext as MainViewModel;
    
    public MainView()
    {
        InitializeComponent();

        async void OnLoadedLocal(object? sender, RoutedEventArgs args) 
            => await ViewModel!.InitializeAsync();

        Loaded += OnLoadedLocal;
    }
}