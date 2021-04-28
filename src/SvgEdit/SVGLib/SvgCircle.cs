// --------------------------------------------------------------------------------
// Name:     SvgCircle
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
	/// It represents the circle SVG element.
	/// </summary>
	public class SvgCircle : SvgBasicShape
	{
		/// <summary>
		/// The x-axis coordinate of the center of the circle.
		/// </summary>
		[Category("(Specific)")]
		[Description("The x-axis coordinate of the center of the circle.")]
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
		/// The y-axis coordinate of the center of the circle.
		/// </summary>
		[Category("(Specific)")]
		[Description("The y-axis coordinate of the center of the circle.")]
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
		/// The radius of the circle.
		/// </summary>
		[Category("(Specific)")]
		[Description("The radius of the circle.")]
		public string R
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_R);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_R, value);
			}
		}

		/// <summary>
		/// It constructs a circle element with no attribute.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		public SvgCircle(SvgDoc doc):base(doc)
		{
			Init();
		}

		/// <summary>
		/// It constructs a circle element.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		/// <param name="sCX"></param>
		/// <param name="sCY"></param>
		/// <param name="sRadius"></param>
		public SvgCircle(SvgDoc doc, string sCX, string sCY, string sRadius):base(doc)
		{
			Init();

			CX = sCX;
			CY = sCY;
			R = sRadius;
		}

		private void Init()
		{
			m_sElementName = "circle";
			m_ElementType = _SvgElementType.typeCircle;

			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_CX, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_CY, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_R, "");
		}
	}
}
