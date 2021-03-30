using System;
using System.Collections.Generic;
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
            get => _html;
            set
            {
                _html = value;
                OnHTMLValueChanged?.DynamicInvoke();
            }
        }
    }
}
