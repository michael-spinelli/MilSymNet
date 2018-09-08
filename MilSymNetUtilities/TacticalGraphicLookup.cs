using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace MilSymNetUtilities
{
    public class TacticalGraphicLookup
    {
        private Dictionary<String, int> _TGLookup = null; //SymbolDef


        private static TacticalGraphicLookup _instance = null;
        private static object syncLock = new object();

        public static TacticalGraphicLookup getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new TacticalGraphicLookup();
                    // _instance.init();
                }
            }

            return _instance;
        }

        private TacticalGraphicLookup()
        {
            init();
        }

        /**
        * @name init
        *
        * @desc Simply calls xmlLoaded
        *
        * @return None
        */
        private void init()
        {
            _TGLookup = new Dictionary<String, int>(); //SymbolDef
            //XDocument lookupXml = XDocument.Parse(Properties.Resources.TacticalGraphicsXML.Replace("&", "&amp;"));
            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.TacticalGraphics.xml");
            StreamReader sr = new StreamReader(s);
            string fileContentPortable = sr.ReadToEnd();
            //MessageDialog msgPortable = new MessageDialog("From PortableClassLibrary: " + fileContentPortable);
            //await msgPortable.ShowAsync();

            XDocument lookupXml = XDocument.Parse(fileContentPortable.Replace("&", "&amp;"));
            populateLookup(lookupXml);
        }

        /**
* @name populateLookup
*
* @desc
*
* @param xml - IN -
* @return None
*/
        private void populateLookup(XDocument xml)//XDocument xml
        {
            IEnumerable<XElement> elements = null;


            if (xml.Element("TACTICALGRAPHICS").HasElements) //"SINGLEPOINTMAPPINGS"
            {
                elements = xml.Elements();

                IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
                //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
                itr = elements.GetEnumerator();


                XElement child = null;
                itr.MoveNext();
                child = itr.Current;//TACTICALGRAPHICS
                if (child.HasElements)
                {
                    elements = child.Elements();
                    itr = elements.GetEnumerator();//SYMBOL
                    while (itr.MoveNext())
                    {
                        child = itr.Current;                                                                        /*SymbolDef .XNodeToSymbolDef*/
                        TacticalGraphicLookupInfo temp = TacticalGraphicLookupInfo.XNodeToTGLookupInfo(child);// SPSymbolDef temp = SPSymbolDef.XNodeToSPSymbolDef(child);
                        if ((temp != null) && _TGLookup.ContainsKey(temp.getBasicSymbolID()) == false)//temp will be null if node is an XCOMMENT
                            _TGLookup[temp.getBasicSymbolID()] = temp.getMapping();

                    }//end while
                }
            }//end if
        }

        /**
        * given the milstd symbol code, find the font index for the symbol.
        * @param symbolCode
        * @return
        */
        public int getCharCodeFromSymbol(String symbolCode)
        {

            try
            {

                String basicID = symbolCode;

                if (SymbolUtilities.isWeather(symbolCode) == false)
                {
                    basicID = SymbolUtilities.getBasicSymbolID(symbolCode);
                }
                if (_TGLookup.ContainsKey(basicID))
                {
                    return _TGLookup[basicID];
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("TacticalGraphicLookup", "getCharCodeFromSymbol", exc, ErrorLevel.WARNING);
            }
            return -1;

        } 
    }
}
