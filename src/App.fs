module App

open Elmish
open Elmish.React
open Feliz

type Page = 
    | Counter
    | TextInput

type CounterState =
    { Count: int }

type TextState =
    { Text: string
      IsUpperCase: bool }

type State =
    { CounterState: CounterState
      TextState: TextState
      Page: Page }

type CounterEvent = 
    | Increment
    | Decrement

type TextEvent =
    | ChangeText of string
    | ToggleUpperCase of bool

type Event =
    | CounterEvent of CounterEvent
    | TextEvent of TextEvent
    | PageChanged of Page

let initCounter () =
    { Count = 0 }

let initText () =
    { Text = "Snog in the Shrubbery!"
      IsUpperCase = false }

let init () =
    { CounterState = initCounter ()
      TextState = initText ()
      Page = Counter }

let updateCounter event state =
    match event with
    | Increment -> { state with Count = state.Count + 1 }
    | Decrement -> { state with Count = state.Count - 1 }

let updateText event state =
    match event with
    | ChangeText text -> { state with Text = text }
    | ToggleUpperCase isUpper -> { state with IsUpperCase = isUpper }

let update event state =
    match event with
    | PageChanged page ->
        { state with Page = page }
    | CounterEvent evnt ->
        { state with CounterState = updateCounter evnt state.CounterState }
    | TextEvent evnt ->
        { state with TextState = updateText evnt state.TextState }

let counterPage state dispatch =
    Html.div [
        Html.button [
            prop.text "-"
            prop.onClick (fun _ -> dispatch Decrement)
        ]
        Html.h1 state.Count
        Html.button [
            prop.text "+"
            prop.onClick (fun _ -> dispatch Increment)
        ]
    ]

let textPage state dispatch =
    Html.div [        
        Html.input [
            prop.valueOrDefault state.Text
            prop.onChange (ChangeText >> dispatch)
        ]
        Html.input [
            prop.id "uppercase-toggle"
            prop.type'.checkbox
            prop.isChecked state.IsUpperCase
            prop.onChange (ToggleUpperCase >> dispatch)
        ]
        Html.label [
            prop.htmlFor "uppercase-toggle"
            prop.text "Uppercase?"
        ]
        Html.h1
            (if state.IsUpperCase
                then state.Text.ToUpper()
                else state.Text)
    ]

let render state dispatch =
    let counterDispatch event =
        dispatch (Event.CounterEvent event)

    let textDispatch event =
        dispatch (Event.TextEvent event)

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
        | Counter -> counterPage state.CounterState counterDispatch
        | TextInput -> textPage state.TextState textDispatch
    ]
    

Program.mkSimple init update render
|> Program.withReactSynchronous "app"
|> Program.run