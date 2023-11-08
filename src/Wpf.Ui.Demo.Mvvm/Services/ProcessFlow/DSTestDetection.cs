using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.Services.ProcessFlow;

class DSTestDetection : IProcessFlow
{
   
    public override bool BreakExecution()
    {
        throw new NotImplementedException();
    }

    public override bool CheckExecution()
    {
        throw new NotImplementedException();
    }

    public override Task<bool> ExecutionDetection()
    {
        throw new NotImplementedException();
    }

    public override Task ExecutionProcess()
    {
        throw new NotImplementedException();
    }
}
