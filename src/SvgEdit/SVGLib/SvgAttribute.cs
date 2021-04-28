// --------------------------------------------------------------------------------
// Name:     SvgAttribute
//
// Author:   Maurizio Bigoloni <big71@fastwebnet.it>
//           See the ReleaseNote.txt file for copyright and license information.
//
// Remarks:
//
// --------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;

namespace SVGLib
{
	/// <summary>
	/// It represents a generic SVG attribute.
	/// </summary>
	public class SvgAttribute
	{
		/// <summary>
		/// List of the supported SVG attributes.
		/// </summary>
		public enum _SvgAttribute
		{
			attrSvg_XmlNs,
			attrSvg_Version,
			attrCore_Id,
			attrCore_XmlBase,
			attrCore_XmlLang,
			attrCore_XmlSpace,
            attrSpecific_ShapeName, //Added by Ajay
			attrSpecific_X,
			attrSpecific_Y,
			attrSpecific_CX,
			attrSpecific_CY,
			attrSpecific_Width,
			attrSpecific_Height,
			attrSpecific_R,
			attrSpecific_RX,
			attrSpecific_RY,
			attrSpecific_DX,
			attrSpecific_DY,
			attrSpecific_X1,
			attrSpecific_Y1,
			attrSpecific_X2,
			attrSpecific_Y2,
			attrSpecific_Rotate,
			attrSpecific_TextLength,
			attrSpecific_LengthAdjust,
			attrSpecific_PathData,
			attrSpecific_PathLength,
			attrSpecific_Points,
			attrStyle_Class,
			attrStyle_Style,
			attrPaint_Color,
			attrPaint_Fill,
			attrPaint_FillRule,
			attrPaint_Stroke,
			attrPaint_StrokeWidth,
			attrPaint_StrokeDashArray,
			attrPaint_StrokeDashOffSet,
			attrPaint_StrokeLineCap,
			attrPaint_StrokeLineJoin,
			attrPaint_StrokeMiterLimit,
			attrPaint_ColorInterpolation,
			attrPaint_ColorInterpolationFilters,
			attrPaint_ColorRendering,
			attrOpacity_Opacity,
			attrOpacity_FillOpacity,
			attrOpacity_StrokeOpacity,
			attrGraphics_Display,
			attrGraphics_ImageRendering,
			attrGraphics_PointerEvents,
			attrGraphics_ShapeRendering,
			attrGraphics_TextRendering,
			attrGraphics_Visiblity,
			attrFont_Family, 
			attrFont_Size, 
			attrFont_SizeAdjust,
			attrFont_Stretch,
			attrFont_Style, 
			attrFont_Variant,
			attrFont_Weight,
			attrXLink_Type,
			attrXLink_Role,
			attrXLink_Title,
			attrXLink_Show,
			attrXLink_Actuate,
			attrXLink_HRef,
			attrXLink_Target,
			attrText_Anchor
		}

		/// <summary>
		/// List of SVG attribute groups.
		/// </summary>
		public enum _SvgAttributeGroup
		{
			groupUnknown,
			groupSvg,
			groupCore,
			groupElementSpecific,
			groupStyle,
			groupPaint,
			groupGraphics,
			groupOpacity,
			groupFont,
			groupXLink
		}

		/// <summary>
		/// List of SVG attribute data types
		/// </summary>
		public enum _SvgAttributeDataType
		{
			datatypeString,
			datatypeEnum,
			datatypeColor,
			datatypeHRef
		}

		/// <summary>
		/// List of fill rule attribute choices.
		/// </summary>
		public enum _SvgFillRule
		{
			attribute_not_set,
			nonzero,
			evenodd,
			inherit
		}

		/// <summary>
		/// List of line cap attribute choices.
		/// </summary>
		public enum _SvgLineCap
		{
			attribute_not_set,
			butt,
			round,
			square,
			inherit
		}

