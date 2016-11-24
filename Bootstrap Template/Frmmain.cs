using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Musoftware
{
    public partial class Frmmain : Form
    {
        public Frmmain()
        {
            InitializeComponent();
        }

        private void Frmmain_Load(object sender, EventArgs e)
        {
      
            // Create a browser component
            ChromiumWebBrowser chromeBrowser = new ChromiumWebBrowser(Path.GetDirectoryName(Application.ExecutablePath) + "\\html_test\\index.html");

            // Add it to the form and fill it to the form window.
            chromeBrowser.MenuHandler = new CustomMenuHandler();

            BrowserSettings browserSettings = new BrowserSettings()
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };
            chromeBrowser.BrowserSettings = browserSettings;

            int stinty = 0;
            CEFLinker.LinkFunction(chromeBrowser, "testBtn", Musoftware.Events.onmouseup,
                () => {
                  
                });

            CEFLinker.LinkFunction(chromeBrowser, "testBtn2", Musoftware.Events.onmouseup,
                () =>
                {

                });



            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            CEFLinker.InitBrowser(chromeBrowser);
            //chromeBrowser.ShowDevTools();
        }
    }
}
