using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM.Web.alipay
{
    public class config
    {

        public config()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        // 应用ID,您的APPID
        public static string app_id = "2018080860963479";

        // 支付宝网关
        public static string gatewayUrl = "https://openapi.alipay.com/gateway.do";

        // 商户私钥，您的原始格式RSA私钥  mbdcSReB7O4+wt3DosZsqA==
        public static string private_key = @"MIIEogIBAAKCAQEAxjskyZyyleNWaIvS/vM0hwXqd+4vAm3AzvWdHv63uH051yDV
FnLyOfV4Vwpgnx9Tx2XFQ7UzGE8eyuSH27guZvdP3S4h+sXjxwlfkoUsPNXcdktb
MrwfJ7pZNvq/4xhprm5deWSa5y75++5EaezexUVmq5uVskL8VpIvDasuwzwuCae7
mwaZEFZ2WLYrTwTCMJY0wPuCSx3XM9sAQvT9Zo+qU4wr4dec/GWMYckQjlh3b/zt
W4wj2NCDnuJIEwa8JmuB1vwMHTpdSZYV3FaU30ipZwT5P7JOPVIO0EnddYnVB5L2
ooz463NC+WgL4CdgTKHAApw9hKGLoperxRKCQQIDAQABAoIBACmdNOXWLW9l4Dfm
Uo8PA1HtHgG7UMcTv0wzJ57gHjVupHvS+qjRzKP6Mh8joJEHBJFxfe3u1iEeSvqQ
6yxnB+a5uFxYWO3KKT0ZbKTXrCfBRZzXyPdnoqq2ZhAyp6HG+DiUFq9rPVGW/Qe0
R0xtW37SSPFxogVgJpUN7y7RZum5E8ioqLnj2lZTK+m44rkYX9MJuYD2zmSBjU8J
diMEefvcc2+M9e3ATwsmayPjY2JLAq94BUewmjKjrLIGdj4YvQXRwHSihXxlGH+2
/Vlh4x7W0B2AKtAl5MEmeS0s1fl5emnJYNQ/oUbTywdWfDULkmu++XpLG8sB1ADP
T8V+gaECgYEA7Dk9z8wQCqe1ddpX9P9/iqIDRLiVdf7ywju0K6rAq5YS+7ESBtdI
UQ4Aofgas861Uw5yOko0gUL550a084jSCF1jS6bpd8VF42pvnNRLfD5Qvnx85Hb/
2cxplhsPm4Qs4EQtGS2atnuTi5dzpBw2SXHz7q3/uVAGxfD2k2Zfm+0CgYEA1tOk
qEpuRZOCaDHD6WNfY8dow2ZJG2HdkS5b84LU624x5Mg0mZ8nV1Q5QeYeBR0mh3AK
2l8ym8oEBml121I3ePnE2RHC+ZZ3AjYX5utm+FAaIy3PfYma4ZKHAAbzaqZiufpL
IimA6Y9M6C/yeofFCXg4WGV1cHrC6HpNwrlPvSUCgYBuDsVTuVmH/Vc4D4CHbzq3
FoC7Vdyc2ymzgSmSZr3Zs9QoW/lwIoFY8lMtI3EdcSNeDQ7/RW9tAg14yaKpDIf+
ybPnnV6NJOZI7GAQC4EX49iux0VRFHhjuy8+dmExLcXSvzDb2DmK58x4bxm7zkEk
uujRvVO1vSlKs9fOGRZWiQKBgAyppwAAeyWAv2QS4uBj6iCAhY1W/yItD5laKTNe
V9yUvz4kCu2vMutz3Hgk70oP4NCE6y7aA6qpKrmSB89otjpcRp0g7eQ2WUd5FsmW
wJgMaK4AKkXVEZsQPwLiZgC4VANwXHNkDOQ3fSoGgX11eUjInFGhkV2S7uAzLVg4
Aho9AoGAbxOmGs54RI9bGc2OtgfmGoTaGiNgr71BbCFUV4WouOnoTVWb1kepZh8p
M9wEk645Jpshpzah6veHQQlCxDNL78l7r4PqiD+QXDBbwF/MwDGNHMoKXTjljck4
a6h6FpuHWmQX2tOmL8M4uwJhZlRNBmg0PjM5ljaRoAGpn7wZBY4 = ";



        //// 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        //public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxjskyZyyleNWaIvS/vM0hwXqd+4vAm3AzvWdHv63uH051yDVFnLyOfV4Vwpgnx9Tx2XFQ7UzGE8eyuSH27guZvdP3S4h+sXjxwlfkoUsPNXcdktbMrwfJ7pZNvq/4xhprm5deWSa5y75++5EaezexUVmq5uVskL8VpIvDasuwzwuCae7mwaZEFZ2WLYrTwTCMJY0wPuCSx3XM9sAQvT9Zo+qU4wr4dec/GWMYckQjlh3b/ztW4wj2NCDnuJIEwa8JmuB1vwMHTpdSZYV3FaU30ipZwT5P7JOPVIO0EnddYnVB5L2ooz463NC+WgL4CdgTKHAApw9hKGLoperxRKCQQIDAQAB";
        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhJanVT6a9WjZ9U3XYO0eUXgvHz0HLEpK+cc80a9V56k72f8QIw6HIAxlI08WfaJwDCmfZUMH+CpvF/IlRPawoH9223yXY+5HUEFdTrmkopVbp2UY3keB5/+yBZxoGOUsKC2GATW7xE8KhrVNAkapaxIFBU0LIeKqvjb4WRz4H5WISdgKIIzJMK3muMh2FVG9QCl3zzt4TVhPK496Ki0KZ58FONb4HKIeRfwe1U4BWXRh2QFyCVzk9b8S8OkLbzAebwSclHey+fkDGxsNxW5UfiZPGc699azd2CMkIxO8ZLGphk12lqngFvlLM/9rvXV71rn7cbmX73w7R8uNiykYGwIDAQAB";

        // 签名方式
        public static string sign_type = "RSA2";

        // 编码格式
        public static string charset = "UTF-8";
    }
}