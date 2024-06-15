using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

namespace Bai3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            await webView2.EnsureCoreWebView2Async(null);
        }

        private void Resource_Load(object sender, EventArgs e)
        {
            // Do something if needed on load
        }

        public void SetSource(string source)
        {
            webView2.NavigateToString(source);
        }
    }
}