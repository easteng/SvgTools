// --------------------------------------------------------------------------------
// Name:     SvgDesc
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
	/// It represents the desc SVG element.
	/// Each container element or graphics element in an SVG drawing can supply 
	/// a 'desc' and/or a 'title' description string where the description is text-only.
	/// </summary>
	public class SvgDesc : SvgElement
	{
		/// <summary>
		/// The value of the element.
		/// </summary>
		[Category("(Specific)")]
		[Description("The value of the element.")]
		public string Value
		{
			get	
			{
				return m_sElementValue;	
			}

			set	
			{
				m_sElementValue =  value;
			}
		}

		/// <summary>
		/// It constructs a desc element with no attribute.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		public SvgDesc(SvgDoc doc):base(doc)
		{
			Init();
		}

		/// <summary>
		/// It constructs a desc element.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		/// <param name="sValue"></param>
		public SvgDesc(SvgDoc doc, string sValue):base(doc)
		{
			Init();

			Value = sValue;
		}

		private void Init()
		{
			m_sElementName = "desc";
			m_bHasValue = true;
			m_ElementType = _SvgElementType.typeDesc;
		}
	}
}
