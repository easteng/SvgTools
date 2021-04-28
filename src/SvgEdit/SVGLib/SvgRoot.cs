// --------------------------------------------------------------------------------
// Name:     SvgRoot
//
// Author:   Maurizio Bigoloni <big71@fastwebnet.it>
//           See the ReleaseNote.txt file for copyright and license information.
//
// Remarks:
//
// --------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace SVGLib
{
	/// <summary>
	/// It represents the svg SVG element that is the root of the document.
	/// </summary>
	public class SvgRoot : SvgElement
	{
		/// <summary>
		/// Standard XML namespace.
		/// </summary>
		[Category("Svg")]
		[Description("Standard XML namespace.")]
		public string XmlNs
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSvg_XmlNs);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSvg_XmlNs, value);
			}
		}

		/// <summary>
		/// Standard XML version.
		/// </summary>
		[Category("Svg")]
		[Description("Standard XML version.")]
		public string Version
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSvg_Version);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSvg_Version, value);
			}
		}

		/// <summary>
		/// The width of the svg area.
		/// </summary>
		[Category("(Specific)")]
		[Description("The width of the svg area.")]
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
		/// The height of the svg area.
		/// </summary>
		[Category("(Specific)")]
		[Description("The height of the svg area.")]
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

		internal SvgRoot(SvgDoc doc):base(doc)
		{
			m_sElementName = "svg";
			m_ElementType = _SvgElementType.typeSvg;

			AddAttr(SvgAttribute._SvgAttribute.attrSvg_XmlNs, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSvg_Version, "");

			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_Width, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_Height, "");
		}
	}
}
