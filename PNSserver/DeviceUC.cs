using Chirpstack.Api;
using Grpc.Core;
using Grpc.Net.Client;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSserver
{
    public partial class DeviceUC : UserControl
    {
        DeviceListItem __deviceInfo;
        String __token;
        public DeviceUC(DeviceListItem deviceListItem, string token)
        {
            InitializeComponent();
            System.Windows.Forms.Timer __timerUplinkDownlink = new System.Windows.Forms.Timer()
            {
                Interval = 5000,
            };
            __deviceInfo = deviceListItem;
            __token = token;
            __ShowDeviceInfo();
            __ReadUplink();
        }

        void __ShowDeviceInfo()
        {
            if (__deviceInfo.LastSeenAt != null)
                label_lastSeen.Text = __deviceInfo.LastSeenAt.ToDateTime().ToString("dd/MM/yyyy  hh:mm:ss");
            label_name.Text = __deviceInfo.Name;
            label_devEUI.Text = __deviceInfo.DevEui;

        }

        async void __ReadUplink()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            // Cấu hình broker
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883) // Broker MQTT (thay localhost nếu khác máy)
                .WithCredentials("chirpstack", "chirpstack") // nếu ChirpStack broker yêu cầu user/pass
                .WithClientId("winform-client-1")
                .Build();

            // Sự kiện khi kết nối thành công
            mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("✅ Đã kết nối tới MQTT broker!");

                // Subscribe uplink topic (thay AppID & DevEUI cho đúng)
                await mqttClient.SubscribeAsync("application/1/device/0102030405060708/event/up");
                Console.WriteLine("📡 Đang lắng nghe uplink...");
            };

            // Sự kiện khi nhận được message
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

                Console.WriteLine($"📩 Nhận topic: {topic}");
                Console.WriteLine($"📦 Payload: {payload}");

                return Task.CompletedTask;
            };

            // Kết nối broker
            await mqttClient.ConnectAsync(options);

            Console.WriteLine("Nhấn Enter để thoát...");
            Console.ReadLine();
        }
    }
    
}
