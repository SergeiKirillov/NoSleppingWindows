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
        public static MenuItem mnuSetting;
        public static NotifyIcon notificationIcon;

        static WorkingReestr reestr;

        static bool exitRequested = false;


        static void Main(string[] args)
        {
            try
            {

                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true; // Предотвращает завершение приложения сразу после обработки сигнала
                    exitRequested = true; // Устанавливает флаг завершения
                };

                AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
                {
                    // Код, который должен выполниться перед завершением приложения
                    //System.Diagnostics.Debug.WriteLine("Завершение программы программы НЕ СПАТЬ!!");
                    MyIOFile.WriteFileTXT(DateTime.Now, "Завершение программы сигналом EXIT", "ActivePC");
                    notificationIcon.Dispose();
                };


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
                            mnuSetting = new MenuItem("Настройка");
                            mnuStart = new MenuItem("Старт");
                            mnuStop = new MenuItem("Стоп");
                            mnuStart.Enabled = false;
                            mnuStop.Enabled = true;
                            menu.MenuItems.Add(0, mnuExit);
                            menu.MenuItems.Add(1, mnuSetting);
                            menu.MenuItems.Add(2, mnuStart);
                            menu.MenuItems.Add(3, mnuStop);

                            notificationIcon = new NotifyIcon()
                            {
                                Icon = Properties.Resources.green,
                                //Icon = reestr.GetBool("blStat") ? Properties.Resources.green : Properties.Resources.red, //https://stackoverflow.com/questions/90697/how-to-create-and-use-resources-in-net
                                ContextMenu = menu,
                                Text = "Main"
                            };
                            mnuExit.Click += new EventHandler(mnuExit_Click);
                            mnuSetting.Click += new EventHandler(mnuSetting_Click);
                            mnuStart.Click += new EventHandler(MnuStart_Click);
                            mnuStop.Click += new EventHandler(MnuStop_Click);

                            notificationIcon.Visible = true;
                            Application.Run();
                        }
                        );
                    notifyThred.Start();
                    #endregion

                    clWinAPI.HideConsoleApp(true); //Прячем программу

                    //// Создание и запуск фонового процесса //----------------------- АРР
                    //Process backgroundProcess = new Process();//----------------------- АРР

                    //backgroundProcess.StartInfo.FileName = "calc.exe";//----------------------- АРР
                    //backgroundProcess.Start();//----------------------- АРР


                    //bool blStat = true;
                    reestr.SetBool("blStat", true);
                    //notificationIcon.Icon?.Dispose();
                    //notificationIcon.Icon = reestr.GetBool("blStat") ? Properties.Resources.green : Properties.Resources.red; //https://stackoverflow.com/questions/90697/how-to-create-and-use-resources-in-net
                    MyIOFile.WriteFileTXT(DateTime.Now, "СTAРТ", "ActivePC");

                    // Предотвращение перехода в режим ожидания
                    SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
                    while (true)
                    {

                        // Ваш код, выполняющийся в фоновом процессе
                        //if ((DateTime.Now.Hour == 8) && (DateTime.Now.Minute == 00))
                        //{
                        //    reestr.SetBool("blStat", false);

                        //}
                        //else
                        //{
                        //    // blStat = true;
                        //}

                        if (reestr.GetBool("8Hour"))
                        {
                            if ((DateTime.Now.Hour == 17) && (DateTime.Now.Minute == 00))
                            {
                                reestr.SetBool("blStat", false);

                                MyIOFile.WriteFileTXT(DateTime.Now, "Завершение программы в 17", "ActivePC");

                            }
                        }

                        if (reestr.GetBool("12Hour"))
                        {
                            if (((DateTime.Now.Hour == 7) || (DateTime.Now.Hour == 19)) && (DateTime.Now.Minute == 00))
                            {
                                reestr.SetBool("blStat", false);

                                MyIOFile.WriteFileTXT(DateTime.Now, "Завершение программы в 7/19", "ActivePC");
                            }
                        }

                        //notificationIcon.Icon?.Dispose();

                        if (notificationIcon !=null) //Обработка ошибки  System.NullReferenceException: "Ссылка на объект не указывает на экземпляр объекта."
                        {
                            notificationIcon.Icon = reestr.GetBool("blStat") ? Properties.Resources.green : Properties.Resources.red; //https://stackoverflow.com/questions/90697/how-to-create-and-use-resources-in-net
                        }
                        
                        if (reestr.GetBool("blStat"))
                        {
                            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
                            mnuStart.Enabled = false;
                            mnuStop.Enabled = true;

                        }
                        else
                        {
                            //backgroundProcess.Close(); ----------------------- АРР
                            // Восстановление нормального режима энергосбережения
                            SetThreadExecutionState(ES_CONTINUOUS);
                            //      System.Diagnostics.Debug.WriteLine("Восстановление нормального режима энергосбережения");
                            //      System.Diagnostics.Debug.WriteLine("Фоновый процесс завершен." + DateTime.Now.ToString());
                            //      Console.ReadLine();


                            mnuStart.Enabled = true;
                            mnuStop.Enabled = false;

                            //Environment.Exit(0); //Выходим из приложения
                        }
                    
                        Thread.Sleep(60000);//1min

                    }

                    //notifyIcon.Visible = false;
                    //notifyIcon.Dispose();

                    //notificationIcon.Visible= false;
                    //notificationIcon.Dispose();
                    //Environment.Exit(0);



                    // Далее, в необходимых местах, проверяйте флаг exitRequested и завершайте приложение, если он установлен
                    if (exitRequested)
                    {
                        // Код завершения приложения
                        //System.Diagnostics.Debug.WriteLine("Завершение программы сигналом из системы НЕ СПАТЬ!!");
                        MyIOFile.WriteFileTXT(DateTime.Now, "Завершение программы сигналом из системы ", "ActivePC");
                        notificationIcon.Dispose();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                notificationIcon.Dispose();
            }


        }

        private static void mnuSetting_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("второй раз");
            MyIOFile.WriteFileTXT(DateTime.Now, "Запуск окна настройки программы", "ActivePC");

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

        private static void MnuStop_Click(object sender, EventArgs e)
        {
            reestr.SetBool("blStat", false);
            mnuStart.Enabled = true;
            mnuStop.Enabled = false;
            MyIOFile.WriteFileTXT(DateTime.Now, "Стоп", "ActivePC");
        }

        private static void MnuStart_Click(object sender, EventArgs e)
        {
            reestr.SetBool("blStat", true);
            mnuStart.Enabled = false;
            mnuStop.Enabled = true;
            MyIOFile.WriteFileTXT(DateTime.Now, "Старт", "ActivePC");
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
