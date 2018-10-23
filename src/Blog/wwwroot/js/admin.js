document.addEventListener("DOMContentLoaded", function () {
    var table = document.getElementsByTagName("table")[0];
    table.addEventListener("click", function (event) {
        if (event.target.classList.contains("delete")) {
            var msgBox = new tingle.modal({
                closeMethods: [],
                footer: true,
                stickyFooter: true
            });
            msgBox.setContent("<h2>Warning!</h2><p>Are you sure you want to delete this post?</p>");
            msgBox.addFooterBtn("Yes", "tingle-btn tingle-btn--danger tingle-btn--pull-right", function () {
                msgBox.close();
                var id = event.target.dataset.id;
                var xhr = new XMLHttpRequest();
                xhr.onreadystatechange = function () {
                    if (this.readyState === 4 && this.status === 200) {
                        var rowIndex = event.target.parentNode.parentNode.rowIndex;
                        table.deleteRow(rowIndex);
                    } else if (this.readyState === 4 && this.status !== 200) {
                        alert("ERROR TO DELETE");
                    }
                };
                xhr.open("DELETE", "/admin/post/delete/" + id);
                xhr.send();
            });
            msgBox.addFooterBtn("No", "tingle-btn tingle-btn--pull-right", function () {
                msgBox.close();
            });

            msgBox.open();
        }
    });
});