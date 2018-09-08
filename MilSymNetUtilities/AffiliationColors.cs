using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MilSymNetUtilities
{
    public class AffiliationColors
    {
        /// <summary>
        /// Friendly Unit Fill Color.
        /// </summary>
        public static Color FriendlyUnitFillColor = Color.Cyan;
        /// <summary>
        /// Hostile Unit Fill Color.
        /// </summary>
        public static Color HostileUnitFillColor = Color.Red;
        /// <summary>
        /// Neutral Unit Fill Color.
        /// </summary>
        public static Color NeutralUnitFillColor = Color.FromArgb(0, 255, 0);//Color.FromArgb(144,238,144);//light green//Color.GREEN;Color.FromArgb(0,226,0);
        /// <summary>
        /// Unknown Unit Fill Color.
        /// </summary>
        public static Color UnknownUnitFillColor = Color.Yellow;

        /// <summary>
        /// Friendly Graphic Fill Color.
        /// </summary>
        public static Color FriendlyGraphicFillColor = Color.FromArgb(128, 224, 255);//Crystal Blue //Color.CYAN;
        /// <summary>
        /// Hostile Graphic Fill Color.
        /// </summary>
        public static Color HostileGraphicFillColor = Color.FromArgb(255, 128, 128);//salmon
        /// <summary>
        /// Neutral Graphic Fill Color.
        /// </summary>
        public static Color NeutralGraphicFillColor = Color.FromArgb(170, 255, 170);//Bamboo Green //Color.FromArgb(144,238,144);//light green
        /// <summary>
        /// Unknown Graphic Fill Color.
        /// </summary>
        public static Color UnknownGraphicFillColor = Color.FromArgb(255, 255, 128);//light yellow  Color.FromArgb(255,255,224);//light yellow

        /// <summary>
        /// Friendly Unit Line Color.
        /// </summary>
        public static Color FriendlyUnitLineColor = Color.Black;
        /// <summary>
        /// Hostile Unit Line Color.
        /// </summary>
        public static Color HostileUnitLineColor = Color.Black;
        /// <summary>
        /// Neutral Unit Line Color.
        /// </summary>
        public static Color NeutralUnitLineColor = Color.Black;
        /// <summary>
        /// Unknown Unit Line Color.
        /// </summary>
        public static Color UnknownUnitLineColor = Color.Black;

        /// <summary>
        /// Friendly Graphic Line Color.
        /// </summary>
        public static Color FriendlyGraphicLineColor = Color.Black;
        /// <summary>
        /// Hostile Graphic Line Color.
        /// </summary>
        public static Color HostileGraphicLineColor = Color.Red;
        /// <summary>
        /// Neutral Graphic Line Color.
        /// </summary>
        public static Color NeutralGraphicLineColor = Color.Green;
        /// <summary>
        /// Unknown Graphic Line Color.
        /// </summary>
        public static Color UnknownGraphicLineColor = Color.Yellow;


        public static Color WeatherRed = Color.FromArgb(198, 16, 33);
        public static Color WeatherBlue = Color.FromArgb(0, 0, 255);

        public static Color WeatherPurpleDark = Color.FromArgb(128, 0, 128);//0x800080;// 128,0,128 Plum Red
        public static Color WeatherPurpleLight =  Color.FromArgb(226, 159, 255);//0xE29FFF;// 226,159,255 Light Orchid

        public static Color WeatherBrownDark = Color.FromArgb(128, 98, 16);//0x806210;// 128,98,16 Safari
        public static Color WeatherBrownLight = Color.FromArgb(210, 176, 106);//0xD2B06A;// 210,176,106 Khaki
       

    }
}
