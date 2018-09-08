using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MilSymNetUtilities;
using MilSymNet;
using System.Diagnostics;
using System.Drawing.Imaging;



namespace MilSymNetTester
{
    public partial class Form1 : Form
    {

        

        public Form1()
        {
            InitializeComponent();
        }



        String symbolID = null;

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                //Graphics gg = this.CreateGraphics();
                //gg.GetHdc();
                //gg.DrawLine(new Pen(Color.Red), 0, 0, 900, 900);
                //gg.DrawArc(new Pen(Color.Red), 500, 500, 50, 50, 0, 90);
                //gg.ReleaseHdc();
                Graphics g = this.CreateGraphics();
                SVGRenderer.getInstance().renderSVGPathToGraphics(g);

                String symbolID = "SFZP------*****";
                String spaceStation = "SFPPT-----*****";
                String ambush = "GFGPSLA---*****";
                String checkPoint = "GFGPGPPK--****X";

                UnitDef ud = UnitDefTable.getInstance().getUnitDef(SymbolUtilities.getBasicSymbolID(symbolID));
                Console.WriteLine(ud.getDescription());
                SymbolDef sd = SymbolDefTable.getInstance().getSymbolDef(SymbolUtilities.getBasicSymbolID(ambush),1);
                Console.WriteLine(sd.getDescription());

                int mapping = SinglePointLookup.getInstance().getCharCodeFromSymbol(checkPoint);
                Console.WriteLine(mapping.ToString());

                UnitFontLookupInfo ufli = UnitFontLookup.getInstance().getLookupInfo(spaceStation);
                Console.WriteLine(ufli.getMapping1(spaceStation).ToString());

                SinglePointRenderer spr = SinglePointRenderer.getInstance();
                //Bitmap tempBMP = spr.DrawTest();
                //tempBMP.Save("C:\\test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //MilStdBMP msb = spr.RenderSymbol(spaceStation, null, null);
                //msb.getBitmap().Save("C:\\test.png", System.Drawing.Imaging.ImageFormat.Png);
                