		/// <summary>
		/// List of line join attribute choices.
		/// </summary>
		public enum _SvgLineJoin
		{
			attribute_not_set,
			miter,
			round,
			bevel,
			inherit
		}

		/// <summary>
		/// List of color interpolation attribute choices.
		/// </summary>
		public enum _SvgColorInterpolation
		{
			attribute_not_set,
			auto,
			sRGB,
			linearRGB,
			inherit
		}

		/// <summary>
		/// List of color rendering attribute choices.
		/// </summary>
		public enum _SvgColorRendering
		{
			attribute_not_set,
			auto,
			optimizeSpeed,
			optimizeQuality,
			inherit
		}

		/// <summary>
		/// List of length adjust attribute choices.
		/// </summary>
		public enum _SvgLengthAdjust
		{
			attribute_not_set,
			spacing,
			spacingAndGlyphs
		}

		/// <summary>
		/// List of font stretch attribute choices.
		/// </summary>
		public enum _SvgFontStretch
		{
			attribute_not_set,
			normal,
			wider,
			narrower,
			ultracondensed,
			extracondensed,
			condensed,
			semicondensed,
			semiexpanded,
			expanded,
			extraexpanded,
			ultraexpanded,
			inherit
		}

		/// <summary>
		/// List of display attribute choices.
		/// </summary>
		public enum _SvgGraphicsDisplay
		{
			attribute_not_set,
			auto,
			block,
			list_item,
			run_in,
			compact,
			marker,
			table,
			inline_table,
			table_row_group,
			table_header_group,
			table_footer_group,
			table_row,
			table_column_group,
			table_column,
			table_cell,
			table_caption,
			none,
			inherit
		}

		/// <summary>
		/// List of image rendering attribute choices.
		/// </summary>
		public enum _SvgImageRendering
		{
			attribute_not_set,
			optimizeSpeed,
			optimizeQuality,
			inherit
		}

		/// <summary>
		/// List of pointer events attribute choices
		/// </summary>
		public enum _SvgPointerEvents
		{
			attribute_not_set,
			visiblePainted,
			visibleFill,
			visibleStroke,
			visible,
			painted,
			fill,
			stroke,
			all,
			none,
			inherit
		}

		/// <summary>
		/// List of shape rendering attribute choices.
		/// </summary>
		public enum _SvgShapeRendering
		{
			attribute_not_set,
			auto,
			optimizeSpeed,
			crispEdges,
			geometricPrecision,
			inherit
		}

		/// <summary>
		/// List of text rendering attribute choices.
		/// </summary>
		public enum _SvgTextRendering
		{
			attribute_not_set,
			auto,
			optimizeSpeed,
			optimizeLegibility,
			geometricPrecision,
			inherit
		}

		/// <summary>
		/// List of visibility attribute choices.
		/// </summary>
		public enum _SvgVisibility
		{
			attribute_not_set,
			visible,
			hidden,
			collapse,
			inherit
		}

		// ---------- PUBLIC PROPERTIES

		/// <summary>
		/// It returns the attribute type.
		/// </summary>
		public _SvgAttribute AttributeType
		{
			get	{return m_AttrType;}
		}
 
		/// <summary>
		/// It returns the attribute group.
		/// </summary>
		public _SvgAttributeGroup AttributeGroup
		{
			get	
			{
				_AttrInfo _info = (_AttrInfo) m_mapAttrInfo[m_AttrType];
				return _info._group;
			}
		}

		/// <summary>
		/// Gets/sets the attribute value. The type of the object depends on the attribute type.
		/// </summary>
		public object Value
		{
			get	{return m_objValue;}

			set	{m_objValue = value;}
		}

		/// <summary>
		/// It returns the attribute name.
		/// </summary>
		public string Name
		{
			get	
			{
				_AttrInfo _info = (_AttrInfo) m_mapAttrInfo[m_AttrType];
				return _info._name;
			}
		}

