using Chirpstack.Api;
using Grpc.Core;
using Grpc.Net.Client;

namespace PNSserver
{
    public partial class Form1 : Form
    {
        string __token = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJjaGlycHN0YWNrIiwiaXNzIjoiY2hpcnBzdGFjayIsInN1YiI6ImVlOGE1MDljLTE2ODItNDQ2MS05YmYyLTkzYTUyNmM4NWYwMiIsInR5cCI6ImtleSJ9.1YiXIYdZFdFDaDjhEAdc0Hkaid-enDr3ZsVeQDRskOs";
        string __applicationID = "540513b9-b232-4fb8-875d-5bf10306f195";
        public Form1()
        {
            InitializeComponent();
            
        }

        async  void __GetListDevices()
        {
            using var channel = GrpcChannel.ForAddress("http://103.1.210.21:8080");

            // Tạo client cho DeviceService
            var client = new DeviceService.DeviceServiceClient(channel);

            // Metadata (API token)
            var headers = new Metadata
            {
                { "authorization", __token}
            };

            // Request lấy danh sách device theo ApplicationID
            var request = new ListDevicesRequest
            {
                ApplicationId = __applicationID,  // ID ứng dụng bạn muốn lấy device
                Limit = 50,         // số lượng tối đa
                Offset = 0          // bắt đầu từ index 0
            };

            // Gọi API
            var response = await client.ListAsync(request, headers);

            Console.WriteLine($"Tổng số device: {response.TotalCount}");
            var __listDevice = response.Result;
            foreach (var dev in response.Result)
            {
                var deviceUC = new DeviceUC(dev, __applicationID, __token);
                flowLayoutPanel_containDevice.Controls.Add(deviceUC);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            __GetListDevices();
        }
    }
}