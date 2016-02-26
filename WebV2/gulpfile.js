var gulp = require('gulp'); //build tools dependencies
var del = require('del');


var requireDir = require('require-dir');
var dir = requireDir('./gulp tasks');

gulp.task('clean', function (callback) {
    del(['build/*'], callback);
});

gulp.task('watch', ['build'], function () {
    gulp.watch(['*.html', 'app/**/*.html', 'app/**/*.js'], ['build'
    ]);
});

gulp.task('default', ['build']);