import { Union, Record } from "./.fable/fable-library.3.2.9/Types.js";
import { union_type, record_type, int32_type } from "./.fable/fable-library.3.2.9/Reflection.js";
import { createElement } from "react";
import { ofArray } from "./.fable/fable-library.3.2.9/List.js";
import { Interop_reactApi } from "./.fable/Feliz.1.49.0/Interop.fs.js";
import { ProgramModule_mkSimple, ProgramModule_run } from "./.fable/Fable.Elmish.3.0.0/program.fs.js";
import { Program_withReactSynchronous } from "./.fable/Fable.Elmish.React.3.0.1/react.fs.js";

export class State extends Record {
    constructor(Count) {
        super();
        this.Count = (Count | 0);
    }
}

export function State$reflection() {
    return record_type("App.State", [], State, () => [["Count", int32_type]]);
}

export class Event$ extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Increment", "Decrement"];
    }
}

export function Event$$reflection() {
    return union_type("App.Event", [], Event$, () => [[], []]);
}

export function init() {
    return new State(0);
}

export function update(msg, state) {
    if (msg.tag === 1) {
        return new State(state.Count - 1);
    }
    else {
        return new State(state.Count + 1);
    }
}

export function render(state, dispatch) {
    const children = ofArray([createElement("button", {
        onClick: (_arg1) => {
            dispatch(new Event$(0));
        },
        children: "Increment",
    }), createElement("button", {
        onClick: (_arg2) => {
            dispatch(new Event$(1));
        },
        children: "Decrement",
    }), createElement("h1", {
        children: [state.Count],
    })]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

ProgramModule_run(Program_withReactSynchronous("app", ProgramModule_mkSimple(init, (msg, state) => update(msg, state), (state_1, dispatch) => render(state_1, dispatch))));

