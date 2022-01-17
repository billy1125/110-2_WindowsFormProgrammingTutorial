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

namespace Calculator
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOne_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumber.Text == "0")
            {
                txtNumber.Text = "";
            }
                
            txtNumber.Text = txtNumber.Text + "1";
            // txtNumber.Text += "1";  // 上面和下面的寫法意思是一樣的
        }

        private void btnTwo_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumber.Text == "0") // 如果你的判斷式簡單到只有一行程式，可以把 { } 大刮號省略掉
                txtNumber.Text = "";
            txtNumber.Text = txtNumber.Text + "2";
        }

        private void btnThree_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumber.Text == "0")
                txtNumber.Text = "";
            txtNumber.Text = txtNumber.Text + "3";
        }

        // 以下留給你 ... 請把每一個數字按鍵都做進去
    }
}
