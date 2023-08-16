using DB.Data.Models.ChatHub;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace APIHolaMundo.Services.Hubs
{
    public class ChatHub:Hub
    {
        private static Dictionary<int, string> deviceConnections;
        private static Dictionary<string, int> connectionDevices;

        public ChatHub()
        {
            deviceConnections = deviceConnections ?? new Dictionary<int, string>();
            connectionDevices = connectionDevices ?? new Dictionary<string, int>();
        }

        public override Task OnConnectedAsync()
        {
            Debug.WriteLine("SignalR server connected");
            return base.OnConnectedAsync();
        }

        [HubMethodName("Init")]
        public Task Init(DeviceInfo info)
        {
            deviceConnections.AddOrUpdate(info.Id, Context.ConnectionId);
            connectionDevices.AddOrUpdate(Context.ConnectionId, info.Id);

            var listNumber = deviceConnections.ToList();

            Clients.All.SendAsync("ReceiveDevices", listNumber);

            return Task.CompletedTask;
        }

        [HubMethodName("SendMessageAll")]
        public async Task SendMessage(string userId, string message)
        {
            await Clients.All.SendAsync("Receive", userId, message);
        }
    }
}
