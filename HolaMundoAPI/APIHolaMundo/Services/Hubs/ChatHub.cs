using DB.Data.Models.ChatHub;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace APIHolaMundo.Services.Hubs
{
    public class ChatHub:Hub
    {
        private static Dictionary<string, string> deviceConnections;
        private static Dictionary<string, string> connectionDevices;

        public ChatHub()
        {
            deviceConnections = deviceConnections ?? new Dictionary<string, string>();
            connectionDevices = connectionDevices ?? new Dictionary<string, string>();
        }

        public override Task OnConnectedAsync()
        {
            Debug.WriteLine("SignalR server connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string? deviceId = connectionDevices.ContainsKey(Context.ConnectionId) ?
                            (string?)connectionDevices[Context.ConnectionId] :
                            null;

            if (!string.IsNullOrEmpty(deviceId))
            {
                deviceConnections.Remove(deviceId);
                connectionDevices.Remove(Context.ConnectionId);
            }

            List<KeyValuePair<string, string>> listNumber = deviceConnections.ToList();

            Clients.All.SendAsync("ReceiveDevices", listNumber);
            return base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("Init")]
        public Task Init(DeviceInfo info)
        {
            deviceConnections.AddOrUpdate(info.Username, Context.ConnectionId);
            connectionDevices.AddOrUpdate(Context.ConnectionId, info.Username);

            List<KeyValuePair<string,string>> listNumber = deviceConnections.ToList();

            Clients.All.SendAsync("ReceiveDevices", listNumber);

            return Task.CompletedTask;
        }

        [HubMethodName("SendMessageToAll")]
        public async Task SendMessageToAll(MessageItem item)
        {
            await Clients.All.SendAsync("Receive", item);
        }

        [HubMethodName("SendMessageToDevice")]
        public async Task SendMessageToDevice(MessageItem item)
        {
            if (deviceConnections.ContainsKey(item.TargetId))
                await Clients.Client(deviceConnections[item.TargetId]).SendAsync("Receive", item);
        }
    }
}
