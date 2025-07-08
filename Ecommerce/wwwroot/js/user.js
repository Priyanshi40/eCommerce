// const connection = new signalR.HubConnectionBuilder()
//     .withUrl("/notificationHub")
//     .build();

// connection.on("ReceiveNotification", function (message) {
//     loadNotifications();
//     toastr.info(message);
// });

// connection.start().catch(err => console.error(err));

function notificationsCount() {
    $.get("/User/Home/GetNotificationCount", function (res) {
        $('#notifCount').text(res.count);
        if (res.count === 0) {
            $('#notifCount').hide();
        }
    });
}
function loadNotifications() {
    $.get("/User/Home/GetNotification", function (html) {
        $("#notificationListContainer").html(html);
        notificationsCount();
    });
}

$(document).on("click", ".markRead", function () {
    const id = $(this).data("id");
    $.post("/User/Home/MarkAsRead", { notificationId: id }, function () {
        loadNotifications();
    });
});

$(document).on("click", "#markAllRead", function () {
    $.get("/User/Home/MarkAllAsRead", function (data) {
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