using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Device
{
    class SimPadV2Lite : SimPad
    {
        public override string DisplayName => "SimPad 版本2 - EX";

        public SimPadV2Lite(HidSharp.HidDevice device) : base(device)
        {

        }

        internal override int getChipID() // 不可用，重写
        {
            return -1;
        }
    }
}
