// --------------------------------------------------------------------------------
// Name:     SvgPath
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
	/// It represents the path SVG element.
	/// Paths represent the outline of a shape which can be filled, stroked, used as a clipping path, or any combination of the three.
	/// </summary>
	public class SvgPath : SvgBasicShape
	{
		/// <summary>
		/// The definition of the outline of a shape.
		/// </summary>
		[Category("(Specific)")]
		[Description("The definition of the outline of a shape.")]
		public string PathData
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_PathData);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_PathData, value);
			}
		}

		/// <summary>
		/// The author's computation of the total length of the path, in user units.
		/// </summary>
		[Category("(Specific)")]
		[Description("The author's computation of the total length of the path, in user units.")]
		public string PathLength
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_PathLength);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrSpecific_PathLength, value);
			}
		}

		/// <summary>
		/// It constructs a path element with no attribute.
		/// </summary>
		/// <param name="doc">SVG document.</param>
		public SvgPath(SvgDoc doc):base(doc)
		{
			Init();
		}
		
		/// <summary>
		/// It constructs a path element.
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="sPathData">SVG document.</param>
		public SvgPath(SvgDoc doc, string sPathData):base(doc)
		{
			Init();

			PathData = sPathData;
		}

		private void Init()
		{
			m_sElementName = "path";
			m_ElementType = _SvgElementType.typePath;

			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_PathData, "");
			AddAttr(SvgAttribute._SvgAttribute.attrSpecific_PathLength, "");
            AddAttr(SvgAttribute._SvgAttribute.attrSpecific_ShapeName, "");
		}
	}
}
