using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bai6
{
    public partial class Form1 : Form
    {
        private string tokenType;
        private string accessToken;

        public Form1()
        {
            InitializeComponent();
            button1.Click += new EventHandler(this.Button1_Click);
            button2.Click += new EventHandler(this.Button2_Click); 
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            string username = textBox2.Text;
            string password = textBox3.Text;

            var url = textBox1.Text = "https://nt106.uitiot.vn/auth/token";

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
               

                tokenType = responseObject["token_type"]?.ToString();
                accessToken = responseObject["access_token"]?.ToString();
                textBox4.Text = $"Token Type: {tokenType}\nAccess Token: {accessToken}\nĐăng nhập thành công";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        private async void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tokenType) || string.IsNullOrEmpty(accessToken))
            {
                textBox4.Text = "Vui lòng đăng nhập trước.";
                return;
            }

            var getUserUrl = "https://nt106.uitiot.vn/api/v1/user/me";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(tokenType, accessToken);

                var getUserResponse = await client.GetAsync(getUserUrl);
                var getUserResponseString = await getUserResponse.Content.ReadAsStringAsync();

                if (getUserResponse.IsSuccessStatusCode)
                {
                    var userObject = JObject.Parse(getUserResponseString);
                    var userId = userObject["id"]?.ToString();
                    var fullName = userObject["full_name"]?.ToString();
                    var email = userObject["email"]?.ToString();

                    if (userId != null)
                    {
                        textBox4.AppendText($"User ID: {userId}\n");
                    }
                    if (fullName != null)
                    {
                        textBox4.AppendText($"Full Name: {fullName}\n");
                    }
                    if (email != null)
                    {
                        textBox4.AppendText($"Email: {email}\n");
                    }

                    textBox4.AppendText("\nĐăng nhập thành công!\n");
                }
                else
                {
                    textBox4.AppendText($"Error retrieving user information: {getUserResponse.StatusCode}\n");
                }

            }
        }

        
    }
    }
 

