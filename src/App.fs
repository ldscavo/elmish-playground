module App

open Elmish
open Elmish.React
open Feliz
open Feliz.Router

type Page = 
    | Counter
    | TextInput

type State =
    { CounterState: Counter.State
      TextInputState: TextInput.State
      Page: Page
      CurrentUrl: string list }

type Event =
    | CounterEvent of Counter.Event
    | TextEvent of TextInput.Event
    | SwitchPage of Page
    | UrlChanged of string list
    | NavigateToCounter
    | NavigateToTextInput

let init () =
    { CounterState = Counter.init ()
      TextInputState = TextInput.init ()
      Page = Counter
      CurrentUrl = Router.currentUrl() }, Cmd.none

let update event state =
    match event with
    | SwitchPage page ->
        { state with Page = page }, Cmd.none

    | CounterEvent evnt ->
        let (counter, cmd) = Counter.update evnt state.CounterState
        { state with
            CounterState = counter }, Cmd.map CounterEvent cmd

    | TextEvent evnt ->
        { state with
            TextInputState = TextInput.update evnt state.TextInputState }, Cmd.none

    | UrlChanged url ->
        { state with CurrentUrl = url }, Router.Cmd.navigate (List.toArray url)

    | NavigateToCounter ->
        state, Router.Cmd.navigate "counter"

    | NavigateToTextInput ->
        state, Router.Cmd.navigate "text-input"

let render state dispatch =
    Html.div [
        Html.a [
            prop.text "Counter"
            prop.onClick (fun _ -> dispatch NavigateToCounter)
            prop.style [ style.margin 10 ]
        ]
        Html.a [
            prop.text "Text Input"
            prop.onClick (fun _ -> dispatch NavigateToTextInput)
            prop.style [ style.margin 10 ]
        ]
        Html.hr []        
        React.router [            
            router.onUrlChanged (UrlChanged >> dispatch)
            router.children [ 
                match state.CurrentUrl with
                | [] -> Html.h1 "Hello World!"
                | [ "counter" ] -> Counter.render state.CounterState (CounterEvent >> dispatch)
                | [ "text-input" ] -> TextInput.render state.TextInputState (TextEvent >> dispatch)
                | _ -> Html.h1 404
            ]
        ]
    ]
    

Program.mkProgram init update render
|> Program.withReactSynchronous "app"
|> Program.run