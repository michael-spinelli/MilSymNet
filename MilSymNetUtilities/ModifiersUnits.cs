﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace MilSymNetUtilities
{
    public class ModifiersUnits
    {

        //public  String SYMBOL_ID = "Symbol ID";
        //public  String SOURCE = "Source";
        //public  String EDITOR_CLASS_TYPE = "Editor Class Type";
        //public  String URN = "URN";
        //public  String UIC = "UIC";
        //public  String ANGLE_OF_ROTATION = "Angle of Rotation";

        /**
         * The innermost part of a symbol that represents a warfighting object
         * Here for completeness, not actually used as this comes from the
         * symbol code.
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: G
         */
        public const int A_SYMBOL_ICON = 1;
        /**
         * A graphic modifier in a unit symbol that identifies command level
         * We feed off of the symbol code so this isn't used
         * Type: U,SO
         * Length: G
         */
        public  const int B_ECHELON = 2;
        /**
         * A text modifier in an equipment symbol that identifies the number of items present.
         * Type: E,EEI
         * Length: 9
         */
        public  const int C_QUANTITY = 3;
        /**
         * A graphic modifier that identifies a unit or SO symbol as a task force (see 5.3.4.6
         * and figures 2 and 3).
         * Type: U,SO
         * Length: G
         */
        public  const int D_TASK_FORCE_INDICATOR = 4;
        /**
         * A graphic modifier that displays standard identity, battle dimension, or exercise
         * amplifying descriptors of an object (see 5.3.1 and table II).
         * Type: U,E,I,SO,EU,EEI,EI
         * Length: G
         */
        public  const int E_FRAME_SHAPE_MODIFIER = 5;
        /**
         * A text modifier in a unit symbol that displays (+) for reinforced, (-) for reduced,(+) reinforced and reduced.
         * R = reinforced,D = reduced,RD = reinforced and reduced
         * Type: U,SO
         * Length: 23
         */
        public  const int F_REINFORCED_REDUCED = 6;
        /**
         * A text modifier for units, equipment and installations; content is implementation specific.
         * Type: U,E,I,SI,SO
         * Length: 20
         */
        public  const int G_STAFF_COMMENTS = 7;
        /**
         * Text modifier for amplifying free text
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 20
         */
        public  const int H_ADDITIONAL_INFO_1 = 8;
        /**
         * Text modifier for amplifying free text
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 20
         */
        public  const int H1_ADDITIONAL_INFO_2 = 9;
        /**
         * Text modifier for amplifying free text
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 20
         */
        public  const int H2_ADDITIONAL_INFO_3 = 10;
        /**
         * A text modifier for units, equipment, and installations that consists of 
         * a one letter reliability rating and a one-number credibility rating.
            Reliability Ratings: A-completely reliable, B-usually reliable, 
            C-fairly reliable, D-not usually reliable, E-unreliable, 
            F-reliability cannot be judged.
            Credibility Ratings: 1-confirmed by other sources,
            2-probably true, 3-possibly true, 4-doubtfully true,
            5-improbable, 6-truth cannot be judged.
            Type: U,E,I,SI,SO,EU,EEI,EI
            Length: 2
         */
        public  const int J_EVALUATION_RATING = 11;
        /**
         * A text modifier for units and installations that indicates unit effectiveness or
         * installation capability.
         * Type: U,I,SO
         * Length: 5,5,3
         */
        public  const int K_COMBAT_EFFECTIVENESS = 12;
        /**
         * A text modifier for hostile equipment; â€œ!â€� indicates detectable electronic
         * signatures.
         * Type: E,SI
         * Length: 1
         */
        public  const int L_SIGNATURE_EQUIP = 13;
        /**
         * A text modifier for units that indicates number or title of higher echelon
         * command (corps are designated by Roman numerals).
         * Type: U,SI
         * Length: 21
         */
        public  const int M_HIGHER_FORMATION = 14;
        /**
         * A text modifier for equipment; letters "ENY" denote hostile symbols.
         * Type: E
         * Length: 3
         */
        public  const int N_HOSTILE = 15;
        /**
         * A text modifier displaying IFF/SIF Identification modes and codes.
         * Type: U,E,SO
         * Length: 5
         */
        public  const int P_IFF_SIF = 16;
        /**
         * A graphic modifier for units and equipment that identifies the direction of
         * movement or intended movement of an object (see 5.3.4.1 and figures 2 and 3).
         * Type: U,E,SO,EU,EEI
         * Length: G
         */
        public  const int Q_DIRECTION_OF_MOVEMENT = 17;
        /**
         * A graphic modifier for equipment that depicts the mobility of an object (see
         *   5.3.4.3, figures 2 and 3, and table VI).
         * We feed off of the symbol code for mobility so this isn't used
         * Type: E,EEI
         * Length: G
         */
        public  const int R_MOBILITY_INDICATOR = 18;
        /**
         * M = Mobile, S = , or U = Uncertain.
         * Type: SI
         * Length: 1
         */
        public  const int R2_SIGNIT_MOBILITY_INDICATOR = 19;
        /**
         * Headquarters staff indicator: A graphic modifier for units, equipment, and
         * installations that identifies a unit as a headquarters (see 5.3.4.8 and figures 2 and
         * 3).
         * Offset location indicator: A graphic modifier for units, equipment, and
         * installations used when placing an object away from its actual location (see
         * 5.3.4.9 and figures 2 and 3).
         * Type: U,E,I,SO,EU,EEI,EI
         * Length: G
         */
        public  const int S_HQ_STAFF_OR_OFFSET_INDICATOR = 20;
        /**
         * A text modifier for units, equipment, and installations that uniquely identifies a
         * particular symbol or track number. Identifies acquisitions number when used
         * with SIGINT symbology.
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 21
         */
        public  const int T_UNIQUE_DESIGNATION_1 = 21;
        /**
         * A text modifier for units, equipment, and installations that uniquely identifies a
         * particular symbol or track number. Identifies acquisitions number when used
         * with SIGINT symbology.
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 21
         */
        public  const int T1_UNIQUE_DESIGNATION_2 = 22;
        /**
         * A text modifier for equipment that indicates types of equipment.
         * For Tactical Graphics:
         * A text modifier that indicates nuclear weapon type.
         * Type: E,SI,EEI
         * Length: 24
         */
        public  const int V_EQUIP_TYPE = 23;
        /**
         * A text modifier for units, equipment, and installations that displays DTG format:
         * DDHHMMSSZMONYYYY or â€œO/Oâ€� for on order (see 5.5.2.6).
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 16
         */
        public  const int W_DTG_1 = 24;
        /**
         * A text modifier for units, equipment, and installations that displays DTG format:
         * DDHHMMSSZMONYYYY or â€œO/Oâ€� for on order (see 5.5.2.6).
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 16
         */
        public  const int W1_DTG_2 = 25;
        /**
         * A text modifier for units, equipment, and installations, that displays either
         * altitude flight level, depth for submerged objects; or height of equipment or
         * structures on the ground. See 5.5.2.5 for content.
         * Type: U,E,I,SO,EU,EEI,EI
         * Length: 14
         */
        public  const int X_ALTITUDE_DEPTH = 26;
        /**
         * A text modifier for units, equipment, and installations that displays a symbolâ€™s
         * location in degrees, minutes, and seconds (or in UTM or other applicable display
         * format).
         * Conforms to decimal
         *  degrees format:
         *  xx.dddddhyyy.dddddh
         *  where
         *  xx = degrees latitude
         *  yyy = degrees longitude
         *  .ddddd = decimal degrees
         *  h = direction (N, E, S, W)
         * Type: U,E,I,SI,SO,EU,EEI,EI
         * Length: 19
         */
        public  const int Y_LOCATION = 27;
        /**
         * A text modifier for units and equipment that displays velocity as set forth in
         * MIL-STD-6040.
         * Type: U,E,SO,EU,EEI
         * Length: 8
         */
        public  const int Z_SPEED = 28;
        /**
         * A text modifier for units; indicator is contained inside the frame (see figures 2
         * and 3); contains the name of the special C2 Headquarters.
         * Type: U,SO
         * Length: 9
         */
        public  const int AA_SPECIAL_C2_HQ = 29;
        /**
         * Feint or dummy indicator: A graphic modifier for units, equipment, and
         * installations that identifies an offensive or defensive unit intended to draw the
         * enemyâ€™s attention away from the area of the main attack (see 5.3.4.7 and figures
         * 2 and 3).
         * Type: U,E,I,SO
         * Length: G
         */
        public  const int AB_FEINT_DUMMY_INDICATOR = 30;
        /**
         * Installation: A graphic modifier for units, equipment, and installations used to
         * show that a particular symbol denotes an installation (see 5.3.4.5 and figures 2
         * and 3).
         * Not used, we feed off of symbol code for this
         * Type: U,E,I,SO,EU,EEI,EI
         * Length: G
         */
        public  const int AC_INSTALLATION = 31;
        /**
         * ELNOT or CENOT
         * Type: SI
         * Length: 6
         */
        public  const int AD_PLATFORM_TYPE = 32;
        /**
         * Equipment teardown time in minutes.
         * Type: SI
         * Length: 3
         */
        public  const int AE_EQUIPMENT_TEARDOWN_TIME = 33;
        /**
         * Example: â€œHawkâ€� for Hawk SAM system.
         * Type: SI
         * Length: 12
         */
        public  const int AF_COMMON_IDENTIFIER = 34;
        /**
         * Towed sonar array indicator: A graphic modifier for equipment that indicates the
         * presence of a towed sonar array (see 5.3.4.4, figures 2 and 3, and table VII).
         * Type: E
         * Length: G
         */
        public  const int AG_AUX_EQUIP_INDICATOR = 35;
        /**
         * A graphic modifier for units and equipment that indicates the area where an
         * object is most likely to be, based on the objectâ€™s last report and the reporting
         * accuracy of the sensor that detected the object (see 5.3.4.11.1 and figure 4).
         * Type: E,U,SO,EU,EEI
         * Length: G
         */
        public  const int AH_AREA_OF_UNCERTAINTY = 36;
        /**
         * A graphic modifier for units and equipment that identifies where an object
         * should be located at present, given its last reported course and speed (see
         * 5.3.4.11.2 and figure 4).
         * Type: E,U,SO,EU,EEI
         * Length: G
         */
        public  const int AI_DEAD_RECKONING_TRAILER = 37;
        /**
         * A graphic modifier for units and equipment that depicts the speed and direction
         * of movement of an object (see 5.3.4.11.3 and figure 4).
         * Type: E,U,SO,EU,EEI
         * Length: G
         */
        public  const int AJ_SPEED_LEADER = 38;
        /**
         * A graphic modifier for units and equipment that connects two objects and is
         * updated dynamically as the positions of the objects change (see 5.3.4.11.4 and
         * figure 4).
         * Type: U,E,SO
         * Length: G
         */
        public  const int AK_PAIRING_LINE = 39;
        /**
         * An optional graphic modifier for equipment or installations that indicates
         * operational condition or capacity.
         * Type: E,I,SI,SO,EU,EEI,EI
         * Length: G
         */
        public  const int AL_OPERATIONAL_CONDITION = 40;

        /**
         * A graphic amplifier placed immediately atop the symbol. May denote, 1)
         * local/remote status; 2) engagement status; and 3) weapon type.
         *
         * Type: U,E,I
         * Length: G/8
         */
        public  const int AO_ENGAGEMENT_BAR = 41;

        /**
         * Pulled from the SymbolID.  Not something the user should set directly.
         */
        public  const int CC_COUNTRY_CODE = 42;

        /**
         * Sonar Classification Confidence level. valid values are 1-5.
         * Only applies to the 4 subsurface MILCO sea mines
         */
        public  const int SCC_SONAR_CLASSIFICATION_CONFIDENCE = 50;

        /**
         * A generic name label that goes to the right of the symbol and
         * any existing labels.  If there are no existing labels, it goes right
         * next to the right side of the symbol.  This is a CPOF label that applies
         * to all force elements.  This IS NOT a MilStd or USAS Label.  
         */
        public  const int CN_CPOF_NAME_LABEL = 60;



        //public  const int LENGTH = "Length";
        //public  const int WIDTH = "Width";
        //public  const int RADIUS = "Radius";
        //public  const int SEGMENT_DATA = "Segment Data";

        /**
         * Returns an Arraylist of the modifer names for units
         * @return
         */
        public  List<int> GetModifierList()
        {
            List<int> list = new List<int>();

            //list.Add(ModifierType.A_SYMBOL_ICON);//graphical, feeds off of symbol code, SIDC positions 3, 5-10
            //list.Add(ModifierType.B_ECHELON);//graphical, feeds off of symbol code, SIDC positions 11-12
            list.Add(C_QUANTITY);
            //list.Add(D_TASK_FORCE_INDICATOR);//graphical, feeds off of symbol code, SIDC positions 11-12
            //list.Add(E_FRAME_SHAPE_MODIFIER);//symbol frame, feeds off of symbol code, SIDC positions 3-4
            list.Add(F_REINFORCED_REDUCED);//R = reinforced, D = reduced, RD = reinforced and reduced
            list.Add(G_STAFF_COMMENTS);
            list.Add(H_ADDITIONAL_INFO_1);
            list.Add(H1_ADDITIONAL_INFO_2);
            list.Add(H2_ADDITIONAL_INFO_3);
            list.Add(J_EVALUATION_RATING);
            list.Add(K_COMBAT_EFFECTIVENESS);
            list.Add(L_SIGNATURE_EQUIP);
            list.Add(M_HIGHER_FORMATION);
            list.Add(N_HOSTILE);
            list.Add(P_IFF_SIF);
            list.Add(Q_DIRECTION_OF_MOVEMENT);//number in mils
            //list.Add(R_MOBILITY_INDICATOR);//graphical, feeds off of symbol code, SIDC positions 11-12
            list.Add(R2_SIGNIT_MOBILITY_INDICATOR);
            //list.Add(S_HQ_STAFF_OR_OFFSET_INDICATOR);//graphical, feeds off of symbol code, SIDC positions 11-12
            list.Add(T_UNIQUE_DESIGNATION_1);
            list.Add(T1_UNIQUE_DESIGNATION_2);
            list.Add(V_EQUIP_TYPE);
            list.Add(W_DTG_1);
            list.Add(W1_DTG_2);
            list.Add(X_ALTITUDE_DEPTH);
            list.Add(Y_LOCATION);
            list.Add(Z_SPEED);

            list.Add(AA_SPECIAL_C2_HQ);
            //list.Add(AB_FEINT_DUMMY_INDICATOR);//graphical, feeds off of symbol code, SIDC positions 11-12
            //list.Add(AC_INSTALLATION);//graphical, feeds off of symbol code, SIDC positions 11-12
            list.Add(AD_PLATFORM_TYPE);
            list.Add(AE_EQUIPMENT_TEARDOWN_TIME);
            list.Add(AF_COMMON_IDENTIFIER);
            list.Add(AG_AUX_EQUIP_INDICATOR);
            list.Add(AH_AREA_OF_UNCERTAINTY);
            list.Add(AI_DEAD_RECKONING_TRAILER);
            list.Add(AJ_SPEED_LEADER);
            list.Add(AK_PAIRING_LINE);
            //list.Add(AL_OPERATIONAL_CONDITION);//2525C ////graphical, feeds off of symbol code, SIDC positions 4
            list.Add(AO_ENGAGEMENT_BAR);//2525C



            return list;
        }

        /**
         *
         * @param modifier like ModifiersTG.C_QUANTITY
         * @return modifier name based on mofidier constants
         */
        public  String getModifierName(int modifier)
        {
            switch (modifier)
            {
                //case A_SYMBOL_ICON:
                //    return "Symbol Icon";
                case B_ECHELON:
                    return "Echelon";
                case C_QUANTITY:
                    return "Quantity";
                //case D_TASK_FORCE_INDICATOR:
                //    return "Task Force Indicator";
                //case E_FRAME_SHAPE_MODIFIER:
                //    return "Frame Shape Modifier";
                case F_REINFORCED_REDUCED:
                    return "Reinforce Reduced";
                case G_STAFF_COMMENTS:
                    return "Staff Comments";
                case H_ADDITIONAL_INFO_1:
                    return "Additional Info 1";
                case H1_ADDITIONAL_INFO_2:
                    return "Additional Info 2";
                case H2_ADDITIONAL_INFO_3:
                    return "Additional Info 3";
                case J_EVALUATION_RATING:
                    return "Evaluation Rating";
                case K_COMBAT_EFFECTIVENESS:
                    return "Combat Effectiveness";
                case L_SIGNATURE_EQUIP:
                    return "Signature Equipment";
                case M_HIGHER_FORMATION:
                    return "Higher Formation";
                case N_HOSTILE:
                    return "Hostile";
                case P_IFF_SIF:
                    return "IFF SIF";
                case Q_DIRECTION_OF_MOVEMENT:
                    return "Direction of Movement";
                case R_MOBILITY_INDICATOR:
                    return "Mobility Indicator";
                case R2_SIGNIT_MOBILITY_INDICATOR:
                    return "Signals Intelligence Mobility Indicator";
                //case S_HQ_STAFF_OR_OFFSET_INDICATOR:
                //    return "HQ Staff / Offset Indicator";
                //case S_OFFSET_INDICATOR:
                //    return "Offset Indicator";
                case T_UNIQUE_DESIGNATION_1:
                    return "Unique Designation 1";
                case T1_UNIQUE_DESIGNATION_2:
                    return "Unique Designation 2";
                case V_EQUIP_TYPE:
                    return "Equipment Type";
                case W_DTG_1:
                    return "Date Time Group 1";
                case W1_DTG_2:
                    return "Date Time Group 2";
                case X_ALTITUDE_DEPTH:
                    return "Altitude Depth";
                case Y_LOCATION:
                    return "Location";
                case Z_SPEED:
                    return "Speed";
                case AA_SPECIAL_C2_HQ:
                    return "Special C2 HQ";
                //case AB_FEINT_DUMMY_INDICATOR:
                //    return "Feint Dummy Indicator";
                //case AC_INSTALLATION:
                //    return "Installation";
                case AD_PLATFORM_TYPE:
                    return "Platform Type";
                case AE_EQUIPMENT_TEARDOWN_TIME:
                    return "Equipment Teardown Time";
                case AF_COMMON_IDENTIFIER:
                    return "Common Identifier";
                //case AG_AUX_EQUIP_INDICATOR:
                //    return "Auxiliary Equipment Indicator";
                /*case AH_AREA_OF_UNCERTAINTY:
                    return "Area of Uncertainty";
                case AI_DEAD_RECKONING_TRAILER:
                    return "Dead Reckoning Trailer";
                case AJ_SPEED_LEADER:
                    return "Speed Leader";
                case AK_PAIRING_LINE:
                    return "Pairing Line";
                case AL_OPERATIONAL_CONDITION:
                    return "Operational Condition";
                case AO_ENGAGEMENT_BAR:
                    return "Engagement Bar";//*/
                case SCC_SONAR_CLASSIFICATION_CONFIDENCE:
                    return "Sonar Classification Confidence";
                default:
                    return "";

            }
        }

    }
}
