using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.ServiceProcess;
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

namespace WIndowsServiceInstaller
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool InstallStatus
        {
            get { return (bool)GetValue(InstallStatusProperty); }
            set { SetValue(InstallStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InstallBtnStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InstallStatusProperty =
            DependencyProperty.Register("InstallStatus", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public string ServiceStatusContent
        {
            get { return (string)GetValue(ServiceStatusContentProperty); }
            set { SetValue(ServiceStatusContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ServiceStatusContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServiceStatusContentProperty =
            DependencyProperty.Register("ServiceStatusContent", typeof(string), typeof(MainWindow), new PropertyMetadata("未安装"));

        public string ServiceName
        {
            get { return (string)GetValue(ServiceNameProperty); }
            set { SetValue(ServiceNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ServiceName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServiceNameProperty =
            DependencyProperty.Register("ServiceName", typeof(string), typeof(MainWindow), new PropertyMetadata(""));

        public static string ThisServiceName { get; set; }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            InstallService("TestService.exe");
        }

        private void btnUninstall_Click(object sender, RoutedEventArgs e)
        {
            UninstallService("TestService.exe");
        }

        //安装服务
        private void InstallService(string serviceFilePath)
        {
            using (var installer = new AssemblyInstaller())
            {
                try
                {
                    installer.UseNewContext = true;
                    installer.Path = serviceFilePath;
                    IDictionary savedState = new Hashtable();
                    installer.Install(savedState);
                    installer.Commit(savedState);
                    MessageBox.Show("安装成功");
                    InitServiceInfo();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }

        //卸载服务
        private void UninstallService(string serviceFilePath)
        {
            using (var installer = new AssemblyInstaller())
            {
                try
                {
                    installer.UseNewContext = true;
                    installer.Path = serviceFilePath;
                    installer.Uninstall(null);
                    MessageBox.Show("卸载成功!");
                    InitServiceInfo();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetRootDirServiceName();
            InitServiceInfo();
        }

        private static void GetRootDirServiceName()
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory).Where(w => w.ToLower().EndsWith(".exe") || w.ToLower().EndsWith(".dll"));

            var serviceBase = typeof(ServiceBase);

            foreach (var item in files)
            {
                var assembly = Assembly.LoadFile(item);
                var type = assembly.GetTypes().FirstOrDefault(f => f.BaseType == serviceBase);
                if (type != null)
                {
                    var service = (ServiceBase)assembly.CreateInstance(type.FullName);
                    ThisServiceName = service.ServiceName;
                    break;
                };
            }

            if (ThisServiceName == null)
            {
                MessageBox.Show("程序所在目录不存在windows服务，请检查后再运行");
                Application.Current.Shutdown();
            }
        }

        private void InitServiceInfo()
        {
            try
            {
                var service = GetService();
                ServiceName = service.ServiceName;
                ServiceStatusContent = service.Status.ToString();
                InstallStatus = true;
            }
            catch (Exception)
            {
                ServiceStatusContent = "uninstall";
                ServiceName = ThisServiceName;
                InstallStatus = false;
            }
        }

        private static ServiceController GetService()
        {
            var services = ServiceController.GetServices().FirstOrDefault(f => f.ServiceName == ThisServiceName);
            if (services == null) throw new KeyNotFoundException();
            return services;
        }

        private void OpenRootDir(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Environment.CurrentDirectory);
        }
    }
}
