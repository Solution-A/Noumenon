using ECommons.DalamudServices;
using ECommons.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.IPC
{
    public static class GlamourerManager
    {
        public const string LabelGetDesignList = "Glamourer.GetDesignList";
        static DesignListEntry[] GetDesignListIPC()
        {
            try
            {
                return Svc.PluginInterface.GetIpcSubscriber<DesignListEntry[]>(LabelGetDesignList).InvokeFunc();
            }
            catch (Exception ex)
            {
                InternalLog.Error(ex.ToString());
            }
            return [];
        }
    }
}
