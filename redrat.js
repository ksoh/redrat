// Overview of edge.js: http://tjanczuk.github.com/edge
// Compile Sample105.dll with
// csc.exe /target:library /debug Sample105.cs
/// <reference path="def/node.d.ts"/>
var edge = require('edge');

var convertImage = edge.func(function () {
});

convertImage(null, function (error) {
    if (error)
        throw error;
    console.log("The edge.png has been asynchronously converted to edge.jpg.");
});
//# sourceMappingURL=redrat.js.map
