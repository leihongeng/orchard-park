namespace Orchard.Park.Identity
{
    public class JwtTokenConfig
    {
        public string SecretKey { get; set; }

        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int ExpireMinutes { get; set; } = 30;
    }
}