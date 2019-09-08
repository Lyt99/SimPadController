using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Model
{
    /// <summary>
    /// 灯光速度
    /// </summary>
    public class LightSpeed
    {
        /// <summary>
        /// 渐隐渐显延迟
        /// </summary>
        public ushort EaseLightDelay;
        /// <summary>
        /// 彩虹渐变延迟
        /// </summary>
        public ushort RainbowLightDelay;
    }
}
