'use strict';
// # Gulp #
const gulp = require('gulp');
const sass = require("gulp-sass");
const sourcemaps = require("gulp-sourcemaps");
const rename = require("gulp-rename");
const concat = require("gulp-concat");
const babel = require("babelify");
const browserify = require("browserify");
const prefix = require("gulp-autoprefixer");
const browserSync = require("browser-sync").create();
const source = require("vinyl-source-stream");
const browserReload = browserSync.reload();
const browserStream = browserSync.stream();
 
// # App #
const devURL = "localhost:54343/"; // Change to URL including port by ASP.net
const path = {  // Enviroment Paths
    stylesDev: "./Content/_scss/",
    stylesProd: "./Content/",
    scriptsDev: "./Scripts/_dev/",
    scriptsProd: "./Scripts/",
    views: "./Views/"
};
const prefixBrowsers = [ // Browsers to include css Prefix
    'last 2 version',
    'safari 5',
    'ie 8',
    'ie 9',
    'opera 12.1',
    'ios 6',
    'android 4'
];

// # Tasks #
gulp.task('styles:sass', () => {
    return gulp.src(`${path.stylesDev}app.scss`)
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(rename("styles.css"))
        .pipe(prefix(prefixBrowsers))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(`${path.stylesProd}`))
        .pipe(browserSync.stream());
});

gulp.task('scripts:build', () => {
    return browserify({ entries: "./Scripts/_dev/app.js", debug: true })
        .bundle()
        .pipe(source("app.js"))
        .pipe(gulp.dest(`${path.scriptsProd}`))
        .pipe(browserSync.stream());
});

gulp.task('serve', ["styles:sass","scripts:build"], () => {
    var files = [
        `${path.views}**/*.*`,
        `${path.stylesDev}**/*.scss`
    ];
    browserSync.init(files, {
        proxy: devURL
    });
    gulp.watch(`${path.scriptsDev}**/*.js`, ['scripts:build']);
    gulp.watch(`${path.stylesDev}**/*.scss`, ['styles:sass']);
    gulp.watch(`${path.views}**/**.*`).on('change', browserReload);
});

gulp.task('default', ['serve']);