		/// <summary>
		/// It returns the attribute data type.
		/// </summary>
		public _SvgAttributeDataType AttributeDataType
		{
			get	
			{
				_AttrInfo _info = (_AttrInfo) m_mapAttrInfo[m_AttrType];
				return _info._datatype;
			}
		}

		/// <summary>
		/// For datatypeEnum attributes it returns the array of acceptable values (string array).
		/// </summary>
		public ArrayList AttributeEnumValues
		{
			get
			{
				_AttrInfo _info = (_AttrInfo) m_mapAttrInfo[m_AttrType];
				return _info._enumvalues;
			}
		}

		// ---------- PUBLIC PROPERTIES END

		// ---------- PRIVATE PROPERTIES

		private class _AttrInfo
		{
			public _SvgAttribute _type;
			public _SvgAttributeGroup _group;
			public _SvgAttributeDataType _datatype;
			public string _name;
			public string _groupname;
			public string _help;
			public ArrayList _enumvalues;

			public _AttrInfo()
			{
				_datatype = _SvgAttributeDataType.datatypeString;

				_enumvalues = new ArrayList();
			}
		}

		private _SvgAttribute m_AttrType;
		private object m_objValue;

		private static Hashtable m_mapAttrInfo;
		private static bool m_bInit;

		// ---------- PRIVATE PROPERTIES END

		// ---------- PUBLIC METHODS

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">Attribute type.</param>
		public SvgAttribute(_SvgAttribute type)
		{
			m_AttrType = type;
			m_objValue = null;

			Init();
		}

		/// <summary>
		/// It returns the XML string of the SVG attribute.
		/// </summary>
		/// <returns>
		/// An XML string in the format [attributename]="attributevalue"
		/// </returns>
		public string GetXML()
		{
			// check if the value has not been initialized
			if (m_objValue == null )
			{
				return "";
			}

			// check if it is empty
			if (m_objValue.ToString() == "")
			{
				return "";
			}

			string sValue = "";

			switch (AttributeDataType)
			{
				case _SvgAttributeDataType.datatypeString:
				case _SvgAttributeDataType.datatypeHRef:
					sValue = m_objValue.ToString(); 
					break;

				case _SvgAttributeDataType.datatypeEnum:
					// for enum attribute the 0 item means that the attribute is empty
					int nValue = (int) m_objValue;

					if ( nValue > 0 && nValue <= AttributeEnumValues.Count )
					{
						sValue = (string) AttributeEnumValues[nValue-1];
					}
					break;

				case _SvgAttributeDataType.datatypeColor:

					if ( (Color) m_objValue != Color.Transparent )
					{
						sValue = Color2String((Color) m_objValue);
					}
					break;
			}

			if ( sValue == "" )
			{
				return "";
			}

			return " " + Name + "=\"" + sValue + "\""; ;
		}

		// ---------- PUBLIC METHODS END

		// ---------- PRIVATE METHODS

		internal string Color2String(Color c)
		{
			if ( c.IsNamedColor )
			{
				return c.Name;
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(c.ToArgb());

				string sColor = "#";
				sColor += BitConverter.ToString(bytes, 2, 1);
				sColor += BitConverter.ToString(bytes, 1, 1);
				sColor += BitConverter.ToString(bytes, 0, 1);

				return sColor;
			}
		}

