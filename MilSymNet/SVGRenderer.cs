using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MilSymNetUtilities;
using System.Drawing;
using System.Diagnostics;

namespace MilSymNet
{
    /**
     * @deprecated
     * */
    public class SVGRenderer
    {
        private static object syncLock = new object();
        private static SVGRenderer _instance = null;
        private static char[] svgCommands = {'M','m','Z','z','L','l','H','h','V','v','C','c','S','s','Q','q','T','t','A','a'};
        private static char[] svgCommands2 = { 'M', 'm', 'L', 'l', 'H', 'h', 'V', 'v', 'C', 'c', 'S', 's', 'Q', 'q', 'T', 't', 'A', 'a' };
        private static string _regex = "";
        
        private SVGRenderer()
        {
            Init();
        }

        public static SVGRenderer getInstance()
        {
            lock (syncLock)
            {
                if (_instance == null)
                    _instance = new SVGRenderer();
            }

            return _instance;
        }

        private void Init()
        {
            //@"(?<=[M,m])"
            StringBuilder sb = new StringBuilder();
            sb.Append(@"(?=[");
            for (int i = 0; i < svgCommands.Length; i++ )
            {
                sb.Append(svgCommands[i]);
                if (i < (svgCommands.Length - 1))
                    sb.Append(',');
            }
            sb.Append("])");
            _regex = sb.ToString();//@"(?=[" + sb.ToString() + "])";

            SymbolSVGTable st = SymbolSVGTable.getInstance();
            UnitSVGTable ut = UnitSVGTable.getInstance();
        }

