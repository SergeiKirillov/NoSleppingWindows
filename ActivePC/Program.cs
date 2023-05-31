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

        static void Main(string[] args)
        {
            #region NotifyIcon - Иконка в системном трее
            
            notifyIcon = new NotifyIcon();
            //notifyIcon.Icon = new System.Drawing.Icon(myIcon.ico);
            notifyIcon.Icon = Properties.Resources.red;
            notifyIcon.Text = "Не спать";

            // Создание контекстного меню для иконки
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Открыть", openCliked);
            contextMenu.MenuItems.Add("Выход", exitClicked);
            notifyIcon.ContextMenu = contextMenu;

            // Отображение иконки в системном трее
            notifyIcon.Visible = true;

            // Установка текста и обработчиков событий при наведении на иконку
            notifyIcon.MouseMove += NotifyIcon_MouseMove;


            #endregion

            // Создание и запуск фонового процесса
            Process backgroundProcess = new Process();
            backgroundProcess.StartInfo.FileName = "calc.exe";
            backgroundProcess.Start();

            // Предотвращение перехода в режим ожидания
            SetThreadExecutionState(ES_CONTINUOUS| ES_SYSTEM_REQUIRED| ES_DISPLAY_REQUIRED);
            while (true)
            {
                // Ваш код, выполняющийся в фоновом процессе
                if ((DateTime.Now.Hour==17)&&(DateTime.Now.Minute==00)) 
                {
                    notifyIcon.Icon = Properties.Resources.red;
                    backgroundProcess.Close();
                    // Восстановление нормального режима энергосбережения
                    SetThreadExecutionState(ES_CONTINUOUS);
                    Console.WriteLine("Восстановление нормального режима энергосбережения");
                    Console.WriteLine("Фоновый процесс завершен." + DateTime.Now.ToString());
                    //Console.ReadLine();

                    

                    //Environment.Exit(0); //Выходим из приложения
                }
                else 
                {
                    notifyIcon.Icon = Properties.Resources.green;
                }
                
                Thread.Sleep(60000);
                
            }

            notifyIcon.Visible = false;
            notifyIcon.Dispose();



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
    }
}
