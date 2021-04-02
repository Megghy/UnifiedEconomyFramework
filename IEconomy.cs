using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEconomyFramework
{
    public interface IEconomy
    {
        /// <summary>
        /// 增加玩家余额, 返回是否成功调用.
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns></returns>
        public bool MoneyUp(long num);
        /// <summary>
        /// 减少玩家余额, 返回是否成功调用.
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns></returns>
        public bool MoneyDown(long num);
        /// <summary>
        /// 显示玩家余额
        /// </summary>
        /// <param name="name">玩家名</param>
        /// <returns></returns>
        public long Balance(string name);
    }
}