        public void renderSVGPathToGraphics(Graphics g)
        {
            //singlepoint graphic with absolute commands
            string path = "M1051 544L876 443L846 504L1018 600L1051 544ZM684 341L509 241L479 301L651 397L684 341ZM395 187L275 119L247 174L362 240L395 187ZM-833 -474L-1009 -568L-1038 -508L-864 -415L-833 -474ZM-237 -158L-411 -251L-439 -193L-271 -101L-237 -158ZM-533 -317L-709 -411L-739 -351L-567 -260L-533 -317ZM662 -399L514 -316L544 -263L690 -344L662 -399ZM-883 456L-1029 537L-999 592L-852 511L-883 456ZM992 -580L826 -488L859 -436L1021 -526L992 -580ZM-593 296L-748 382L-718 438L-566 350L-593 296ZM-305 137L-442 214L-415 268L-277 193L-305 137ZM381 -242L235 -161L272 -112L411 -188L381 -242ZM-210 -90L-111 -80Q-102 -130 -75 -153T-1 -176Q48 -176 73 -156T98 -107Q98 -89 88 -77T51 -55Q33 -49 -31 -33Q-112 -13 -145 17Q-191 58 -191 118Q-191 156 -170 189T-107 240T-8 258Q87 258 134 217T185 106L83 101Q77 140 56 157T-9 174Q-53 174 -78 156Q-94 144 -94 125Q-94 107 -79 94Q-60 78 14 61T123 25T179 -26T199 -107Q199 -150 175 -187T107 -243T-2 -262Q-98 -262 -149 -218T-210 -90Z";
            //singlepoint graphic with relative commands
            string pathLC = "M1051 544l-175 -101l-30 61l172 96zM684 341l-175 -100l-30 60l172 96zM395 187l-120 -68l-28 55l115 66zM-833 -474l-176 -94l-29 60l174 93zM-237 -158l-174 -93l-28 58l168 92zM-533 -317l-176 -94l-30 60l172 91zM662 -399l-148 83l30 53l146 -81zM-883 456l-146 81l30 55l147 -81zM992 -580l-166 92l33 52l162 -90zM-593 296l-155 86l30 56l152 -88zM-305 137l-137 77l27 54l138 -75zM381 -242l-146 81l37 49l139 -76zM-210 -90l98.999 10c6 -33.333 18.167 -57.666 36.5 -72.999s42.833 -23 73.5 -23c32.667 0 57.334 6.83301 74.001 20.5s25 29.834 25 48.501c0 12 -3.5 22.167 -10.5 30.5s-19.167 15.5 -36.5 21.5c-12 4 -39.333 11.333 -82 22c-54 13.333 -92 30 -114 50c-30.667 27.333 -46 61 -46 101c0 25.333 7.16699 49.166 21.5 71.499s35.166 39.333 62.499 51s60.333 17.5 99 17.5c63.333 0 110.833 -13.833 142.5 -41.5s48.5 -64.5 50.5 -110.5l-102 -5c-4 26 -13.167 44.667 -27.5 56s-35.833 17 -64.5 17c-29.333 0 -52.333 -6 -69 -18c-10.667 -8 -16 -18.333 -16 -31c0 -12 5 -22.333 15 -31c12.667 -10.667 43.667 -21.834 93 -33.501s85.833 -23.667 109.5 -36s42.167 -29.333 55.5 -51s20 -48.5 20 -80.5c0 -28.667 -8 -55.5 -24 -80.5s-38.667 -43.667 -68 -56s-65.666 -18.5 -108.999 -18.5c-64 0 -113 14.667 -147 44s-54.333 72 -61 128z";
            //the letter S
            string pathS = "M74 477L362 505Q388 360 467 292T682 224Q825 224 897 284T970 426Q970 478 940 514T833 578Q781 596 596 642Q358 701 262 787Q127 908 127 1082Q127 1194 190 1291T373 1440T662 1491Q938 1491 1077 1370T1224 1047L928 1034Q909 1147 847 1196T659 1246Q530 1246 457 1193Q410 1159 410 1102Q410 1050 454 1013Q510 966 726 915T1045 810T1207 661T1266 427Q1266 301 1196 191T998 28T679 -26Q401 -26 252 102T74 477Z";
            //air control point
            string pathACP = "M-355 354c-96.667 -97.333 -145 -215 -145 -353c0 -137.333 48.833 -254.666 146.5 -351.999s215.167 -146 352.5 -146c138 0 255.833 48.667 353.5 146s146.5 214.666 146.5 351.999c0 138 -48.5 255.667 -145.5 353s-215.167 146 -354.5 146 s-257.333 -48.667 -354 -146zM287.5 291c79.667 -80 119.498 -176.667 119.498 -290s-40.167 -210 -120.5 -290s-177.166 -120 -290.499 -120c-114 0 -211 40 -291 120s-120 176.667 -120 290s40 210 120 290s177 120 291 120c114.667 0 211.834 -40 291.501 -120zM-84.002 18h-47l-20 55h-89l-18 -55h-49l88 243h47zM-171.002 111l-29 86l-29 -86h58zM56.998 105l42.999 -15.999c-7.33301 -25.333 -18.666 -44.5 -33.999 -57.5s-34.333 -19.5 -57 -19.5c-29.333 0 -53.333 11 -72 33s-28 52 -28 90c0 40 9.66699 71.167 29 93.5 s44 33.5 74 33.5c26.667 0 48.334 -8.66699 65.001 -26c10 -10 17.667 -25 23 -45l-44 -12c-2.66699 13.333 -8.33398 23.666 -17.001 30.999s-18.334 11 -29.001 11c-16 0 -29 -6.66699 -39 -20s-15 -34 -15 -62c0 -30.667 4.83301 -52.667 14.5 -66s22.5 -20 38.5 -20 c10.667 0 20.334 4.33301 29.001 13s15 21.667 19 39zM126.997 18.001l0.000976562 242.998h72c27.333 0 45.333 -1.66699 54 -5c13.333 -2.66699 24.166 -10.167 32.499 -22.5s12.5 -28.166 12.5 -47.499c0 -15.333 -2.5 -28.166 -7.5 -38.499s-11.333 -18.333 -19 -24 s-15.5 -9.5 -23.5 -11.5c-10.667 -2 -26 -3 -46 -3h-30v-91h-45zM171.998 218.999v-68.999h25c18 0 30 1.16699 36 3.5s10.667 6.16602 14 11.499s5 12 5 20s-2.5 15 -7.5 21s-10.833 10 -17.5 12c-5.33301 0.666992 -16.333 1 -33 1h-22z";
            string AAMFrame = "M825 -750q-37 1143 -373 1519q-178 251 -452 281q-274 -30 -452 -283q-336 -374 -373 -1517h-129v168q9 1127 453 1560q248 228 501 232q253 -4 501 -232q444 -433 453 -1560v-168h-129z";
            string AAMFill = "M-866 -750q39 1200 391 1593q187 266 475 297q288 -31 475 -295q352 -395 391 -1595h-1732z";
            string AAMS1 = "M-182 -100h-88l-34 91h-162l-32 -91h-87l158 400h84zM-331 60l-57 145l-54 -145h111zM0 600l-110 -130v-745l-150 -95v-200l260 105l260 -105v200l-150 95v745zM588 -100h-88l-34 91h-162l-32 -91h-87l158 400h84zM439 60l-57 145l-54 -145h111zM70 440v-744l154 -89 v-122l-224 93l-224 -93v122l154 89v744l70 90z";
            string AAMS2 = "M70 440v-744l154 -89v-122l-224 93l-224 -93v122l154 89v744l70 90z";
            //string regex = 
            string[] parts = Regex.Split(path,_regex);

            foreach (String val in parts)
            {
                Console.WriteLine(val);
            }
            Console.WriteLine(_regex);

            SVGPath s = new SVGPath("0", pathACP);
            Pen lineColor = new Pen(Color.Black,2f);
            lineColor.MiterLimit = 3;
            Pen fillColor = new Pen(Color.Cyan);
            /*Image foo = s.Draw(400,400,lineColor,fillColor);
            g.DrawImage(foo, 200f, 200f);*/

            //Air to Air Missle test
            SVGPath sFill = new SVGPath("0",AAMFill);
            SVGPath sFrame = new SVGPath("0", AAMFrame);
            SVGPath sS1 = new SVGPath("0", AAMS1);
            SVGPath sS2 = new SVGPath("0", AAMS2);
            Image foo = sFrame.Draw(400, 400, lineColor, fillColor);
            g.DrawImage(foo, 200f, 200f);

            //speed test
            int count = 1;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for(int k = 0; k < count; k++)
            {
                s = new SVGPath("0", path);
                s.Draw(60, 60, lineColor, fillColor);
            }
            sw.Stop();
            Console.WriteLine("Rendered " + count.ToString() + " SP tactical graphics in " + Convert.ToString(sw.ElapsedMilliseconds / 1000.0) + " seconds.");
            
        }
    }
}
