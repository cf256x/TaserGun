using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.CustomItems.API;

namespace Taser
{
    public class Plugin : Plugin<Config>
    {
        TaserItem ti;
        public override void OnEnabled()
        {
            ti = new TaserItem();
            ti.Register();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            ti = null;
            ti.Unregister();
            base.OnDisabled();
        }
    }
}
