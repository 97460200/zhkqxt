using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace MsgCryptTest
{
    public class Sample
    {


        static void Main(string[] args)
        {
            //公众平台上开发者设置的token, appID, EncodingAESKey
            string sToken = "sewapower";
            string sAppID = "wx1939f9ee65384f14";
            string sEncodingAESKey = "Sd8AFrmKTlF2u5jbQl8vQEYJX57aALEz1OivuIUgD3r";

            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

            string sReqMsgSig = "c4a5de44f8cfead9f0344023c176204a6283d903";
            string sReqTimeStamp = "1552468390";
            string sReqNonce = "570040320";
            string sReqData = @"<xml>
    <ToUserName><![CDATA[gh_f0a63a0b8dee]]></ToUserName>
    <Encrypt><![CDATA[Vu7zu+d43fi2wi35UFAhY5UbNult7drtKnAuWkgEOVSTC+LO9EdsGUBpqsrY44YE6QfPmQ75kg2k2eRyDTQRB57lRHXdLGMuuJqCCSzgh7k7e+UvvqQ69EwPZBhBv1Td63RCgGPl9UYtSOP4AhWWFHWN1Yw5NOTQVid8pPkWYC3fPymUJ5cRadAoqvOpwIpsYxqBnmv7vVg48x/VyXcEH+/Yj9LiLkVIKFkZBriMeMD9tOthvxX65eSIh5AOtYEBnVHepy3WpEOQSlQ9jSgJVY3uXN9s6IYGLoPntpIW29jAFlHeNHw1l7dmP29wPvd6QOEQtg1XnYfazpdqU2MLWHtBw9PHAFEKaJvhdoVC0/zyPbRUGeNnBMKagtEekGdthM44PQub5mUFC/PjgEj1g4ej2ELwq1RuJvbGnG+R097JIufz76T50ckeDc2QTxVGfJZgKMk//lwjF58PfYfZrA==]]></Encrypt>
</xml>
";
            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            if (ret != 0)
            {
                System.Console.WriteLine("ERR: Decrypt fail, ret: " + ret);
                return;
            }
            System.Console.WriteLine(sMsg);


            string sRespData = "<xml><ToUserName><![CDATA[o4jhJ6DQSJP6m8RsJ_Qt6Iexh48I]]></ToUserName><FromUserName><![CDATA[gh_f0a63a0b8dee]]></FromUserName><CreateTime>1552468390</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[TESTCOMPONENT_MSG_TYPE_TEXT_callback]]></Content></xml>";

            string sEncryptMsg = ""; //xml格式的密文
            ret = wxcpt.EncryptMsg(sRespData, sReqTimeStamp, sReqNonce, ref sEncryptMsg);
            System.Console.WriteLine("sEncryptMsg");
            System.Console.WriteLine(sEncryptMsg);

            /*测试：
             * 将sEncryptMsg解密看看是否是原文
             * */
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sEncryptMsg);
            XmlNode root = doc.FirstChild;
            string sig = root["MsgSignature"].InnerText;
            string enc = root["Encrypt"].InnerText;
            string timestamp = root["TimeStamp"].InnerText;
            string nonce = root["Nonce"].InnerText;
            string stmp = "";
            ret = wxcpt.DecryptMsg(sig, timestamp, nonce, sEncryptMsg, ref stmp);
            System.Console.WriteLine("stemp");
            System.Console.WriteLine(stmp + ret);
            return;
        }
    }
}
