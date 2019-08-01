using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/// <summary>
///GenThumManager 的摘要说明
/// </summary>
public class CompressPic
{
    public CompressPic()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public static void Compress(string fileName, string newFile)
    {
        int maxHeight = 1075;
        int maxWidth = 765;
        long qualitys = 100;

        using (System.Drawing.Image img = System.Drawing.Image.FromFile(fileName))
        {

            System.Drawing.Imaging.ImageFormat

            thisFormat = img.RawFormat;

            Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);

            Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);

            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height),

           0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

            g.Dispose();

            // 以下代码为保存图片时,设置压缩质量

            EncoderParameters encoderParams = new EncoderParameters();

            long[] quality = new long[1];

            quality[0] = qualitys;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.

            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

            ImageCodecInfo jpegICI = null;

            for (int x = 0;

            x < arrayICI.Length;

            x++)
            {

                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {

                    jpegICI = arrayICI[x];
                    //设置JPEG编码

                    break;

                }

            }

            if (jpegICI != null)
            {
                outBmp.Save(newFile, jpegICI, encoderParams);

            }

            else
            {

                outBmp.Save(newFile, thisFormat);

            }

            img.Dispose();

            outBmp.Dispose();

        }

    }

    private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
    {
        double w = 0.0;
        double h = 0.0;
        double sw = Convert.ToDouble(width);
        double sh = Convert.ToDouble(height);
        double mw = Convert.ToDouble(maxWidth);
        double mh = Convert.ToDouble(maxHeight);
        if (sw < mw && sh < mh)
        {
            w = sw;
            h = sh;
        }
        else if ((sw / sh) > (mw / mh))
        {
            w = maxWidth;
            h = (w * sh) / sw;
        }
        else
        {
            h = maxHeight;
            w = (h * sw) / sh;
        }
        return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
    }
}
