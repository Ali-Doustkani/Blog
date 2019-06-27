import { create } from '@alidoustkani/richtext';

document.querySelectorAll(".delete-button").forEach(btn => btn.addEventListener("click", ask));
function ask(e) {
    if (!confirm("Are you sure you want to delete this post?")) {
        e.preventDefault();
    }
}

const editor = document.getElementById("editor");
console.log(editor);
if (editor) {
    create(editor, {
        defaultLink: "/",
        staySelected: true,
        decors: {
            important: 'strong',
            box: {
                parent: true,
                tag: 'div',
                className: 'box-wrapper'
            },
            note: {
                tag: 'span',
                className: 'note'
            },
            warning: {
                tag: 'span',
                className: 'warning'
            }
        }
    });
}