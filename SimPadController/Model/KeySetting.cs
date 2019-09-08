using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimPadController.Enum;

namespace SimPadController.Model
{
    /// <summary>
    /// 键位设置
    /// </summary>
    public class KeySetting
    {
        public SimPadKeyNormal Normal = SimPadKeyNormal.None;
        public SimPadKeySpecial Special = SimPadKeySpecial.None;
        public SimPadKeyMouse Mouse = SimPadKeyMouse.None;
    }
}
