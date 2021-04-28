// --------------------------------------------------------------------------------
// Name:     SvgEllipse
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
	/// It represents the ellipse SVG element.
	/// </summary>
	public class SvgEllipse : SvgBasicShape
	{
		/// <summary>
		/// The x-axis coordinate of the center of the ellipse.
		/// </summary>
		[Category("(Specific)")]
		[Description("The x-axis coordinate of the center of the ellipse.")]
		public string CX
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CX);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_CX, value);
			}
		}

		/// <summary>
		/// The y-axis coordinate of the center of the ellipse.
		/// </summary>
		[Category("(Specific)")]
		[Description("The y-axis coordinate of the center of the ellipse.")]
		public string CY
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CY);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_CY, value);
			}
		}

		/// <summary>
		/// The x-axis radius of the ellipse.
		/// </summary>
		[Category("(Specific)")]
		[Description("The x-axis radius of the ellipse.")]
		public string RX
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_RX);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_RX, value);
			}
		}

		/// <summary>
		/// The y-axis radius of the ellipse.
		/// </summary>
		[Category("(Specific)")]
		[Description("The y-axis radius of the ellipse.")]
		public string RY
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_RY);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_RY, value);
			}
		}

		/// <summary>
		/// It constructs an ellipse element with no attribute.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		public SvgEllipse(SvgDoc doc):base(doc)
		{
			Init();
		}

		/// <summary>
		/// It constructs an ellipse element.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		/// <param name="sCX"></param>
		/// <param name="sCY"></param>
		/// <param name="sRX"></param>
		/// <param name="sRY"></param>
		public SvgEllipse(SvgDoc doc, string sCX, string sCY, string sRX, string sRY):base(doc)
		{
			Init();

			CX = sCX;
			CY = sCY;
			RX = sRX;
			RY = sRY;
		}

		private void Init()
		{
			m_sElementName = "ellipse";
			m_ElementType = _SvgElementType.typeEllipse;

			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_CX, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_CY, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_RX, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_RY, "");
		}
	}
}
