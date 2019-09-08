using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Model
{
    /// <summary>
    /// 颜色
    /// </summary>
    public class Color
    {
        /// <summary>
        /// RGB的值
        /// </summary>
        public byte R, G, B;

        /// <summary>
        /// 转换为16进制表示的颜色
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string r = Convert.ToString(R, 16).PadLeft(2, '0'),
                   g = Convert.ToString(G, 16).PadLeft(2, '0'),
                   b = Convert.ToString(B, 16).PadLeft(2, '0');

            return String.Format("#{0}{1}{2}", r, g, b);
        }
    }
}
