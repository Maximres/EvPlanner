function editEvent(event) {

    //$('#event-modal input[name="date"]').val(event ? event.id : '');
    //$('#event-modal input[name="event-name"]').val(event ? event.name : '');
    //$('#event-modal input[name="event-location"]').val(event ? event.location : '');
    //$('#event-modal').modal();

    //var vl = $('#event-modal input[name="date"]').val();
    //console.log(new Date(event.date).toISOString("DD-MM-YYYY"));
    //$('#event-modal input[name="date"]').datepicker('setDate', event.date);
    //$('#event-modal input[name="event-end-date"]').datepicker('update', event ? event.endDate : '');

    //$('#event-modal input[name="event-index"]').val(event ? event.id : '');
    //$('#event-modal input[name="event-name"]').val(event ? event.name : '');
    //$('#event-modal input[name="event-location"]').val(event ? event.location : '');
    //var dt = event.date;
    //$('#event-modal input[name="date"]').val(new Date());

    //$('#event-modal input[name="date"]').datepicker('setDate',new Date(event.date).toISOString().substr(0,10));
    //$('#event-modal input[name="event-end-date"]').datepicker('update', event ? event.endDate : '');
    let year = event.date.getFullYear();
    let month = event.date.getMonth();
    let day = event.date.getDate() + 1;
    document.getElementById("date").valueAsDate = new Date(year, month, day);

    $('#event-modal').modal();
}


function deleteEvent(event) {
    if (IsAuthorized !== undefined && IsAuthorized === "True") {
        var dataSource = $('#calendar').data('calendar').getDataSource();

        let id = -100;
        for (var i in dataSource) {
            if (dataSource[i].id === event.id) {
                dataSource.splice(i, 1);
                id = event.id;
                break;
            }
        }
        if (id >= 0) {
            DeleteEvent(id);
        }

        //$('#calendar').data('calendar').setDataSource(dataSource);
    }

    function saveEvent() {
        var event = {
            id: $('#event-modal input[name="event-index"]').val(),
            name: $('#event-modal input[name="event-name"]').val(),
            location: $('#event-modal input[name="event-location"]').val(),
            startDate: $('#event-modal input[name="event-start-date"]').datepicker('getDate'),
            endDate: $('#event-modal input[name="event-end-date"]').datepicker('getDate')
        };

        var dataSource = $('#calendar').data('calendar').getDataSource();

        if (event.id) {
            for (let i in dataSource) {
                if (dataSource[i].id === event.id) {
                    dataSource[i].name = event.name;
                    dataSource[i].location = event.location;
                    dataSource[i].startDate = event.startDate;
                    dataSource[i].endDate = event.endDate;
                }
            }
        }
        else {
            var newId = 0;
            for (let i in dataSource) {
                if (dataSource[i].id > newId) {
                    newId = dataSource[i].id;
                }
            }

            newId++;
            event.id = newId;

            dataSource.push(event);
        }

        $('#calendar').data('calendar').setDataSource(dataSource);
        $('#event-modal').modal('hide');

    }
}



