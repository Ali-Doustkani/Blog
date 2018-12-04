(function () {
    var table = document.getElementsByTagName("table")[0];

    document.addEventListener("DOMContentLoaded", function () {
        table.addEventListener("click", function (event) {
            if (event.target.id === "delete") {
                deletePost(event.target.dataset.id);
            }
        });
    });

    function deletePost(postId) {
        var msgBox = new tingle.modal({
            closeMethods: [],
            footer: true,
            stickyFooter: true
        });
        msgBox.setContent("<h2>Warning!</h2><p>Are you sure you want to delete this post?</p>");
        msgBox.addFooterBtn("Yes", "tingle-btn tingle-btn--danger tingle-btn--pull-right", function () {
            msgBox.close();
            var form = document.getElementById("deleteForm");
            form.action = "/admin/DeletePost/" + postId;
            form.submit();
        });
        msgBox.addFooterBtn("No", "tingle-btn tingle-btn--pull-right", function () {
            msgBox.close();
        });
        msgBox.open();
    }
})();

