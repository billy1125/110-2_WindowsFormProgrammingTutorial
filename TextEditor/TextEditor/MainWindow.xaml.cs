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
using System.IO;
using Microsoft.Win32;

namespace TextEditor
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 清除rtbText的內容
            rtbText.Document.Blocks.Clear();
            // 設定字型下拉選單的選單內容，存取你的電腦裡面的字型庫，將你安裝的字型清單都放進去
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            // 設定字體大小下拉選單的選單內容，設定8`72的數字，這要用來設定字體大小
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            // 跟記事本範例程式類似，不過要改成過濾為RTF檔案格式
            dlg.Filter = "RTF文件 (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);
                // DataFormats 檔案格式也要設定為RTF檔案格式
                range.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "RTF文件 (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 判斷式：必須要有選擇項目，才會做文字格式改變
            if (cmbFontFamily.SelectedItem != null)
                // 將rtbText豐富文字框所選的項目，套用所設定的字型
                rtbText.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null)
                // 將rtbText豐富文字框所選的項目，套用所設定的字體大小
                rtbText.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.SelectedItem);
        }
    }
}
