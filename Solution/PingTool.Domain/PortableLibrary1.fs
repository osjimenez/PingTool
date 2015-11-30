namespace PingTool.Domain
open System
open System.Windows
open System.Windows.Input
open System.ComponentModel

type ShowCommand() =
 
  // This is the member field for the event
  let CanExecuteChangedEvent = new Event<_,_>()
 
  interface ICommand with
    member self.Execute (argument:obj) = ()
    member self.CanExecute (argument:obj) = true
    [<CLIEvent>]
    member self.CanExecuteChanged = 
        CanExecuteChangedEvent.Publish



type FuncCommand (canExec:(obj -> bool),doExec:(obj -> unit)) =
    let cecEvent = new DelegateEvent<EventHandler>()
    interface ICommand with
        [<CLIEvent>]
        member x.CanExecuteChanged = cecEvent.Publish
        member x.CanExecute arg = canExec(arg)
        member x.Execute arg = doExec(arg)






type PingToolViewModel() = 
    member this.X = "F#"
    member val TargetUri = "google.com" with get, set
    member val Timeout = 500 with get, set // Timeout for each request in milliseconds
    member val Interval = 1000 with get, set // Interval between requests in milliseconds
    member val ErrorLimit = 2 with get, set // Number of consecutive errors to consider connection broken
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