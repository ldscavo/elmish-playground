[<RequireQualifiedAccess>]
module TextInput

open Feliz

type State =
    { Text: string
      IsUpperCase: bool }

type Event =
    | ChangeText of string
    | ToggleUpperCase of bool

let init () =
    { Text = "Snog in the Shrubbery!"
      IsUpperCase = false }
  
let update event state =
    match event with
    | ChangeText text -> { state with Text = text }
    | ToggleUpperCase isUpper -> { state with IsUpperCase = isUpper }

let render state dispatch =
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