		internal Color String2Color(string sColor)
		{
			Color c;

			if ( sColor.Length == 7 && sColor[0] == '#' )
			{
				string s1 = sColor.Substring(1, 2); 
				string s2 = sColor.Substring(3, 2);
				string s3 = sColor.Substring(5, 2);

				byte r = 0;
				byte g = 0;
				byte b = 0;

				try
				{
					r = Convert.ToByte(s1, 16);
					g = Convert.ToByte(s2, 16);
					b = Convert.ToByte(s3, 16);
				}
				catch
				{
				}

				c = Color.FromArgb(r, g, b);
			}
            else if(sColor.Trim().ToLower().StartsWith("rgb("))
            {
                sColor = sColor.Trim();
                sColor = sColor.Remove(0, 4);
                sColor = sColor.Remove(sColor.Length - 1, 1);
                String[] arrColor = sColor.Split(',');

                byte r = 0;
                byte g = 0;
                byte b = 0;

                try
                {
                    r = Convert.ToByte(arrColor[0].Trim(), 10);
                    g = Convert.ToByte(arrColor[1].Trim(), 10);
                    b = Convert.ToByte(arrColor[2].Trim(), 10);
                }
                catch
                {
                }

                c = Color.FromArgb(r, g, b);

            }
			else
			{
				c = Color.FromName(sColor);
			}

			return c;
		}

		private void Init()
		{
			if ( m_bInit )
			{
				return;
			}
			m_bInit = true;

			m_mapAttrInfo = new Hashtable();
			
			InitSvg(); // attributes that apply to the svg element only
			InitCore();
			InitElementSpecific();
			InitStyle();
			InitPaint();
			InitGraphics();
			InitOpacity();
			InitFont();
			InitXLink();
		}

		private void InitSvg()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSvg_XmlNs;
			info._group = _SvgAttributeGroup.groupSvg;
			info._name = "xmlns";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSvg_Version;
			info._group = _SvgAttributeGroup.groupSvg;
			info._name = "version";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitCore()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrCore_Id;
			info._group = _SvgAttributeGroup.groupCore;
			info._name = "id";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrCore_XmlBase;
			info._group = _SvgAttributeGroup.groupCore;
			info._name = "xml:base";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrCore_XmlLang;
			info._group = _SvgAttributeGroup.groupCore;
			info._name = "xml:lang";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrCore_XmlSpace;
			info._group = _SvgAttributeGroup.groupCore;
			info._name = "xml:space";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("default");
			info._enumvalues.Add("preserve");
			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitElementSpecific()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_Width;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "width";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_Height;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "height";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_X;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "x";
            m_mapAttrInfo.Add(info._type, info);

