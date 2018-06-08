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
    console.log(event.date);
    let year = event.date.getFullYear();
    let month = event.date.getMonth();
    let day = event.date.getDate()+1;
    document.getElementById("date").valueAsDate = new Date(year,month,day);

    $('#event-modal').modal();
}


function deleteEvent(event) {
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

    }

    $('#calendar').data('calendar').setDataSource(dataSource);
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

$(function () {
    var currentYear = new Date().getFullYear();

    $('#calendar').calendar({
        enableContextMenu: true,
        enableRangeSelection: false,
        allowOverlap:true,
        contextMenuItems: [
            //{
            //    text: 'Update',
            //    click: editEvent
            //},
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
                //console.log(e.events[0].startDate.getDate());
                //console.error(e.events[0].endDate.getDate());
                //console.error(e.events[0].startDate.getMonth());
            } else {
                console.log(e);
                console.log(e.date.getDate());
                editEvent(e);

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
});

function LoadEvents() {
    $.ajax({
        method: "POST",
        processData: true,
        url: '/Occasion/UpdateCalendar',
        success: function (arrJson) {
            let arr = [];
            for (var i = 0; i < arrJson.length; i++) {
                arr.push({
                    id: arrJson[i].id,
                    name: arrJson[i].name,
                    location: arrJson[i].location,
                    startDate: new Date(arrJson[i].startDate),
                    endDate: new Date(arrJson[i].endDate)
                });
            }
            console.log(arr);
            $('#calendar').data('calendar').setDataSource(arr);
        },
        error: function (jqXhr) {
            console.log(jqXhr);
        }
    });
}