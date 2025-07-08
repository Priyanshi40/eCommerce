 connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveNotification", function (message) {
    loadNotifications();
    toastr.info(message);
});

connection.start().catch(err => console.error(err));

function notificationsCount() {
    $.get("/Notification/GetNotificationCount", function (res) {
        $('#notifCount').text(res.count);
        if (res.count === 0) {
            $('#notifCount').hide();
        }
    });
}
function loadNotifications() {
    $.get("/Notification/GetNotification", function (html) {
        $("#notificationListContainer").html(html);
        notificationsCount();
    });
}

$(document).on("click", ".markRead", function () {
    const id = $(this).data("id");
    $.post("/Notification/MarkAsRead", { notificationId: id }, function () {
        loadNotifications();
    });
});

$(document).on("click", "#markAllRead", function () {
    $.get("/Notification/MarkAllAsRead", function (data) {
        if (data.status == "Error") {
            toastr.warning("Failed to Read All Notification");
        }
        else {
            loadNotifications();
        }
    });
});

$("#notifDropdown").on("click", function () {
    loadNotifications();
});