using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.WinForms;

namespace Musoftware
{
    public class AsJSObject
    {
        private ChromiumWebBrowser browser;
        private Action func;

        public AsJSObject(Action func, ChromiumWebBrowser browser)
        {
            this.func = func;
            this.browser = browser;
        }

        public void Call(object obj1 = null)
        {
            if (func == null) return;

            func.Invoke();
        }


    }
}
