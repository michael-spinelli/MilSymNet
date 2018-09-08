using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MilSymNetUtilities
{
    public class SPSymbolDef
    {
          public SPSymbolDef()
          {
            // Set fields to their default values.
            _strBasicSymbolId = "";
            _strDescription = "";
            _intUpperleftX = 0;
            _intUpperleftY = 0;
            _intLowerrightX = 0;
            _intLowerrightY = 0;
            _strModifiers = "";
            _intMapping = -1;
          }
          /**
           * The basic 15 character basic symbol Id.
           */
          protected String _strBasicSymbolId;

          public String getBasicSymbolId()
          {
            return _strBasicSymbolId;
          }

          public void setBasicSymbolId(String value)
          {
            _strBasicSymbolId = value;
          }
          /**
           * The description of this tactical graphic. Typically the name of the
           * tactical graphic in MIL-STD-2525B.
           */
          protected String _strDescription;

          public String getDescription()
          {
            return _strDescription;
          }

          public void setDescription(String value)
          {
            _strDescription = value;
          }
          /**
           * Defines the upperleftX fields.
           */
          protected int _intUpperleftX;

          public int getUpperleftX()
          {
            return _intUpperleftX;
          }

          public void setUpperleftX(int value)
          {
            _intUpperleftX = value;
          }
          /**
           * Defines the upperleftY fields.
           */
          protected int _intUpperleftY;

          public int getUpperleftY()
          {
            return _intUpperleftY;
          }

          public void setUpperleftY(int value)
          {
            _intUpperleftY = value;
          }
          /**
           * Defines the lowerrightY fields.
           */
          protected int _intLowerrightY;

          public int getLowerrightY()
          {
            return _intLowerrightY;
          }

          public void setLowerrightY(int value)
          {
            _intLowerrightY = value;
          }
          /**
           * Defines the lowerrightX fields.
           */
          protected int _intLowerrightX;

          public int getLowerrightX()
          {
            return _intLowerrightX;
          }

          public void setLowerrightX(int value)
          {
            _intLowerrightX = value;
          }

          protected int _intHeight = 0;
          public int getHeight()
          {
              return _intHeight;
          }

          protected int _intWidth = 0;
          public int getWidth()
          {
              return _intWidth;
          }

          /**
           * Defines the modifiers fields.
           */
          protected String _strModifiers;

          public String getModifiers()
          {
            return _strModifiers;
          }

          public void setModifiers(String value)
          {
            _strModifiers = value;
          }
          /**
           * Defines the mapping fields.
           */
          protected int _intMapping;

          public int getMapping()
          {
            return _intMapping;
          }

          public void setMapping(int value)
          {
              _intMapping = value;
          }


          public String toXML()
          {
            String symbolId = "<SYMBOLID>" +  getBasicSymbolId() + "</SYMBOLID>";
            String mapping = "<MAPPING>" + getMapping() + "</MAPPING>";
            String description = "<DESCRIPTION>" + getDescription() + "</DESCRIPTION>";
            String modifier = "<MODIFIER>" + getModifiers() + "</MODIFIER>";
            String ulx = "<ULX>" + getUpperleftX() + "</ULX>";
            String uly = "<ULY>" + getUpperleftY() + "</ULY>";
            String lrx = "<LRX>" + getLowerrightX() + "</LRX>";
            String lry = "<LRY>" + getLowerrightY() + "</LRY>";
            String xml = symbolId + mapping + description + modifier + ulx + uly + lrx + lry;
            return xml;
          }

          public static SPSymbolDef XNodeToSPSymbolDef(XNode node)
          {
              SPSymbolDef spsd = null;
              XElement element = null;
              XElement subElement = null;
              //XAttribute attribute = null;
              XNode nextNode = null;
              try
              {
                  if (node is XElement)
                  {
                      element = (XElement)node;

                      if (element.HasElements)
                      {
                          spsd = new SPSymbolDef();
                          subElement = null;
                          if (element.FirstNode is XElement)
                              subElement = (XElement)element.FirstNode;
                          while (subElement != null)
                          {

                              if (subElement.Name == "DESCRIPTION")
                              {
                                  spsd._strDescription = subElement.FirstNode.ToString();//get element value
                                  //spsd._strDescription = spsd._strDescription.Replace("&amp;", "&");
                              }
                              if (subElement.Name.LocalName == "SYMBOLID")
                              {
                                  spsd._strBasicSymbolId = subElement.FirstNode.ToString();
                                  //if (spsd.BasicSymbolCode == "S*A*MFQH--*****")
                                  //{
                                  //    Console.WriteLine("");
                                  //}
                              }
                              if (subElement.Name.LocalName == "MAPPING")
                                  spsd._intMapping = int.Parse(subElement.FirstNode.ToString());
                              //////////////////////////////////////////
                              //hopefully get rid of these 4 values soon and turn them into height & width
                              if (subElement.Name.LocalName == "ULX")
                              {
                                  spsd._intUpperleftX = int.Parse(subElement.FirstNode.ToString());
                              }
                              if (subElement.Name.LocalName == "ULY")
                              {
                                  spsd._intUpperleftY = int.Parse(subElement.FirstNode.ToString());
                              }
                              if (subElement.Name.LocalName == "LRX")
                              {
                                  spsd._intLowerrightX = int.Parse(subElement.FirstNode.ToString());
                              }
                              if (subElement.Name.LocalName == "LRY")
                              {
                                  spsd._intLowerrightY = int.Parse(subElement.FirstNode.ToString());
                              }
                              spsd._intWidth = spsd._intLowerrightX - spsd._intUpperleftX;
                              spsd._intHeight = spsd._intLowerrightY - spsd._intUpperleftY;
                              //////////////////////////////
                              if (subElement.Name.LocalName == "WIDTH")
                              {
                                  spsd._intWidth = int.Parse(subElement.FirstNode.ToString());
                              }
                              if (subElement.Name.LocalName == "HEIGHT")
                              {
                                  spsd._intHeight = int.Parse(subElement.FirstNode.ToString());
                              }

                              nextNode = subElement.NextNode;
                              subElement = null;
                              while (subElement == null && nextNode != null)
                              {
                                  if (nextNode is XElement)
                                      subElement = (XElement)nextNode;
                                  else
                                      nextNode = nextNode.NextNode;
                              }
                          }
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
              if (spsd.getBasicSymbolId() != null && spsd.getBasicSymbolId() != "")
                  return spsd;
              else
                  return null;
          }
    }
}
