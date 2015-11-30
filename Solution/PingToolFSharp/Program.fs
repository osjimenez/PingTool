module Program
open System
open System.Windows
open System.Windows.Controls
open System.Windows.Markup
open ViewModels
// Create the View and bind it to the View Model
let myView = Application.LoadComponent(new System.Uri("/PingToolFSharp;component/MainWindow.xaml", System.UriKind.Relative)) :?> Window
myView.DataContext <- new MainViewModel() :> obj

// Application Entry point
[<STAThread>]
[<EntryPoint>]
let main(_) = (new Application()).Run(myView)
