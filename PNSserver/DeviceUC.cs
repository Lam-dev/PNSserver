using Chirpstack.Api;
using Grpc.Core;
using Grpc.Net.Client;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using PNSserver.Model;
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
        String __applicationID;
        GrpcChannel channel;
        Metadata headers;
        public DeviceUC(DeviceListItem deviceListItem, string applicationID, string token)
        {
            InitializeComponent();
            System.Windows.Forms.Timer __timerUplinkDownlink = new System.Windows.Forms.Timer()
            {
                Interval = 5000,
            };
            __deviceInfo = deviceListItem;
            __token = token;
            __applicationID = applicationID;
            headers = new Metadata { { "authorization", __token } };
            __ShowDeviceInfo();
            __ReadUplink();
            dataGridView_queue.Columns[0].DataPropertyName = "ExpiresAt";
            dataGridView_queue.Columns[1].DataPropertyName = "Data";
 
            channel = GrpcChannel.ForAddress("http://103.1.210.21:8080");
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

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("103.1.210.21", 1883) // MQTT broker (host, port)
                                                     //.WithCredentials("username", "password") // nếu broker yêu cầu
                .WithCleanSession()
                .Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                __ShowUplinkData(payload);
                Console.WriteLine($"[{topic}] {payload}");
                return Task.CompletedTask;
            };

            await mqttClient.ConnectAsync(options);

            // Subscribe tất cả uplink
            var topic = $"application/{__applicationID}/device/{__deviceInfo.DevEui}/event/up";
            await mqttClient.SubscribeAsync(topic);

            Console.WriteLine("Đang lắng nghe uplink... Nhấn Ctrl+C để thoát.");
            await Task.Delay(-1);
        }

        void __ShowUplinkData(string  payload)
        {

            var uplinkData = JsonConvert.DeserializeObject<UplinkMessage>(payload);
            this.Invoke(new Action(() =>
            {
                var time = uplinkData.Time.ToString("dd/MM/yyyy hh:mm:ss");
                byte[] bytes = Convert.FromBase64String(uplinkData.Data);
                var messageToRichTextBox = $"{time} {BitConverter.ToString(bytes)}\n";
                richTextBox1.AppendText(messageToRichTextBox);
                label_lastSeen.Text = time;
            }));
            
        }

        async void __ReadListDownlinkQueue()
        {
          
            var client = new DeviceService.DeviceServiceClient(channel);
            var req = new GetDeviceQueueItemsRequest
            {
                DevEui = __deviceInfo.DevEui
            };

            var resp = await client.GetQueueAsync(req, headers);


            this.Invoke(new Action(() =>
            {
                dataGridView_queue.DataSource = resp.Result.ToList();
            }));
        }

        async void __AddToDownlink()
        {
            if (string.IsNullOrEmpty(textBox_inputForSend.Text))
                return;
            var client = new DeviceService.DeviceServiceClient(channel);
            var req = new EnqueueDeviceQueueItemRequest
            {
                QueueItem = new DeviceQueueItem
                {
                    DevEui = __deviceInfo.DevEui, // DevEUI của device
                    Confirmed = false,
                    FPort = 10,
                    Data = Google.Protobuf.ByteString.CopyFromUtf8(textBox_inputForSend.Text)
                }
            };

            var resp = await client.EnqueueAsync(req, headers);
         
        }


    
        private void button_send_Click_1(object sender, EventArgs e)
        {
            __AddToDownlink();
            __ReadListDownlinkQueue();
        }
    }
}
    