            // --- Added by Ajay
            info = new _AttrInfo();
            info._type = _SvgAttribute.attrSpecific_ShapeName;
            info._group = _SvgAttributeGroup.groupElementSpecific;
            info._name = "ShapeName";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_Y;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "y";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_CX;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "cx";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_CY;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "cy";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_R;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "r";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_RX;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "rx";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_RY;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "ry";
			info._groupname = "specific";
			info._help = "For rounded rectangles, the y-axis radius of the ellipse used to round off the corners of the rectangle.";

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_DX;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "dx";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_DY;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "dy";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_Rotate;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "rotate";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_TextLength;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "textLength";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_LengthAdjust;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "lengthAdjust";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("spacing");
			info._enumvalues.Add("spacingAndGlyphs");
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_X1;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "x1";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_Y1;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "y1";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_X2;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "x2";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_Y2;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "y2";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_PathData;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "d";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_PathLength;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "pathLength";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrSpecific_Points;
			info._group = _SvgAttributeGroup.groupElementSpecific;
			info._name = "points";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitStyle()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrStyle_Class;
			info._group = _SvgAttributeGroup.groupStyle;
			info._name = "class";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrStyle_Style;
			info._group = _SvgAttributeGroup.groupStyle;
			info._name = "style";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitPaint()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_Color;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "color";
			info._datatype = _SvgAttributeDataType.datatypeColor;

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_Fill;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "fill";
			info._datatype = _SvgAttributeDataType.datatypeColor;

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_FillRule;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "fill-rule";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("nonzero");
			info._enumvalues.Add("evenodd");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_Stroke;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "stroke";
			info._datatype = _SvgAttributeDataType.datatypeColor;

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_StrokeWidth;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "stroke-width";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_StrokeDashArray;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "stroke-dasharray";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_StrokeDashOffSet;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "stroke-dashoffset";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_StrokeLineCap;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "stroke-linecap";
		
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("butt");
			info._enumvalues.Add("round");
			info._enumvalues.Add("square");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_StrokeLineJoin;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "stroke-linejoin";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("miter");
			info._enumvalues.Add("round");
			info._enumvalues.Add("bevel");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_StrokeMiterLimit;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "stroke-miterlimit";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---
			
			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_ColorInterpolation;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "color-interpolation";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("auto");
			info._enumvalues.Add("sRGB");
			info._enumvalues.Add("linearRGB");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_ColorInterpolationFilters;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "color-interpolation-filters";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("auto");
			info._enumvalues.Add("sRGB");
			info._enumvalues.Add("linearRGB");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrPaint_ColorRendering;
			info._group = _SvgAttributeGroup.groupPaint;
			info._name = "color-rendering";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("auto");
			info._enumvalues.Add("optimizeSpeed");
			info._enumvalues.Add("optimizeQuality");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitGraphics()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrGraphics_Display;
			info._group = _SvgAttributeGroup.groupGraphics;
			info._name = "display";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("auto");
			info._enumvalues.Add("block");
			info._enumvalues.Add("list-item");
			info._enumvalues.Add("run-in");
			info._enumvalues.Add("compact");
			info._enumvalues.Add("marker");
			info._enumvalues.Add("table");
			info._enumvalues.Add("inline-table");
			info._enumvalues.Add("table-row-group");
			info._enumvalues.Add("table-header-group");
			info._enumvalues.Add("table-footer-group");
			info._enumvalues.Add("table-row");
			info._enumvalues.Add("table-column-group");
			info._enumvalues.Add("table-column");
			info._enumvalues.Add("table-cell");
			info._enumvalues.Add("table-caption");
			info._enumvalues.Add("none");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrGraphics_ImageRendering;
			info._group = _SvgAttributeGroup.groupGraphics;
			info._name = "image-rendering";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("auto");
			info._enumvalues.Add("optimizeSpeed");
			info._enumvalues.Add("optimizeQuality");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrGraphics_PointerEvents;
			info._group = _SvgAttributeGroup.groupGraphics;
			info._name = "pointer-events";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("visiblePainted");
			info._enumvalues.Add("visibleFill");
			info._enumvalues.Add("visibleStroke");
			info._enumvalues.Add("visible");
			info._enumvalues.Add("painted");
			info._enumvalues.Add("fill");
			info._enumvalues.Add("stroke");
			info._enumvalues.Add("all");
			info._enumvalues.Add("none");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrGraphics_ShapeRendering;
			info._group = _SvgAttributeGroup.groupGraphics;
			info._name = "shape-rendering";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("auto");
			info._enumvalues.Add("optimizeSpeed");
			info._enumvalues.Add("crispEdges");
			info._enumvalues.Add("geometricPrecision");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrGraphics_TextRendering;
			info._group = _SvgAttributeGroup.groupGraphics;
			info._name = "text-rendering";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("auto");
			info._enumvalues.Add("optimizeSpeed");
			info._enumvalues.Add("optimizeLegibility");
			info._enumvalues.Add("geometricPrecision");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrGraphics_Visiblity;
			info._group = _SvgAttributeGroup.groupGraphics;
			info._name = "visibility";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;
			info._enumvalues.Add("visible");
			info._enumvalues.Add("hidden");
			info._enumvalues.Add("collapse");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitOpacity()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrOpacity_Opacity;
			info._group = _SvgAttributeGroup.groupOpacity;
			info._name = "opacity";
				
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrOpacity_FillOpacity;
			info._group = _SvgAttributeGroup.groupOpacity;
			info._name = "fill-opacity";
				
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrOpacity_StrokeOpacity;
			info._group = _SvgAttributeGroup.groupOpacity;
			info._name = "stroke-opacity";
				
			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitFont()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrFont_Family;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "font-family";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrFont_Size;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "font-size";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrFont_SizeAdjust;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "font-size-adjust";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrFont_Stretch;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "font-stretch";
			
