using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MilSymNetUtilities
{
    public class SymbolDimensions
    {
        /**
     * Gets bounds for symbol at a given character index and font size.
     * specify font size using "SymbolDimensions.SymbolFontSize".
     * Curently, only SymbolFontSizeX supported.
     * */
        public static RectangleF getUnitBounds(int charIndex = 800, float fontSize = 50)
		{
            
			int index = charIndex;
			RectangleF rect = Rectangle.Empty;
                
			
			switch(index)
			{
				case 800:
				case 801:
				case 802:
					//rect = new RectangleF(1f,1f,60.78125f,60.78125f);//unknown
                    rect = new RectangleF(-30.390625f,-30.390625f,60.78125f,60.78125f);//unknown
                    rect = new RectangleF(-31f,-30f,62f,60f);//unknown
					break;
				case 803:
				case 804:
				case 805:
					//rect = new RectangleF(0f,-14f,64.6875f,46.4375f);//friendly
                    //rect = new RectangleF(-32.34375f,-23.21875f,64.6875f,46.4375f);//friendly
                    rect = new RectangleF(-32.0f,-37.0f,64.0f,46.0f);//friendly
					break;
				case 806:
				case 807:
				case 808:
					//rect = new RectangleF(0f,2f,62.5f,62.5f);//hostile
                    //rect = new RectangleF(-31.25f,-31.25f,62.5f,62.5f);//hostile
                    rect = new RectangleF(-31.25f,-29.0f,62.5f,62.0f);//hostile
					break;
				case 809:
				case 810:
				case 811:
					//rect = new RectangleF(0f,-10f,50.0625f,50.0625f);//neutral
                    //rect = new RectangleF(-25.03125f,-25.03125f,50.0625f,50.0625f);//neutral
                    rect = new RectangleF(-25.03125f,-35.0f,50.0625f,50.0625f);//neutral
					break;
				case 812:
				case 813:
				case 814:
					//rect = new RectangleF(0f,-7f,53.703125f,53.71875f);//friendly equipment
                    //rect = new RectangleF(-26.78125f,-26.859375f,53.703125f,53.71875f);//friendly equipment
                    rect = new RectangleF(-26.78125f,-32.859375f,53.703125f,53.71875f);//friendly equipment
					break;
				case 816:
				case 817:
				case 818:
				case 840:
				case 841:
				case 842:
					//rect = new RectangleF(0f,-7f,50.28125f,52.734375f);//air & space hostile
                    //rect = new RectangleF(-25.140625f,-34.421875f,50.28125f,52.734375f);//air & space hostile
                    rect = new RectangleF(-25.140625f,-26.421875f,50.28125f,52.0f);//air & space hostile
					break;
				case 819:
				case 820:
				case 821:
				case 843:
				case 844:
				case 845:
					//rect = new RectangleF(0f,-7f,46.59375f,47.859375f);//air space friendly
                    //rect = new RectangleF(-23.296875f,-29.546875f,46.9f,47.859375f);//air space friendly
                    rect = new RectangleF(-24.0f,-30.546875f,48f,47.859375f);//air space friendly
					break;
				case 822:
				case 823:
				case 824:
				case 846:
				case 847:
				case 848:
					//rect = new RectangleF(0f,-7f,46.5f,47.859375f);//air space neutral
                    //rect = new RectangleF(-23.25f,-29.546875f,46.5f,47.859375f);//air space neutral
                    rect = new RectangleF(-23.25f,-30.546875f,46.5f,47.859375f);//air space neutral
					break;
				case 825:
				case 826:
				case 827:
				case 849:
				case 850:
				case 851:
					//rect = new RectangleF(0f,2f,64.6875f,56.5f);//air space unknown
                    //rect = new RectangleF(-32.34375f,-34.1875f,64.6875f,56.5f);//air space unknown
                    rect = new RectangleF(-33.0f,-26.1875f,66f,56f);//air space unknown
					break;
				case 828:
				case 829:
				case 830:
					//rect = new RectangleF(0f,-15f,50.28125f,52.734375f);//subsurface hostile
                    //rect = new RectangleF(-25.140625f,-18.3125f,50.28125f,52.734375f);//subsurface hostile
                    rect = new RectangleF(-25.140625f,-42f,50.28125f,52f);//subsurface hostile
					break;
				case 831:
				case 832:
				case 833:
					//rect = new RectangleF(0f,-18f,46.578125f,47.859375f);//subsurface friendly
                    //rect = new RectangleF(-23.3125f,-18.3125f,46.578125f,47.859375f);//subsurface friendly
                    rect = new RectangleF(-24.3125f,-42f,48f,49f);//subsurface friendly
					break;
				case 834:
				case 835:
				case 836:
					//rect = new RectangleF(0f,-18f,46.5f,47.875f);//subsurface neutral
                    //rect = new RectangleF(-23.25f,-18.3125f,46.5f,47.875f);//subsurface neutral
                    rect = new RectangleF(-23.25f,-42f,46.5f,47.875f);//subsurface neutral
					break;
				case 837:
				case 838:
				case 839:
					//rect = new RectangleF(0f,-10f,64.703125f,56.5f);//subsurface unknown
                    //rect = new RectangleF(-32.28125f,-22.3125f,64.703125f,56.5f);//subsurface unknown
                    rect = new RectangleF(-32.28125f,-38f,64.703125f,56f);//subsurface unknown
					break;
				default:
					rect = new RectangleF(0,0,54,54);
					break;
			}
			
			float ratio = 1;
			if(fontSize != 50)
			{
				ratio = fontSize / 50;
				//I only measured for a font size of 50.  if we get the ratio and multiply the values
				//by it, we in theory should have a correct adjusted rectangle.
				rect = new RectangleF(0,(rect.Y*ratio), (rect.Width*ratio), (rect.Height*ratio));
			}
			
			return rect;
		}

        public static Rectangle getSymbolBounds(String symbolID, int fontSize)
		{
			//var spsd:SPSymbolDef = SinglePointLookup.instance.getSPSymbolDef(symbolID);
			SinglePointLookupInfo spli = SinglePointLookup.getInstance().getSPLookupInfo(symbolID);
			
			Rectangle rect = new Rectangle(0,0,spli.getWidth(), spli.getHeight());
			
			if(fontSize != 60)//adjust boundaries ratio if font size is not at the default setting.
			{
				double ratio = fontSize/60.0;
				
				rect = new Rectangle(0,0,(int)((rect.Width*ratio)+0.5), (int)((rect.Height*ratio)+0.5));
			}
			
			return rect; 
		}
		

		/**
		 * 
		 * */
		public static PointF getSymbolCenter(String symbolID, RectangleF bounds)
		{
			String basicID = SymbolUtilities.getBasicSymbolID(symbolID);
            PointF center = new PointF();
			
			if(basicID == "G*G*GPUUB-****X" ||
				basicID == "G*G*GPUUL-****X" ||
				basicID == "G*G*GPUUS-****X" ||
				basicID == "G*G*GPRI--****X" ||
				basicID == "G*G*GPWE--****X" ||
				basicID == "G*G*GPWG--****X" ||
				basicID == "G*G*GPWM--****X" ||
				basicID == "G*G*GPP---****X" ||
				basicID == "G*G*GPPC--****X" ||
				basicID == "G*G*GPPL--****X" ||
				basicID == "G*G*GPPP--****X" ||
				basicID == "G*G*GPPR--****X" ||
				basicID == "G*G*GPPA--****X" ||
				basicID == "G*G*APD---****X" ||
				basicID == "G*G*OPP---****X" ||
				basicID.Substring(0,7) == "G*M*OAO" ||//antitank obstacles
				basicID == "G*M*BCP---****X" ||
				basicID == "G*F*PCS---****X" ||
				basicID == "G*F*PCB---****X" ||
				basicID == "G*F*PCR---****X" ||
				basicID == "G*F*PCH---****X" ||
				basicID == "G*F*PCL---****X" ||
                basicID.Substring(0, 5) == "G*S*P" ||//combat service suppport/points
				basicID == "G*O*ED----****X" ||
				basicID == "G*O*EP----****X" ||
				basicID == "G*O*EV----****X" ||
				basicID == "G*O*SB----****X" ||
				basicID == "G*O*SBM---****X" ||
				basicID == "G*O*SBN---****X" ||
				basicID == "G*O*SS----****X" ||
				basicID == "G*G*GPPN--****X" || //entry control point
				basicID == "G*S*PX----****X" || //ambulance exchange point
				basicID == "G*O*ES----****X" || //emergency distress call
				SymbolUtilities.isNBC(basicID) ||
				SymbolUtilities.isDeconPoint(basicID) ||
				SymbolUtilities.isCheckPoint(basicID))
			{
				//center on bottom middle
				center.X = bounds.Width/2;
				center.Y = bounds.Height;
			}
			else if(SymbolUtilities.isSonobuoy(basicID))
			{
				//bottom third
				center.X = bounds.Width/2;
                center.Y = (int)((bounds.Height * 0.66));
			}
            else if ((basicID.Substring(0, 7) == "G*G*GPO" && basicID.Substring(7, 1) != "-"))//antitank mine w/ handling device
			{
				//upper third
				center.X = bounds.Width/2;
				center.Y = (int)((bounds.Height * 0.33));
			}
			else if(basicID=="G*M*OMD---****X")
			{
				//upper third
				center.X = bounds.Width/2;
				center.Y = (int)((bounds.Height * 0.28));
			}
            else if (basicID.Substring(0, 7) == "G*G*DPO")//OBSERVATION POST/OUTPOST
			{
                if (basicID.Substring(7, 1) == "C")//combat outpost
				{
					center.X = bounds.Width/2;
					center.Y = (int)((bounds.Height * 0.55));
				}
				else//everything else under OBSERVATION POST/OUTPOST
				{
					center.X = bounds.Width/2;
					center.Y = (int)((bounds.Height * 0.65));
				}
			}
			else if(basicID == "G*G*GPWD--****X"||//drop point
				basicID == "G*G*PN----****X" ||//dummy minefield static
				basicID == "G*M*OB----****X" ||//booby trap
				basicID == "G*M*OME---****X" ||//antitank mine directional
				basicID == "G*M*OMW---****X" ||//wide area mines
				basicID == "G*M*OMP---****X" ||//anti-personnel mines
				basicID == "G*M*OHTL--****X" ||//Aviation/tower/low
				basicID == "G*M*OHTH--****X" ||//Aviation/tower/high
				basicID == "G*O*HM----****X" ||//
				basicID == "G*O*HI----****X" ||//
				basicID == "G*O*SM----****X")
			{
				if(basicID == "G*G*GPWD--****X")//drop point
				{
					center.X = bounds.Width/2;
					center.Y = (int)((bounds.Height * 0.85));
				}
				if(basicID == "G*G*PN----****X")//dummy minefield static
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.69));
				}
				if(basicID == "G*M*OB----****X")//booby trap
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.8));
				}
				if(basicID == "G*M*OME---****X")//antitank mine directional
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.77));
				}
				if(basicID == "G*M*OMW---****X")//wide area mines
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.34));
				}
				if(basicID == "G*M*OMP---****X")//anti personnel mines
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.59));
				}
				if(basicID == "G*M*OHTL--****X")//Aviation/tower/low//2525C
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.95));
				}
				if(basicID == "G*M*OHTH--****X")//Aviation/tower/high//2525C
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.95));
				}
				if(basicID == "G*O*HM----****X")//sea mine-like
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.7));
				}
				if(basicID == "G*O*HI----****X")
				{
					center.X = bounds.Width/2;
                    center.Y = (int)((bounds.Height * 0.58));
				}
				if(basicID == "G*O*SM----****X")
				{
					center.X = 0;
                    center.Y = (int)((bounds.Height * 0.5));
				}
				
				
			}
			else
			{
				//center on center
				center.X = bounds.Width/2f;
				center.Y = bounds.Height/2f;
			}
			
			return center;
		}
    }

    	
}
