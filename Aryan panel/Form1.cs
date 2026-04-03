using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FF;
using Memory;


namespace Aryan_panel
{
    public partial class Form1 : Form
    {



        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AIMBOT DATA.txt");
        private readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AIMBOT DATA .txt");
        private Thread monitorThread;
        private bool isMonitoring = true;

        private static FAHIM PLAYBOX = new FAHIM();
        private string aimAOB = "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
        private string readFORhead = "0xAA";
        private string write = "0xA6";
        private Dictionary<long, int> originalvalues = new Dictionary<long, int>();
        private Dictionary<long, int> originallvalues = new Dictionary<long, int>();
        private Dictionary<long, int> originalvalues2 = new Dictionary<long, int>();
        private Dictionary<long, int> originallvalues2 = new Dictionary<long, int>();





        Mem Memory = new Mem();
        public Form1()
        {
            InitializeComponent();

        }
 

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            originalvalues.Clear();
            originallvalues.Clear();
            originalvalues2.Clear();
            originallvalues2.Clear();
            Sta1.Text = "Applying...";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Int64 readOffset = Convert.ToInt64(readFORhead, 16);
            Int64 writeOffset = Convert.ToInt64(write, 16);
            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            PLAYBOX.OpenProcess(proc);

            var result = await PLAYBOX.AoBScan2(aimAOB, true, true);


            List<long> resultList = result.ToList();


            using (StreamWriter writer = new StreamWriter(logFilePath, false))
            {
                writer.WriteLine($"Total Patterns Found: {resultList.Count}");
                writer.WriteLine("=========================================");

                if (resultList.Count != 0)
                {
                    foreach (var CurrentAddress in resultList)
                    {
                        writer.WriteLine($"=========================================");
                        writer.WriteLine($"Pattern Found at Address: 0x{CurrentAddress:X}");
                        writer.WriteLine("=========================================");
                        writer.WriteLine($"Full Array of Bytes:");
                        writer.WriteLine(aimAOB);
                        writer.WriteLine();

                        writer.WriteLine("Replacements:");

                        Int64 AddressToSave = CurrentAddress + writeOffset;
                        var currentBytes = PLAYBOX.readMemory(AddressToSave.ToString("X"), sizeof(int));
                        int currentValue = BitConverter.ToInt32(currentBytes, 0);
                        originalvalues[AddressToSave] = currentValue;

                        Int64 addressToSave9 = CurrentAddress + readOffset;
                        var currentBytes9 = PLAYBOX.readMemory(addressToSave9.ToString("X"), sizeof(int));
                        int currentValue9 = BitConverter.ToInt32(currentBytes9, 0);
                        originallvalues[addressToSave9] = currentValue9;

                        writer.WriteLine($"  Address: 0x{AddressToSave:X}");
                        writer.WriteLine($"    Original Value (Dec): {currentValue}");
                        writer.WriteLine($"    Original Value (Hex): 0x{currentValue:X}");
                        writer.WriteLine($"    Replaced Value (Dec): {currentValue9}");
                        writer.WriteLine($"    Replaced Value (Hex): 0x{currentValue9:X}");
                        writer.WriteLine();


                        PLAYBOX.WriteMemory(addressToSave9.ToString("X"), "int", currentValue.ToString());
                        PLAYBOX.WriteMemory(AddressToSave.ToString("X"), "int", currentValue9.ToString());

                        writer.WriteLine($"  Address: 0x{addressToSave9:X}");
                        writer.WriteLine($"    Original Value (Dec): {currentValue9}");
                        writer.WriteLine($"    Original Value (Hex): 0x{currentValue9:X}");
                        writer.WriteLine($"    Replaced Value (Dec): {currentValue}");
                        writer.WriteLine($"    Replaced Value (Hex): 0x{currentValue:X}");
                        writer.WriteLine();
                    }
                }
                else
                {
                    writer.WriteLine("No patterns found.");
                }


                stopwatch.Stop();
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                Console.Beep();
                Sta1.Text = $"Successful, Time: {elapsedSeconds:F2} Seconds;";
            }


        }



        private static void ExtractEmbeddedResource(string resourceName, string outputPath)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            // Get the embedded resource stream
            using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException($"Resource '{resourceName}' not found.");
                }

                // Read the embedded resource and save it to the specified path
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    byte[] buffer = new byte[resourceStream.Length];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        const uint PROCESS_CREATE_THREAD = 0x2;
        const uint PROCESS_QUERY_INFORMATION = 0x400;
        const uint PROCESS_VM_OPERATION = 0x8;
        const uint PROCESS_VM_WRITE = 0x20;
        const uint PROCESS_VM_READ = 0x10;

        const uint MEM_COMMIT = 0x1000;
        const uint PAGE_READWRITE = 4;
        private void guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            string processName = "HD-Player"; // Specify your target process name
            string dllResourceName = "Aryan_panel.CHAMS MENU.dll"; // Correct resource name

            // Extract the embedded msdrmi.dll to a temporary file
            string tempDllPath = Path.Combine(Path.GetTempPath(), "CHAMS MENU.dll");
            ExtractEmbeddedResource(dllResourceName, tempDllPath);

            Console.WriteLine($"DLL extracted successfully to: {tempDllPath}");


            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                Console.WriteLine($"Waiting for {processName}.exe...");
            }
            else
            {
                Process targetProcess = targetProcesses[0];
                IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);

                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)tempDllPath.Length, MEM_COMMIT, PAGE_READWRITE);

                IntPtr bytesWritten;
                WriteProcessMemory(hProcess, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(tempDllPath), (uint)tempDllPath.Length, out bytesWritten);

                CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);

                Console.Beep(240, 300);
                //Type Here Chams Is Already Injected or code invaible

            }
        }

        private async void guna2ToggleSwitch3_CheckedChanged(object sender, EventArgs e)
        {


            if (Process.GetProcessesByName("HD-Player").Length == 0)
            {
                Snp.Text = "emulator not found";
                Console.Beep(240, 300);
            }
            else
            {
                Snp.Text = "wait... aryan";

                string search = "08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40 00 00 B0 3F 00 00 80 3F 01";
                string replace = "08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 80 3f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 33 13 40 00 00 B0 3F 00 00 80 3F 01";

                bool k = false;
                Memory.OpenProcess("HD-Player");

                int i2 = 22000000;
                IEnumerable<long> wl = await Memory.AoBScan(search, writable: true);
                string u = "0x" + wl.FirstOrDefault().ToString("X");
                if (wl.Count() != 0)
                {
                    for (int i = 0; i < wl.Count(); i++)
                    {
                        i2++;
                        Memory.WriteMemory(wl.ElementAt(i).ToString("X"), "bytes", replace);
                    }
                    k = true;
                }

                if (k == true)
                {
                    Console.Beep(400, 300);
                    Snp.Text = "working found";

                }
                else
                {
                    Snp.Text = "working not found";
                    Console.Beep(240, 300);
                }
            }

        }

        private void Sta1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/txpFF");
            linkLabel1.LinkVisited = true;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
