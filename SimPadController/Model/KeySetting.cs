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
        public SimPadKeyNormal Normal { get; set; } = SimPadKeyNormal.None;
        public SimPadKeySpecial Special { get; set; } = SimPadKeySpecial.None;
        public SimPadKeyMouse Mouse { get; set; } = SimPadKeyMouse.None;

        public override bool Equals(object obj)
        {
            KeySetting s = obj as KeySetting;
            if(s == null)
            {
                return base.Equals(obj);
            }

            return s.Mouse == this.Mouse &&
                s.Normal == this.Normal &&
                s.Special == this.Special;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
