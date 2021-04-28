// --------------------------------------------------------------------------------
// Name:     SvgElement
//
// Author:   Maurizio Bigoloni <big71@fastwebnet.it>
//           See the ReleaseNote.txt file for copyright and license information.
//
// Remarks:
//
// --------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace SVGLib
{
	/// <summary>
	/// This is the base class of any Svg element.
	/// </summary>
	[DefaultProperty("Id")]
	public class SvgElement
	{
		/// <summary>
		/// List all SVG element types. For each element a specific class is defined in the library.
		/// </summary>
		public enum _SvgElementType
		{
			typeUnsupported,
			typeSvg,
			typeDesc,
			typeText,
			typeGroup,
			typeRect,
			typeCircle,
			typeEllipse,
			typeLine,
			typePath,
			typePolygon,
			typeImage,
			typePolyline
		};

		// ---------- PUBLIC PROPERTIES

		/// <summary>
		/// Standard XML attribute for assigning a unique name to an element.
		/// </summary>
		/// <remarks></remarks>
		[Category("(Core)")]
		[Description("Standard XML attribute for assigning a unique name to an element.")]
		public string Id
		{
			get	
			{
				return GetAttributeStringValue(SvgAttribute._SvgAttribute.attrCore_Id);	
			}

			set	
			{
				SetAttributeValue(SvgAttribute._SvgAttribute.attrCore_Id, value);
			}
		}

		// ---------- PUBLIC PROPERTIES END

		// ---------- PRIVATE PROPERTIES

		private class CEleComparer : IComparer  
		{
			int IComparer.Compare( Object x, Object y )  
			{
				SvgAttribute ax = (SvgAttribute) x;
				SvgAttribute ay = (SvgAttribute) y;

				if ( ax.AttributeGroup == ay.AttributeGroup )
				{
					if ( ax.AttributeType < ay.AttributeType )
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				else if ( ax.AttributeGroup < ay.AttributeGroup )
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
		}


		// navigation
		protected SvgElement m_Parent;
		protected SvgElement m_Child;
		protected SvgElement m_Next; 
		protected SvgElement m_Previous;

		// document
		protected SvgDoc m_doc;
		
		// internal stuff
		protected int m_nInternalId;
		protected string m_sElementName;
		protected string m_sElementValue;
		protected bool m_bHasValue;
		protected _SvgElementType m_ElementType;

		// attributes
		private ArrayList m_attributes;
		
		// ---------- PRIVATE PROPERTIES END

		// ---------- PUBLIC METHODS

		// constructor is protected!

		/// <summary>
		/// It returns the parent element.
		/// </summary>
		/// <returns></returns>
		public SvgElement getParent()
		{
			return m_Parent;
		}

		/// <summary>
		/// It sets the parent element.
		/// </summary>
		/// <param name="ele">New parent element</param>
		public void setParent(SvgElement ele)
		{
			m_Parent = ele;
		}

		/// <summary>
		/// It gest the first child element.
		/// </summary>
		/// <returns>First child element.</returns>
		public SvgElement getChild()
		{
			return m_Child;
		}

		/// <summary>
		/// It sets the first child element.
		/// </summary>
		/// <param name="ele">New child.</param>
		public void setChild(SvgElement ele)
		{
			m_Child = ele;
		}

		/// <summary>
		/// It gets the next sibling element.
		/// </summary>
		/// <returns>Next element.</returns>
		public SvgElement getNext()
		{
			return m_Next;
		}

		/// <summary>
		/// It sets the next sibling element.
		/// </summary>
		/// <param name="ele">New next element.</param>
		public void setNext(SvgElement ele)
		{
			m_Next = ele;
		}

		/// <summary>
		/// It gets the previous sibling element.
		/// </summary>
		/// <returns>Previous element.</returns>
		public SvgElement getPrevious()
		{
			return m_Previous;
		}

		/// <summary>
		/// It sets the previous element.
		/// </summary>
		/// <param name="ele">New previous element.</param>
		public void setPrevious(SvgElement ele)
		{
			m_Previous = ele;
		}

		/// <summary>
		/// It gets the internal Id of the element.
		/// </summary>
		/// <returns>Internal Id.</returns>
		/// <remarks>The internal Id is a unique number inside the SVG document.
		/// It is assigned when the element is added to the document.</remarks>
		public int getInternalId()
		{
			return m_nInternalId;
		}

		/// <summary>
		/// It sets the internal Id of the element.
		/// </summary>
		/// <param name="nId">New internal Id.</param>
		public void setInternalId(int nId)
		{
			m_nInternalId = nId;
		}

		/// <summary>
		/// It returns the SVG element name.
		/// </summary>
		/// <returns>SVG name.</returns>
		public string getElementName()
		{
			return m_sElementName;
		}

		/// <summary>
		/// It returns the current element value.
		/// </summary>
		/// <returns>Element value.</returns>
		/// <remarks>Not all SVG elements are supposed to have a value. For instance a rect element or 
		/// a circle do not usually have a value while a desc or a text they normally have it.</remarks>
		public string getElementValue()
		{
			return m_sElementValue;
		}

		/// <summary>
		/// Sets the element value.
		/// </summary>
		/// <param name="sValue">New element value.</param>
		public void setElementValue(string sValue)
		{
			m_sElementValue = sValue;
		}

		/// <summary>
		/// Flag indicating if a value is expected for the SVG element.
		/// </summary>
		/// <returns>true if the SVG element has normally a value.</returns>
		public bool HasValue()
		{
			return m_bHasValue;
		}

		/// <summary>
		/// It returns the SVG element type.
		/// </summary>
		/// <returns></returns>
		public _SvgElementType getElementType()
		{
			return m_ElementType;
		}

		/// <summary>
		/// It returns the XML string of the SVG tree starting from the element.
		/// </summary>
		/// <returns>XML string.</returns>
		/// <remarks>The method is recursive so it creates the SVG string for the current element and for its
		/// sub-tree. If the element is the root of the SVG document the method return the entire SVG XML string.</remarks>
		public string GetXML()
		{
			string sXML;

			sXML = OpenXMLTag();

			if ( m_Child != null )
			{
				sXML += m_Child.GetXML();
			}

			sXML += CloseXMLTag();

			SvgElement ele = m_Next;
			if (ele != null)
			{
				sXML += ele.GetXML();
			}

			ErrH.Log("SvgElement", "GetXML", ElementInfo(), ErrH._LogPriority.Info);
		
			return sXML;
		}

		/// <summary>
		/// It returns the XML string of the SVG element.
		/// </summary>
		/// <returns>XML string.</returns>
		public string GetTagXml()
		{
			string sXML;

			sXML = OpenXMLTag();
			sXML += CloseXMLTag();

			return sXML;
		}

		/// <summary>
		/// It gets all element attributes.
		/// </summary>
		/// <param name="aType">Attribute type array.</param>
		/// <param name="aName">Attribute name array.</param>
		/// <param name="aValue">Attribute value array.</param>
		public void FillAttributeList(ArrayList aType, ArrayList aName, ArrayList aValue)
		{
			IComparer myComparer = new CEleComparer();
			m_attributes.Sort(myComparer);


			for (int i = 0; i < m_attributes.Count; i++ )
			{
				SvgAttribute attr = (SvgAttribute) m_attributes[i];

				aType.Add(attr.AttributeType);
				aName.Add(attr.Name);
				aValue.Add(attr.Value);
			}
		}

		/// <summary>
		/// It copies all the attributes of the element eleToClone to the
		/// current element.
		/// </summary>
		/// <param name="eleToClone">Element that has to be cloned.</param>
		public void CloneAttributeList(SvgElement eleToClone)
		{
			ArrayList aType = new ArrayList();
			ArrayList aName = new ArrayList();
			ArrayList aValue = new ArrayList();

			eleToClone.FillAttributeList(aType, aName, aValue);

			m_attributes.Clear();


			// copy the attributes
			for (int i = 0; i < aType.Count; i++ )
			{
				AddAttr((SvgAttribute._SvgAttribute) aType[i], aValue[i]);
			}

			// copy the value
			if ( m_bHasValue )
			{
				m_sElementValue = eleToClone.m_sElementValue;
			}
		}

		/// <summary>
		/// It returns a string containing current element info for logging purposes.
		/// </summary>
		/// <returns></returns>
		public string ElementInfo()
		{
			string sMsg = "InternalId:" + m_nInternalId.ToString();

			if ( m_Parent != null )
			{
				sMsg += " - Parent:" + m_Parent.getInternalId().ToString();
			}

			if ( m_Previous != null )
			{
				sMsg += " - Previous:" + m_Previous.getInternalId().ToString();
			}

			if ( m_Next != null )
			{
				sMsg += " - Next:" + m_Next.getInternalId().ToString();
			}

			if ( m_Child != null )
			{
				sMsg += " - Child:" + m_Child.getInternalId().ToString();
			}

			return sMsg;
		}

		// ---------- PUBLIC METHODS END

		// ---------- PRIVATE METHODS

		protected SvgElement(SvgDoc doc)
		{
			ErrH.Log("SvgElement", "SvgElement", "Element created", ErrH._LogPriority.Info);

			m_doc = doc;

			m_attributes = new ArrayList();

			AddAttr(SvgAttribute._SvgAttribute.attrCore_Id, null);

			m_Parent = null;
			m_Child = null;
			m_Next = null;
			m_Previous = null;

			m_sElementName = "unsupported";
			m_sElementValue = "";
			m_bHasValue = false;
			m_ElementType = _SvgElementType.typeUnsupported;
		}

		~SvgElement()
		{
			ErrH.Log("SvgElement", "SvgElement", "Element destroyed, InternalId:" + m_nInternalId.ToString(), ErrH._LogPriority.Info);

			m_Parent = null;
			m_Child = null;
			m_Next = null;
			m_Previous = null;
		}

		protected string OpenXMLTag()
		{
			string sXML;

			sXML = "<" + m_sElementName;

			for (int i = 0; i < m_attributes.Count; i++ )
			{
				SvgAttribute attr = (SvgAttribute) m_attributes[i];
				sXML += attr.GetXML();
			}

			if ( m_sElementValue == "")
			{
				if (m_Child == null)
				{
					sXML += " />\r\n";
				}
				else
				{
					sXML += ">\r\n";
				}
			}
			else
			{
				sXML += ">";
				sXML += m_sElementValue;
			}
			
			return sXML;
		}

		protected string CloseXMLTag()
		{
			if ( (m_sElementValue == "") && (m_Child == null) )
			{
				return "";
			}
			else
			{
				return "</" + m_sElementName + ">\r\n";
			}
		}

		protected void AddAttr(SvgAttribute._SvgAttribute type, object objValue)
		{
			SvgAttribute attrToAdd = new SvgAttribute(type);
			attrToAdd.Value = objValue;

			m_attributes.Add(attrToAdd);
		}

		internal SvgAttribute GetAttribute(string sName)
		{
			for (int i = 0; i < m_attributes.Count; i++ )
			{
				SvgAttribute attr = (SvgAttribute) m_attributes[i];
				if ( attr.Name == sName )
				{
					return attr;
				}
			}
			
			return null;
		}

		internal SvgAttribute GetAttribute(SvgAttribute._SvgAttribute type)
		{
			for (int i = 0; i < m_attributes.Count; i++ )
			{
				SvgAttribute attr = (SvgAttribute) m_attributes[i];
				if ( attr.AttributeType == type )
				{
					return attr;
				}
			}
			
			return null;
		}
		internal bool SetAttributeValue(string sName, string sValue)
		{
			bool bReturn = false;

			for (int i = 0; i < m_attributes.Count; i++ )
			{
				SvgAttribute attr = (SvgAttribute) m_attributes[i];
				if ( attr.Name == sName )
				{
					switch (attr.AttributeDataType)
					{
						case SvgAttribute._SvgAttributeDataType.datatypeString:
						case SvgAttribute._SvgAttributeDataType.datatypeHRef:
							attr.Value = sValue;
							if (attr.AttributeType == SvgAttribute._SvgAttribute.attrStyle_Style)
								ParseStyle(sValue);
							break;

						case SvgAttribute._SvgAttributeDataType.datatypeEnum:
							int nValue = 0;
							try
							{
								nValue = Convert.ToInt32(sValue);
							}
							catch
							{
							}

							attr.Value = nValue;
							break;

						case SvgAttribute._SvgAttributeDataType.datatypeColor:

							if (sValue == "")
							{
								attr.Value = Color.Transparent;
							}
							else
							{
								Color c = attr.String2Color(sValue);
								attr.Value = c;
							}
							break;
					}

					bReturn = true;

					break;
				}
			}
			
			return bReturn;
		}

		internal bool SetAttributeValue(SvgAttribute._SvgAttribute type, object objValue)
		{
			bool bReturn = false;

			for (int i = 0; i < m_attributes.Count; i++ )
			{
				SvgAttribute attr = (SvgAttribute) m_attributes[i];
				if ( attr.AttributeType == type )
				{
					bReturn = true;
					attr.Value = objValue;

					break;
				}
			}
			
			return bReturn;
		}

		internal bool GetAttributeValue(SvgAttribute._SvgAttribute type, out object objValue)
		{
			bool bReturn = false;
			objValue = null;

			for (int i = 0; i < m_attributes.Count; i++ )
			{
				SvgAttribute attr = (SvgAttribute) m_attributes[i];
				if ( attr.AttributeType == type )
				{
					bReturn = true;
					objValue = attr.Value;

					break;
				}
			}
			
			return bReturn;
		}

		internal object GetAttributeValue(SvgAttribute._SvgAttribute type)
		{
			object objValue;

			if ( GetAttributeValue(type, out objValue) )
			{
				return objValue;
			}
			else
			{
				return null;
			}
		}

		internal string GetAttributeStringValue(SvgAttribute._SvgAttribute type)
		{
			object objValue = GetAttributeValue(type);

			if ( objValue != null )
			{
				return objValue.ToString();
			}
			else
			{
				return "";
			}
		}

		internal int GetAttributeIntValue(SvgAttribute._SvgAttribute type)
		{
			object objValue = GetAttributeValue(type);

			if ( objValue != null )
			{
				int nValue = 0;
				try
				{
					nValue = Convert.ToInt32(objValue.ToString());
				}
				catch
				{
				}

				return nValue;
			}
			else
			{
				return 0;
			}
		}

		internal Color GetAttributeColorValue(SvgAttribute._SvgAttribute type)
		{
			object objValue = GetAttributeValue(type);

			if ( objValue != null )
			{
				Color cValue = Color.Black;
				try
				{
					cValue = (Color) (objValue);
				}
				catch
				{
				}

				return cValue;
			}
			else
			{
				return Color.Black;
			}
		}

		// ---------- PRIVATE METHODS END
		public virtual void ParseStyle(string sval)
		{
		}
	}
}
