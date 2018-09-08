using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MilSymNetUtilities;
using System.Reflection;
using System.IO;

namespace MilSymUwp
{
    public class TGSVGTable
    {
        private static Dictionary<int, SVGPath> _TGDefinitions = null;
         private static TGSVGTable _instance = null;
        private static object syncLock = new object();

        public static TGSVGTable getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new TGSVGTable();
                }
            }

            return _instance;
        }

        private TGSVGTable()
        {
            Init();
        }

        private void Init()
        {
            _TGDefinitions = new Dictionary<int, SVGPath>();
            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.TacticalGraphics.svg");
            StreamReader sr = new StreamReader(s);
            string fileContentPortable = sr.ReadToEnd();
            //MessageDialog msgPortable = new MessageDialog("From PortableClassLibrary: " + fileContentPortable);
            //await msgPortable.ShowAsync();

            XDocument lookupXml = XDocument.Parse(fileContentPortable.Replace("&#x", "").Replace("&", "&amp;"));
            populateLookup(lookupXml);
        }

        private void populateLookup(XDocument xml)
        {
            IEnumerable<XElement> elements = null;
            try
            {
                //svg/def/font
                if (xml.Root.HasElements)
                {
                    //elements = xml.Root.Descendants("{http://www.w3.org/2000/svg}glyph");
                    // if(elements == null)
                    //elements = xml.Root.Descendants("glyph");
                    elements = xml.Root.Elements();
                    int ec = elements.Count();
                    IEnumerator<XElement> itr = null;

                    itr = elements.GetEnumerator();


                    XElement child = null;
                    int count = 0;
                    while (itr.MoveNext())
                    {
                        child = itr.Current;
                        SVGPath temp = XNodeToSVGPath(child);
                        if ((temp != null) && _TGDefinitions.ContainsKey(temp.getID()) == false)//temp will be null if node is an XCOMMENT
                        {
                            int tempID = Convert.ToInt32(temp.getID());
                            tempID = tempID - 57000;
                            _TGDefinitions[tempID] = temp;
                        }
                        count++;
                    }//end while
                    Debug.WriteLine("# of glyphs: " + Convert.ToString(count));
                }//end if
            }
            catch(Exception exc)
            {
                ErrorLogger.LogException("TGSVGTable", "populateLookup", exc);
            }
        }//end populate lookup

        private XElement getFirstChildElement(IEnumerable<XElement> elements)
        {
            XElement element = null;

            IEnumerator<XElement> itr = elements.GetEnumerator();

            itr.MoveNext();
            element = itr.Current;
            return element;
        }

        private void printElements(IEnumerator<XElement> itr)
        {
            //IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
            //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
            //itr = elements.GetEnumerator();
            IEnumerable<XElement> elements = null;

            XElement child = null;
            itr.MoveNext();
            child = itr.Current;//SYMBOLCONSTANTS
            if (child.HasElements)
            {
                Debug.WriteLine(child.Name);
                /*
                elements = child.Elements();
                itr = elements.GetEnumerator();//SYMBOLS
                while (itr.MoveNext())
                {
                    child = itr.Current;
                    SymbolDef temp = SymbolDef.XNodeToSymbolDef(child);
                    if ((temp != null) && _TGDefinitions.ContainsKey(temp.getBasicSymbolId()) == false)//temp will be null if node is an XCOMMENT
                        _TGDefinitions[temp.getBasicSymbolId()] = temp;

                }//end while*/
            }
        }

        private SVGPath XNodeToSVGPath(XNode node)
        {
            SVGPath sp = null;
            XElement element = null;
            XAttribute attribute = null;
            
            string id = null;
            string path = null;
            try
            {
                if (node is XElement)
                {
                    element = (XElement)node;

                    if (element.HasAttributes)
                    {
                        IEnumerator<XAttribute> itr = null;// = xml.DescendantNodes().GetEnumerator();
                        itr = element.Attributes().GetEnumerator();
                        
                        while (itr.MoveNext())
                        {
                            attribute = itr.Current;

                            if (attribute.Name == "unicode")
                            {
                                id = attribute.Value;//get element value
                            }
                            if (attribute.Name == "d")
                            {
                                path = attribute.Value;
                            }
                        }
                        if(id != null && path != null)
                            sp = new SVGPath(id, path);
                    }
                }
                else
                    return null;
            }
            catch (System.Xml.XmlException xe)
            {
                Debug.WriteLine(xe.Message);
                Debug.WriteLine(xe.StackTrace);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
            }

            return sp;
        }
        
        /**
        * @name getSymbolDef
        *
        * @desc Returns a SymbolDef from the SymbolDefTable that matches the passed in Symbol Id
        *
        * @param index String representation of the index number
        * @return SVGPath
        */
        public SVGPath getSVGPath(int index)
        {
            SVGPath returnVal = null;
            try
            {
                if (_TGDefinitions.ContainsKey(index))
                    returnVal = new SVGPath(_TGDefinitions[index]);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
            }
            return returnVal;
        }



        /**
        *
        * @return
        */
        public Dictionary<int, SVGPath> getAllSVGPaths()
        {
            return _TGDefinitions;
        }

        /**
         * 
         * @param index
         * @return
         */
        public Boolean HasSVGPath(int index)
        {
            if (index > -1)
                return _TGDefinitions.ContainsKey(index);
            else
                return false;
        }
    }
}
