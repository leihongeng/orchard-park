using Masuit.Tools.DateTimeExt;
using Orchard.Park.Core.IdGenerator;
using System.Collections.Concurrent;

namespace System
{
    public static class IdHelper
    {
        /// <summary>
        /// 生成一个雪花ID
        /// </summary>
        /// <returns></returns>
        public static long CreateLongId()
        {
            return YitIdHelper.NextId();
            // return SnowFlake.GetInstance().GetLongId();
        }

        public static ConcurrentDictionary<string, short> ShortIdDic = new();

        public static ConcurrentDictionary<string, int> SequenceDic = new();

        /// <summary>
        /// 生成一个CMD命令消息ID
        /// </summary>
        /// <returns></returns>
        public static short CreateMessageId()
        {
            const short min = 1;
            const string key = "TcpSocket";
            if (ShortIdDic.TryGetValue(key, out var val))
            {
                var id = val >= short.MaxValue ? min : (short)(val + 1);
                ShortIdDic[key] = id;
                return id;
            }

            ShortIdDic[key] = min;
            return min;
        }

        /// <summary>
        /// 4位自增序列取自时间戳，同一秒内按序列自增，新秒重计，如0001
        /// </summary>
        /// <returns></returns>
        public static string CreateSeq(string channel)
        {
            const int min = 1;
            var key = $"{channel}_{DateTime.Now.GetTotalSeconds()}";//获取秒级时间戳

            if (SequenceDic.TryGetValue(key, out var val))
            {
                var seq = val >= 9999 ? min : val + 1;
                SequenceDic[key] = seq;
                return seq.ToString().PadLeft(4, '0');
            }

            SequenceDic.Clear();//清空
            SequenceDic[key] = min;
            return min.ToString().PadLeft(4, '0');
        }
    }
}