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
using System.Windows.Threading; // 要加入這段才能使用計時器

namespace DigitalClock
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> hours = new List<string>();            // 小時清單
        List<string> minutes = new List<string>();          // 分鐘清單 
        DispatcherTimer timer = new DispatcherTimer();      // 宣告一個「時鐘」計時器

        string strSelectTime = "";   // 用來記錄鬧鐘設定時間
        DispatcherTimer timerAlert = new DispatcherTimer(); // 宣告一個「鬧鐘」計時器

        // 程式初始化動作
        public MainWindow() 
        {
            InitializeComponent();

            // 建立小時的清單，數字範圍為00-23
            for (int i = 0; i <= 23; i++)
                hours.Add(string.Format("{0:00}", i));
            // 建立分鐘的清單，數字範圍為00-59
            for (int i = 0; i <= 59; i++)
                minutes.Add(string.Format("{0:00}", i));

            // 設定小時下拉選單的選單內容
            cmbHour.ItemsSource = hours;
            // 設定分鐘下拉選單的選單內容
            cmbMin.ItemsSource = minutes;

            // 設定「時鐘」計時器  
            timer.Interval = TimeSpan.FromSeconds(1);   // 這個計時器設定每一個刻度為1秒
            timer.Tick += new EventHandler(timer_tick); // 每一個時間刻度設定一個小程序timer_tick
            timer.Start(); // 啟動這個計時器

            // 設定「鬧鐘」計時器  
            timerAlert.Interval = TimeSpan.FromSeconds(1);        // 這個計時器設定每一個刻度為1秒
            timerAlert.Tick += new EventHandler(timerAlert_tick); // 每一個時間刻度設定一個小程序timerAlert_tic

            meSound.LoadedBehavior = MediaState.Stop; // 關閉鬧鐘聲音
        }

        // timer_tick事件：每一秒執行一次
        private void timer_tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");    // 顯示時間
            txtDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");  // 顯示日期
            txtWeekDay.Text = DateTime.Now.ToString("dddd");     // 顯示星期幾
        }

        // timerAlert_tick事件：每一秒執行一次
        private void timerAlert_tick(object sender, EventArgs e)
        {
            // 判斷現在時間是不是已經是鬧鐘設定時間？如果時間到了，就要播放鬧鐘聲音
            if (strSelectTime == DateTime.Now.ToString("HH:mm"))
            {
                meSound.LoadedBehavior = MediaState.Play; // 開啟鬧鐘聲音
                timerAlert.Stop(); // 停止鬧鐘計時器
            }
        }

        private void btnSetAlert_Click(object sender, RoutedEventArgs e)
        {
            timerAlert.Start(); // 啟動鬧鐘計時器
            btnSetAlert.IsEnabled = false;
            btnCancelAlert.IsEnabled = true;
            strSelectTime = cmbHour.SelectedItem + ":" + cmbMin.SelectedItem; // 擷取小時和分鐘的下拉選單文字，用來設定鬧鐘時間
        }

        private void btnCancelAlert_Click(object sender, RoutedEventArgs e)
        {
            meSound.LoadedBehavior = MediaState.Stop; // 關閉鬧鐘聲音
            timerAlert.Stop(); // 停止鬧鐘計時器
            btnSetAlert.IsEnabled = true;
            btnCancelAlert.IsEnabled = false;
        }

        // 讓鬧鐘聲音可以重複播放
        private void meSound_MediaEnded(object sender, RoutedEventArgs e)
        {
            meSound.Position = new TimeSpan(0, 0, 1); // 把播放時間一到一開始的播放時間
            meSound.LoadedBehavior = MediaState.Play; // 播放鬧鐘聲音
        }
    }
}
