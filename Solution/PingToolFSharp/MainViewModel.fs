module ViewModels

open System
open System.Windows
open System.Windows.Input
open System.ComponentModel
open ViewModelBase

type MainViewModel() =          
    // private variables
    let mutable _title = "Bound Data to Textbox"    

    // public properties
    member x.Title 
        with get() = 
                _title 
        and set(v) = 
                _title <- v

    // public commands
    member x.MyCommand = 
        new FuncCommand
            (
                (fun d -> true), 
                (fun e -> x.ShowMessage) 
            )

    // public methods
    member public x.ShowMessage = 
        let msg = MessageBox.Show(x.Title)
        ()    