			info._datatype = _SvgAttributeDataType.datatypeEnum;

			info._enumvalues.Add("normal");
			info._enumvalues.Add("wider");
			info._enumvalues.Add("narrower");
			info._enumvalues.Add("ultra-condensed");
			info._enumvalues.Add("extra-condensed");
			info._enumvalues.Add("condensed");
			info._enumvalues.Add("semi-condensed");
			info._enumvalues.Add("semi-expanded");
			info._enumvalues.Add("expanded");
			info._enumvalues.Add("extra-expanded");
			info._enumvalues.Add("ultra-expanded");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrFont_Style;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "font-style";
			info._groupname = "font";
			info._help = "This property specifies whether the text is to be rendered using a normal, italic or oblique face.";

/*!!!			info._datatype = _SvgAttributeDataType.datatypeEnum;

			info._enumvalues.Add("normal");
			info._enumvalues.Add("italic");
			info._enumvalues.Add("oblique");
			info._enumvalues.Add("inherit");*/

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrFont_Variant;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "font-variant";
			info._groupname = "font";
			info._help = "This property indicates whether the text is to be rendered using the normal glyphs for lowercase characters or using small-caps glyphs for lowercase characters.";

			info._datatype = _SvgAttributeDataType.datatypeEnum;

			info._enumvalues.Add("normal");
			info._enumvalues.Add("small-caps");
			info._enumvalues.Add("inherit");

			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrFont_Weight;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "font-weight";
			info._groupname = "font";
			info._help = "This property refers to the boldness or lightness of the glyphs used to render the text, relative to other fonts in the same font family.";
/*!!!
			info._datatype = _SvgAttributeDataType.datatypeEnum;

			info._enumvalues.Add("normal");
			info._enumvalues.Add("bold");
			info._enumvalues.Add("bolder");
			info._enumvalues.Add("lighter");
			info._enumvalues.Add("100");
			info._enumvalues.Add("200");
			info._enumvalues.Add("300");
			info._enumvalues.Add("400");
			info._enumvalues.Add("500");
			info._enumvalues.Add("600");
			info._enumvalues.Add("700");
			info._enumvalues.Add("800");
			info._enumvalues.Add("900");
			info._enumvalues.Add("inherit");*/

			m_mapAttrInfo.Add(info._type, info);
			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrText_Anchor;
			info._group = _SvgAttributeGroup.groupFont;
			info._name = "text-anchor";
			info._groupname = "font";
			info._help = "This property specifies whether the text is to be rendered using a normal, italic or oblique face.";
			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		private void InitXLink()
		{
			_AttrInfo info;

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrXLink_Actuate;
			info._group = _SvgAttributeGroup.groupXLink;
			info._name = "xlink:actuate";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrXLink_HRef;
			info._group = _SvgAttributeGroup.groupXLink;
			info._name = "xlink:href";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrXLink_Role;
			info._group = _SvgAttributeGroup.groupXLink;
			info._name = "xlink:role";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrXLink_Show;
			info._group = _SvgAttributeGroup.groupXLink;
			info._name = "xlink:show";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrXLink_Target;
			info._group = _SvgAttributeGroup.groupXLink;
			info._name = "target";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrXLink_Title;
			info._group = _SvgAttributeGroup.groupXLink;
			info._name = "xlink:title";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---

			// ---
			info = new _AttrInfo();
			info._type = _SvgAttribute.attrXLink_Type;
			info._group = _SvgAttributeGroup.groupXLink;
			info._name = "xlink:type";
			
			m_mapAttrInfo.Add(info._type, info);
			// ---
		}

		// ---------- PRIVATE METHODS END
	}
}
