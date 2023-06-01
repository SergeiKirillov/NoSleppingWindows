using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading;
using MyLibenNetFramework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Management;

namespace ActivePC
{
    internal class Program
    {
        // Константы для функции SetThreadExecutionState
        private const uint ES_CONTINUOUS = 0x80000000; //0x80000000; 
        private const uint ES_SYSTEM_REQUIRED = 0x00000001; //не дает системе перейти в спящий режим
        private const uint ES_DISPLAY_REQUIRED = 0x00000002; //отвечает за активность дисплея.

        // Импортирование функции SetThreadExecutionState из библиотеки kernel32
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(uint esFlags);

        static NotifyIcon notifyIcon;

        public static ContextMenu menu;
        public static MenuItem mnuExit;
        public static MenuItem mnuStart;
        public static MenuItem mnuStop;
        public static NotifyIcon notificationIcon;

        static WorkingReestr reestr;

        static void Main(string[] args)
        {
            reestr = new WorkingReestr("ActivePC");

            System.Diagnostics.Debug.WriteLine("Старт программы НЕ СПАТЬ!!");


            if (isStillRunning()) //Если запустили второй раз то показываем форму ввода пароля
            {
                System.Diagnostics.Debug.WriteLine("второй раз");

                frmSetting setting = new frmSetting();

                if (setting.Visible)
                {
                    setting.Focus();
                }
                else
                {
                    setting.ShowDialog();         
                }

            }
            else
            {
                #region //NotifyIcon 1 - Иконка в системном трее

                //notifyIcon = new NotifyIcon();
                ////notifyIcon.Icon = new System.Drawing.Icon(myIcon.ico);
                //notifyIcon.Icon = Properties.Resources.red;
                //notifyIcon.Text = "Не спать";

                //// Создание контекстного меню для иконки
                //ContextMenu contextMenu = new ContextMenu();
                //contextMenu.MenuItems.Add("Открыть", openCliked);
                //contextMenu.MenuItems.Add("Выход", exitClicked);
                //notifyIcon.ContextMenu = contextMenu;


                //// Установка текста и обработчиков событий при наведении на иконку
                //notifyIcon.MouseMove += NotifyIcon_MouseMove;

                //// Отображение иконки в системном трее
                //notifyIcon.Visible = true;





                #endregion

                #region NotifyIcon 2 - Иконка в системном трее
                Thread notifyThred = new Thread(
                    delegate ()
                    {
                        menu = new ContextMenu();
                        mnuExit = new MenuItem("Exit");
                        mnuStart = new MenuItem("Старт");
                        mnuStop = new MenuItem("Стоп");
                        menu.MenuItems.Add(0, mnuExit);
                        menu.MenuItems.Add(1, mnuStart);
                        menu.MenuItems.Add(2, mnuStop);

                        notificationIcon = new NotifyIcon()
                        {
                            Icon = Properties.Resources.green,
                            ContextMenu = menu,
                            Text = "Main"
                        };
                        mnuExit.Click += new EventHandler(mnuExit_Click);
                        mnuStart.Click += new EventHandler(MnuStart_Click);
                        mnuStop.Click += new EventHandler(MnuStop_Click);

                        notificationIcon.Visible = true;
                        Application.Run();
                    }
                    );
                notifyThred.Start();
                #endregion

                clWinAPI.HideConsoleApp(true); //Прячем программу

                // Создание и запуск фонового процесса
                Process backgroundProcess = new Process();

                backgroundProcess.StartInfo.FileName = "calc.exe";
                backgroundProcess.Start();


                //bool blStat = true;
                reestr.SetBool("blStat", true);

                // Предотвращение перехода в режим ожидания
                SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
                while (true)
                {
                    // Ваш код, выполняющийся в фоновом процессе
                    if ((DateTime.Now.Hour == 12) && (DateTime.Now.Minute == 35))
                    {
                        reestr.SetBool("blStat", false);
                    }
                    else
                    {
                        // blStat = true;
                    }

                    if (reestr.GetBool("blStat"))
                    {
                        //notifyIcon.Icon = Properties.Resources.green;
                        notificationIcon.Icon = Properties.Resources.green;
                    }
                    else
                    {
                        //notifyIcon.Icon = Properties.Resources.red;
                        notificationIcon.Icon = Properties.Resources.red;
                        backgroundProcess.Close();
                        // Восстановление нормального режима энергосбережения
                        SetThreadExecutionState(ES_CONTINUOUS);
                        System.Diagnostics.Debug.WriteLine("Восстановление нормального режима энергосбережения");
                        System.Diagnostics.Debug.WriteLine("Фоновый процесс завершен." + DateTime.Now.ToString());
                        //Console.ReadLine();



                        //Environment.Exit(0); //Выходим из приложения
                    }

                    Thread.Sleep(60000);

                }

                //notifyIcon.Visible = false;
                //notifyIcon.Dispose();

                //notificationIcon.Visible= false;
                //notificationIcon.Dispose();
                //Environment.Exit(0);


            }


        }

        private static void MnuStop_Click(object sender, EventArgs e)
        {
            reestr.SetBool("blStat", false);
        }

        private static void MnuStart_Click(object sender, EventArgs e)
        {
            reestr.SetBool("blStat", true);
        }

        private static void mnuExit_Click(object sender, EventArgs e)
        {
            notificationIcon.Visible = false;
            notificationIcon.Dispose();
            Environment.Exit(0);
        }

        private static void NotifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            // Обновление текста при наведении на иконку
            notifyIcon.Text = "Статус программы: активна";
        }

        private static void exitClicked(object sender, EventArgs e)
        {
            
            Console.WriteLine("Пункт меню 'Выход' выбран.");
            Environment.Exit(0);
        }

        private static void openCliked(object sender, EventArgs e)
        {
            Console.WriteLine("Пункт меню 'Открыть' выбран.");
        }

        static bool isStillRunning() //Проверка есть ли приложение в памяти
        {
            string processName = Process.GetCurrentProcess().MainModule.ModuleName;
            ManagementObjectSearcher mos = new ManagementObjectSearcher();
            mos.Query.QueryString = @"SELECT * FROM Win32_Process WHERE Name = '" + processName + @"'";
            if (mos.Get().Count > 1)
            {
                return true;
            }
            else
                return false;
        }
    }
}
