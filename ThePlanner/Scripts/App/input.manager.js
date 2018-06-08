$(document).ready(function () {
    $("#add").click(function () {
        var lastField = $("#buildinput div:last");
        console.log(lastField);

        var intId = (lastField && lastField.length && lastField.data("idx") + 1) || 0;
        console.log(intId);
        var fieldWrapper = $("<div class=\"fieldwrapper\" id=\"field" + intId + "\"/>");
        fieldWrapper.data("idx", intId);

        console.log(fieldWrapper.data("idx", intId));
        var fName = $("<input type=\"text\" class=\"fieldname\" name=\"field[" + intId + "].Name\" required />");
        var fVal = $("<input type=\"text\" class=\"fieldname\" name=\"field[" + intId + "].Val\" required />");
        var removeButton = $("<input type=\"button\" class=\"remove\" value=\"Delete\" />");
        removeButton.click(function () {
            $(this).parent().remove();
        }); 
        fieldWrapper.append(fName);
        fieldWrapper.append(fVal);
        fieldWrapper.append(removeButton);
        $("#buildinput").append(fieldWrapper);
    });

	$("#theForm").submit(function (e) {
		$.ajax({
			method: "Post",
			cache: false,
			data: $("#theForm").serialize(),
			url: '/Occasion/Create',
			success: function (response) {
                $("#event-modal").modal('hide');
                LoadEvents();
			}
		});
		e.preventDefault();
    });




});
