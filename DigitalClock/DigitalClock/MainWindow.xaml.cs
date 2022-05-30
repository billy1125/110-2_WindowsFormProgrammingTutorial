﻿using System;
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
using System.Diagnostics;       // 引用「系統診斷」的函式庫

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

        List<string> StopWatchLog = new List<string>();         // 碼表紀錄清單 
        DispatcherTimer timerStopWatch = new DispatcherTimer(); // 宣告一個「碼表」計時器
        Stopwatch sw = new Stopwatch();                         // 宣告一個碼表物件

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

            // 設定「碼表時間更新」計時器  
            timerStopWatch.Interval = TimeSpan.FromMilliseconds(1);        // 這個計時器設定每一個刻度為「1毫秒」
            timerStopWatch.Tick += new EventHandler(timerStopWatch_tick);  // 每一個時間刻度設定一個小程序timerStopWatch_tick

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

        // timerStopWatch_tick：每毫秒執行一次，所以更新的速度會比較快
        private void timerStopWatch_tick(object sender, EventArgs e)
        {
            txtStopWatch.Text = sw.Elapsed.ToString("hh':'mm':'ss':'fff");    // 顯示碼表時間
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

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            sw.Start();             // 啟動碼表
            timerStopWatch.Start(); // 開始讓碼表文字顯示
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            sw.Reset();                           // 停止並歸零碼表
            timerStopWatch.Stop();                // 停止讓碼表文字顯示     
            txtStopWatch.Text = "00:00:00:000";   // 讓碼表文字「歸零」
            txtStopWatchLog.Text = "";            // 清除紀錄表
            StopWatchLog.Clear();                 // 清除暫存碼表紀錄清單
        }

        // 歸零按鍵會判斷你是否先按下暫停？來決定是否記錄碼表時間
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // 如果碼表還在跑，就紀錄目前的時間，最後歸零再啟動碼錶
            if (sw.IsRunning)
            {
                txtStopWatchLog.Text = "";
                // 判斷暫存碼表紀錄清單是不是已經紀錄10次？如果是，就把原本第一筆資料刪除，然後才增加新的一筆紀錄
                if (StopWatchLog.Count == 10)
                    StopWatchLog.RemoveAt(0);
                StopWatchLog.Add(txtStopWatch.Text); // 將碼表時間增加到暫存碼表紀錄清單裡

                // 依照碼表紀錄清單「依照最新時間順序」顯示
                int i = StopWatchLog.Count;
                while (i > 0)
                {
                    txtStopWatchLog.Text += String.Format("第 {0} 筆紀錄：{1}", i.ToString(), StopWatchLog[i - 1] + "\n");
                    i--;
                }
                sw.Restart(); // 歸零碼表，碼表仍繼續進行  
            }
            else
            {
                sw.Reset(); // 如果碼表沒在跑，停止並歸零碼表
                txtStopWatch.Text = "00:00:00:000";
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            sw.Stop();                  // 停止碼表，但不歸零
            timerStopWatch.Stop();      // 停止讓碼表文字顯示  
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            txtStopWatchLog.Text = "";
            if (StopWatchLog.Count == 10)
                StopWatchLog.RemoveAt(0);
            StopWatchLog.Add(txtStopWatch.Text);

            int i = StopWatchLog.Count;
            while (i > 0)
            {
                txtStopWatchLog.Text += String.Format("第 {0} 筆紀錄：{1}", i.ToString(), StopWatchLog[i - 1] + "\n");
                i--;
            }
        }
    }
}
