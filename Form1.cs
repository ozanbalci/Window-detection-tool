using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PencereAlgılama.Form1;

namespace PencereAlgılama
{
    public partial class Form1 : Form
    {

        // TR  => "Bu kod parçacığı, oyun penceresinin pencere modunda mı yoksa tam ekran modunda mı olduğunu ayıran bir modüldür ve kullanımı kolaydır, isteyenler rahatlıkla kullanabilir."
        // ENG => "This code snippet is a module that distinguishes whether the game window is windowed or fullscreen, and it is easy to use, so feel free to use it."

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Oyunun process adını belirleyin
            // Set the process name of the game
            //  sample code ==> string processName = "csgo"; 
            string processName = "";

            // Oyunun Process nesnesini alma işlemi
            //The process of getting the game's Process object
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                // Oyun açık değil
                // The game is not open
                MessageBox.Show("The game is not open.");
                return;
            }

            // Oyunun pencere modunu alma işlemi
            // The process of getting the window mode of the game
            WindowMode windowMode = GetWindowMode(processes[0]);

            // Pencere modunu ekranda gösterin
            // Show window mode on screen
            string modeString = windowMode.ToString();
            MessageBox.Show($"Pencere modu: {modeString}");
        }

        #region Window detection

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left, Top, Right, Bottom;
        }


        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public enum WindowMode
        {
            Unknown,
            Fullscreen,
            Windowed
        }

        public WindowMode GetWindowMode(Process process)
        {
            IntPtr hWnd = process.MainWindowHandle;

            RECT windowRect;
            GetWindowRect(hWnd, out windowRect);

            RECT clientRect;
            GetClientRect(hWnd, out clientRect);

            int windowWidth = windowRect.right - windowRect.left;
            int windowHeight = windowRect.bottom - windowRect.top;

            int clientWidth = clientRect.right - clientRect.left;
            int clientHeight = clientRect.bottom - clientRect.top;

            if (windowWidth == clientWidth && windowHeight == clientHeight)
            {
                return WindowMode.Fullscreen;
            }
            else if (windowWidth != clientWidth || windowHeight != clientHeight)
            {
                return WindowMode.Windowed;
            }
            else
            {
                return WindowMode.Unknown;
            }
        }

        #endregion
    }
}
