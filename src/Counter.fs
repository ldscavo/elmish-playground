module Counter

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
            prop.text "-"
            prop.onClick (fun _ -> dispatch Decrement)
        ]
        Html.h1 state.Count
        Html.button [
            prop.text "+"
            prop.onClick (fun _ -> dispatch Increment)
        ]
    ]