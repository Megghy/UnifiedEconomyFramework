using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShockAPI;
using TShockAPI.Hooks;

namespace UnifiedEconomyFramework
{
    public class Config
    {
        public void Load(ReloadEventArgs args = null)
        {
            if (!File.Exists(Path.Combine(TShock.SavePath, "UnifiedEconomyFramework.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                TextReader tr = new StringReader(JsonConvert.SerializeObject(UEF.Config));
                JsonTextReader jtr = new JsonTextReader(tr);
                object obj = serializer.Deserialize(jtr);
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,//格式化缩进
                    Indentation = 4,  //缩进四个字符
                    IndentChar = ' '  //缩进的字符是空格
                };
                serializer.Serialize(jsonWriter, obj);
                FileTools.CreateIfNot(Path.Combine(TShock.SavePath, "UnifiedEconomyFramework.json"), textWriter.ToString());
            }
            try
            {
                UEF.Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(TShock.SavePath, "UnifiedEconomyFramework.json")));
                TShock.Log.ConsoleInfo($"<UEF> 成功读取配置文件, 当前使用经济框架 {UEF.Config.Type}{(UEF.Config.InterfaceFirst ? ", 优先调用已注册接口." : ".")}");
            }
            catch (Exception ex) { TShock.Log.Error(ex.Message); TShock.Log.ConsoleError("<UEF> 读取配置文件失败."); }
        }
        public enum MoneyType
        {
            SEconomy,
            BeanPoint,
            POBC
        }
        [JsonProperty]
        public MoneyType Type = MoneyType.SEconomy;
        [JsonProperty]
        public string Type描述 = "修改上方的数字来修改使用的经济框架. 0: SEconomy, 1: BeanPoint, 2: POBC.";
        [JsonProperty]
        public bool InterfaceFirst = true;
        [JsonProperty]
        public string InterfaceFirst描述 = "是否优先使用动态注册的接口. 如果你不理解此选项是什么意思那就保持默认.";
    }
}
