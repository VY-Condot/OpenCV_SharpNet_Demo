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


    //    // 1. Map the original assembly name to your fake hash name
    //    private static readonly Dictionary<string, string> DllNameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    //{
    //    { "ClosedXML", "a3f8b9c2.dll" },
    //    { "ClosedXML.Parser", "b7d4e1f5.dll" },
    //    { "DocumentFormat.OpenXml", "c9a2b4d6.dll" },
    //    { "DocumentFormat.OpenXml.Framework", "d1c8e7f9.dll" },
    //    { "ExcelNumberFormat", "e5f2a1b9.dll" },
    //    { "OpenCvSharp", "f8b3c7d1.dll" },
    //    { "OpenCvSharp.Extensions", "a1d9e2f4.dll" },
    //    { "RBush", "b6c3d8e1.dll" },
    //    { "SixLabors.Fonts", "c4e7f2a9.dll" },
    //    { "System.Drawing.Common", "d9f1a8b2.dll" },
    //    { "System.IO.Packaging", "e2b5c9d4.dll" },
    //    { "System.Runtime.WindowsRuntime", "f3a7d2e8.dll" },
    //    { "ZXingCpp", "a8c4e1f7.dll" }
    //};

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 1. Tell Windows to look in 'CSPL_OCRLibs' for Native (C++) DLLs
            string libsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSPL_OCRLibs");
            SetDllDirectory(libsFolder);

            // 2. Tell .NET to look in 'CSPL_OCRLibs' for Managed (C#) DLLs
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    string assemblyName = new AssemblyName(args.Name).Name + ".dll";
                    string assemblyPath = Path.Combine(libsFolder, assemblyName);
                    return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
                };

            try
            {
                StartApp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CRASH LOG");
            }


            //string libsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSPL_OCRLibs");

            //// Keep this just in case OpenCvSharp needs to load its native Extern file
            //[System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
            //static extern bool SetDllDirectory(string lpPathName);
            //SetDllDirectory(libsFolder);

            //AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            //{
            //    string originalName = new AssemblyName(args.Name).Name;
            //    string fileNameToLoad = originalName + ".dll";

            //    // If the requested DLL is in our list, use the fake hash name instead
            //    if (DllNameMap.ContainsKey(originalName))
            //    {
            //        fileNameToLoad = DllNameMap[originalName];
            //    }

            //    string assemblyPath = Path.Combine(libsFolder, fileNameToLoad);
            //    return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
            //};

            //try { StartApp(); }
            //catch (Exception ex) { MessageBox.Show(ex.ToString(), "CRASH LOG"); }
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