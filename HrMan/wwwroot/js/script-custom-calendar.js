
var routeURL = location.protocol + "//" + location.host;
$(document).ready(function () {

    InitializeCalendar();
});
var calendar;

function InitializeCalendar() {
    try {
        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                selectable: true,
                editable: true,
                allDay: true,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay: 'block',
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: routeURL + '/api/LeaveRequest/GetCalendarData?superVisorId=' + $("#superVisorId").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {
                            var events = [];

                            if (response.status === 1) {
                                $.each(response.dataenum, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startDate,
                                        end: data.endDate,
                                        backgroundColor: data.approved ? "#28a745" : "#dc3545",
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: data.id
                                    });

                                });
                            }
                            successCallback(events);
                        },
                        error: function (xhr) {
                            $.notify("Error", "error");
                        }
                    });
                },
                timeFormat: 'H(:mm)',
                displayEventTime: false,
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event)
                }
                /*  eventClick: function (info) {
                      getEventDetailsByEventId(info.event);
                  }*/
            });
            $("td.fc-day.fc-widget-content").each(function (index) {
                var start = moment('2014-06-04'); //start date from start date input
                var end = moment('2014-06-18'); //end date from end date input
                var random = Math.floor(Math.random() * 100);
                var date = $(this).data('date'); //get current date in loop
                var day = moment(date); //create a moment

                //if day is greater than start date and less than end date, display.
                if (day > start && day < end) {
                    $(this).append("<span style='font-size: 30px;font-weight: 600;color: green;'>" + random + "%</span>");
                }
            });

            calendar.render();
        }

    }
    catch (e) {
        alert(e);
    }

}

function onShowModal(obj, isEventDetail) {
    if (isEventDetail != null) {

        $("#title").val(obj.title);
        $("#description").val(obj.description);
        $("#appointmentDate").val(obj.startDate);
        $("#endDate").val(obj.endDate);
        $("#leavetypeId").val(obj.leaveTypeId);
        $("#id").val(obj.id);
        $("#superVisorId").val(obj.superVisorId);
        $("#managerId").val(obj.managerId);
        $("#regularId").val(obj.regularId);
        $("#requestComments").val(obj.requestComments);
        $("#lblSuperVisorName").html(obj.superVisorName);
        $("#lblManagerName").html(obj.managerName);
        $("#lblRegularName").html(obj.regularName);

        /* if (obj.isDoctorApproved) {
             $("#lblStatus").html('Approved');
             $("#btnConfirm").addClass("d-none");
             $("#btnSubmit").addClass("d-none");
         }
         else {
             $("#lblStatus").html('Pending');
             $("#btnConfirm").removeClass("d-none");
             $("#btnSubmit").removeClass("d-none");
         }*/
        $("#btnDelete").removeClass("d-none");
    }
    else {
        $("#appointmentDate").val(obj.startStr + " " + new moment().format("hh:mm A"));

        $("#id").val(0);
        $("#btnDelete").addClass("d-none");
        $("#btnSubmit").removeClass("d-none");
    }

    $("#appointmentInput").modal("show");
}

function onCloseModal() {

    $('#appointmentInput').modal("hide");

    // Reset
    $("#apointmentForm")[0].reset();
    $("#id").val(0);
    $("#appointmentDate").val('');
    $("#endDate").val('');
    $("#requestComments").val('');

    $("#leavetypeId").val('');

    console.log("rtytr");

};

function getEventDetailsByEventId(info) {
    $.ajax({
        url: routeURL + '/api/LeaveRequest/GetCalendarDataById/' + info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1 && response.dataenum != undefined) {
                onShowModal(response.dataenum, true);
            }   
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onSubmitForm() {
    if (checkValidation()) {
        var requestData = {
            Id: parseInt($("#id").val()),
            StartDate: $("#appointmentDate").val(),
            EndDate: $("#endDate").val(),
            title: $("#title").val(),
            description: $("#description").val(),
            RequestComments: $("#requestComments").val(),
            LeaveTypeId: parseInt($("#leavetypeId").val()),
            SuperVisorId: $("#superVisorId").val(),
            RegularId: $("#regularId").val(),
            ManagerId: $("#managerId").val(),
        };
        $.ajax({
            url: routeURL + '/api/LeaveRequest/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status === 1 || response.status === 2) {
                    calendar.refetchEvents();
                    $.notify(response.message, "success");
                    onCloseModal();
                }
                else {
                    $.notify(response.message, "error");
                }

            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    }
    console.log("ewr");
}


function checkValidation() {
    var isValid = true;

    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass("error");
    } else {
        $("#title").removeClass("error");

    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $("#appointmentDate").addClass('error');
    } else if ($("#endDate").val() === undefined || $("#endDate").val() === "") {
        isValid = false;
        $("#endDate").addClass('error');
    }
    else {
        $("#appointmentDate").removeClass('error');
        $("#endDate").removeClass('error');
    }

    return isValid;

}




function onDoctorChange() {
    calendar.refetchEvents();
}