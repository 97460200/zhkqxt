using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Config
/// </summary>
namespace Alipay
{
    public class Config
    {
        //@"此处填写支付宝公钥";
        public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhtMo9R2xsdc5YR23U2UI5EkvsHxmYVjICI7MWJgfTxeW6B6NTDoauuSQqYArji02F0uSpjzwOTB4a0DghwMfIRhjU1UHg7scwcuSyWoNr/iQpdEYkuqxgqUglzVLeetnxnXNnMbinyACESyMgWf2ObOVCxjrL2L13lgkBgyYG80r9p1FRtJWM9WNBZD08ABWEkjwyRnI06Tz/Lo33aBLPZqL+8pqNvi9F492vO9gkcFV9meW6Cec2fsHXggsGVkDn7WScrlxUCOpCTKnxLEPksWdI72Dsir2qgihtj0BomSYtCnB/500GQutkreLsq0ipDM0y1IL8AkgWRNin06BZQIDAQAB";


        //这里要配置没有经过PKCS8转换的原始私钥

        //开发者私钥
        public static string merchant_private_key = "MIIEpQIBAAKCAQEAx2elBYk/LxBjMPPb1mlLflwrZES4I1sXuwKMHQ2c8aN/h0l4qMlxE392r8Qf8KKR0a7BN7cro42VdMv+5ZoqdjxEKkwcz0myXP/SsG96WqzeEE7LLWWkCPDbFhmS6h0uybOA3573m9CcpDkornuDtYgh+YWvbYjdd3Z6IFxDFnWpEecq6UXtCnglMCwgbFRYaAo06aruu4G8b9hcynl6B57em7PvXwcf1j+jTq0aE8ID4ibO0qVTaczCrytB5MdAHnS3Qkfp3NUoJLrYP/AYJQg9b0IU83Nn7nAjP0fEbipbQ8N2PaSfanyUqwvwoZWYMwzRh6XezdjIkRm1mrb4rwIDAQABAoIBAQDEKItH1OnZDI/nWh0K4W4okdcFviw+a5/+kdTvx7J6atJLbdVKhU+9VIiXA6JAAEzeLsFYY9KEBAXgbcUEbRIUWdjcmx9EM0VJA0hp1GBSzvdsp3Zr5C7ntLhxFxtVPvwF45WFZkZCe4d8bdY9PrgkXhS99+rJH/htSpl9OAWvErc+Ug2hdcRNEc70VYZ8ixJk+NGxYTfN0uUHWydSTQuTQnKYoc1iShVbsl6Hy72ftIrTTCcx7K0T+4+rQUnt3GJiRVAKgokXoToNzS9B/K+87S/JSvWGDjDarwecQsonpjKLUQ8kW1m7vERUa5m2vZ7441QGSmyERr5QJGPH2xHBAoGBAOonU0K/7DiZj1pEJ90s2PsL2i6Vc8/4Aeiya2yuFT2zHxZlDUe+1w3neb/C9FF3SeZEoc+psGxC6PK+in7I9ADK9HdUV3aXrPPOeZUFbvGHI+r4XyzGrrS0NmPGF42y3s9fDJF0A3H3RpRqsnebtoVfARhkm/OFDReQ02y3bjZfAoGBANoCW6HhLcfGlW/MbkHLXETwdDOOWEOX0Tvex6UkxMBZ8GlakdLeKKdnZnXzMV9UoMx94ECpotyF+Rr0LJIYlW+yQyUstT8j63vO1iITR+Bl4izPnaLpVjIr9HCTXM3+ufuQM16nrU3bEHPU8k9P18KpuTCMqUoa73dhdD5XHj+xAoGBALOK6+Dm4O6VcMI6OIbzeH0nHWlS17bD6FLRXGW9JjRlQJUfbwtwXd4fe896YXSGD3gtcBWXe9vHgm9/gxqsY8yn0sQdO+OHceqQi9GklOlGQlhNkkz3G8cb2AixQCY63XYM38o4NtwQS2JKXgYws8eYjXJy0tHrV5qWRke6bxvpAoGAZ8bpkbScU1X/utlovt8sTBDeb9BZyctSKZrSFJXdJudpXu1jLyrb1VyOVKVwoj2p0c412vIlea44t5D2SpFzSVllRyPyH11c8nGyU0Q0WeiF8ujd1DgY9QchdBlh+xk/tTWejndpMv+N5GXA9uo9/gdHBYpf58vMpFp6uFZGKvECgYEAoxNOFy3t7yoO7gqszHNVCXFyJHnnIdiibE+815uHykilFQFmRdj3UbedyVOpHwFQhXGUlvTrkFyC6RdAZy37uew2HKNMeoaTEMJraPLmP36LNTZnColyp1iOY2sbgWd/vvxQN7RVpMl1Ms7wr8e/FabQZGArQ+8RjXx0wX5lunc=";

        //开发者公钥
        public static string merchant_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx2elBYk/LxBjMPPb1mlLflwrZES4I1sXuwKMHQ2c8aN/h0l4qMlxE392r8Qf8KKR0a7BN7cro42VdMv+5ZoqdjxEKkwcz0myXP/SsG96WqzeEE7LLWWkCPDbFhmS6h0uybOA3573m9CcpDkornuDtYgh+YWvbYjdd3Z6IFxDFnWpEecq6UXtCnglMCwgbFRYaAo06aruu4G8b9hcynl6B57em7PvXwcf1j+jTq0aE8ID4ibO0qVTaczCrytB5MdAHnS3Qkfp3NUoJLrYP/AYJQg9b0IU83Nn7nAjP0fEbipbQ8N2PaSfanyUqwvwoZWYMwzRh6XezdjIkRm1mrb4rwIDAQAB";


        public static string appId = "2019031963568728";
        public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
        public static string monitorUrl = "http://mcloudmonitor.com/gateway.do";
        
        //编码，无需修改
        public static string charset = "utf-8";

        //签名类型，支持RSA2（推荐！）、RSA
        public static string signtype = "RSA2";

        //版本号，无需修改
        public static string version = "1.0";

        public Config()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public static string getMerchantPublicKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_public_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

        /// <summary>
        /// 私钥文件类型转换成纯文本类型
        /// </summary>
        /// <returns>过滤后的字符串类型私钥</returns>
        public static string getMerchantPriveteKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_private_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }
    }
}