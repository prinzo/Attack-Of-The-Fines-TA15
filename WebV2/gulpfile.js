var gulp = require('gulp'); //build tools dependencies
var gutil = require('gulp-util');
var concat = require('gulp-concat');
var del = require('del');
var path = require('path');
var less = require('gulp-less');
var inject = require('gulp-inject');

gulp.task('clean', function (callback) {
    del(['build/*'], callback);
});

gulp.task('build:js', function () {
    return gulp.src([
            'app/app.js',
            'app/**/*.js',
        ])
        .pipe(concat('app.js'))
        .pipe(gulp.dest('build/app'))
        .on('error', gutil.log);
});

gulp.task('build:css', function () {
    var opts = {
        paths: [path.join(__dirname, 'less', 'includes')]
    };
    return gulp.src('app/**/*.less')
        .pipe(less(opts))
        .pipe(concat('app.css'))
        .pipe(gulp.dest('build/'))
        .on('error', gutil.log);
});

gulp.task('watch', ['build'], function () {
    // Watch our scripts
    gulp.watch(['*.html', 'app/**/*.html', 'app/**/*.js'], ['build'
    ]);
});

gulp.task('build:views', function () {
    gulp.src('./app/index.html')
        .pipe(gulp.dest('build/'));

    gulp.src('./app/**/*.html')
        .pipe(gulp.dest('build/app/'))
});

gulp.task('build:bower', function () {
    gulp.src(['bower_components/**/*.*'])
        .pipe(gulp.dest('build/bower_components'))
        .on('error', gutil.log);

});



gulp.task('build:content', function () {
    gulp.src(['content/**/*.*'])
        .pipe(gulp.dest('build/content'))
        .on('error', gutil.log);

});

gulp.task('build:index', ['build:js', 'build:css', 'build:views', 'build:bower', 'build:content'], function () {
    gulp.src(['index.html'])
        .pipe(gulp.dest('build/'))
        .on('error', gutil.log);
    var target = gulp.src('index.html');

    var sources = gulp.src([
            'bower_components/bootstrap/dist/css/bootstrap.css',
            'bower_components/font-awesome/css/font-awesome.css',
            'bower_components/jquery/dist/jquery.js',
            'bower_components/angular/angular.min.js',
            'bower_components/angular-spinner/angular-spinner.js',
            'bower_components/angular-ui-router/release/angular-ui-router.js',
            'bower_components/angular-ui-router/release/angular-ui-router.js',
            'app/app.js'


        ])
        .on('error', gutil.log);

    var opts = {
        addRootSlash: false
    };
    return target
        .pipe(inject(sources, opts))
        .pipe(gulp.dest('build'))
        .on('error', gutil.log);
});

gulp.task('build',  function () {
     gulp.run('build:index');
});

gulp.task('default', ['build']);