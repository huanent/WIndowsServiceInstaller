using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "windows服务文件（*.exe）|*.exe",
                RestoreDirectory = true
            };
            if (dialog.ShowDialog().Equals(true))
            {
                InstallService(dialog.FileName);
            }
        }

        private void btnUninstall_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "windows服务文件（*.exe）|*.exe",
                RestoreDirectory = true
            };
            if (dialog.ShowDialog().Equals(true))
            {
                UninstallService(dialog.FileName);
            }
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
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }
    }
}
