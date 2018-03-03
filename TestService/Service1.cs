using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    public partial class TestService : ServiceBase
    {
        public TestService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry($"测试服务开始");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry($"测试服务结束");
        }
    }
}
