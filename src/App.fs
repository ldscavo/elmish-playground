module App

open Elmish
open Elmish.React
open Feliz

type Page = 
    | Counter
    | TextInput

type State =
    { CounterState: Counter.State
      TextInputState: TextInput.State
      Page: Page }

type Event =
    | CounterEvent of Counter.Event
    | TextEvent of TextInput.Event
    | SwitchPage of Page

let init () =
    { CounterState = Counter.init ()
      TextInputState = TextInput.init ()
      Page = Counter }, Cmd.none

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

let render state dispatch =
    let counterDispatch event =
        dispatch (CounterEvent event)

    let textDispatch event =
        dispatch (TextEvent event)

    Html.div [
        Html.button [
            prop.text "Show counter page"
            prop.onClick (fun _ -> dispatch (SwitchPage Counter))
        ]
        Html.button [
            prop.text "Show text page"
            prop.onClick (fun _ -> dispatch (SwitchPage TextInput))
        ]
        Html.hr []
        match state.Page with
        | Counter -> Counter.render state.CounterState counterDispatch
        | TextInput -> TextInput.render state.TextInputState textDispatch
    ]
    

Program.mkProgram init update render
|> Program.withReactSynchronous "app"
|> Program.run