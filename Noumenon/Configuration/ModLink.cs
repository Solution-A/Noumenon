using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.Configuration
{
    [Serializable]
    public class ModLink
    {
        [NonSerialized] internal string GUID = Guid.NewGuid().ToString();
    }
}
