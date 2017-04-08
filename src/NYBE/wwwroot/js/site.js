// Write your Javascript code.
$("#toggleAdvancedSearch").click(function () {
    if (document.getElementById("toggleAdvancedSearch").innerHTML === "Hide Advanced Search") {
        $("#advancedSearchFields").hide();
        $("#generalSearch").show()
        document.getElementById("toggleAdvancedSearch").innerHTML = "Show Advanced Search";
    } else {
        $("#advancedSearchFields").show();
        $("#generalSearch").hide();
        document.getElementById("toggleAdvancedSearch").innerHTML = "Hide Advanced Search";
    }
});