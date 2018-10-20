var gulp = require("gulp");
var sass = require("gulp-sass");
var rename = require("gulp-rename");

gulp.task("sass", function () {
    return sassTask({ outputStyle: "expanded" });
});

gulp.task("build", function () {
    return sassTask({ outputStyle: "compressed" });
});

function sassTask(options) {
    return gulp.src("./Styles/Site.scss")
        .pipe(sass(options))
        .pipe(rename("~site.css"))
        .pipe(gulp.dest("./wwwroot/css"));
}