using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Device
{
    class SimPadV2Ex : SimPad
    {
        public override string DisplayName => "SimPad 版本2 - EX";

        public SimPadV2Ex(HidSharp.HidDevice device) : base(device)
        {

        }

        internal override int getChipID() // 不可用，重写
        {
            return -1;
        }
    }
}
