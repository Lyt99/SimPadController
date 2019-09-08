using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Enum
{
    public enum LightsType
    {
        /// <summary>
        /// 按下立即亮起，然后渐隐
        /// </summary>
        EaseOut,
        /// <summary>
        /// 按下立即熄灭，然后渐显
        /// </summary>
        EaseIn,
        /// <summary>
        /// 常亮
        /// </summary>
        KeepOn,
        /// <summary>
        /// 关闭
        /// </summary>
        KeepOff,
        /// <summary>
        /// 按下时亮起
        /// </summary>
        KeyPress = 5,
        /// <summary>
        /// 彩虹
        /// </summary>
        RainbowColor
    }
}
