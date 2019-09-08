using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HidSharp;
using SimPadController.Device;

namespace SimPadController
{
    public class SimPadController
    {
        private readonly int vendorID = 0x8088;

        public IEnumerable<SimPad> GetDevices()
        {
            return HidSharp.DeviceList.Local.GetHidDevices(vendorID)
                .Where(i => i.DevicePath.Contains("&mi_01"))
                .Select(i => getSimPadInstance(i))
                .Where(i => i != null);
        }

        private SimPad getSimPadInstance(HidSharp.HidDevice device)
        {
            if (device.VendorID != vendorID) return null;
            
            switch(device.ProductID)
            {
                case 0x0001: // SimPad V2
                    return new SimPadV2(device);
                case 0x0002: // SimPad V2Ex
                    return new SimPadV2Ex(device);
                case 0x0003: // SimPad V2Lite
                    return new SimPadV2Lite(device);
                case 0x0004: // SimPad Nano
                    return new SimPadNano(device);
                default:
                    return null;
            }
        }

        // TODO: OnChange
    }
}
