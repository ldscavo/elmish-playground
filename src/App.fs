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

let update msg state =
    match msg with
    | Increment -> { state with Count = state.Count + 1 }
    | Decrement -> { state with Count = state.Count - 1 }

let render state dispatch =
    Html.div [
        Html.button [
            prop.onClick (fun _ -> dispatch Increment)
            prop.text "Increment"
        ]
        Html.button [
            prop.onClick (fun _ -> dispatch Decrement)
            prop.text "Decrement"
        ]
        Html.h1 state.Count
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "app"
|> Program.run