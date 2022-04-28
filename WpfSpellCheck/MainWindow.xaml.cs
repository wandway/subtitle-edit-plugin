using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSpellCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            textBox.SpellCheck.IsEnabled = true;

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            label.Content = "开始检查";
            var word = textBox.Text;
            if (!string.IsNullOrEmpty(word))
            {
                var error = textBox.GetSpellingError(0);
                if (error != null)
                {
                    var list = error.Suggestions.ToList();
                    var result = string.Join(",", list);
                    label.Content = result;
                } else
                {
                    label.Content = "没有问题2";
                }
            } else
            {
                label.Content = "没有内容";
            }
        }
    }
}
