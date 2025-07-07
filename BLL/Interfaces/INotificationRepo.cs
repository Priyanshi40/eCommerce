using DAL.Models;

namespace BLL.Interfaces;

public interface INotificationRepo
{
    List<Notification> GetNotifications(string userId);
    bool AddNotification(Notification notification);
    bool MarkAsRead(int notificationId);
    bool MarkAllAsRead(string userId);
}
