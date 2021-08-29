module App

open Elmish
open Elmish.React
open Feliz

type Page = 
    | Counter
    | TextInput

type State =
    { Count: int
      Text: string
      IsUpperCase: bool
      Page: Page }

type Event =
    | Increment
    | Decrement
    | InputTextChanged of string
    | UpperCaseToggled of bool
    | PageChanged of Page

let init () =
    { Count = 0
      Text = "Snog in the Shrubbery"
      IsUpperCase = false
      Page = Counter }, Cmd.none

let update event state =
    match event with
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none
    | InputTextChanged text -> { state with Text = text }, Cmd.none
    | UpperCaseToggled isUpper -> { state with IsUpperCase = isUpper }, Cmd.none
    | PageChanged page -> { state with Page = page }, Cmd.none

let counterPage state dispatch =
    Html.div [
        Html.button [
            prop.text "Show text page"
            prop.onClick (fun _ -> dispatch (PageChanged TextInput))
        ]
        Html.hr []
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
        Html.button [
            prop.text "Show counter page"
            prop.onClick (fun _ -> dispatch (PageChanged Counter))
        ]
        Html.hr []
        Html.input [
            prop.valueOrDefault state.Text
            prop.onChange (InputTextChanged >> dispatch)
        ]
        Html.input [
            prop.id "uppercase-toggle"
            prop.type'.checkbox
            prop.isChecked state.IsUpperCase
            prop.onChange (UpperCaseToggled >> dispatch)
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
    match state.Page with
    | Counter -> counterPage state dispatch
    | TextInput -> textPage state dispatch

Program.mkProgram init update render
|> Program.withReactSynchronous "app"
|> Program.run