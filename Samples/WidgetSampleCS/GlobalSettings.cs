using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetSampleCS
{
    public static class GlobalSettings
    {
        public delegate void HTMLValueChanged();


        public static HTMLValueChanged OnHTMLValueChanged;

        private static string _html;
        public static string HTML
        {
            get
            {
                if (string.IsNullOrEmpty(_html))
                {
                    try
                    {
                        TextReader tr = new StreamReader(@"HTMLCode.txt");
                        _html = tr.ReadToEnd();
                        tr.Close();
                    }
                    catch
                    {
                        _html = @"<h1>Unable to load data</h1>";
                    }
                }
                return _html;
            }
            set
            {
                try
                {
                    TextWriter tr = new StreamWriter(@"HTMLCode.txt");
                    tr.Write(value);
                    tr.Close();
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("could not write to file");
                }
                _html = value;
                OnHTMLValueChanged?.Invoke();
            }
        }
    }
}
