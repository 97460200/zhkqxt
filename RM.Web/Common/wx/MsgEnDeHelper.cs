using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace Tencent
{
    public class MsgEnDeHelper
    {
        /// <summary>
        /// 检验消息的真实性，并且获取解密后的明文
        /// </summary>
        /// <param name="sMsgSignature">签名串，对应URL参数的msg_signature</param>
        /// <param name="sTimeStamp">时间戳，对应URL参数的timestamp</param>
        /// <param name="sNonce"> 随机串，对应URL参数的nonce</param>
        /// <param name="sPostData">密文，对应POST请求的数据</param>
        /// <param name="sMsg">解密后的原文，当return返回0时有效</param>
        /// <returns>成功0，失败返回对应的错误码</returns>
        public static int DecryptMsg(string appId, string sMsgSignature, string sTimeStamp, string sNonce, string sPostData, ref string sMsg)
        {
            string sToken = "sewapower";
            string sAppID = appId;
            string sEncodingAESKey = "Sd8AFrmKTlF2u5jbQl8vQEYJX57aALEz1OivuIUgD3r";
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
            return wxcpt.DecryptMsg(sMsgSignature, sTimeStamp, sNonce, sPostData, ref sMsg);

        }
        /// <summary>
        /// 将企业号回复用户的消息加密打包
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="sReplyMsg">企业号待回复用户的消息，xml格式的字符串</param>
        /// <param name="sTimeStamp">时间戳，可以自己生成，也可以用URL参数的timestamp</param>
        /// <param name="sNonce">随机串，可以自己生成，也可以用URL参数的nonce</param>
        /// <param name="sEncryptMsg">加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,</param>
        /// <returns>成功0，失败返回对应的错误码</returns>
        public static int EncryptMsg(string appId, string sReplyMsg, string sTimeStamp, string sNonce, ref string sEncryptMsg)
        {
            string sToken = "sewapower";
            string sAppID = appId;
            string sEncodingAESKey = "Sd8AFrmKTlF2u5jbQl8vQEYJX57aALEz1OivuIUgD3r";
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
            return wxcpt.EncryptMsg(sReplyMsg, sTimeStamp, sNonce, ref sEncryptMsg);
        }
    }
}
