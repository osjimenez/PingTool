module ViewModelBase

open System
open System.Windows
open System.Windows.Input
open System.ComponentModel

type FuncCommand (canExec:(obj -> bool),doExec:(obj -> unit)) =
    let cecEvent = new DelegateEvent<EventHandler>()
    interface ICommand with
        [<CLIEvent>]
        member x.CanExecuteChanged = cecEvent.Publish
        member x.CanExecute arg = canExec(arg)
        member x.Execute arg = doExec(arg)
