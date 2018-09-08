using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace MilSymNetUtilities
{
    public class SinglePointLookup
    {

        private Dictionary<String, SinglePointLookupInfo> _SPLookup = null; //SymbolDef


        private static SinglePointLookup _instance = null;
        private static object syncLock = new object();

        //private var myXML:XML = new XML();
        //private var XML_URL:String = "xml/SinglePoint.xml";
        //private var myXMLURL:URLRequest;
        //private var myLoader:URLLoader; 
      //  private Dictionary<String, SinglePointLookupInfo> _dictionary = new Dictionary<String, SinglePointLookupInfo>();

        public static SinglePointLookup getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new SinglePointLookup();
                    // _instance.init();
                }
            }

            return _instance;
        }

        private SinglePointLookup()
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
            _SPLookup = new Dictionary<String, SinglePointLookupInfo>(); //SymbolDef
            //XDocument lookupXml = XDocument.Parse(Properties.Resources.SinglePointC.Replace("&", "&amp;"));
            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.SinglePointC.xml");
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
          
            
            if (xml.Element("SINGLEPOINTMAPPINGS").HasElements) //"SINGLEPOINTMAPPINGS"
            {
                elements = xml.Elements();

                IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
                //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
                itr = elements.GetEnumerator();


                XElement child = null;
                itr.MoveNext();
                child = itr.Current;//SINGLEPOINTMAPPINGS
                if (child.HasElements)
                {
                    elements = child.Elements();
                    itr = elements.GetEnumerator();//SYMBOLS
                    while (itr.MoveNext())
                    {
                        child = itr.Current;                                                                        /*SymbolDef .XNodeToSymbolDef*/
                        SinglePointLookupInfo /*SymbolDef*/ temp = SinglePointLookupInfo.XNodeToSinglePointFontLookupInfo(child);// SPSymbolDef temp = SPSymbolDef.XNodeToSPSymbolDef(child);
                        if ((temp != null) && _SPLookup.ContainsKey(temp.getBasicSymbolID()) == false)//temp will be null if node is an XCOMMENT
                            _SPLookup[temp.getBasicSymbolID()] = temp;

                    }//end while
                }
            }//end if
        }


        /**
		 * @name getCharCodeFromSymbol
		 * 
		 * @desc  
		 * 
		 * @param symbolCode - IN - 
		 * @return 
		 */		
        public int getCharCodeFromSymbol(String symbolCode)
        {
            String basic = SymbolUtilities.getBasicSymbolID(symbolCode);
            SinglePointLookupInfo spli = _SPLookup[basic]; // _dictionary
			
			if(spli != null)
			{
				if(SymbolUtilities.isWeather(symbolCode)==false)
					return spli.getMappingP();
				else
				{
					if(symbolCode.Substring(3, 1)=="A")
						return spli.getMappingA();
					else
						return spli.getMappingP();
				}
			}
			else
				return -1;
        }

        public int getCharCodeFromFillID(String symbolCode)
		{
            SinglePointLookupInfo spli = _SPLookup[symbolCode]; //_dictionary
			if(spli != null)
				return spli.getMappingP();
			else
				return -1;
		}
       
        
        /**
		 * @name getSPLookupInfo
		 * 
		 * @desc Method that retrieves a reference to a SinglePointLookupInfo object from the SinglePointLookup Dictionary.	
		 * 
		 * @param strSymbolID - IN - The 15 character symbol Id.
		 * @return SinglePointLookupInfo, or null if there was an error.
		 */							
		public SinglePointLookupInfo getSPLookupInfo(String strSymbolID)
		{
		    String basic = null;
			SinglePointLookupInfo spli = null;
			try
			{
				basic = SymbolUtilities.getBasicSymbolID(strSymbolID);
                spli = _SPLookup[basic]; 
				return spli;
			}
			catch(Exception err) 
			{
				Debug.WriteLine(err);
			}
			return null;
		}	
    }
}


















/**
 * @name populateLookup
 *
 * @desc
 *
 * @param xml - IN -
 * @return None
 */


//private void populateLookup(XDocument xml)
//{
//  IEnumerable<XElement> elements = null;

//  if (xml.Element("SINGLEPOINTMAPPINGS").HasElements)
//  {
//      elements = xml.Elements();

//      IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
//      //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
//      itr = elements.GetEnumerator();


