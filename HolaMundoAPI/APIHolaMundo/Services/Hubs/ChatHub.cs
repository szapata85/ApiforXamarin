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

        public override Task OnDisconnectedAsync(Exception exception)
        {
            int? deviceId = connectionDevices.ContainsKey(Context.ConnectionId) ?
                            (int?)connectionDevices[Context.ConnectionId] :
                            null;

            if (deviceId.HasValue)
            {
                deviceConnections.Remove(deviceId.Value);
                connectionDevices.Remove(Context.ConnectionId);
            }

            List<KeyValuePair<int, string>> listNumber = deviceConnections.ToList();

            Clients.All.SendAsync("ReceiveDevices", listNumber);
            return base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("Init")]
        public Task Init(DeviceInfo info)
        {
            deviceConnections.AddOrUpdate(info.Id, Context.ConnectionId);
            connectionDevices.AddOrUpdate(Context.ConnectionId, info.Id);

            List<KeyValuePair<int,string>> listNumber = deviceConnections.ToList();

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
