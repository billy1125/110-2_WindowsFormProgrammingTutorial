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
using MySql.Data.MySqlClient; // 導入第三方 MySQL.Data 套件的連線程式庫

namespace Login
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string strAccount = txtAccount.Text;         // 取得帳號輸入文字框的內容
            string strPassword = pbPassword.Password;    // 取得密碼輸入文字框的內容
            bool isLoginSuccess = false;                 // 設定一個布林變數，用來判別是否登入成功
            string strUserName = "";                     // 設定一個字串變數，屆時用來儲存資料庫中的使用者姓名

            try
            {
                // 資料庫連線字串，Server：資料庫管理系統的位址（IP或網址）、Uid：可以使用資料庫的帳號、Pwd：可以使用資料庫的密碼、database：要連線的資料庫名稱
                var connstr = "Server=120.101.58.4;Port=3306;Uid=user;Pwd=user;database=login"; 
                // 使用 using 語法來進行資料庫連線
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open(); // 開啟資料庫連線

                    // 使用 using 語法來設定資料庫的操作命令
                    using (var cmd = conn.CreateCommand())
                    {
                        // 我們之前講過的 SQL 指令
                        cmd.CommandText = "SELECT * FROM users WHERE account=?account and password=?password";
                        cmd.Parameters.Add(new MySqlParameter("account", strAccount));    // 綁定帳號參數
                        cmd.Parameters.Add(new MySqlParameter("password", strPassword));  // 綁定密碼參數

                        // 使用 using 語法來對資料庫執行操作命令
                        using (var reader = cmd.ExecuteReader())
                        {
                            //檢查是否有資料列
                            if (reader.HasRows)
                            {
                                isLoginSuccess = true; // 有資料的話，代表的確帳號密碼無誤，然後將是否登入成功的變數（isLoginSuccess）設定為「true」

                                // 利用一個迴圈來讀取每一筆資料
                                while (reader.Read())
                                {
                                    strUserName = reader["name"].ToString(); // 擷取欄位是「name」的資料，然後儲存到使用者姓名的變數（strUserName）
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // 如果中間的資料庫連線流程有問題，就會跳出錯誤訊息
            }
            finally
            {
                // 最後依照是否登入成功，顯示成功或失敗的訊息
                if (isLoginSuccess == true)
                    MessageBox.Show(strUserName + " 你好！你已經登入成功");
                else
                    MessageBox.Show("登入失敗");
            }
        }
    }
}
