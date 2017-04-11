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

$("#schoolSelectBox").change(function() {
    var schoolSelected = $("#schoolSelectBox").find(":selected").val();
    
    //From: http://stackoverflow.com/questions/2008287/removing-and-adding-options-from-a-group-of-select-menus-with-jquery
    //Show all options
    $("#courseSelectBox > option").show().removeAttr('disabled');

    if (schoolSelected == "-1") {
        return;
    }

    //Get values that should be hidden
    $('#courseSelectBox > option').each(function () {
        
        var schoolForCourse = $(this).attr('class');
        console.log(schoolForCourse);
        if (schoolForCourse > 0 && schoolForCourse != schoolSelected) {
            $(this).hide().attr('disabled', 'disabled');
        }
    });
});