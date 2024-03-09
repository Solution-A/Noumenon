using ECommons.DalamudServices;
using ECommons.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.IPC
{
    public class GlamourerManager
    {
        public const string LabelGetDesignList = "Glamourer.GetDesignList";
        public DesignListEntry[] GetDesignListIPC()
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
        public DesignListEntry[] GetDesigns()
        {
            return GetDesignListIPC();
        }
    }
}
