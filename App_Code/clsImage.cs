using System;
using System.Drawing.Imaging;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for clsImage
/// </summary>
public class clsImage
{
	public clsImage()
	{
		//
		// TODO: Add constructor logic here
		//


	}

    public static ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        if (mimeType.ToUpper() == "IMAGE/PJPEG")
            mimeType = "image/jpeg";

        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j < encoders.Length; ++j)
        {
            if (encoders[j].MimeType.ToUpper() == mimeType.ToUpper())
                return encoders[j];
        } return null;
    }

}
