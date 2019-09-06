using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Device
{
    class SimPad
    {
        private byte[] dataGetVersion = { 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] dataGetChipID = { 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private int settingsCount = 12;
        
        private HidSharp.HidDevice device;
        private byte[][] settings;

        public int Version { get => getVersion(); }
        public int ChipID { get => getChipID(); }

        public enum SimPadSetting
        {
            Blank = 0,
            Key1,
            Key2,
            Key3,
            Key4,
            Key5,
            LED0RGB,
            LED1RGB,
            LightMode,
            DelayInput,
            SuperSpeed,
            LightSpeed
        }

        public SimPad(HidSharp.HidDevice device)
        {
            this.device = device;
        }

        public void SendData(byte[] bytes)
        {
            if (device == null) throw new Exception("Device not initialized!");

            var s = device.Open();

            // byte[] prepend = new byte[bytes.Length + 1]; 0

            // prepend[0] = 0;
            // bytes.CopyTo(prepend, 1);

            s.Write(bytes.Prepend<byte>(0).ToArray()); // prepend throwaway byte

            s.Close();
        }

        public byte[] SendDataAndReceive(byte[] bytes)
        {
            if (device == null) throw new Exception("Device not initialized!");

            var s = device.Open();

            //byte[] prepend = new byte[bytes.Length + 1]; // prepend throwaway byte

            //prepend[0] = 0;
            //bytes.CopyTo(prepend, 1);

            s.Write(bytes.Prepend<byte>(0).ToArray());

            while (!s.CanRead) ; // 这里可能会出事
            var read = s.Read();

            return read.Skip(1).ToArray();
        }

        private int getVersion()
        {
            var result = this.SendDataAndReceive(dataGetVersion);

            var str = String.Join("", from i in result.Take(4) select i.ToString().PadLeft(2, '0'));

            return Convert.ToInt32(str);
        }

        private int getChipID()
        {
            var result = this.SendDataAndReceive(dataGetChipID);

            var str = String.Join("", from i in result.Take(4) select i.ToString().PadLeft(2, '0'));

            return Convert.ToInt32(str);
        }

        public virtual byte[] GetSetting(byte pointer)
        {
            var dat = new byte[] { 0x00, 0x01, pointer, 0x00, 0x00, 0x00, 0x00 };

            return SendDataAndReceive(dat);
        }

        public virtual void RefreshSettings()
        {
            this.settings = new byte[settingsCount][];
            for(int i = 0; i < settingsCount; ++i)
            {
                this.settings[i] = GetSetting((byte)i);
            }   
        }


        
    }
}
