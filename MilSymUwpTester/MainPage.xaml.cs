using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Reflection;
using Microsoft.Graphics.Canvas.Text;
using MilSymNetUtilities;
using MilSymUwp;
using System.Diagnostics;
using Windows.UI.Popups;
using Microsoft.Graphics.Canvas;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MilSymUwpTester
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Windows.UI.Input.PointerPoint _pp = null;
        private Boolean drawUnit = false;
        private Boolean drawTG = false;
        
        public MainPage()
        {
            this.InitializeComponent();

            init();
        }

        /// <summary>
        /// Setup test page controls
        /// </summary>
        private void init()
        {
            try
            {
                //First get instance call initialized the renderer and its required classes.
                MilStdIconRenderer.getInstance();
                //initialize controls

                //Populate TG list
                System.Collections.Generic.List<SymbolDef> symbolDefs = SymbolDefTable.getInstance().getAllSymbolDefs().Values.ToList();
                symbolDefs.Sort();
                foreach (SymbolDef sd in symbolDefs)
                {
                    String itemTG = sd.getDescription() + "|" + sd.getBasicSymbolId();
                    if (sd.getDrawCategory() != 0 && sd.getBasicSymbolId().Contains("BS_") == false)
                        lbTGList.Items.Add(itemTG);
                }

                //Populate Unit List
                System.Collections.Generic.List<UnitDef> unitDefs = UnitDefTable.getInstance().GetAllUnitDefs().Values.ToList();
                unitDefs.Sort();
                foreach (UnitDef ud in unitDefs)
                {
                    String itemUnit = ud.getDescription() + "|" + ud.getBasicSymbolId();
                    if (ud.getDrawCategory() != 0)
                        lbUnitList.Items.Add(itemUnit);
                }

                //setup affiliations
                cbAffiliation.Items.Add("P");
                cbAffiliation.Items.Add("U");//Unknown
                cbAffiliation.Items.Add("F");//Friend
                cbAffiliation.Items.Add("N");//NEutral
                cbAffiliation.Items.Add("H");//Hostile
                cbAffiliation.Items.Add("A");
                cbAffiliation.Items.Add("S");
                cbAffiliation.Items.Add("G");
                cbAffiliation.Items.Add("W");
                cbAffiliation.Items.Add("D");
                cbAffiliation.Items.Add("L");
                cbAffiliation.Items.Add("M");
                cbAffiliation.Items.Add("J");//Joker
                cbAffiliation.Items.Add("K");//Faker
                cbAffiliation.SelectedIndex = 2;

                //setup status
                cbStatus.Items.Add("A");//anticipated/planned
                cbStatus.Items.Add("P");//present
                cbStatus.Items.Add("C");//present/fully capable
                cbStatus.Items.Add("D");//present/damaged
                cbStatus.Items.Add("X");//present/destroyed
                cbStatus.Items.Add("F");//present/full to capacity
                cbStatus.SelectedIndex = 1;

                chkbKeepUnitRatio.IsChecked = true;
                chkbLabels.IsChecked = false;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
                Debug.WriteLine(exc.Source);
            }

        }

        void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {

            /*args.DrawingSession.DrawEllipse(155, 115, 80, 30, Colors.Black, 3);

            var format = new CanvasTextFormat();

            format.FontFamily = "Arial";
            format.FontSize = 32.0f;
            format.FontStyle = Windows.UI.Text.FontStyle.Normal;//normal / italic
            format.FontWeight = Windows.UI.Text.FontWeights.Normal;//normal / bold
            format.VerticalAlignment = CanvasVerticalAlignment.Top;
            args.DrawingSession.DrawText("Hello, world!", 100, 100, Colors.Yellow,format);//*/

            MilStdIconRenderer r = MilStdIconRenderer.getInstance();
            ImageInfo foo = null;
            try
            {
                char[] separator = { '|' };
                String symbolID = null; //"SUPPT----------";
                //String status = "P";
                //String affiliation = "F";
                String size = "100";
                Dictionary<int, String> attributes = new Dictionary<int, String>();
                string keepUnitRatio = "false";
                if (chkbKeepUnitRatio.IsChecked == true)
                    keepUnitRatio = "true";
                if (SymbolUtilities.isNumber(tbSize.Text))
                {
                    size = tbSize.Text;
                }
                if (lbUnitList.SelectedItem != null && drawUnit == true)
                {
                    symbolID = ((String)lbUnitList.SelectedItem).Split(separator)[1];
                    symbolID = SymbolUtilities.setAffiliation(symbolID, (String)cbAffiliation.SelectedItem);
                    symbolID = SymbolUtilities.setStatus(symbolID, (String)cbStatus.SelectedItem);
                    symbolID = symbolID.Substring(0, 10) + Convert.ToString(tbModifier.Text);
                    attributes.Add(MilStdAttributes.PixelSize, size);
                    attributes.Add(MilStdAttributes.KeepUnitRatio, keepUnitRatio);
                    foo = r.Render(symbolID, null, attributes,null);
                }
                if (lbTGList.SelectedItem != null && drawTG == true)
                {
                    symbolID = ((String)lbTGList.SelectedItem).Split(separator)[1];
                    if (SymbolUtilities.isWeather(symbolID) == false)
                    {
                        symbolID = SymbolUtilities.setAffiliation(symbolID, (String)cbAffiliation.SelectedItem);
                        symbolID = SymbolUtilities.setStatus(symbolID, (String)cbStatus.SelectedItem);
                    }
                    attributes.Add(MilStdAttributes.PixelSize, size);
                    attributes.Add(MilStdAttributes.KeepUnitRatio, keepUnitRatio);
                    foo = r.Render(symbolID, null, attributes, null);
                }
                if (foo != null && _pp != null)
                    args.DrawingSession.DrawImage(foo.getCanvasRenderTarget(), (float)(_pp.Position.X - foo.getAnchorPoint().X), (float)(_pp.Position.Y - foo.getAnchorPoint().Y));

                //////SPEEDTEST///////////////////////////////////////////////
                bool speedTest = false;
                if(speedTest && lbUnitList.SelectedItem != null)
                {
                    CanvasDevice cd = CanvasDevice.GetSharedDevice();
                    Stopwatch sw = new Stopwatch();
                    try
                    {
                        
                        sw.Start();
                        //MilStdBMP fooo = null;
                        for (int i = 0; i < 10000; i++)
                        {
                            r.Render(symbolID, null, attributes, cd);
                            /*fooo = r.Render(symbolID, null, attributes,cd);
                            fooo.getCanvasRenderTarget().Dispose();
                            fooo = null;//*/
                        }
                        sw.Stop();
                    }
                    catch(Exception exc)
                    {
                        Debug.WriteLine(exc.Message);
                        Debug.WriteLine(exc.StackTrace);
                    }
                    cd = null;
                    MessageDialog md = new MessageDialog(sw.Elapsed.Seconds.ToString() + "." + sw.Elapsed.Milliseconds);
                    Debug.WriteLine(sw.Elapsed.Seconds.ToString() + "." + sw.Elapsed.Milliseconds);
                }
                
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Microsoft.Graphics.Canvas
            //Assembly foo = System.Reflection.Assembly.
        }

        private void CanvasControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _pp = e.GetCurrentPoint(ccCanvas);
            ccCanvas.Invalidate();
        }

        private void lbUnitList_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            drawUnit = true;
            drawTG = false;
        }

        private void lbTGList_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            drawUnit = false;
            drawTG = true;
        }

        private void lbUnitList_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            drawUnit = true;
            drawTG = false;
        }

        private void lbTGList_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            drawUnit = false;
            drawTG = true;
        }

        private void lbTGList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drawUnit = false;
            drawTG = true;
        }

        private void lbUnitList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drawUnit = true;
            drawTG = false;
        }
    }
}
