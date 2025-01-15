using JDBrowser;

namespace JDBrowser
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // 设置应用程序图标
            SetApplicationIcon("icon.ico");

            Application.Run(new JDBrowser());
        }

        private static void SetApplicationIcon(string iconPath)
        {
            if (System.IO.File.Exists(iconPath) && Application.OpenForms.Count > 0)
            {
                Icon icon = new Icon(iconPath);
                var form = Application.OpenForms[0];
                if (form != null)
                {
                    typeof(Form).GetProperty("Icon")?.SetValue(form, icon, null);
                }
            }
        }
    }
}