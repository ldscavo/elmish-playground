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
    { Count = 0 }, Cmd.none

let update event state =
    match event with
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none

let render state dispatch =
    Html.div [
        Html.h1 state.Count
        Html.button [
            prop.onClick (fun _ -> dispatch Decrement)
            prop.text "Decrement"
        ]
        Html.button [
            prop.onClick (fun _ -> dispatch Increment)
            prop.text "Increment"
        ]
    ]

Program.mkProgram init update render
|> Program.withReactSynchronous "app"
|> Program.run