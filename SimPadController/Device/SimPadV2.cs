﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Device
{
    public class SimPadV2 : SimPad
    {
        public override string DisplayName => "Simpad 版本2";

        public SimPadV2(HidSharp.HidDevice device) : base(device)
        {
            
        }

        internal override int getChipID() // 不可用，重写
        {
            return -1;
        }
    }
}
