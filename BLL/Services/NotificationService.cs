using BLL.Interfaces;
using DAL.Models;

namespace BLL.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepo _notification;
    public NotificationService(INotificationRepo notification)
    {
        _notification = notification;
    }
    public int GetNotificationCount(string userId)
    {
        return _notification.GetNotificationCount(userId);
    }
    public List<Notification> GetNotifications(string userId)
    {
        return _notification.GetNotifications(userId);
    }
    public bool AddNotification(Notification notification)
    {
        return _notification.AddNotification(notification);
    }
    public bool MarkAsRead(int notificationId)
    {
        return _notification.MarkAsRead(notificationId);
    }
    public bool MarkAllAsRead(string userId)
    {
        return _notification.MarkAllAsRead(userId);
    }
}
