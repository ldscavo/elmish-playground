module App

open Elmish
open Elmish.React
open Feliz

type Page = 
    | Counter
    | TextInput

type State =
    { CounterState: Counter.State
      TextState: TextInput.State
      Page: Page }

type Event =
    | CounterEvent of Counter.Event
    | TextEvent of TextInput.Event
    | PageChanged of Page

let init () =
    { CounterState = Counter.init ()
      TextState = TextInput.init ()
      Page = Counter }

let update event state =
    match event with
    | PageChanged page ->
        { state with Page = page }
    | CounterEvent evnt ->
        { state with
            CounterState = Counter.update evnt state.CounterState }
    | TextEvent evnt ->
        { state with
            TextState = TextInput.update evnt state.TextState }

let render state dispatch =
    let counterDispatch event =
        dispatch (CounterEvent event)

    let textDispatch event =
        dispatch (TextEvent event)

    Html.div [
        Html.button [
            prop.text "Show counter page"
            prop.onClick (fun _ -> dispatch (PageChanged Counter))
        ]
        Html.button [
            prop.text "Show text page"
            prop.onClick (fun _ -> dispatch (PageChanged TextInput))
        ]
        Html.hr []
        match state.Page with
        | Counter -> Counter.render state.CounterState counterDispatch
        | TextInput -> TextInput.render state.TextState textDispatch
    ]
    

Program.mkSimple init update render
|> Program.withReactSynchronous "app"
|> Program.run