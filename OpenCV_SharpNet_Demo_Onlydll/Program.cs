using OpenCV_SharpNet_Demo.UI;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OpenCV_SharpNet_Demo
{
    internal static class Program
    {
        // Add this at the top of your file
        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);


        // 1. Map the original assembly name to your fake hash name
        private static readonly Dictionary<string, string> DllNameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OpenCvSharp", "Core.dll" },
            { "OpenCvSharp.Extensions", "Core.Extensions.dll" },
            { "ZXingCpp", "Core.Bar.dll" },
        };

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //// 1. Tell Windows to look in 'CSPL_OCRLibs' for Native (C++) DLLs
            //string libsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSPL_OCRLibs");
            //SetDllDirectory(libsFolder);

            //// 2. Tell .NET to look in 'CSPL_OCRLibs' for Managed (C#) DLLs
            //AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            //    {
            //        string assemblyName = new AssemblyName(args.Name).Name + ".dll";
            //        string assemblyPath = Path.Combine(libsFolder, assemblyName);
            //        return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
            //    };

            //try
            //{
            //    StartApp();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "CRASH LOG");
            //}


            string libsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSPL_OCRLibs");

            // Keep this just in case OpenCvSharp needs to load its native Extern file
            [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
            static extern bool SetDllDirectory(string lpPathName);
            SetDllDirectory(libsFolder);

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string originalName = new AssemblyName(args.Name).Name;
                string fileNameToLoad = originalName + ".dll";

                // If the requested DLL is in our list, use the fake hash name instead
                if (DllNameMap.ContainsKey(originalName))
                {
                    fileNameToLoad = DllNameMap[originalName];
                }

                string assemblyPath = Path.Combine(libsFolder, fileNameToLoad);
                return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
            };

            try { StartApp(); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "CRASH LOG"); }
        }

        // 3. Keep all UI and forms logic in a separate method that cannot be inlined.
        // This ensures the JIT compiler doesn't look for the DLLs until AFTER the 
        // AssemblyResolve event is registered.
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void StartApp()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // 1. Show the beautiful new startup form
            using (var startup = new FrmRedirect())
            {
                // If the user clicks 'X' or closes it, exit the app entirely
                if (startup.ShowDialog() != DialogResult.OK)
                    return;

                // 2. Check which card they clicked, and load the correct form!
                if (startup.SelectedMode == "IMAGE")
                    Application.Run(new MainForm()); // Your Image Form
                else if (startup.SelectedMode == "CAMERA")
                    Application.Run(new CameraDemo()); // Your Camera Form
            }
        }
    }
}