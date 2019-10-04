using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController
{
    public class SimPadDeviceChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 是否为新设备插入(否则为设备拔出)
        /// </summary>
        public bool IsNew;
        /// <summary>
        /// 存在变动的设备
        /// </summary>
        public Device.SimPad[] devices;
    }
}
