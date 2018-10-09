var gulp = require("gulp");
var sass = require("gulp-sass");
var rename = require("gulp-rename");

gulp.task("sass", function () {
    var options = {
        outputStyle: "expanded"
    };
    return gulp.src("./Styles/Site.scss")
        .pipe(sass(options))
        .pipe(rename("~site.css"))
        .pipe(gulp.dest("./wwwroot/css"));
});