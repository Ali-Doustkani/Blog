document.querySelectorAll(".delete-button").forEach(btn => btn.addEventListener("click", ask));
function ask(e) {
    if (!confirm("Are you sure you want to delete this post?")) {
        e.preventDefault();
    }
}
