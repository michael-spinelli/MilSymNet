using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MilSymNetUtilities;
using System.Reflection;
using System.IO;

namespace MilSymNet
{
    public class UnitSVGTable
    {
        private static Dictionary<int, SVGPath> _SymbolDefinitions = null;
        private static UnitSVGTable _instance = null;
        private static object syncLock = new object();

        public static UnitSVGTable getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new UnitSVGTable();
                }
            }

            return _instance;
        }

        private UnitSVGTable()
        {
            Init();
        }

        private void Init()
        {
            _SymbolDefinitions = new Dictionary<int, SVGPath>();
            //XDocument lookupXml = XDocument.Parse(Properties.Resources.UnitFont.Replace("&#x",""));
            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.UnitFont.svg");
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

                if (xml.Root.HasElements)
                {
                    //elements = xml.Root.Descendants("{http://www.w3.org/2000/svg}glyph");
                    // if(elements == null)
                    //elements = xml.Root.Descendants("glyph");
                    elements = xml.Root.Elements();


                    IEnumerator<XElement> itr = null;

                    itr = elements.GetEnumerator();


                    XElement child = null;
                    int count = 0;
                    while (itr.MoveNext())
                    {
                        child = itr.Current;
                        SVGPath temp = XNodeToSVGPath(child);
                        if ((temp != null) && _SymbolDefinitions.ContainsKey(temp.getID()) == false)//temp will be null if node is an XCOMMENT
                        {
                            int tempID = Convert.ToInt32(temp.getID());
                            if(tempID < 63000)
                                tempID = tempID - 57000;
                            else
                                tempID = tempID - 54000;
                            //if(tempID < )
                            _SymbolDefinitions[tempID] = temp;
                        }
                        //Console.WriteLine("ID: " + temp.getID().ToString());
                        count++;
                    }//end while
                    Console.WriteLine("# of glyphs: " + Convert.ToString(count));
                }//end if
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SymbolDefTableSVG", "populateLookup", exc);
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
                Console.WriteLine(child.Name);
                /*
                elements = child.Elements();
                itr = elements.GetEnumerator();//SYMBOLS
                while (itr.MoveNext())
                {
                    child = itr.Current;
                    SymbolDef temp = SymbolDef.XNodeToSymbolDef(child);
                    if ((temp != null) && _SymbolDefinitions.ContainsKey(temp.getBasicSymbolId()) == false)//temp will be null if node is an XCOMMENT
                        _SymbolDefinitions[temp.getBasicSymbolId()] = temp;

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
                        if (id != null && path != null)
                            sp = new SVGPath(id, path);
                    }
                }
                else
                    return null;
            }
            catch (System.Xml.XmlException xe)
            {
                Console.WriteLine(xe.Message);
                Console.WriteLine(xe.StackTrace);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
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
                if (_SymbolDefinitions.ContainsKey(index))
                    returnVal = new SVGPath(_SymbolDefinitions[index]);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
            return returnVal;
        }



        /**
        *
        * @return
        */
        public Dictionary<int, SVGPath> getAllSVGPaths()
        {
            return _SymbolDefinitions;
        }

        /**
         * 
         * @param index
         * @return
         */
        public Boolean HasSVGPath(int index)
        {
            if (index > -1)
                return _SymbolDefinitions.ContainsKey(index);
            else
                return false;
        }
    }
}
