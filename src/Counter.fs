module Counter

open Feliz
open Elmish

type State =
    { Count: int }

type Event = 
    | Increment
    | Decrement
    | IncrementDelayed

let init () =
    { Count = 0 }

let update event state =
    match event with
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none
    | IncrementDelayed ->
        let delayedIncrement = async {
            do! Async.Sleep 1000
            return Increment
        }
        state, Cmd.OfAsync.result delayedIncrement 

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
        Html.button [
            prop.text "+ (slow)"
            prop.onClick (fun _ -> dispatch IncrementDelayed)
        ]
    ]