//      XElement child = null;
//      itr.MoveNext();
//      child = itr.Current;//SINGLEPOINTMAPPINGS
//      if (child.HasElements)
//      {
//          elements = child.Elements();
//          itr = elements.GetEnumerator();//SYMBOLS
//          while (itr.MoveNext())
//          {
//              child = itr.Current;
//              SPSymbolDef temp = SPSymbolDef.XNodeToSPSymbolDef(child);
//              if ((temp != null) && _SPLookup.ContainsKey(temp.getBasicSymbolId()) == false)//temp will be null if node is an XCOMMENT
//                  _SPLookup[temp.getBasicSymbolId()] = temp;

//          }//end while
//      }
//  }//end if
//}

///**
// * given the milstd symbol code, find the font index for the symbol.
// * @param symbolCode
// * @return
// */
//public int getCharCodeFromSymbol(String symbolCode)
//{

//    try
//    {
//        int end = 10;
//        String dash = "";

//        String strSymbolLookup = symbolCode;

//        if(symbolCode.Substring(0, 1)==("G"))
//        {
//            if(!_SPLookup.ContainsKey(strSymbolLookup))
//                strSymbolLookup = strSymbolLookup.Substring(0, 1) + "*" + strSymbolLookup.Substring(2, 13);

//            if(!_SPLookup.ContainsKey(strSymbolLookup))
//                strSymbolLookup = strSymbolLookup.Substring(0, 10) + "****X";

//            if(!_SPLookup.ContainsKey(strSymbolLookup))
//                strSymbolLookup = strSymbolLookup.Substring(0, 3) + "P" + strSymbolLookup.Substring(4, 11);

//            if(!_SPLookup.ContainsKey(strSymbolLookup))
//                strSymbolLookup = strSymbolLookup.Substring(0, 10) + "-----";


//            if(!_SPLookup.ContainsKey(strSymbolLookup))
//                strSymbolLookup = symbolCode.Substring(0, 1) + "*" + symbolCode.Substring(2, 1) + "-" +
//                          symbolCode.Substring(4, 11);

//            if(!_SPLookup.ContainsKey(strSymbolLookup))
//                strSymbolLookup = strSymbolLookup.Substring(0, 10) + "-----";

//            if(!_SPLookup.ContainsKey(strSymbolLookup))
//                strSymbolLookup = strSymbolLookup.Substring(0, 10) + "****X";
//        }
//        else if(!_SPLookup.ContainsKey(strSymbolLookup))
//        {
//            strSymbolLookup = symbolCode.Substring(0, 10);
//        }

//          int intMapping = -1;
//          SPSymbolDef data = null;
//          String mapping = null;
//          if(_SPLookup.ContainsKey(strSymbolLookup))
//          {
//              data = _SPLookup[strSymbolLookup];
//              intMapping = data.getMapping();
//          }

//          return intMapping;
//    }
//    catch (Exception exc)
//    {
//        Console.WriteLine(exc.Message);
//        Console.WriteLine(exc.StackTrace);
//    }
//  return -1;

//}
//  #region old lookup
//  ///**
//// * Method that retrieves a reference to a SPSymbolDef object from the
//// * SinglePointLookup Dictionary.
//// *
//// * @param strSymbolID - IN - The 15 character symbol Id.
//// * @return SPSymbolDef, or null if there was an error.
//// */
////public SPSymbolDef getSPSymbolDef(String strSymbolID)
////{
////    String strGenericSymbolID = "";
////    if (strSymbolID.Substring(0, 1) == ("G"))
////    {
////        strGenericSymbolID = strSymbolID.Substring(0, 1) + "*" + strSymbolID.Substring(2, 12);
////    }
////    else
////    {
////        strGenericSymbolID = strSymbolID.Substring(0, 10);
////    }

////    SPSymbolDef data = null;
////    if (_SPLookup.ContainsKey(strSymbolID))
////        data = _SPLookup[strGenericSymbolID];
////    return data;
//  //}
//  #endregion

//  /**
//   * * * Method that retrieves a reference to a SinglePointLookupInfo object from the
//  * SinglePointLookup Dictionary.
//  * @param basicSymbolID
//  * @return 
//  */
////public SinglePointLookupInfo getSPLookupInfo(String basicSymbolID, int symStd)
////{
////    SinglePointLookupInfo spli = null;
////    if (symStd == MilStd.Symbology_2525Bch2_USAS_13_14)
////        spli = hashMapB.get(basicSymbolID);
////    else if (symStd == MilStd.Symbology_2525C)
////        spli = hashMapC.get(basicSymbolID);
////    return spli;
////}