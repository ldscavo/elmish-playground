module App

open Elmish
open Elmish.React
open Feliz

type State =
    { Count: int
      Loading: bool }

type Event =
    | Increment
    | Decrement
    | IncrementDelayed

let init () =
    { Count = 0
      Loading = false }, Cmd.none

let update event state =
    match event with
    | Increment ->
        { state with
            Loading = false
            Count = state.Count + 1 }, Cmd.none
    | Decrement ->
        { state with
            Loading = false
            Count = state.Count - 1 }, Cmd.none
    | IncrementDelayed when state.Loading -> state, Cmd.none
    | IncrementDelayed ->
        let cmd dispatch = 
            let delayedDispatch = async {
                do! Async.Sleep 750
                dispatch Increment
            }
            Async.StartImmediate delayedDispatch
            
        { state with Loading = true }, Cmd.ofSub cmd

let render state dispatch =
    Html.div [
        if (not state.Loading)
            then Html.h1 state.Count
            else Html.h1 "LOADING"
        Html.button [
            prop.disabled state.Loading
            prop.onClick (fun _ -> dispatch Decrement)
            prop.text "Decrement"
        ]
        Html.button [
            prop.disabled state.Loading
            prop.onClick (fun _ -> dispatch Increment)
            prop.text "Increment"
        ]
        Html.button [
            prop.disabled state.Loading
            prop.onClick (fun _ -> dispatch IncrementDelayed)
            prop.text "Increment, but slowly"
        ]
    ]

Program.mkProgram init update render
|> Program.withReactSynchronous "app"
|> Program.run