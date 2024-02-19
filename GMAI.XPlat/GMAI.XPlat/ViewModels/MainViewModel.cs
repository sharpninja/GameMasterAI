using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.SemanticKernel;

namespace GMAI.XPlat.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private int _selectedIndex = 0;

    public List<string> Classes => ["fighter", "wizard", "bard"];
    
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }
    
    [RelayCommand]
    private async Task<string> SetSummaryAsync()
    {
        var plugin = App.Dice!["Summarize"];
        var args = new KernelArguments
        {
            ["profession"] = Prompt
        };

        try
        {
            Summary = await App.Kernel.InvokeAsync<string>(
                plugin, args) ?? "";

            History.Add(new(Prompt, Summary));
        }
        catch (Exception ex)
        {
            History.Add(new(Prompt, ex.ToString()));
        }

        return Summary;
    }    
    
    [RelayCommand]
    private async Task<string> RollD4Async()
    {
        int count = 5;
        Summary = await App.Kernel.InvokeAsync<string>(
            App.Dice!["D4"],
            new KernelArguments
            {
                ["count"]=count,
            }) ?? "<<null>>";

        History.Add(new($"Roll a d4 {count} time and sum results", Summary));
        
        return Summary;
    }
    
    [RelayCommand]
    private async Task<string> RollStatAsync()
    {
        Summary = await App.Kernel.InvokeAsync<string>(
            App.Dice!["CharacterStats"],
            new KernelArguments
            {
                ["profession"]=Prompt,
            }) ?? "<<null>>";

        History.Add(new($"roll a {Prompt}", Summary));
        
        return Summary;
    }

    public string Prompt => Classes[SelectedIndex];
    
    [ObservableProperty] 
    private string _summary = "";

    [ObservableProperty] 
    private ObservableCollection<HistoryItem> _history = new();
}

public record HistoryItem(string Prompt, string Response);