using Chirpstack.Api;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS
{
    internal class LoraNode
    {
        public event Action<byte[]> EventDataRecipted;
        string __applicationID;
        GrpcChannel channel;
        Metadata headers;
        DeviceListItem __deviceInfo;

        public LoraNode(DeviceListItem deviceInfo, string applicationID, string token)
        {
            __deviceInfo = deviceInfo;
            __applicationID = applicationID;
        }
        public async void __ReadUplink()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("103.1.210.21", 1883) // MQTT broker (host, port)
                                                 //.WithCredentials("username", "password") // nếu broker yêu cầu
                .WithCleanSession()
                .Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = e.ApplicationMessage.PayloadSegment;
                EventDataRecipted?.Invoke(payload.ToArray());
                return Task.CompletedTask;
            };

            await mqttClient.ConnectAsync(options);

            // Subscribe tất cả uplink
            var topic = $"application/{__applicationID}/device/{__deviceInfo.DevEui}/event/up";
            await mqttClient.SubscribeAsync(topic);

            Console.WriteLine("Đang lắng nghe uplink... Nhấn Ctrl+C để thoát.");
            await Task.Delay(-1);
        }

        public async void AddToDownlink(byte[] data)
        {
       
            var client = new DeviceService.DeviceServiceClient(channel);
            var req = new EnqueueDeviceQueueItemRequest
            {
                QueueItem = new DeviceQueueItem
                {
                    DevEui = __deviceInfo.DevEui, // DevEUI của device
                    Confirmed = false,
                    Data = ByteString.CopyFrom(data)
                }
            };
            var resp = await client.EnqueueAsync(req, headers);
        }

        public async Task<List<DeviceQueueItem>> ReadListDownlinkQueue()
        {

            var client = new DeviceService.DeviceServiceClient(channel);
            var req = new GetDeviceQueueItemsRequest
            {
                DevEui = __deviceInfo.DevEui
            };

            var resp = await client.GetQueueAsync(req, headers);

            return resp.Result.ToList();
         
        }
    }
}
