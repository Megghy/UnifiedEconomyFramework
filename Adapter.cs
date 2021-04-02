using System;
using TShockAPI;
using Wolfje.Plugins.SEconomy;
using Wolfje.Plugins.SEconomy.Journal;

namespace UnifiedEconomyFramework
{
    internal class Adapter
    {
        public static bool BeanPoint_Change(string name, long money)
        {
            try
            {
                var plr = BeanPoints.BeanPlayer.GetBeanPlayer(name);
                if (plr == null)
                {
                    TShock.Log.ConsoleError($"未能找到玩家账户 {name}");
                    return false;
                }
                if (money > 0)
                {
                    plr.AddPoints((int)money);
                    return true;
                }
                else
                {
                    plr.DecreasePoints((int)money);
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine($"未能修改玩家余额.\n" + ex); return false; }
        }

        public static bool SEconomy_Change(string name, long money)
        {
            try
            {
                Money m = Money.Parse(money < 0L ? (-money).ToString() : money.ToString());
                IBankAccount selectedAccount = SEconomyPlugin.Instance.RunningJournal.GetBankAccountByName(name);
                if (selectedAccount != null)
                {
                    SEconomyPlugin.Instance.WorldAccount.TransferTo(selectedAccount, money, BankAccountTransferOptions.AnnounceToReceiver, (money < 0 ? "购买商品" : "出售商品"), $"SE: {name} {(money < 0 ? "购买商品" : "出售商品")}");
                    return true;
                }
                else
                {
                    TShock.Log.ConsoleError($"未能找到玩家账户 {name}");
                    return false;
                }
            }
            catch (Exception ex) { Console.WriteLine($"未能修改玩家余额.\n" + ex); return false; }
        }

        public static bool POBC_Change(string name, long money)
        {
            try
            {
                if (money > 0)
                {
                    POBC2.Db.UpC(name, (int)money);
                    return true;
                }
                else
                {
                    POBC2.Db.DownC(name, (int)money);
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine($"未能修改玩家余额.\n" + ex); return false; }
        }

        public static long SEconomy_Balance(string name)
        {
            IBankAccount selectedAccount = SEconomyPlugin.Instance.GetPlayerBankAccount(name);
            if (selectedAccount != null) return selectedAccount.Balance.Value;
            else
            {
                TShock.Log.ConsoleError($"尝试获取玩家 {name} 的余额时发生错误, 可能不存在此玩家的账户.");
                return -1;
            }
        }

        public static long BeanPoint_Balance(string name)
        {
            try
            {
                var plr = BeanPoints.BeanPlayer.GetBeanPlayer(name);
                if (plr == null)
                {
                    TShock.Log.ConsoleError($"未能找到玩家账户 {name}");
                    return -1;
                }
                else
                {
                    return plr.Points;
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"尝试获取玩家 {name} 的余额时发生错误, 可能不存在此玩家的账户.\n{ex}");
                return -1;
            }
        }

        public static long POBC_Balance(string name)
        {
            try
            {
                return POBC2.Db.QueryCurrency(name);
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"尝试获取玩家 {name} 的余额时发生错误, 可能不存在此玩家的账户.\n{ex}");
                return -1;
            }
        }
    }
}
