using APIHolaMundo.Services.Interfaces;
using DB.Data.Models.ChatHub;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using DB.Data.Models;

namespace APIHolaMundo.Services.Hubs
{
    public class ChatBotHub : Hub
    {
        private static Dictionary<string, string> deviceConnections;
        private static Dictionary<string, string> connectionDevices;
        private readonly IChatService _chatService;

        public ChatBotHub(IChatService chatService)
        {
            deviceConnections = deviceConnections ?? new Dictionary<string, string>();
            connectionDevices = connectionDevices ?? new Dictionary<string, string>();

            _chatService = chatService;
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
        public Task Init(InfoDevice info)
        {
            deviceConnections.AddOrUpdate(info.Username, Context.ConnectionId);
            connectionDevices.AddOrUpdate(Context.ConnectionId, info.Username);

            List<KeyValuePair<string,string>> listNumber = deviceConnections.ToList();

            Clients.All.SendAsync("ReceiveDevices", listNumber);

            return Task.CompletedTask;
        }

        [HubMethodName("SendMessageToDevice")]
        public async Task SendMessageToDevice(MessageItem item)
        {
            item.Message = await _chatService.SendAsync(new SendMessageInput { Text = item.Message });
            await Clients.Client(deviceConnections[item.SourceId]).SendAsync("Receive", item);
        }
    }
}
