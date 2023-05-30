using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

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

        static void Main(string[] args)
        {
            // Создание и запуск фонового процесса
            Process backgroundProcess = new Process();
            backgroundProcess.StartInfo.FileName = "calc.exe";
            backgroundProcess.Start();

            // Предотвращение перехода в режим ожидания
            SetThreadExecutionState(ES_CONTINUOUS| ES_SYSTEM_REQUIRED| ES_DISPLAY_REQUIRED);
            while (true)
            {
                // Ваш код, выполняющийся в фоновом процессе
                if ((DateTime.Now.Hour==15)&&(DateTime.Now.Minute==30)) 
                {
                    backgroundProcess.Close();
                    // Восстановление нормального режима энергосбережения
                    SetThreadExecutionState(ES_CONTINUOUS);
                    //Console.WriteLine("Фоновый процесс завершен." + DateTime.Now.ToString());
                    //Console.ReadLine();
                    Environment.Exit(0);
                }
                
            }

            

            

        }
    }
}
