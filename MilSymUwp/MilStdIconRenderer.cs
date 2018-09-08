using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using MilSymNetUtilities;

namespace MilSymUwp
{
    public class MilStdIconRenderer
    {
        private static MilStdIconRenderer _instance = null;

        private static object syncLock = new object();

        public static MilStdIconRenderer getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new MilStdIconRenderer();
                }
            }

            return _instance;
        }

        private MilStdIconRenderer()
        {
            Init();
        }

        private void Init()
        {
            try
            {
                Debug.Write("MilStdIconRenderer.Init()");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                SymbolDefTable.getInstance();
                Debug.Write("SymbolDefTable Loaded");
                SinglePointLookup.getInstance();
                Debug.Write("SinglePointLookup Loaded");
                SymbolSVGTable.getInstance();
                Debug.Write("SymbolSVGTable Loaded");


                UnitDefTable.getInstance();
                Debug.Write("UnitDefTable Loaded");
                UnitFontLookup.getInstance();
                Debug.Write("UnitFontLookup Loaded");
                UnitSVGTable.getInstance();
                Debug.Write("UnitSVGTable Loaded");

                TacticalGraphicLookup.getInstance();
                Debug.Write("TacticalGraphicLookup Loaded");
                TGSVGTable.getInstance();
                Debug.Write("TGSVGTable Loaded");//*/
                sw.Stop();
                string ExecutionTimeTaken = string.Format("Minutes :{0} Seconds :{1} Mili seconds :{2}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds);
                Debug.WriteLine("Init Time: " + ExecutionTimeTaken);
            }
            catch (InvalidOperationException ioe)
            {
                Debug.Write(ioe.Message);
                Debug.Write(ioe.StackTrace);
            }
            catch (Exception exc)
            {
                Debug.Write(exc.Message);
                Debug.Write(exc.StackTrace);
            }
        }


        public ImageInfo Render(String symbolID, Dictionary<int, String> modifiers, Dictionary<int, String> attributes, CanvasDevice device)
        {
            SinglePointRenderer spr = SinglePointRenderer.getInstance();

            int symStd = 1;
            
            if (modifiers != null && modifiers[MilStdAttributes.SymbologyStandard] != null)
            {
                symStd = Convert.ToInt32(modifiers[MilStdAttributes.SymbologyStandard]);
            }
            else
            {
                if (modifiers == null)
                    modifiers = new Dictionary<int, String>();
                modifiers[MilStdAttributes.SymbologyStandard] = Convert.ToString(RendererSettings.getInstance().getSymbologyStandard());
            }

            var basicID = SymbolUtilities.getBasicSymbolIDStrict(symbolID);

            if (SymbolUtilities.isTacticalGraphic(symbolID))
            {
                SymbolDef sd = SymbolDefTable.getInstance().getSymbolDef(basicID, symStd);
                if (sd == null)
                {
                    sd = SymbolDefTable.getInstance().getSymbolDef(basicID, symStd);
                }

                if (sd != null && sd.getDrawCategory() == SymbolDef.DRAW_CATEGORY_POINT)
                {
                    return spr.RenderSPTG(symbolID, modifiers, attributes);
                }
                else
                {
                    Color color = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getLineColorOfAffiliation(symbolID));
                    int size = 35;
                    if (attributes.ContainsKey(MilStdAttributes.PixelSize))
                        size = int.Parse(attributes[MilStdAttributes.PixelSize]);

                    if (modifiers.ContainsKey(MilStdAttributes.LineColor))
                        color = RenderUtilities.getColorFromHexString(modifiers[MilStdAttributes.LineColor]);
                    if (modifiers.ContainsKey(MilStdAttributes.PixelSize))
                        size = int.Parse(modifiers[MilStdAttributes.PixelSize]);
                    return TacticalGraphicIconRenderer.getIcon(symbolID,size, color,0);
                    
                }
            }
            else if (UnitFontLookup.getInstance().getLookupInfo(basicID) != null)
            {
                return spr.RenderUnit(symbolID, modifiers, attributes, device);
            }
            else
            {
                //symbolID = SymbolUtilities.reconcileSymbolID(symbolID, false);
                return spr.RenderUnit(symbolID, modifiers, attributes, null);
            }
        }

        private ImageInfo renderTacticalMultipointIcon(String symbolID)
        {
            return TacticalGraphicIconRenderer.getIcon(symbolID, 30, Colors.Red, 0);
            //return spr.renderTacticalMultipointIcon(symbolID, modifiers);
        }
    }
}
