using System;
using System.ComponentModel;

namespace SVGLib
{
	/// <summary>
	/// It represents the image SVG element.
	/// </summary>
	public class SvgImage : SvgElement
	{
		/// <summary>
		/// The x-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.
		/// </summary>
		[Category("(Specific)")]
		[Description("The x-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
		public string X
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_X);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_X, value);
			}
		}

		/// <summary>
		/// The y-axis coordinate of the side of the element which has the smaller y-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.
		/// </summary>
		[Category("(Specific)")]
		[Description("The y-axis coordinate of the side of the element which has the smaller y-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
		public string Y
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Y);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_Y, value);
			}
		}

		/// <summary>
		/// The width of the element. A value of zero disables rendering of the element.
		/// </summary>
		[Category("(Specific)")]
		[Description("The width of the element. A value of zero disables rendering of the element.")]
		public string Width
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Width);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_Width, value);
			}
		}

		/// <summary>
		/// The height of the element. A value of zero disables rendering of the element.
		/// </summary>
		[Category("(Specific)")]
		[Description("The height of the element. A value of zero disables rendering of the element.")]
		public string Height
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Height);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_Height, value);
			}
		}

		/// <summary>
		/// This attribute assigns a (CSS) class name or set of class names to an element.
		/// </summary>
		[Category("Style")]
		[Description("This attribute assigns a (CSS) class name or set of class names to an element.")]
		public string Class
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrStyle_Class);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrStyle_Class, value);
			}
		}

		/// <summary>
		/// This attribute specifies style information for the current element. The style attribute specifies style information for a single element.
		/// </summary>
		[Category("Style")]
		[Description("This attribute specifies style information for the current element. The style attribute specifies style information for a single element.")]
		public string Style
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrStyle_Style);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrStyle_Style, value);
			}
		}

		/// <summary>
		/// Url of the image file.
		/// </summary>
		[Category("(Specific)")]
		[Description("Url of the image file.")]
		public string HRef
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrXLink_HRef);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrXLink_HRef, value);
			}
		}

		/// <summary>
		/// It constructs an image element with no attribute.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		public SvgImage(SvgDoc doc):base(doc)
		{
			Init();
		}

		/// <summary>
		/// It constructs an image element.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		/// <param name="sX"></param>
		/// <param name="sY"></param>
		/// <param name="sWidth"></param>
		/// <param name="sHeight"></param>
		/// <param name="sHRef"></param>
		/// <param name="doc"></param>
		public SvgImage(SvgDoc doc, 
						string sX, 
						string sY, 
						string sWidth, 
						string sHeight,
						string sHRef):base(doc)
		{
			Init();

			X = sX;
			Y = sY;
			Width = sWidth;
			Height = sHeight;
			HRef = sHRef;
		}

		private void Init()
		{
			m_sElementName = "image";
			m_ElementType = _SvgElementType.typeImage;

			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_X, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_Y, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_Width, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_Height, "");

			AddAttr(SvgAttribute._SvgAttribute.attrXLink_HRef, "");

			AddAttr(SvgAttribute._SvgAttribute.attrStyle_Class, "");
			AddAttr(SvgAttribute._SvgAttribute.attrStyle_Style, "");
		}
	}
}
