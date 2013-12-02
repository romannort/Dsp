using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Dsp.ImageProcessingDemo
{
    public class OpenFileDialogEx
    {
        public static readonly DependencyProperty FilterProperty =
          DependencyProperty.RegisterAttached("Filter",
            typeof(string),
            typeof(OpenFileDialogEx),
            new PropertyMetadata("All documents (.*)|*.*", (d, e) => AttachFileDialog((TextBox)d, e)));

        public static string GetFilter(UIElement element)
        {
            string result = (string) element.GetValue(FilterProperty);
            return result;
        }

        public static void SetFilter(UIElement element, string value)
        {
            element.SetValue(FilterProperty, value);
        }

        private static void AttachFileDialog(TextBox textBox, DependencyPropertyChangedEventArgs args)
        {
            Panel parent = (Panel)textBox.Parent;

            parent.Loaded += delegate
            {

                Button button = (Button)parent.Children.Cast<object>().FirstOrDefault(x => x is Button);

                string filter = (string)args.NewValue;

                if (button != null)
                    button.Click += (s, e) =>
                        {
                            OpenFileDialog dlg = new OpenFileDialog();
                            dlg.Filter = filter;

                            bool? result = dlg.ShowDialog();

                            if (result == true)
                            {
                                textBox.Text = dlg.FileName;
                            }
                        };
            };
        }
    }
}


