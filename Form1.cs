using Microsoft.Web.WebView2.Core;
using System.Net;

namespace JDBrowser
{
    public partial class JDBrowser : Form
    {
        private TextBox ptKeyTextBox = null!;
        private TextBox ptPinTextBox = null!;

        public JDBrowser()
        {
            InitializeComponent();
            InitializeWebView();
            InitializeDebugButton();
            InitializeUIComponents();
            InitializeClearCookiesButton();
        }

        private async void InitializeWebView()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate("http://bean.m.jd.com/bean/signIndex.action");
            webView21.CoreWebView2.NavigationCompleted += WebView21_NavigationCompleted;
        }

        private void InitializeDebugButton()
        {
            Button debugButton = new Button();
            debugButton.Text = "读取Cookies";
            debugButton.Location = new System.Drawing.Point(600, 100);
            debugButton.Click += DebugButton_Click;
            this.Controls.Add(debugButton);
        }

        private void InitializeUIComponents()
        {
            Label ptKeyLabel = new Label();
            ptKeyLabel.Text = "pt_key:";
            ptKeyLabel.Width = 80;
            ptKeyLabel.Location = new System.Drawing.Point(520, 10);
            this.Controls.Add(ptKeyLabel);

            ptKeyTextBox = new TextBox();
            ptKeyTextBox.Location = new System.Drawing.Point(600, 10);
            ptKeyTextBox.Width = 300;
            this.Controls.Add(ptKeyTextBox);

            Label ptPinLabel = new Label();
            ptPinLabel.Text = "pt_pin:";
            ptPinLabel.Width = 80;
            ptPinLabel.Location = new System.Drawing.Point(520, 40);
            this.Controls.Add(ptPinLabel);

            ptPinTextBox = new TextBox();
            ptPinTextBox.Location = new System.Drawing.Point(600, 40);
            ptPinTextBox.Width = 300;
            this.Controls.Add(ptPinTextBox);

            Button copyButton = new Button();
            copyButton.Text = "复制";
            copyButton.Location = new System.Drawing.Point(700, 100);
            copyButton.Click += CopyButton_Click;
            this.Controls.Add(copyButton);
        }

        private void InitializeClearCookiesButton()
        {
            Button clearCookiesButton = new Button();
            clearCookiesButton.Text = "清除Cookies并刷新";
            clearCookiesButton.Location = new System.Drawing.Point(800, 100);
            clearCookiesButton.Click += ClearCookiesButton_Click;
            this.Controls.Add(clearCookiesButton);
        }

        private async void WebView21_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await LoadCookiesToTextBoxes();
        }

        private async void DebugButton_Click(object? sender, EventArgs e)
        {
            await LoadCookiesToTextBoxes();
        }

        private async Task LoadCookiesToTextBoxes()
        {
            var cookieManager = webView21.CoreWebView2.CookieManager;
            var cookies = await cookieManager.GetCookiesAsync("http://bean.m.jd.com");
            foreach (var cookie in cookies)
            {
                if (cookie.Name == "pt_key")
                {
                    ptKeyTextBox.Text = cookie.Value;
                }
                if (cookie.Name == "pt_pin")
                {
                    ptPinTextBox.Text = WebUtility.UrlDecode(cookie.Value);
                }
            }
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            string textToCopy = $"pt_key={ptKeyTextBox.Text}; pt_pin={ptPinTextBox.Text};";
            Clipboard.SetText(textToCopy);
            MessageBox.Show("已复制到剪贴板");
        }

        private async void ClearCookiesButton_Click(object? sender, EventArgs e)
        {
            var cookieManager = webView21.CoreWebView2.CookieManager;
            var cookies = await cookieManager.GetCookiesAsync("http://bean.m.jd.com");
            foreach (var cookie in cookies)
            {
                cookieManager.DeleteCookie(cookie);
            }
            webView21.CoreWebView2.Reload();
            MessageBox.Show("Cookies已清除并刷新页面");
        }
    }
}
