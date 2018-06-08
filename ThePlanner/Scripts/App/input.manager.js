$(document).ready(function () {
	$("#add").click(function () {
		var lastField = $("#buildinput div:last");
		var intId = (lastField && lastField.length && lastField.data("idx") + 1) || 0;
		var fieldWrapper = $("<div class=\"fieldwrapper\" id=\"field" + intId + "\"/>");
		fieldWrapper.data("idx", intId);
		var fName = $("<input type=\"text\" class=\"fieldname\" name=\"field[" + intId + "].Name\" required />");
		var fVal = $("<input type=\"text\" class=\"fieldname\" name=\"field[" + intId + "].Val\" required />");
		//var fType = $("<select class=\"fieldtype\"><option value=\"checkbox\">Checked</option><option value=\"textbox\">Text</option><option value=\"textarea\">Paragraph</option></select>");
		var removeButton = $("<input type=\"button\" class=\"remove\" value=\"Delete\" />");
		removeButton.click(function () {
			$(this).parent().remove();
		});
		fieldWrapper.append(fName);
		fieldWrapper.append(fVal);
		//fieldWrapper.append(fType);
		fieldWrapper.append(removeButton);
		$("#buildinput").append(fieldWrapper);
	});

	$("#theForm").submit(function (e) {
		$.ajax({
			method: "Post",
			cache: false,
			data: $("#theForm").serialize(),
			url: '\\Occasion\\CreateOccasion',
			success: function (response) {
                $("#event-modal").modal('hide');
                LoadEvents();
			}
		});
		e.preventDefault();
	});



});