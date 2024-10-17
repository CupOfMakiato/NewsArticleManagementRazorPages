// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/NewsHub")
    .build();

connection.on("ArticleModified", function (accountId) {
    var currentPath = window.location.pathname;
    var reloadPaths = ["/News/ViewNews", "/NewsArticleManagement/Index"];

    // Check if the current path starts with any of the reload paths
    if (reloadPaths.some(path => currentPath.startsWith(path))) {
        location.reload();
    }
});

connection.start()
    .then(function () {
        console.log("SignalR Connected.");
    })
    .catch(function (err) {
        console.error("Error while starting connection: " + err.toString());
    });
