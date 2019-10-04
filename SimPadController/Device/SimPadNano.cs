using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Device
{
    class SimPadNano : SimPad
    {
        public override string DisplayName => "SimPad 迷你版";
        public override int KeyCount => 3;

        public SimPadNano(HidSharp.HidDevice device) : base(device)
        {

        }
    }
}
