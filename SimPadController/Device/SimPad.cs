using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimPadController.Enum;
using SimPadController.Model;

namespace SimPadController.Device
{
    public class SimPad
    {
        internal byte[] dataGetVersion = { 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        internal byte[] dataGetChipID = { 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

        internal int settingsCount = 12;

        internal HidSharp.HidDevice device;
        internal byte[][] settings;

        internal HashSet<SimPadSetting> dirtySet = new HashSet<SimPadSetting>();

        /// <summary>
        /// 该设备的名字
        /// </summary>
        public virtual string DisplayName => "SimPad";

        /// <summary>
        /// 该设备拥有的键的个数
        /// </summary>
        public virtual int KeyCount => 5;

        /// <summary>
        /// 当前设备的固件版本
        /// </summary>
        public int Version { get => getVersion(); }

        /// <summary>
        /// 当前设备的芯片ID
        /// </summary>
        public int ChipID { get => getChipID(); }


        public SimPad(HidSharp.HidDevice device)
        {
            this.device = device;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes"></param>
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

        /// <summary>
        /// 发送并接收数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 获得版本
        /// </summary>
        /// <returns></returns>
        internal virtual int getVersion()
        {
            var result = SendDataAndReceive(dataGetVersion);

            var str = String.Join("", from i in result.Take(4) select i.ToString().PadLeft(2, '0'));

            return Convert.ToInt32(str);
        }

        /// <summary>
        /// 获得芯片ID
        /// </summary>
        /// <returns></returns>
        internal virtual int getChipID()
        {
            var result = SendDataAndReceive(dataGetChipID);

            var str = String.Join("", from i in result.Take(4) select i.ToString().PadLeft(2, '0'));

            return Convert.ToInt32(str);
        }

        
        internal byte[] getSettingFromDevice(byte setting)
        {
            var dat = new byte[] { 0x00, 0x01, setting, 0x00, 0x00, 0x00, 0x00 };

            return SendDataAndReceive(dat);
        }

        /// <summary>
        /// 刷新所有设置
        /// </summary>
        public virtual void RefreshSettings()
        {
            settings = new byte[settingsCount][];
            for (int i = 0; i < settingsCount; ++i)
            {
                settings[i] = getSettingFromDevice((byte)i);
            }
        }

        /// <summary>
        /// 应用设置到设备
        /// </summary>
        public virtual void ApplyAllSettings()
        {
            if (dirtySet.Count == 0) return;
            foreach(var i in dirtySet)
            {
                ApplySetting(i, false);
            }

            dirtySet.Clear();
        }
         
        /// <summary>
        /// 应用某项设置
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="clearDirty">是否清除Dirty</param>
        public virtual void ApplySetting(SimPadSetting setting, bool clearDirty = true)
        {
            var bytes = GetSettingBytes(setting);


            var dataBytes = new byte[]
            {
                (byte)setting,
                bytes[0],
                bytes[1],
                bytes[2],
                bytes[3],
                (byte)(bytes[0] ^ bytes[1] ^ bytes[2] ^ bytes[3]),
                0x00,
                0x00
            };

            SendData(dataBytes);

            if (clearDirty) dirtySet.Remove(setting);
        }

        /// <summary>
        /// 进入升级模式
        /// 注意，在进入升级模式之后再对机器进行写入/读入操作将会出错！
        /// </summary>
        public virtual void SetBootMode()
        {
            var dat = new byte[] { 0x00, 0x0b, 0x00, 0x00, 0x00, 0x0b };

            try
            {
                SendData(dat);
            }
            catch (IOException) { } // 为啥没发完就炸了？
        }

        /// <summary>
        /// 获得某一设置数据(bytes)
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>设置数据</returns>
        public virtual byte[] GetSettingBytes(SimPadSetting setting)
        {
            if (settings == null) RefreshSettings();
            return settings[(int)setting];
        }

        /// <summary>
        /// 设置某一设置数据(bytes)
        /// 并不会立即应用到设备上，需要使用Apply开头的方法来应用
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="bytes"></param>
        public virtual void SetSettingBytes(SimPadSetting setting, byte[] bytes)
        {
            if (settings == null) RefreshSettings();
            byte[] original = settings[(int)setting];
            if(!original.SequenceEqual(bytes)) // 不相等的时候才会应用设置
            {
                dirtySet.Add(setting);
                settings[(int)setting] = bytes;
            }

        }

        /// <summary>
        /// 获得键位设置数据
        /// </summary>
        /// <param name="number">键序号，从1开始</param>
        /// <returns></returns>
        public virtual KeySetting GetKeySetting(uint number)
        {
            if (number < 1 || number > KeyCount)
            {
                throw new Exception("Key number out of range!");
            }

            var settingKey = (SimPadSetting)number;
            var bytes = GetSettingBytes(settingKey);

            return new KeySetting
            {
                Normal = (SimPadKeyNormal)bytes[1],
                Special = (SimPadKeySpecial)bytes[0],
                Mouse = (SimPadKeyMouse)bytes[2]
            };
        }

        /// <summary>
        /// 设置键位设置数据
        /// 并不会立即应用到设备上，需要使用Apply开头的方法来应用
        /// </summary>
        /// <param name="number">键序号，从1开始</param>
        /// <param name="setting">设置</param>
        public virtual void SetKeySetting(uint number, KeySetting setting)
        {
            if (number < 1 || number > KeyCount)
            {
                throw new Exception("Key number out of range!");
            }

            var settingKey = (SimPadSetting)number;

            var bytes = new byte[8];

            bytes[1] = (byte)setting.Normal;
            bytes[0] = (byte)setting.Special;
            bytes[2] = (byte)setting.Mouse;

            SetSettingBytes(settingKey, bytes);
        }

        /// <summary>
        /// 获得LED灯的颜色数据
        /// </summary>
        /// <param name="number">LED灯序号</param>
        /// <returns>颜色</returns>
        public virtual Color GetLEDColor(uint number)
        {
            if (number < 1 || number > 2)
            {
                throw new Exception("Key number out of range!");
            }

            var settingKey = (SimPadSetting)(number + 5);
            var bytes = GetSettingBytes(settingKey);

            return new Color
            {
                R = bytes[0],
                G = bytes[1],
                B = bytes[2]
            };
        }

        /// <summary>
        /// 设置LED灯的颜色数据
        /// 并不会立即应用到设备上，需要使用Apply开头的方法来应用
        /// </summary>
        /// <param name="number">LED等序号</param>
        /// <param name="clr">颜色</param>
        public virtual void SetLEDColor(uint number, Color clr)
        {
            if (number < 1 || number > 2)
            {
                throw new Exception("Key number out of range!");
            }

            var settingKey = (SimPadSetting)(number + 5);

            var bytes = new byte[] { clr.R, clr.G, clr.B, 0, 0, 0, 0, 0 };

            SetSettingBytes(settingKey, bytes);
        }

        /// <summary>
        /// 消抖数值
        /// 并不会立即应用到设备上，需要使用Apply开头的方法来应用
        /// </summary>
        public virtual int DelayInput
        {
            get
            {
                var bytes = GetSettingBytes(SimPadSetting.DelayInput);

                int value = (bytes[0] << 24)
                    + (bytes[1] << 16)
                    + (bytes[2] << 8)
                    + bytes[3]; // 竟然是大端

                return value;
            }

            set
            {
                var bytes = new byte[]
{
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value,
                0, 0, 0, 0
};

                SetSettingBytes(SimPadSetting.DelayInput, bytes);
            }
        }

        /// <summary>
        /// 极速模式
        /// 并不会立即应用到设备上，需要使用Apply开头的方法来应用
        /// </summary>
        public virtual bool SuperSpeed
        {
            get
            {
                return GetSettingBytes(SimPadSetting.SuperSpeed)[0] != 0;
            }

            set
            {
                var bytes = GetSettingBytes(SimPadSetting.SuperSpeed);
                bytes[0] = (byte)(value ? 1 : 0);
                SetSettingBytes(SimPadSetting.SuperSpeed, bytes);
            }
        }

        /// <summary>
        /// 夜灯
        /// 并不会立即应用到设备上，需要使用Apply开头的方法来应用
        /// </summary>
        public virtual NightLights NightLights
        {
            get
            {
                var bytes = GetSettingBytes(SimPadSetting.SuperSpeed);
                return (NightLights)bytes[2];
            }

            set
            {
                var bytes = GetSettingBytes(SimPadSetting.SuperSpeed);
                bytes[2] = (byte)value;
                SetSettingBytes(SimPadSetting.SuperSpeed, bytes);
            }
        }


        /// <summary>
        /// 灯光速度
        /// 并不会立即应用到设备上，需要使用Apply开头的方法来应用
        /// </summary>
        public virtual LightSpeed LightSpeed
        {
            get
            {
                var bytes = GetSettingBytes(SimPadSetting.LightSpeed);
                ushort ease = (ushort)(bytes[0] << 8 + bytes[1]),
                    rainbow = (ushort)(bytes[2] << 8 + bytes[3]);

                if (ease < 0x0a) ease = 0x0a; // TODO: 可能有问题
                if (rainbow < 0x0a) rainbow = 0x0a;

                return new LightSpeed
                {
                    EaseLightDelay = ease,
                    RainbowLightDelay = rainbow
                };
            }

            set
            {
                var bytes = GetSettingBytes(SimPadSetting.LightSpeed);

                bytes[0] = (byte)(value.EaseLightDelay >> 8);
                bytes[1] = (byte)(value.EaseLightDelay);
                bytes[2] = (byte)(value.RainbowLightDelay >> 8);
                bytes[3] = (byte)(value.RainbowLightDelay);

                SetSettingBytes(SimPadSetting.LightSpeed, bytes);
            }
        }

        /// <summary>
        /// 灯光模式
        /// </summary>
        public virtual LightsType LightsType
        {
            get
            {
                var bytes = GetSettingBytes(SimPadSetting.LightMode);

                return (LightsType)bytes[0];

            }

            set
            {
                SetSettingBytes(SimPadSetting.LightMode, new byte[] { (byte)value, 0, 0, 0, 0, 0, 0, 0 });
            }
        }

    }
}
