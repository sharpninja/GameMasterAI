using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GMAI.XPlat.ViewModels;
using GMAI.XPlat.Views;
using Microsoft.SemanticKernel;

namespace GMAI.XPlat;

public partial class App : Application
{
    private static Kernel? _kernel;

    public static Kernel Kernel => _kernel!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var builder = Kernel.CreateBuilder();
        
        
        string modelName = "gpt-3.5-turbo";
        string apiKey = "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";

        builder.AddOpenAIChatCompletion(modelName, apiKey);

        _kernel = builder.Build();
        
        //Skills = _kernel.ImportPluginFromPromptDirectory("plugins", "SummarizationPlugin");
        // FunPlugin directory path
        var funPluginDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "plugins", "SummarizationPlugin");
        var dicePluginDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "plugins", "DicePlugin");

        // Load the FunPlugin from the Plugins Directory
        if (!Path.Exists(funPluginDirectoryPath)) throw new DirectoryNotFoundException(funPluginDirectoryPath);
        
        Skills = _kernel.ImportPluginFromPromptDirectory(funPluginDirectoryPath);
        Dice = _kernel.ImportPluginFromPromptDirectory(dicePluginDirectoryPath);
    }

    public static KernelPlugin? Dice { get; set; }

    public static KernelPlugin? Skills { get; set; }

    public override void OnFrameworkInitializationCompleted()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel()
                };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }
}