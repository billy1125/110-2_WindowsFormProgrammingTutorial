using System;
using System.Collections.Generic;
using System.Data;
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

namespace Datatable
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

        // 資料庫連線字串，Server：資料庫管理系統的位址（IP或網址）、Uid：可以使用資料庫的帳號、Pwd：可以使用資料庫的密碼、database：要連線的資料庫名稱
        private string connstr = "Server=120.101.58.4;Uid=user;Pwd=user;database=login";
        // 建立一個資料表物件 dt，我們把資料庫中的資料抓出來之後，放到這個物件中，然後顯示在表格上
        private DataTable dt = new DataTable();

        // 取得所有資料表內容
        private void GetAllData()
        {
            try
            {
                // 使用 using 語法來進行資料庫連線
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open(); // 開啟資料庫連線

                    // 使用 using 語法來設定資料庫的操作命令(SQL指令)
                    using (var da = new MySqlDataAdapter())
                    {
                        string sqlSelectAll = "SELECT * FROM users";                // SQL指令
                        da.SelectCommand = new MySqlCommand(sqlSelectAll, conn);    // 執行查詢

                        dt.Clear();              // 清除資料表物件 
                        da.Fill(dt);             // 將查詢到的資料放到資料表物件
                        dgData.DataContext = dt; // 讓資料表物件顯示在表格裡面
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // 如果中間的資料庫連線流程有問題，就會跳出錯誤訊息
            }
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            GetAllData();
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected_index = dgData.SelectedIndex; // 取得你按的是第幾列的資料？

            // 因為要取得資料顯示在輸入文字框中，必須將在DataGrid中所選擇的列，與資料表物件的資料列位置彼此對應
            // 但可能DataGrid沒有資料顯示(selected_index = -1)，或者選到最下面的一列(selected_index = 資料表物件總列數)，造成與資料表物件無法相對應，會產生錯誤，因此必須要做判斷
            if (selected_index >= 0 && selected_index != dt.Rows.Count)
            {
                txtId.Text = dt.Rows[selected_index].ItemArray[0].ToString();
                txtName.Text = dt.Rows[selected_index].ItemArray[3].ToString();
                txtAccount.Text = dt.Rows[selected_index].ItemArray[1].ToString();
                txtPassword.Text = dt.Rows[selected_index].ItemArray[2].ToString();
                txtAdmin.Text = dt.Rows[selected_index].ItemArray[4].ToString();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    // 使用 using 語法來設定資料庫的操作命令
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE users SET account=@account, password=@password, name=@name WHERE id=@id", conn))
                    {                       
                        cmd.Parameters.AddWithValue("@account", txtAccount.Text);     // 將輸入文字框的資料與資料庫操作指令做對應
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@id", txtId.Text);
                        cmd.ExecuteNonQuery(); // 執行資料庫操作
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // 如果中間的資料庫連線流程有問題，就會跳出錯誤訊息
            }
            finally
            {
                GetAllData(); // 執行完操作後，重新整理資料
            }            
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 使用 using 語法來進行資料庫連線
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO users (account, password, name) VALUES (@account, @password, @name)", conn))
                    {
                        cmd.Parameters.AddWithValue("@account", txtAccount.Text);
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // 如果中間的資料庫連線流程有問題，就會跳出錯誤訊息
            }
            finally
            {
                GetAllData();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM users WHERE id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // 如果中間的資料庫連線流程有問題，就會跳出錯誤訊息
            }
            finally
            {
                GetAllData();
            }
        }
    }
}
