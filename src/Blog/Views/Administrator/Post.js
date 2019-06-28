import { create } from '@alidoustkani/richtext';

const saveForm = document.getElementById("save-post");
const editor = document.getElementById("editor");
saveForm.addEventListener("submit", () => {
    console.log('start');
    const input = document.createElement("input");
    input.type = "hidden";
    input.name = "MarkedContent";
    input.value = editor.innerHTML;
    saveForm.appendChild(input);
});

const richtext = create(editor, {
    defaultLink: "/",
    staySelected: false,
    decors: {
        important: 'strong',
        h1: {
            parent: true,
            tag: "h3"
        },
        h2: {
            parent: true,
            tag: "h4"
        },
        code: {
            parent: true,
            tag: "pre",
            className: "code"
        },
        terminal: {
            parent: true,
            tag: "pre",
            className: "terminal"
        },
        note: {
            parent: true,
            tag: "div",
            className: "note"
        },
        warning: {
            parent: true,
            tag: "div",
            className: "warning"
        }
    }
});

const click = (id, func) => document.getElementById(id).addEventListener("click", e => {
    e.preventDefault();
    func();
});

click("important", () => richtext.style("important"));
click("h1", () => richtext.apply("h1"));
click("h2", () => richtext.apply("h2"));
click("code", () => richtext.apply("code"));
click("terminal", () => richtext.apply("terminal"));
click("note", () => richtext.apply("note"));
click("warning", () => richtext.apply("note"));
click("unorderedList", () => richtext.applyUnorderedList());
click("orderedList", () => richtext.applyOrderedList());
click("image", () => richtext.selectImage());
click("link", () => richtext.styleLink());