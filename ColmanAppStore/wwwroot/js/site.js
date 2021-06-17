// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


//Dark mode function
$(document).ready(function () {
    $('#checkbox').click(function () {
        var element = document.body;
        element.classList.toggle("dark");
        document.getElementById("navBarStyle").classList.toggle("dark"); //dark mode for nav bar

        document.getElementById("mainWindow").classList.toggle("dark");
    //    document.getElementById("body").classList.toggle("dark");
    });
});