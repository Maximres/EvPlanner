$(document).ready(function () {
    $("#add").click(function () {
        var lastField = $("#buildinput div:last");
        console.log(lastField);

		var intId = (lastField && lastField.length && lastField.data("idx") + 1) || 0;
		var fieldWrapper = $("<div class=\"fieldwrapper\" id=\"field" + intId + "\"/>");
        fieldWrapper.data("idx", intId);
        var row = $("<div class=\"row\">");
        var Div1 = $("<div class=\"col-sm-4  form-group\">");
        var Div2 = $("<div class=\"col-sm-4  form-group\">");
        var fName = $("<input type=\"text\" class=\"fieldname col-sm-offset-1 form-control\" name=\"field[" + intId + "].Name\" required />");
        var fVal = $("<input type=\"text\" class=\"fieldname col-sm-offset-2 form-control\" name=\"field[" + intId + "].Val\" required />");
		//var fType = $("<select class=\"fieldtype\"><option value=\"checkbox\">Checked</option><option value=\"textbox\">Text</option><option value=\"textarea\">Paragraph</option></select>");
        var removeButton = $("<input type=\"button\" class=\"remove col-sm-offset-1 btn btn-primary\" value=\"Delete\" />");
        var cDiv = $("</div>");
		removeButton.click(function () {
			$(this).parent().remove();
        });
        fieldWrapper.append(row);
        row.append(Div1);
        Div1.append(fName);
        row.append(Div2);
        Div2.append(fVal);
		//fieldWrapper.append(fType);
        row.append(removeButton);
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