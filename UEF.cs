using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace UnifiedEconomyFramework
{
    [ApiVersion(2, 1)]
    public class UEF : TerrariaPlugin
    {
        public UEF(Main game) : base(game)
        {
        }
        public override string Name => "UnifiedEconomyFramework";
        public override Version Version => new Version(1, 0);
        public override string Author => "Megghy";
        public override string Description => "为不同的经济插件提供统一接口.";
        public override void Initialize()
        {
            Config.Load();
            TShockAPI.Hooks.GeneralHooks.ReloadEvent += Config.Load;
        }
        protected override void Dispose(bool disposing)
        {
            TShockAPI.Hooks.GeneralHooks.ReloadEvent -= Config.Load;
            base.Dispose(disposing);
        }
        internal readonly static List<IEconomy> FrameList = new List<IEconomy>();
        internal static Config Config = new Config();
        public static void ResisterEconomy(IEconomy framework)
        {
            if (!FrameList.Contains(framework)) FrameList.Add(framework);
        }
        public static void DeresisterEconomy(IEconomy framework)
        {
            if (FrameList.Contains(framework)) FrameList.Remove(framework);
        }
        static bool CheckLoaded()
        {
            switch (Config.Type)
            {
                case Config.MoneyType.BeanPoint:
                    return ServerApi.Plugins.Where(p => p.Plugin.Name == "BeanPoints").Any();
                case Config.MoneyType.POBC:
                    return ServerApi.Plugins.Where(p => p.Plugin.ToString() == "POBC2.POBCSystem").Any();
                case Config.MoneyType.SEconomy:
                    return ServerApi.Plugins.Where(p => p.Plugin.ToString() == "Wolfje.Plugins.SEconomy.SEconomyPlugin").Any();
                default:
                    return false;
            }
        }
        /// <summary>
        /// 增加玩家余额, 返回是否成功调用.
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns></returns>
        public static bool MoneyUp(string name, long num)
        {
            if (Config.InterfaceFirst) FrameList.ForEach(f => f.MoneyUp(name, num));
            else
            {
                if (!CheckLoaded())
                {
                    if (FrameList.Any())
                    {
                        FrameList.ForEach(f => f.MoneyUp(name, num));
                        return true;
                    }
                    else
                    {
                        TShock.Log.ConsoleError($"未能找到任何可用经济框架.");
                        return false;
                    }
                }
                return Config.Type switch
                {
                    Config.MoneyType.SEconomy => Adapter.SEconomy_Change(name, num),
                    Config.MoneyType.BeanPoint => Adapter.BeanPoint_Change(name, num),
                    Config.MoneyType.POBC => Adapter.POBC_Change(name, num),
                    _ => false,
                };
            }
            return true;
        }
        /// <summary>
        /// 减少玩家余额, 返回是否成功调用.
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns></returns>
        public static bool MoneyDown(string name, long num)
        {
            if (Config.InterfaceFirst) FrameList.ForEach(f => f.MoneyDown(name, num));
            else
            {
                if (!CheckLoaded())
                {
                    if (FrameList.Any())
                    {
                        FrameList.ForEach(f => f.MoneyDown(name, num));
                        return true;
                    }
                    else
                    {
                        TShock.Log.ConsoleError($"未能找到任何可用经济框架.");
                        return false;
                    }
                }
                return Config.Type switch
                {
                    Config.MoneyType.SEconomy => Adapter.SEconomy_Change(name, -num),
                    Config.MoneyType.BeanPoint => Adapter.BeanPoint_Change(name, -num),
                    Config.MoneyType.POBC => Adapter.POBC_Change(name, -num),
                    _ => false,
                };
            }
            return true;
        }
        /// <summary>
        /// 显示玩家余额
        /// </summary>
        /// <param name="name">玩家名</param>
        /// <returns></returns>
        public static long Balance(string name) {
            if (Config.InterfaceFirst && FrameList.Any()) return FrameList[0].Balance(name);
            if (!CheckLoaded())
            {
                if (FrameList.Any())
                {
                    return FrameList[0].Balance(name);
                }
                else
                {
                    TShock.Log.ConsoleError($"未能找到任何可用经济框架.");
                    return -1;
                }
            }
            return Config.Type switch
            {
                Config.MoneyType.SEconomy => Adapter.SEconomy_Balance(name),
                Config.MoneyType.BeanPoint => Adapter.BeanPoint_Balance(name),
                Config.MoneyType.POBC => Adapter.POBC_Balance(name),
                _ => -1,
            };
        }
    }
}