$(function () {
    var currentYear = new Date().getFullYear();
    

    $('#calendar').calendar({
        enableContextMenu: true,
        enableRangeSelection: false,
        allowOverlap: true,
        contextMenuItems: [
            {
                text: 'Delete',
                click: deleteEvent
            }
        ],
        selectRange: function (e) {
            console.log(e);
            console.log(e.startDate.getDate());
            console.error(e.endDate.getDate());
            console.error(e.startDate.getMonth());
            console.error(e.endDate.getMonth());
            editEvent({ startDate: e.startDate, endDate: e.endDate });
        },
        mouseOnDay: function (e) {
            if (e.events.length > 0) {
                var content = '';

                for (var i in e.events) {
                    content += '<div class="event-tooltip-content">'
                        + '<div class="event-name" style="color:' + e.events[i].color + '">' + e.events[i].name + '</div>'
                        + '<div class="event-location">' + e.events[i].location + '</div>'
                        + '</div>';
                }

                $(e.element).popover({
                    trigger: 'manual',
                    container: 'body',
                    html: true,
                    content: content
                });

                $(e.element).popover('show');
            }
        },
        clickDay: function (e) {

            if (e.events.length > 0) {
                //console.log(e);
                console.log(dataGlobal);
                if (dataGlobal !== undefined) {
                    let index = e.events[0].id;

                    for (let i = 0; i < dataGlobal.length; ++i) {

                        if (dataGlobal[i].Id === index) {
                            console.log(dataGlobal[i].Id + "i");
                            console.log(index + " index");

                            $.ajax({
                                method: "POST",
                                processData: true,
                                dataType: "html",
                                url: '/Occasion/Display',
                                data: { "id": index },
                                success: function (data) {
                                    $("#target").empty().html(data);
                                    $("#sub-modal").modal({
                                    });
                                }
                            });
                            //получение доп полей
                            //if (dataGlobal[i].Fields !== undefined) {
                            //    for (var j = 0; j < dataGlobal[i].Fields.length; j++) {
                            //        console.log(dataGlobal[i].Fields[j].Name);
                            //        console.log(dataGlobal[i].Fields[j].Value);
                            //        //TODO: добавить поля

                            //    }
                            //}
                        }
                    }

                }

                //console.log(e.events[0].startDate.getDate());
                //console.error(e.events[0].endDate.getDate());
                //console.error(e.events[0].startDate.getMonth());
            } else {
                if (IsAuthorized !== undefined && IsAuthorized === "True") {
                    editEvent(e);

                }
            }

            //    /UNROLL LATER
            //$.ajax({
            //    method: "POST",
            //    processData: false,
            //    url: '/Occasion/UpdateCalendar',
            //    success: function (arrJson) {
            //        let arr = [];
            //        for (var i = 0; i < arrJson.length; i++) {
            //            arr.push({
            //                id: arrJson[0].id,
            //                name: arrJson[0].name,
            //                location: arrJson[0].location,
            //                startDate: new Date(arrJson[0].startDate),
            //                endDate: new Date(arrJson[0].endDate)
            //            });
            //        }
            //        $('#calendar').data('calendar').setDataSource(arr);
            //    },
            //    error: function (jqXhr) {
            //        console.log(jqXhr);
            //    }
            //});

        },
        mouseOutDay: function (e) {
            if (e.events.length > 0) {
                $(e.element).popover('hide');
            }
        },
        dayContextMenu: function (e) {
            $(e.element).popover('hide');
        },
        dataSource: []
    });

    ///Init calendar
    LoadEvents();

    $('#save-event').click(function () {
        saveEvent();
    });

    //$('#subscribe').click(function (e) {
    //    let value = $('#subscribe').val();
    //    if (value > -1) {
    //        $.ajax({
    //            method: "POST",
    //            data: { 'id': value },
    //            url: '/Occasion/Subscribe',
    //            success: function (resp) {
    //                if (resp.success === "true") {
    //                    $("#messageBox").empty().text("Вы успешно подписались")
    //                    $(".alert").alert();
    //                    //alert("Вы успешно подписались");
    //                } else {
    //                    alert("Подписка не удалась");
    //                }
    //            },
    //            error: function () {

    //            }
    //        });
    //    }
    //});
});

function LoadEvents() {
    dataGlobal = [];
    $.ajax({
        method: "POST",
        processData: true,
        url: '/Occasion/UpdateCalendar',
        success: function (arrJson) {
            //console.log(arrJson);
            //console.log();
            var arr = [];

            for (let i = 0; i < arrJson.length; i++) {
                arr.push({
                    id: arrJson[i].id,
                    name: arrJson[i].name,
                    location: arrJson[i].location,
                    startDate: new Date(arrJson[i].startDate),
                    endDate: new Date(arrJson[i].endDate)
                });
                //TODO: add to days'data
                let obj = {};
                let tempArr = [];
                obj.Id = arrJson[i].id;
                if (arrJson[i].inputs.length > 0) {
                    for (var j = 0; j < arrJson[i].inputs.length; j++) {
                        //console.log(arrJson[i].inputs[j].Val);
                        //tempArr.Name = arrJson[i].inputs[j].Name;
                        //tempArr.Value = arrJson[i].inputs[j].Val;
                        tempArr.push(
                            {
                                'Name': arrJson[i].inputs[j].Name,
                                'Value': arrJson[i].inputs[j].Val
                            });
                    }
                    obj.Fields = tempArr;
                }
                dataGlobal.push(obj);
            }

            console.log($('#calendar').data('calendar'));
            $('#calendar').data('calendar').setDataSource(arr);
        },
        error: function (jqXhr) {
            console.log(jqXhr);
        }
    });
}

function DeleteEvent(id) {
    $.ajax({
        method: "POST",
        processData: true,
        data: { "id": id },
        url: '/Occasion/Delete',
        success: function (resp) {
            if (resp.success === "true") {
                LoadEvents();
            }
        },
        error: function (jqXhr) {
            console.log(jqXhr);
        }
    });
}

function DoSubscribe() {
    let value = $('#subscribe').val();
    if (value > -1) {
        $.ajax({
            method: "POST",
            data: { 'id': value },
            url: '/Occasion/Subscribe',
            success: function (resp) {
                if (resp.success === true) {
                   $("#messageBox").empty().html("Вы успешно подписались");
                    $(".alert").css('display', "");
                    $(".alert").removeClass('alert-danger');
                    $(".alert").addClass('alert-success');
                    $(".alert").alert().delay(2500).fadeOut(500);
                } else {
                    $("#messageBox").empty().html(resp.message);
                    $(".alert").css('display', "");
                    $(".alert").removeClass('alert-success');
                    $(".alert").addClass('alert-danger');
                    $(".alert").alert().delay(2500).fadeOut(500);
                }
            },
            error: function () {

            }
        });
    }
}