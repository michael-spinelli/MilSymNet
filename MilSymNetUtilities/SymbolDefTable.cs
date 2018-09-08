using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace MilSymNetUtilities
{
    public class SymbolDefTable
    {
        private static Dictionary<String, SymbolDef> _SymbolDefinitions = null;
        /*
        private static String propSymbolID = "SYMBOLID";
        private static String propGeometry = "GEOMETRY";
        private static String propDrawCategory = "DRAWCATEGORY";
        private static String propMaxPoint = "MAXPOINTS";
        private static String propMinPoints = "MINPOINTS";
        private static String propHasWidth = "HASWIDTH";
        private static String propModifiers = "MODIFIERS";
        private static String propDescription = "DESCRIPTION";
        private static String propHierarchy = "HIERARCHY";*/
        private static SymbolDefTable _instance = null;
        private static object syncLock = new object();

        public static SymbolDefTable getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new SymbolDefTable();
                }
            }

            return _instance;
        }

        private SymbolDefTable()
        {
            Init();
        }

        private void Init()
        {
            _SymbolDefinitions = new Dictionary<String, SymbolDef>();
            //XDocument lookupXml = XDocument.Parse(Properties.Resources.SymbolConstantsC.Replace("&", "&amp;"));
            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.SymbolConstantsC.xml");
            StreamReader sr = new StreamReader(s);
            string fileContentPortable = sr.ReadToEnd();
            //MessageDialog msgPortable = new MessageDialog("From PortableClassLibrary: " + fileContentPortable);
            //await msgPortable.ShowAsync();

            XDocument lookupXml = XDocument.Parse(fileContentPortable.Replace("&", "&amp;"));
            populateLookup(lookupXml);
        }

        private void populateLookup(XDocument xml)
        {
            IEnumerable<XElement> elements = null;

            if (xml.Element("SYMBOLCONSTANTS").HasElements)
            {
                elements = xml.Elements();

                IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
                //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
                itr = elements.GetEnumerator();


                XElement child = null;
                itr.MoveNext();
                child = itr.Current;//SYMBOLCONSTANTS
                if (child.HasElements)
                {
                    elements = child.Elements();
                    itr = elements.GetEnumerator();//SYMBOLS
                    while (itr.MoveNext())
                    {
                        child = itr.Current;
                        SymbolDef temp = SymbolDef.XNodeToSymbolDef(child);
                        if ((temp != null) && _SymbolDefinitions.ContainsKey(temp.getBasicSymbolId()) == false)//temp will be null if node is an XCOMMENT
                            _SymbolDefinitions[temp.getBasicSymbolId()] = temp;

                    }//end while
                }
            }//end if
        }//end populate lookup
        
        /**
        * @name getSymbolDef
        *
        * @desc Returns a SymbolDef from the SymbolDefTable that matches the passed in Symbol Id
        *
        * @param strBasicSymbolID - IN - A 15 character MilStd code
        * @return SymbolDef whose Symbol Id matches what is passed in
        */
        public SymbolDef getSymbolDef(String basicSymbolID, int symStd)
        {
            SymbolDef returnVal = null;
            try
            {
                if (_SymbolDefinitions.ContainsKey(basicSymbolID))
                    returnVal = _SymbolDefinitions[basicSymbolID];
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
        public Dictionary<String, SymbolDef> getAllSymbolDefs()
        {
            return _SymbolDefinitions;
        }

        /**
         * 
         * @param basicSymbolID
         * @return
         */
        public Boolean HasSymbolDef(String basicSymbolID)
        {
            if (basicSymbolID != null && basicSymbolID.Length == 15)
                return _SymbolDefinitions.ContainsKey(basicSymbolID);
            else
                return false;
        }



    }
}
