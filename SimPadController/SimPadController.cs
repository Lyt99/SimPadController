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

        private SimPad[] deviceList;
        private int DeviceCount => deviceList.Length;

        public delegate void SimpadDeviceChangedEvent(object sender, SimPadDeviceChangedEventArgs e);
        public event SimpadDeviceChangedEvent OnSimpadDeviceChanged;



        public SimPadController()
        {
            DeviceList.Local.Changed += OnDeviceChanged;
            this.deviceList = getDevices();
        }

        public IEnumerable<SimPad> GetDevices()
        {
            return this.deviceList;
        }

        private SimPad[] getDevices()
        {
            return DeviceList.Local.GetHidDevices(vendorID)
                .Where(i => i.DevicePath.Contains("&mi_01")) // 难看的硬编码
                .Select(i => getSimPadInstance(i))
                .Where(i => i != null).ToArray();
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

        private void OnDeviceChanged(object sender, DeviceListChangedEventArgs e)
        {
            var newDevices = getDevices();
            if(DeviceCount != newDevices.Length)
            {
                bool isNew = this.deviceList.Length < newDevices.Length;
                var distinct = isNew ? newDevices.Except(this.deviceList) : this.deviceList.Except(newDevices);
                var arg = new SimPadDeviceChangedEventArgs
                {
                    IsNew = isNew,
                    devices = distinct.ToArray()
                };


                this.deviceList = newDevices;

                OnSimpadDeviceChanged?.Invoke(this, null);
                
            }
        }
    }
}
