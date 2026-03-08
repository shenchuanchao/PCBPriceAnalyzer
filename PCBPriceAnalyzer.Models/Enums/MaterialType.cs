using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.Enums
{
    /// <summary>
    /// 材料类型枚举
    /// </summary>
    public enum MaterialType
    {
        /// <summary>
        /// 覆铜板 (CCL)
        /// </summary>
        CCL,

        /// <summary>
        /// 铜箔
        /// </summary>
        CopperFoil,

        /// <summary>
        /// 半固化片
        /// </summary>
        Prepreg,

        /// <summary>
        /// 金
        /// </summary>
        Gold,

        /// <summary>
        /// 银
        /// </summary>
        Silver,

        /// <summary>
        /// 铜
        /// </summary>
        Copper,

        /// <summary>
        /// 锡
        /// </summary>
        Tin,

        /// <summary>
        /// 油墨
        /// </summary>
        Ink,

        /// <summary>
        /// 其他（用于扩展）
        /// </summary>
        Other
    }
}
