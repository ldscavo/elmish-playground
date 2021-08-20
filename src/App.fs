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
    | DecrementDelayed

let init () =
    { Count = 0
      Loading = false }, Cmd.none

let delayedEvent (delay: int) event =
    let cmd dispatch = 
        let delayedDispatch = async {
            do! Async.Sleep delay
            dispatch event
        }

        Async.StartImmediate delayedDispatch

    Cmd.ofSub cmd

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
    | IncrementDelayed when state.Loading ->
        state, Cmd.none
    | IncrementDelayed ->                   
        { state with Loading = true }, delayedEvent 500 Increment
    | DecrementDelayed when state.Loading ->
        state, Cmd.none
    | DecrementDelayed ->
        { state with Loading = true }, delayedEvent 2500 Decrement

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
        Html.button [
            prop.disabled state.Loading
            prop.onClick (fun _ -> dispatch DecrementDelayed)
            prop.text "Decrement even S L O W E R"
        ]
    ]

Program.mkProgram init update render
|> Program.withReactSynchronous "app"
|> Program.run