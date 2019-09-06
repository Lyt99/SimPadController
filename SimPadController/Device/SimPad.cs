using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimPadController.Enum;
using SimPadController.Model;

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
            var result = SendDataAndReceive(dataGetVersion);

            var str = String.Join("", from i in result.Take(4) select i.ToString().PadLeft(2, '0'));

            return Convert.ToInt32(str);
        }

        private int getChipID()
        {
            var result = SendDataAndReceive(dataGetChipID);

            var str = String.Join("", from i in result.Take(4) select i.ToString().PadLeft(2, '0'));

            return Convert.ToInt32(str);
        }

        public virtual byte[] GetSettingFromDevice(byte pointer)
        {
            var dat = new byte[] { 0x00, 0x01, pointer, 0x00, 0x00, 0x00, 0x00 };

            return SendDataAndReceive(dat);
        }

        public virtual void RefreshSettings()
        {
            settings = new byte[settingsCount][];
            for(int i = 0; i < settingsCount; ++i)
            {
                settings[i] = GetSettingFromDevice((byte)i);
            }   
        }

        public virtual void ApplySettings()
        {
            // TODO
        }

        public virtual void SetBootMode()
        {
            var dat = new byte[] { 0x00, 0x0b, 0x00, 0x00, 0x00, 0x0b };

            SendData(dat);
        }

        public virtual byte[] GetSettingBytes(SimPadSetting setting)
        {
            if (settings == null) RefreshSettings();
            return settings[(int)setting];
        }

        public virtual void SetSettingBytes(SimPadSetting setting, byte[] bytes)
        {
            if (settings == null) RefreshSettings();
            settings[(int)setting] = bytes;
        }


        public virtual KeySetting GetKeySetting(uint number)
        {
            if( number < 1 && number > 5)
            {
                throw new Exception("Key number out of range!");
            }

            var settingKey = (SimPadSetting)(number + 1);
            var keyBytes = GetSettingBytes(settingKey);

            return new KeySetting
            {
                Normal = (SimPadKeyNormal)keyBytes[0],
                Special = (SimPadKeySpecial)keyBytes[1],
                Mouse = (SimPadKeyMouse)keyBytes[2]
            };
        }

        public virtual void SetKeySetting(int number, KeySetting setting)
        {
            if (number < 1 && number > 5)
            {
                throw new Exception("Key number out of range!");
            }

            var settingKey = (SimPadSetting)(number + 1);

            var bytes = new byte[8];

            bytes[0] = (byte)setting.Normal;
            bytes[1] = (byte)setting.Special;
            bytes[2] = (byte)setting.Mouse;

            SetSettingBytes(settingKey, bytes);
        }

        
    }
}
