using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Bai5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            button1.Click += new EventHandler(this.Button1_Click);


        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            var url = textBox1.Text = "https://nt106.uitiot.vn/auth/token";
            var username = textBox2.Text = "kien";
            var password = textBox3.Text = "12345";

            using (var client = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(username), "username" },
                    { new StringContent(password), "password" }
                };

                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseString);

                if (!response.IsSuccessStatusCode)
                {
                    var detail = responseObject["detail"]?.ToString();
                    textBox4.Text = $"Detail: {detail}";
                    return;
                }

                var tokenType = responseObject["token_type"]?.ToString();
                var accessToken = responseObject["access_token"]?.ToString();
                textBox4.Text = $"Token Type: {tokenType}\nAccess Token: {accessToken}\nĐăng nhập thành công";
            }
        }
    }
}