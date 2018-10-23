(function () {
    var table = document.getElementsByTagName("table")[0];

    document.addEventListener("DOMContentLoaded", function () {
        table.addEventListener("click", function (event) {
            if (event.target.id === "delete") {
                deletePost(event.target.parentNode.parentNode);
            } else if (event.target.id === "edit") {
                editPost(event.target.parentNode.parentNode);
            }
        });
    });

    function deletePost(row) {
        var msgBox = new tingle.modal({
            closeMethods: [],
            footer: true,
            stickyFooter: true
        });
        msgBox.setContent("<h2>Warning!</h2><p>Are you sure you want to delete this post?</p>");
        msgBox.addFooterBtn("Yes", "tingle-btn tingle-btn--danger tingle-btn--pull-right", function () {
            msgBox.close();
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    table.deleteRow(row.rowIndex);
                } else if (this.readyState === 4 && this.status !== 200) {
                    alert("ERROR TO DELETE");
                }
            };
            xhr.open("DELETE", "/admin/post/delete/" + row.dataset.id);
            xhr.send();
        });
        msgBox.addFooterBtn("No", "tingle-btn tingle-btn--pull-right", function () {
            msgBox.close();
        });
        msgBox.open();
    }

    function editPost(row) {
        document.location.href = "/admin/post/" + row.dataset.id;
    }
})();

