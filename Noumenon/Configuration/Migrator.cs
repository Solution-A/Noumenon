using Dalamud.Logging;
using ECommons;
using ECommons.DalamudServices;
using Noumenon.IPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.Configuration
{
    public class Migrator
    {
        public Migrator()
        {
            Svc.Framework.Update += DoGlamourerMigration;
        }

        void DoGlamourerMigration(object a)
        {
            {
                var entries = GlamourerManager.GetDesigns();
                if (entries.Any())
                {
                    PluginLog.Information($"Finished Glamourer name to guid migration");
                    var x = new D ;
                    while (x != null)
                    {
                        for (int i = 0; i < x.Designs.Count; i++)
                        {
                            if (!Guid.TryParse(x.Designs[i], out _))
                            {
                                if (entries.TryGetFirst(z => z.Name == x.Designs[i], out var value))
                                {
                                    PluginLog.Information($">> Complex Glamourer Entry: changing {x.Designs[i]} -> {value.Identifier}");
                                    x.Designs[i] = value.Identifier.ToString();
                                }
                            }
                        }
                    }
                    Svc.Framework.Update -= DoGlamourerMigration;
                }
            }

        }
    }
}
