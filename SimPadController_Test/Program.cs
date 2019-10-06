using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimPadController;
using SimPadController.Model;
using SimPadController.Enum;

namespace SimPadController_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new SimPadController.SimPadController();

            foreach(var i in c.GetDevices())
            {
                Console.WriteLine("发现设备: {0}，固件版本 {1}", i.DisplayName, i.Version);
                Console.WriteLine("设备灯光模式: {0}", i.LightsType);
                for (uint j = 1; j <= 5; j++) {
                    var kc = i.GetKeySetting(j);
                    Console.WriteLine("按键{0}绑定: {1} + {2} + {3}", j, kc.Normal, kc.Special, kc.Mouse);
                }

                Console.Write("消抖数值: {0} {1}", i.DelayInput, String.Join(",", i.GetSettingBytes(SimPadSetting.DelayInput)));
            }

            Console.ReadLine();
        }
    }
}
