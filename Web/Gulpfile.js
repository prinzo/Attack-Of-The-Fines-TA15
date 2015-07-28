var gulp = require('gulp'),
    gutil = require('gulp-util'),
    jshint = require('gulp-jshint'),
    browserify = require('gulp-browserify'),
    concat = require('gulp-concat'),
    clean = require('gulp-clean');
reload = require('gulp-livereload');
runSequence = require('run-sequence');

gulp.task('buildme', ['build:content', 'build:font', 'build:common', 'build:resources', 'build:scripts', 'build:index', 'livereload'], function () {
    gulp.src(['app/**/*.js', 'app/**/*.html'])
        .pipe(gulp.dest('dist/app'))
        .on('error', gutil.log);


});

gulp.task('build', function () {
    runSequence('buildme', 'livereload');
});
gulp.task('livereload', function () {
    reload.changed();
});

gulp.task('build:common', function () {
    gulp.src(['common/*.js', 'common/*.html'])
        .pipe(gulp.dest('dist/common'))
        .on('error', gutil.log);

});

gulp.task('build:resources', function () {
    gulp.src(['common/resources/*.js'])
        .pipe(gulp.dest('dist/common/resources'))
        .on('error', gutil.log);

});

gulp.task('build:index', function () {
    gulp.src(['index.html', 'favicon.ico'])
        .pipe(gulp.dest('dist/'))
        .on('error', gutil.log);

});

gulp.task('build:content', function () {
    gulp.src(['content/*.css', 'content/*.png', 'content/*.scss', 'content/*.map'])
        .pipe(gulp.dest('dist/content'))
        .on('error', gutil.log);

});

gulp.task('build:font', function () {
    gulp.src(['fonts/*.eot', 'fonts/*.svg', 'fonts/*.ttf', 'fonts/*.woff', 'fonts/*.woff2'])
        .pipe(gulp.dest('dist/fonts'))
        .on('error', gutil.log);

});

gulp.task('build:scripts', function () {
    gulp.src(['scripts/*.js', 'scripts/*.map'])
        .pipe(gulp.dest('dist/scripts'))
        .on('error', gutil.log);

});

gulp.task('clean', function () {
    return gulp.src('dist/*', {
            read: false
        })
        .pipe(clean());
});
// JSHint task
gulp.task('lint', function () {
    gulp.src('./app/**/*.js')
        .pipe(jshint())
    // You can look into pretty reporters as well, but that's another story
    .pipe(jshint.reporter('default'));
});

// Browserify task
gulp.task('browserify', function () {
    // Single point of entry (make sure not to src ALL your files, browserify will figure it out for you)
    gulp.src(['app/app.js'])
        .pipe(browserify({
            insertGlobals: true,
            debug: true
        }))
    // Bundle to a single file
    .pipe(concat('bundle.js'))
    // Output it to our dist folder
    .pipe(gulp.dest('dist/js'));
});

gulp.task('watch', ['build'], function () {
    // Watch our scripts
    gulp.watch(['*.html', 'app/**/*.html', 'app/**/*.js', 'common/*.html'], ['build'
  ]);
});

gulp.task('views', function () {
    gulp.src('./app/index.html')
        .pipe(gulp.dest('dist/'));

    gulp.src('./app/**/*.html')
        .pipe(gulp.dest('dist/views/'))
        .pipe(refresh(lrserver)); // Tell the lrserver to refresh
});



var embedlr = require('gulp-embedlr'),
    refresh = require('gulp-livereload'),
    lrserver = require('tiny-lr')(),
    express = require('express'),
    livereload = require('connect-livereload'),
    livereloadport = 35729,
    serverport = 5000;

// Set up an express server (but not starting it yet)
var server = express();
// Add live reload
server.use(livereload({
    port: livereloadport
}));
// Use our 'dist' folder as rootfolder
server.use(express.static('./dist'));
// Because I like HTML5 pushstate .. this redirects everything back to our index.html
server.all('/*', function (req, res) {
    res.sendfile('index.html', {
        root: 'dist'
    });
});

gulp.task('dev', function () {
    // Start webserver
    server.listen(serverport);
    // Start live reload
    lrserver.listen(livereloadport);
    // Run the watch task, to keep taps on changes
    gulp.run('watch');
});