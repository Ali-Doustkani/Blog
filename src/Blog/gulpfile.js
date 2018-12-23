var gulp = require("gulp");
var sass = require("gulp-sass");
var rename = require("gulp-rename");
var dist = require("gulp-npm-dist");

gulp.task("dep", function () {
    return gulp
        .src(dist(), { base: "./node_modules" })
        .pipe(rename(function (path) {
            path.dirname = path.dirname.replace(/\/dist/, "").replace(/\\dist/, "");
        }))
        .pipe(gulp.dest("./wwwroot/libs"));
});

gulp.task("sass", function () {
    return sassTask({ outputStyle: "expanded" });
});

gulp.task("build", function () {
    return sassTask({ outputStyle: "compressed" });
});

function sassTask(options) {
    return gulp.src("./Styles/Site*.scss")
        .pipe(sass(options))
        .pipe(
            rename(path => {
                path.basename = "~" + path.basename.toLowerCase();
                path.extname = ".css";
            }))
        .pipe(gulp.dest("./wwwroot/css"));
}