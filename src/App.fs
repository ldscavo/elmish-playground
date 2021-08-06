module App

open Elmish
open Elmish.React
open Feliz

type State =
    { Count: int }

type Event =
    | Increment
    | Decrement

let init () =
    { Count = 0 }

let update event state =
    match event with
    | Increment -> { state with Count = state.Count + 1 }
    | Decrement -> { state with Count = state.Count - 1 }

let render state dispatch =
    Html.div [
        Html.button [
            prop.onClick (fun _ -> dispatch Increment)
            prop.text "Increment"
        ]

        Html.div state.Count

        Html.button [
            prop.onClick (fun _ -> dispatch Decrement)
            prop.text "Decrement"
        ]
        
        match state.Count with
        | 0 -> Html.none
        | _ -> Html.h1 (if state.Count % 2 = 0 then "Count is even" else "Count is odd")
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "app"
|> Program.run