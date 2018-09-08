using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using MilSymNetUtilities;

namespace MilSymUwp
{
    public class RenderUtilities
    {
        public static Color getIdealTextBackgroundColor(Color fgColor)
        {
            try
            {
                //an array of three elements containing the
                //hue, saturation, and brightness (in that order),
                //of the color with the indicated red, green, and blue components/
                float[] hsbvals = new float[3];

                if(fgColor != null)
                {
                    int nThreshold = RendererSettings.getInstance().getTextBackgroundAutoColorThreshold();//160;
                    int bgDelta = (int)((fgColor.R * 0.299) + (fgColor.G * 0.587) + (fgColor.B * 0.114));
                    //ErrorLogger.LogMessage("bgDelta: " + String.valueOf(255-bgDelta));
                    //if less than threshold, black, otherwise white.
                    return (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
                }
            }
            catch(Exception exc)
            {
                ErrorLogger.LogException("SymbolDraw", "getIdealtextBGColor", exc);
            }
            return Colors.White;
        }

        /**
        *
        * @param hexValue - String representing hex value
        * (formatted "0xRRGGBB" i.e. "0xFFFFFF")
        * OR
        * formatted "0xAARRGGBB" i.e. "0x00FFFFFF" for a color with an alpha value
        * I will also put up with "RRGGBB" and "AARRGGBB" without the starting "0x"
        * @return
        */
        public static Color getColorFromHexString(String hexValue)
        {
            try
            {
                String hexOriginal = hexValue;

                String hexAlphabet = "0123456789ABCDEF";


                if (hexValue[0] == '#')
                    hexValue = hexValue.Substring(1);
                if (hexValue.Substring(0, 2) == ("0x") || hexValue.Substring(0, 2) == ("0X"))
                    hexValue = hexValue.Substring(2);

                hexValue = hexValue.ToUpper();

                int count = hexValue.Length;
                int[] value = null;
                int k = 0;
                int int1 = 0;
                int int2 = 0;

                if (count == 8 || count == 6)
                {
                    value = new int[(count / 2)];
                    for (int i = 0; i < count; i += 2)
                    {
                        int1 = hexAlphabet.IndexOf(hexValue[i]);
                        int2 = hexAlphabet.IndexOf(hexValue[i + 1]);
                        value[k] = (int1 * 16) + int2;
                        k++;
                    }
                    
                    if (count == 8)
                    {

                        return Color.FromArgb((Byte)value[1], (Byte)value[2], (Byte)value[3], (Byte)value[0]);
                    }
                    else if (count == 6)
                    {
                        return Color.FromArgb(255,(Byte)value[0], (Byte)value[1], (Byte)value[2]);
                    }
                }
                else
                {
                    ErrorLogger.LogMessage("RendererUtilities", "getColorFromHexString", "Bad hex value: " + hexOriginal);
                }
                return Color.FromArgb(0,0,0,0);//transparent
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("RendererUtilities", "GetColorFromHexString", exc);
            }
            return Color.FromArgb(0, 0, 0, 0);//transparent
        }

        public static Windows.UI.Color DrawingColorToUIColor(System.Drawing.Color color)
        {
            if (color != null)
                return Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);
            else
                return Windows.UI.Color.FromArgb(0, 0, 0, 0);//transparent
        }


    }
}
