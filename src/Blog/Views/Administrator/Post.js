import { create } from '@alidoustkani/richtext';

const saveForm = document.getElementById("save-post");
const english = document.getElementById("english");
const farsi = document.getElementById("farsi");
const editor = document.getElementById("editor");
const summary = document.getElementById("Summary");
const content = document.getElementById("Content");

english.addEventListener("change", () => saveForm.classList.replace("farsi", "english"));
farsi.addEventListener("change", () => saveForm.classList.replace("english", "farsi"));

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

richtext.setInnerHTML(content.value);

saveForm.addEventListener("submit", () => {
    content.value = editor.innerHTML;
    saveForm.appendChild(content);
});

summary.addEventListener("keydown", e => {
    if (e.key === "Tab") {
        console.log(richtext.focus);
        e.preventDefault();
        richtext.focus();
    }
});

click("important", () => richtext.style("important"));
click("h1", () => richtext.apply("h1"));
click("h2", () => richtext.apply("h2"));
click("code", () => richtext.apply("code"));
click("terminal", () => richtext.apply("terminal"));
click("note", () => richtext.apply("note"));
click("warning", () => richtext.apply("warning"));
click("unorderedList", () => richtext.applyUnorderedList());
click("orderedList", () => richtext.applyOrderedList());
click("image", () => richtext.selectImage());
click("link", () => richtext.styleLink());

function click(id, func) {
    document.getElementById(id).addEventListener("click", e => {
        e.preventDefault();
        func();
    });
}