using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Musoftware
{
    public static class CEFLinker
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        static List<Action> queueLinking = new List<Action>();

        public static void LinkFunction(
            ChromiumWebBrowser browser,
            string elementID,
            Events _event,
            Action func)
        {
            AsJSObject obj = new AsJSObject(func, browser);

            string _className = RandomString(5).ToLower();
            browser.RegisterJsObject(_className, obj);
            queueLinking.Add(() => {
                ChromiumWebBrowser _browser = browser;
                string _elementID = elementID;
                Events __event = _event;
                Action _func = func;
                string _eventStr = __event.ToString();
                _browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(@"document.getElementById(""" + _elementID + @""")." + _eventStr + " = function(e) { e.preventDefault(); " + _className + ".call(); }");
           });
        }

        public static void InitBrowser(ChromiumWebBrowser browser)
        {
            while (!browser.IsBrowserInitialized)
            {
                Application.DoEvents();
            }
            browser.FrameLoadEnd += (sender, args) =>
            {
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
                    foreach (var LinkingFunc in queueLinking)
                    {
                        LinkingFunc.Invoke();
                    }
                }

                ThreadPool.QueueUserWorkItem((c) =>
                {
                    try
                    {
                        while (true)
                        {
                            var elementLength = browser.GetBrowser().MainFrame.EvaluateScriptAsync(
                                @"document.getElementsByClassName(""loadfile"").length");
                            elementLength.Wait();
                            var res = elementLength.Result;

                            if (int.Parse(res.Result.ToString()) > 0)
                            {
                                try
                                {
                                    var filenameObj = browser.GetBrowser().MainFrame.EvaluateScriptAsync(
                                     @"document.getElementsByClassName(""loadfile"")[0].getAttribute(""data-filename"")");
                                    filenameObj.Wait();
                                    string filename = filenameObj.Result.Result.ToString();
                                    string html = File.ReadAllText(filename, Encoding.UTF8);

                                    var FrameObj = browser.GetBrowser().MainFrame.EvaluateScriptAsync(
                                   @"document.getElementsByClassName(""loadfile"")[0].outerHTML =""" + StringEditing.StringToJs(html) + @"""");
                                    FrameObj.Wait();

                                }
                                catch (FileNotFoundException ex)
                                {
                                    var FrameObj = browser.GetBrowser().MainFrame.EvaluateScriptAsync(
                                 @"document.getElementsByClassName(""loadfile"")[0].setAttribute(""class"", """")");
                                    FrameObj.Wait();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show(ex.StackTrace);
                    }
                });

            };
         


        }





    }
}
 