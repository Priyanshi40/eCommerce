using Microsoft.AspNetCore.SignalR;

namespace Ecommerce.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"------> User Connected with ID: {userId}");
        return base.OnConnectedAsync();
    }

}


