using BLL.Interfaces;
using DAL.Models;

namespace BLL.Repositories;

public class NotificationRepo : INotificationRepo
{
    private readonly E_CommerceContext _context;

    public NotificationRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public int GetNotificationCount(string userId)
    {
        var count = _context.Notification.Count(w => w.UserId == userId && !w.IsRead);
        return count;
    }
    public List<Notification> GetNotifications(string userId)
    {
        return _context.Notification
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
    }
    public bool AddNotification(Notification notification)
    {
        _context.Notification.Add(notification);
        _context.SaveChanges();
        return true;
    }
    public bool MarkAsRead(int notificationId)
    {
        var notification = _context.Notification.Find(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            _context.SaveChanges();
            return true;
        }
        return false;
    }
    public bool MarkAllAsRead(string userId)
    {
        var notifications = _context.Notification.Where(n => n.UserId == userId && !n.IsRead).ToList();
        if (notifications != null)
        {
            notifications.ForEach((notif) =>
            {
                notif.IsRead = true;
            });
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}