                //Graphics g = Graphics.FromHwnd(this.Handle);
                //Graphics g = this.CreateGraphics();
                float x = this.Width / 2.0f;
                float y = this.Height / 2.0f;
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //g.DrawImageUnscaled(tempBMP, (int)x, (int)y);
                //g.Flush();
                //g.Dispose();
               // g.DrawImage(spr.DrawTest(), x, y);

                

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            SVGRenderer sr = SVGRenderer.getInstance();
            //sr.renderSVGPathToGraphics(this.CreateGraphics());
            
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (symbolID != null)
                {
                    SinglePointRenderer spr = SinglePointRenderer.getInstance();
                    Dictionary<int, String> attribs = new Dictionary<int, string>();
                    Dictionary<int, String> modifiers = new Dictionary<int, string>();
                    attribs.Add(MilStdAttributes.PixelSize, cbSize.Text);
                   // attribs.Add(MilStdAttributes.SymbolOutlineSize, "0");
                    //attribs.Add(MilStdAttributes.SymbolOutlineColor, "FF0000");
                    //attribs.Add(MilStdAttributes.SymbolOutlineSize, "2");

                    if (cbDrawModifiers.Checked == true)
                    {
                        if (symbolID[0] != 'G')
                            populateModifiersForUnits(modifiers);
                        else
                            populateModifiersForTGs(modifiers);
                    }
                    

                    ImageInfo ii = spr.RenderSymbol(symbolID, modifiers, attribs);
                    Graphics g = this.CreateGraphics();
                    Point offset = ii.getCenterPoint();
                    g.DrawImageUnscaled(ii.getBitmap(), e.X - offset.X, e.Y - offset.Y);
                    //bmp.getBitmap().Save("C:\\iconC.png", ImageFormat.Png);
                    symbolID = null;

                    //TEXT test render, TextRenderer much faster
                    /*
                    Font font = new Font("arial", 24);
                    String alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    Color[] colors = {Color.Black,Color.Magenta};
                    Brush[] brushes = {Brushes.Black,Brushes.Magenta};

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    for (int i = 0; i < 10000; i++ )
                        g.DrawString(alpha, font, brushes[i%2], 200, 300);
                    stopwatch.Stop();

                    Stopwatch stopwatch2 = Stopwatch.StartNew();
                    for (int j = 0; j < 10000; j++)
                        TextRenderer.DrawText(g, alpha, font, new Point(200, 340), colors[j % 2]);
                    stopwatch2.Stop();

                    
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    Console.WriteLine(stopwatch2.ElapsedMilliseconds);//*/

                    //text orientation test
                    /*Font font = new Font("arial", 24);
                    TextFormatFlags tff = TextFormatFlags.Top;
                    TextRenderer.DrawText(g, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", font, new Point(200, 340), Color.Black,tff);
                    Size fb = TextRenderer.MeasureText(g, "ABCDEFGHIJKLMNOPQRSTUVWXYZ",font,new Size(),tff);
                    g.DrawRectangle(Pens.Red, 200, 340, fb.Width, fb.Height);//*/
                }
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SinglePointRenderer", "DrawTest", exc);
            }

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            textBox1.Text = "pixels: " + e.X + ", " + e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            textBox1.Text = "pixels: " + e.X + ", " + e.Y;
        }

        /**
         * 
         * */
        private void Init()
        {
            try
            {
                SymbolDefTable sdTable = SymbolDefTable.getInstance();

                Dictionary<String, SymbolDef> symbolDefs = sdTable.getAllSymbolDefs();

                ICollection<SymbolDef> vc = symbolDefs.Values;

                IEnumerator<SymbolDef> enumerator = vc.GetEnumerator();
                SymbolDef sdTemp = null;
                UnitDef udTemp = null;
                String item = null;
                while (enumerator.MoveNext())
                {
                    sdTemp = enumerator.Current;
                    item = sdTemp.getDescription() + ":" + sdTemp.getBasicSymbolId();

                    if (sdTemp.getDrawCategory() != 0)//0 means category, not drawable
                    {
                        lbTGs.Items.Add(item);
                    }
                }
                lbTGs.Sorted = true;

                ////////////////////////////////////////////////////////////////////////

                UnitDefTable udTable = UnitDefTable.getInstance();

                Dictionary<String, UnitDef> unitDefs = udTable.GetAllUnitDefs();

                ICollection<UnitDef> c = unitDefs.Values;

                IEnumerator<UnitDef> ude = c.GetEnumerator();
                //SymbolDef temp = null;
                //String item = null;
                while (ude.MoveNext())
                {
                    udTemp = ude.Current;
                    item = udTemp.getDescription() + ":" + udTemp.getBasicSymbolId();
                    lbFEs.Items.Add(item);
                }
                lbFEs.Sorted = true;

                /////////////////////////////////////////////////////////////////////////
                cbAffiliation.SelectedIndex = 0;
                cbStatus.SelectedIndex = 1;
                cbModifiers.SelectedIndex = 0;
                cbSize.SelectedIndex = 0;
                cbOutlineType.SelectedIndex = 0;
                cbSpeedTestType.SelectedIndex = 1;
                cbDoubleBuffer.CheckState = CheckState.Checked;

                //RENDERER SETTINGS//////////////////////////////////////////////////////
                RendererSettings RS = RendererSettings.getInstance();
                RS.setTextBackgroundMethod(RendererSettings.TextBackgroundMethod_OUTLINE_QUICK);
                //RS.setTextBackgroundMethod(RendererSettings.TextBackgroundMethod_OUTLINE);
                //RS.setTextBackgroundMethod(RendererSettings.TextBackgroundMethod_NONE);
                //RS.setTextBackgroundMethod(RendererSettings.TextBackgroundMethod_COLORFILL);

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
        }

        private String getCodeFromFEList()
        {
            if (lbFEs.SelectedItem == null)
                lbFEs.SelectedIndex = 0;
            String code = Convert.ToString(lbFEs.SelectedItem);
            char[] splitter = {':'};
            code = code.Split(splitter)[1];

            String modifier = code.Substring(10, 2);
            if (cbModifiers.SelectedItem.ToString().Equals("--") == false)
            {
                modifier = cbModifiers.SelectedItem.ToString();
            }
            

            code = code.Substring(0, 1) + cbAffiliation.SelectedItem.ToString() +
                     code.Substring(2, 1) + cbStatus.SelectedItem.ToString()
                     + code.Substring(4, 6) + modifier + code.Substring(12,3);
            return code;
        }

        private String getCodeFromTGList()
        {
            if (lbTGs.SelectedItem == null)
                lbTGs.SelectedIndex = 0;
            String code = Convert.ToString(lbTGs.SelectedItem);
            char[] splitter = { ':' };

            code = code.Split(splitter)[1];

            if (code.Substring(0, 1) != "W")
            {
                code = code.Substring(0, 1) + cbAffiliation.SelectedItem.ToString() +
                         code.Substring(2, 1) + cbStatus.SelectedItem.ToString()
                         + code.Substring(4, 11);
            }
            return code;
        }

        private void btnSpeedTest_Click(object sender, EventArgs e)
        {
            Dictionary<int, String> modifiersTG = new Dictionary<int, string>();
            Dictionary<int, String> modifiersU = new Dictionary<int, string>();
            Dictionary<int, String> attributes = new Dictionary<int, string>();
            String feCode = getCodeFromFEList();
            String tgCode = getCodeFromTGList();
            

            int numberToMake = Convert.ToInt32(tbSpeedTestCount.Text);

            int x = 0;
            int y = 0;
            int height = this.Height;
            int width = this.Width;

            

            List<ImageInfo> BMPs = new List<ImageInfo>();

            SinglePointRenderer spr = SinglePointRenderer.getInstance();

            Stopwatch sw = Stopwatch.StartNew();
            
            attributes.Add(MilStdAttributes.PixelSize, this.cbSize.Text);

            if (cbDrawModifiers.Checked == true)
            {
                populateModifiersForUnits(modifiersU);
                populateModifiersForTGs(modifiersTG);
            }
            for (int i = 0; i < numberToMake; i++)
            {
                
                if (cbSpeedTestType.SelectedItem.ToString()==("Mixed"))
                {
                    if ((i % 2) == 0)
                        BMPs.Add(spr.RenderSymbol(feCode, modifiersU, attributes));
                    else
                        BMPs.Add(spr.RenderSymbol(tgCode, modifiersTG, attributes));
                }
                else if (cbSpeedTestType.SelectedItem.ToString() == ("Units"))
                {
                    BMPs.Add(spr.RenderSymbol(feCode, modifiersU, attributes));
                }
                else//"Tactical"
                {
                    BMPs.Add(spr.RenderSymbol(tgCode, modifiersTG, attributes));
                }
            }

            String generateTime = "Seconds to Generate: " + Convert.ToString(sw.ElapsedMilliseconds / 1000.0);
            Console.WriteLine(generateTime);

            Graphics g = Graphics.FromHwnd(this.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            if (cbDoubleBuffer.CheckState != CheckState.Checked)
            {
                
                sw.Restart();
                for (int j = 0; j < numberToMake; j++)
                {
                    
                    g.DrawImageUnscaled(BMPs[j].getBitmap(), x, y);
                    x += 50;
                    if (x > width)
                    {
                        x = 0;
                        y += 50;
                    }
                    if (y > height)
                    {
                        y = 0;
                    }
                }
                sw.Stop();
            }
            else
            {

                Bitmap buffer = new Bitmap(width, height);
                Graphics bg = Graphics.FromImage(buffer);
                bg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                sw.Restart();
                for (int j = 0; j < numberToMake; j++)
                {

                   bg.DrawImageUnscaled(BMPs[j].getBitmap(), x, y);
                    x += 50;
                    if (x > width)
                    {
                        x = 0;
                        y += 50;
                    }
                    if (y > height)
                    {
                        y = 0;
                    }
                }
                g.DrawImageUnscaled(buffer, 0, 0);
                sw.Stop();
                bg.Dispose();
                bg = null;
            }

            String renderTime = "Seconds to Render: " + Convert.ToString(sw.ElapsedMilliseconds / 1000.0);
            Console.WriteLine(renderTime);
            

            g.Dispose();
            g = null;

            textBox1.Text = generateTime;


        }

        private void btnDrawFE_Click(object sender, EventArgs e)
        {
            symbolID = getCodeFromFEList();
        }

        private void btnDrawTG_Click(object sender, EventArgs e)
        {
            symbolID = getCodeFromTGList();
            SymbolDef sd = SymbolDefTable.getInstance().getSymbolDef(SymbolUtilities.getBasicSymbolID(symbolID),1);
            if (sd != null && sd.getMinPoints() == 1 && sd.getMaxPoints() == 1 && sd.HasWidth() == false)
            {
                //code is good
            }
            else //symbol is a multipoint and renderer isn't ready for that yet.
            {
                //symbolID = null;
            }

        }

        private void cbFontSize_SelectedValueChanged(object sender, EventArgs e)
        {
            //TODO: est font size
            SinglePointRenderer spr = SinglePointRenderer.getInstance();
            
            //spr.
        }

        private void populateModifiersForUnits(Dictionary<int,String> modifiers)
        {
            modifiers.Add(ModifiersUnits.C_QUANTITY, "10");
            modifiers.Add(ModifiersUnits.H_ADDITIONAL_INFO_1, "Hj");
            modifiers.Add(ModifiersUnits.H1_ADDITIONAL_INFO_2, "H1");
            modifiers.Add(ModifiersUnits.H2_ADDITIONAL_INFO_3, "H2");
            modifiers.Add(ModifiersUnits.X_ALTITUDE_DEPTH, "X");//X
            modifiers.Add(ModifiersUnits.K_COMBAT_EFFECTIVENESS, "K");//K
            modifiers.Add(ModifiersUnits.Q_DIRECTION_OF_MOVEMENT,"45");//Q

            modifiers.Add(ModifiersUnits.W_DTG_1, "W1");
            modifiers.Add(ModifiersUnits.W1_DTG_2, "W2");
            modifiers.Add(ModifiersUnits.J_EVALUATION_RATING, "J");
            modifiers.Add(ModifiersUnits.M_HIGHER_FORMATION, "Mj");
            modifiers.Add(ModifiersUnits.N_HOSTILE, "ENY");
            modifiers.Add(ModifiersUnits.P_IFF_SIF, "P");
            modifiers.Add(ModifiersUnits.Y_LOCATION, "Yj");


            modifiers.Add(ModifiersUnits.F_REINFORCED_REDUCED, "RD");

            modifiers.Add(ModifiersUnits.L_SIGNATURE_EQUIP, "!");

            modifiers.Add(ModifiersUnits.G_STAFF_COMMENTS, "Gj");

            modifiers.Add(ModifiersUnits.V_EQUIP_TYPE, "Vj");
            modifiers.Add(ModifiersUnits.T_UNIQUE_DESIGNATION_1, "Tj");
            modifiers.Add(ModifiersUnits.T1_UNIQUE_DESIGNATION_2, "T1");
            modifiers.Add(ModifiersUnits.Z_SPEED, "999");//Z
        }

        private void populateModifiersForTGs(Dictionary<int,String> modifiers)
        {
            modifiers.Add(ModifiersTG.C_QUANTITY, "10");
            modifiers.Add(ModifiersTG.H_ADDITIONAL_INFO_1, "H");
            modifiers.Add(ModifiersTG.H1_ADDITIONAL_INFO_2, "H1");
            modifiers.Add(ModifiersTG.H2_ADDITIONAL_INFO_3, "H2");
            modifiers.Add(ModifiersTG.X_ALTITUDE_DEPTH, "X");//X
            modifiers.Add(ModifiersTG.Q_DIRECTION_OF_MOVEMENT, "45");//Q

            modifiers.Add(ModifiersTG.W_DTG_1, "W1");
            modifiers.Add(ModifiersTG.W1_DTG_2, "W2");
            modifiers.Add(ModifiersTG.N_HOSTILE, "ENY");
            modifiers.Add(ModifiersTG.Y_LOCATION, "Y");

            modifiers.Add(ModifiersTG.V_EQUIP_TYPE, "V");
            modifiers.Add(ModifiersTG.T_UNIQUE_DESIGNATION_1, "T");
            modifiers.Add(ModifiersTG.T1_UNIQUE_DESIGNATION_2, "T1");

        }
    }
}
