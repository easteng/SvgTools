// --------------------------------------------------------------------------------
// Name:     SvgUnsupported
//
// Author:   Maurizio Bigoloni <big71@fastwebnet.it>
//           See the ReleaseNote.txt file for copyright and license information.
//
// Remarks:
//
// --------------------------------------------------------------------------------

using System;

namespace SVGLib
{
	/// <summary>
	/// This class does not represent any SVG element. It is used when parsing an SVG file an
	/// unknown element is found.
	/// </summary>
	public class SvgUnsupported : SvgElement
	{
		public SvgUnsupported(SvgDoc doc, string sName):base(doc)
		{
			m_sElementName = sName + ":unsupported";
			m_ElementType = _SvgElementType.typeUnsupported;
		}
	}
}
