// --------------------------------------------------------------------------------
// Name:     SvgDoc
//
// Author:   Maurizio Bigoloni <big71@fastwebnet.it>
//           See the ReleaseNote.txt file for copyright and license information.
//
// Remarks:  It represents the SVG document.
//
// --------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace SVGLib
{
	/// <summary>
	/// It represents the SVG document.
	/// </summary>
	public class SvgDoc
	{
		// ---------- PUBLIC PROPERTIES

		// ---------- PUBLIC PROPERTIES END

		// ---------- PRIVATE PROPERTIES

		// root of the document 
		private SvgRoot m_root;
		
		// document elements, hashtable key is the InternalId 
		private Hashtable m_elements;

		// store the next InternalId to be assigned to a new element
		private int m_nNextId;

		private string m_sXmlDeclaration;
		private string m_sXmlDocType;

		// ---------- PRIVATE PROPERTIES END

		// ---------- PUBLIC METHODS

		/// <summary>
		/// Constructor.
		/// </summary>
		public SvgDoc()
		{
			m_root = null;
			m_nNextId = 1;
			m_elements = new Hashtable();
		}

		/// <summary>
		/// It creates a new empty SVG document that contains just the root element.
		/// If a current document exists, it is destroyed.
		/// </summary>
		/// <returns>
		/// The root element of the SVG document.
		/// </returns>
		public SvgRoot CreateNewDocument()
		{
			if ( m_root != null )
			{
				m_root = null;
				m_nNextId = 1;
				m_elements.Clear();
			}

			m_root = new SvgRoot(this);
			m_root.setInternalId(m_nNextId++);
			
			m_elements.Add(m_root.getInternalId(), m_root);

			m_sXmlDeclaration = "<?xml version=\"1.0\" standalone=\"no\"?>";
			m_sXmlDocType = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">";

			m_root.SetAttributeValue(SvgAttribute._SvgAttribute.attrSvg_XmlNs, "http://www.w3.org/2000/svg");
			m_root.SetAttributeValue(SvgAttribute._SvgAttribute.attrSvg_Version, "1.1");

			return m_root;
		}
		public bool LoadFromFile(string sFilename)
		{
			ErrH err = new ErrH("SvgDoc", "LoadFromFile");
			bool bResult = true;
			try
			{
				XmlTextReader reader;
				reader = new XmlTextReader(sFilename);
				if (!LoadFromFile(reader))
					MessageBox.Show("Error reading Svg document","SvgPaint");
			}
			catch(Exception e)
			{
				err.LogException(e);
				bResult = false;
			}
			return bResult;
		}
		/// <summary>
		/// It creates a SVG document reading from a file.
		/// If a current document exists, it is destroyed.
		/// </summary>
		/// <param name="sFilename">The complete path of a valid SVG file.</param>
		/// <returns>
		/// true if the file is loaded successfully and it is a valid SVG document, false if the file cannot be open or if it is not
		/// a valid SVG document.
		/// </returns>
		public bool LoadFromFile(XmlTextReader reader)
		{
			ErrH err = new ErrH("SvgDoc", "LoadFromFile");
			err.LogParameter("sFilename", reader.BaseURI);

			if ( m_root != null )
			{
				m_root = null;
				m_nNextId = 1;
				m_elements.Clear();
			}

			bool bResult = true;

			try
			{
//				XmlTextReader reader;
//				reader = new XmlTextReader(sFilename);
				reader.WhitespaceHandling = WhitespaceHandling.None;
				reader.Normalization = false;
				reader.XmlResolver = null;
				reader.Namespaces = false;

				string tmp;
				SvgElement eleParent = null;
                SvgElement eleLast = null;

				try 
				{
					// parse the file and display each of the nodes.
					while ( reader.Read() && bResult ) 
					{
						switch (reader.NodeType) 
						{
							case XmlNodeType.Attribute:
								tmp = reader.Name;
								tmp = reader.Value;
								break;

							case XmlNodeType.Element:
							{
								SvgElement ele = AddElement(eleParent, reader.Name, ref eleLast);

								if ( ele == null )
								{
									err.Log("Svg element cannot be added. Name: " + reader.Name, ErrH._LogPriority.Warning);
									bResult = false;
								}
								else
								{
									eleParent = ele;

									if (reader.IsEmptyElement)
									{
										if ( eleParent != null )
										{
											eleParent = eleParent.getParent();
										}
									}

									bool bLoop = reader.MoveToFirstAttribute();
									while (bLoop)
									{
										ele.SetAttributeValue(reader.Name, reader.Value);
												
										bLoop = reader.MoveToNextAttribute();
									}
								}
							}
								break;

							case XmlNodeType.Text:
								if ( eleParent != null )
								{
									eleParent.setElementValue(reader.Value);
								}
								break;

							case XmlNodeType.CDATA:

								err.Log("Unexpected item: " + reader.Value, ErrH._LogPriority.Warning);
								break;

							case XmlNodeType.ProcessingInstruction:

								err.Log("Unexpected item: " + reader.Value, ErrH._LogPriority.Warning);
								break;

							case XmlNodeType.Comment:

								err.Log("Unexpected item: " + reader.Value, ErrH._LogPriority.Warning);
								break;

							case XmlNodeType.XmlDeclaration:
								m_sXmlDeclaration = "<?xml " + reader.Value + "?>";
								break;

							case XmlNodeType.Document:
								err.Log("Unexpected item: " + reader.Value, ErrH._LogPriority.Warning);
								break;

							case XmlNodeType.DocumentType:
							{
								string sDTD1;
								string sDTD2;

								sDTD1 = reader.GetAttribute("PUBLIC");
								sDTD2 = reader.GetAttribute("SYSTEM");

								m_sXmlDocType = "<!DOCTYPE svg PUBLIC \"" + sDTD1 + "\" \"" + sDTD2 + "\">";
							}
								break;

							case XmlNodeType.EntityReference:
								err.Log("Unexpected item: " + reader.Value, ErrH._LogPriority.Warning);
								break;

							case XmlNodeType.EndElement:
								if ( eleParent != null )
								{
									eleParent = eleParent.getParent();
								}
								break;
						} // switch
					} // while
				} // read try
				catch(XmlException xmle)
				{
					err.LogException(xmle);
					err.LogParameter("Line Number", xmle.LineNumber.ToString());
					err.LogParameter("Line Position", xmle.LinePosition.ToString());

					bResult = false;
				}
				catch(Exception e)
				{
					err.LogException(e);
					bResult = false;
				}
				finally
				{
					reader.Close();
				}
			}
			catch
			{
				err.LogUnhandledException();
				bResult = false;
			}

			err.LogEnd(bResult);

			return bResult;
		}

		/// <summary>
		/// It saves the current SVG document to a file.
		/// </summary>
		/// <param name="sFilename">The complete path of the file.</param>
		/// <returns>
		/// true if the file is saved successfully, false otherwise
		/// </returns>
		public bool SaveToFile(string sFilename)
		{
			ErrH err = new ErrH("SvgDoc", "SaveToFile");
			err.LogParameter("sFilename", sFilename);

			bool bResult = false;
			StreamWriter sw = null;
			try
			{
				sw = File.CreateText(sFilename);
				bResult = true;
			}
			catch (UnauthorizedAccessException uae)
			{
				err.LogException(uae);
			}
			catch (DirectoryNotFoundException dnfe)
			{
				err.LogException(dnfe);
			}
			catch (ArgumentException ae)
			{
				err.LogException(ae);
			}
			catch
			{
				err.LogUnhandledException();
			}

			if ( !bResult )
			{
				err.LogEnd(false);
				
				return false;
			}

			try
			{
				sw.Write(GetXML());
				sw.Close();
			}
			catch
			{
				err.LogUnhandledException();
				err.LogEnd(false);

				return false;
			}

			err.LogEnd(true);

			return true;
		}

		/// <summary>
		/// It returns the XML string of the entire SVG document.
		/// </summary>
		/// <returns>
		/// The SVG document. An empty string if the document is empty.
		/// </returns>
		public string GetXML()
		{
			if ( m_root == null )
			{
				return "";
			}

			string sXML;

			sXML = m_sXmlDeclaration + "\r\n";
			sXML += m_sXmlDocType;
			sXML += "\r\n";
			
			sXML += m_root.GetXML();

			return sXML;
		}

		/// <summary>
		/// It returns the SvgElement with the given internal (numeric) identifier.
		/// </summary>
		/// <param name="nInternalId">Internal unique identifier of the element.</param>
		/// <returns>
		/// The SvgElement with the given internal Id. Null if no element can be found.
		/// </returns>
		public SvgElement GetSvgElement(int nInternalId)
		{
			if (!m_elements.ContainsKey(nInternalId))
			{
				return null;
			}

			return (SvgElement) m_elements[nInternalId];
		}

		/// <summary>
		/// It returns the root element of the SVG document.
		/// </summary>
		/// <returns>
		/// Root element.
		/// </returns>
		public SvgRoot GetSvgRoot()
		{
			return m_root;
		}

		/// <summary>
		/// It returns the SvgElement with the given XML Id.
		/// </summary>
		/// <param name="sId">XML identifier of the element.</param>
		/// <returns>
		/// The SvgElement with the given XML Id. Null if no element can be found.
		/// </returns>
		public SvgElement GetSvgElement(string sId)
		{
			SvgElement eleToReturn = null;

			IDictionaryEnumerator e = m_elements.GetEnumerator();
			
			bool bLoop = e.MoveNext();
			while ( bLoop )
			{
				string sValue = "";

				SvgElement ele = (SvgElement) e.Value;
				sValue = ele.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrCore_Id);
				if ( sValue == sId )
				{
					eleToReturn = ele;
					bLoop = false;
				}

				bLoop = e.MoveNext();
			}

			return eleToReturn;
		}

		/// <summary>
		/// It adds the new element eleToAdd as the last children of the given parent element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <param name="eleToAdd">Element to be added.</param>
        public void AddElement(SvgElement parent, SvgElement eleToAdd,ref SvgElement last)
		{
			ErrH err = new ErrH("SvgDoc", "AddElement");

			if ( eleToAdd == null || m_root == null )
			{
				err.LogEnd(false);
				return;
			}

			SvgElement parentToAdd = m_root;
			if ( parent != null )
			{
				parentToAdd = parent;
			}

			eleToAdd.setInternalId(m_nNextId++);
			m_elements.Add(eleToAdd.getInternalId(), eleToAdd);

			eleToAdd.setParent(parentToAdd);
			if (parentToAdd.getChild() == null )
			{
				// the element is the first child
				parentToAdd.setChild(eleToAdd);
                last = eleToAdd;
			}
			else
			{
				// add the element as the last sibling
				//SvgElement last = GetLastSibling(parentToAdd.getChild());

                if (last != null)
                {
                    last.setNext(eleToAdd);
                    eleToAdd.setPrevious(last);
                    last = eleToAdd;
                }
                else
                {
                    last = parentToAdd.getChild();
                }
			}

			err.Log(eleToAdd.ElementInfo(), ErrH._LogPriority.Info);
			err.LogEnd(true);
		}

		/// <summary>
		/// It creates a new element according to the element name provided
		/// and it adds the new element as the last children of the given parent element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <param name="sName">SVG element name.</param>
		/// <returns>The new created element.</returns>
        public SvgElement AddElement(SvgElement parent, string sName, ref SvgElement last)
		{
			SvgElement eleToReturn = null;

			if ( sName == "svg" )
			{
				m_root = new SvgRoot(this);
				m_root.setInternalId(m_nNextId++);
			
				m_elements.Add(m_root.getInternalId(), m_root);
				eleToReturn = m_root;
			}
			else if ( sName == "desc" )
			{
				eleToReturn = AddDesc(parent, ref last);
			}
			else if ( sName == "text" )
			{
				eleToReturn = AddText(parent, ref last);
			}
			else if ( sName == "g" )
			{
				eleToReturn = AddGroup(parent, ref last);
			}
			else if ( sName == "rect" )
			{
				eleToReturn = AddRect(parent, ref last);
			}
			else if ( sName == "circle" )
			{
				eleToReturn = AddCircle(parent, ref last);
			}
			else if ( sName == "ellipse" )
			{
				eleToReturn = AddEllipse(parent, ref last);
			}
			else if ( sName == "line" )
			{
				eleToReturn = AddLine(parent,ref last);
			}
			else if ( sName == "path" )
			{
				eleToReturn = AddPath(parent, ref last);
			}
			else if ( sName == "polygon" )
			{
				eleToReturn = AddPolygon(parent,ref last);
			}
			else if ( sName == "image" )
			{
				eleToReturn = AddImage(parent,ref last);
			}
			else if ( sName == "polyline" )
			{
				eleToReturn = AddPolyline(parent,ref last);
			}
			else
			{
				if ( parent != null )
				{
					eleToReturn = AddUnsupported(parent, sName, ref last);
				}
			}

			return eleToReturn;
		}

		/// <summary>
		/// It creates a new element copying all attributes from eleToClone; the new
		/// element is inserted under the parent element provided. 
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <param name="eleToClone">Element to be cloned</param>
		/// <returns></returns>
		public SvgElement CloneElement(SvgElement parent, SvgElement eleToClone, SvgElement last)
		{
			// calculate unique id
			string sOldId = eleToClone.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrCore_Id);
			string sNewId = sOldId;
			
			if ( sOldId != "" )
			{
				int i = 1;
				
				// check if it is unique
				while ( GetSvgElement(sNewId) != null )
				{
					sNewId = sOldId + "_" + i.ToString();
					i++;
				} 
			}

			// clone operation
			SvgElement eleNew = AddElement(parent, eleToClone.getElementName(), ref last);
			eleNew.CloneAttributeList(eleToClone);

			if ( sNewId != "" )
			{
				eleNew.SetAttributeValue(SvgAttribute._SvgAttribute.attrCore_Id, sNewId);
			}

			if ( eleToClone.getChild() != null )
			{
				eleNew.setChild(CloneElement(eleNew, eleToClone.getChild(),last));

				if ( eleToClone.getChild().getNext() != null )
				{
					eleNew.getChild().setNext(CloneElement(eleNew, eleToClone.getChild().getNext(), last));
				}
			}

			return eleNew;
		}

		/// <summary>
		/// It creates a new SVG Unsupported element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <param name="sName">Name</param>
		/// <returns>
		/// New element created.
		/// </returns>
		/// <remarks>
		/// The unsupported element is used when during the parsing of a file an unknown
		/// element tag is found.
		/// </remarks>
		public SvgUnsupported AddUnsupported(SvgElement parent, string sName,ref SvgElement last)
		{
			SvgUnsupported uns = new SvgUnsupported(this, sName);
			
			AddElement(parent, uns,ref last);
			
			return uns;
		}

		/// <summary>
		/// It creates a new SVG Desc element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
		public SvgDesc AddDesc(SvgElement parent, ref SvgElement last)
		{
			SvgDesc desc = new SvgDesc(this);
			
			AddElement(parent, desc, ref last);
			
			return desc;
		}

		/// <summary>
		/// It creates a new SVG Group element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
		public SvgGroup AddGroup(SvgElement parent, ref SvgElement last)
		{
			SvgGroup grp = new SvgGroup(this);
			
			AddElement(parent, grp, ref last);
			
			return grp;
		}
		
		/// <summary>
		/// It creates a new SVG Text element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
		public SvgText AddText(SvgElement parent, ref SvgElement last)
		{
			SvgText txt = new SvgText(this);
			
			AddElement(parent, txt, ref last);
			
			return txt;
		}

		/// <summary>
		/// It creates a new SVG Rect element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
		public SvgRect AddRect(SvgElement parent, ref SvgElement last)
		{
			SvgRect rect = new SvgRect(this);
			
			AddElement(parent, rect, ref last);
			
			return rect;
		}

		/// <summary>
		/// It creates a new SVG Circle element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
		public SvgCircle AddCircle(SvgElement parent, ref SvgElement last)
		{
			SvgCircle circle = new SvgCircle(this);
			
			AddElement(parent, circle, ref last);
			
			return circle;
		}

		/// <summary>
		/// It creates a new SVG Ellipse element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
        public SvgEllipse AddEllipse(SvgElement parent, ref SvgElement last)
		{
			SvgEllipse ellipse = new SvgEllipse(this);
			
			AddElement(parent, ellipse,ref last);
			
			return ellipse;
		}

		/// <summary>
		/// It creates a new SVG Line element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
        public SvgLine AddLine(SvgElement parent, ref SvgElement last)
		{
			SvgLine line = new SvgLine(this);
			
			AddElement(parent, line,ref last);
			
			return line;
		}

		/// <summary>
		/// It creates a new SVG Path element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
		public SvgPath AddPath(SvgElement parent, ref SvgElement last)
		{
			SvgPath path = new SvgPath(this);
			
			AddElement(parent, path, ref last);
			
			return path;
		}

		/// <summary>
		/// It creates a new SVG Polygon element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
        public SvgPolygon AddPolygon(SvgElement parent, ref SvgElement last)
		{
			SvgPolygon poly = new SvgPolygon(this);
			
			AddElement(parent, poly,ref last);
			
			return poly;
		}
		/// <summary>
		/// It creates a new SVG Polyline element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
        public SvgPolyline AddPolyline(SvgElement parent, ref SvgElement last)
		{
			SvgPolyline poly = new SvgPolyline(this);
			
			AddElement(parent, poly,ref last);
			
			return poly;
		}

		/// <summary>
		/// It creates a new SVG Image element.
		/// </summary>
		/// <param name="parent">Parent element. If null the element is added under the root.</param>
		/// <returns>New element created.</returns>
        public SvgImage AddImage(SvgElement parent, ref SvgElement last)
		{
			SvgImage img = new SvgImage(this);
			
			AddElement(parent, img, ref last);
			
			return img;
		}

		/// <summary>
		/// It deletes an element from the document.
		/// </summary>
		/// <param name="ele">Element to be deleted.</param>
		/// <returns>
		/// true if the element has been successfully deleted; false otherwise.
		/// </returns>
		public bool DeleteElement(SvgElement ele)
		{
			return DeleteElement(ele, true);
		}

		/// <summary>
		/// It deletes an element from the document.
		/// </summary>
		/// <param name="nInternalId">Internal Id of the element to be deleted.</param>
		/// <returns>
		/// true if the element has been successfully deleted; false otherwise.
		/// </returns>
		public bool DeleteElement(int nInternalId)
		{
			return DeleteElement(GetSvgElement(nInternalId), true);
		}

		/// <summary>
		/// It deletes an element from the document.
		/// </summary>
		/// <param name="sId">XML Id of the element to be deleted.</param>
		/// <returns>
		/// true if the element has been successfully deleted; false otherwise.
		/// </returns>
		public bool DeleteElement(string sId)
		{
			return DeleteElement(GetSvgElement(sId), true);
		}

		/// <summary>
		/// It moves the element before its current previous sibling.
		/// </summary>
		/// <param name="ele">Element to be moved.</param>
		/// <returns>
		/// true if the operation succeeded.
		/// </returns>
		public bool ElementPositionUp(SvgElement ele)
		{
			ErrH err = new ErrH("SvgDoc", "ElementPositionUp");
			err.Log("Element to move " + ele.ElementInfo(), ErrH._LogPriority.Info);

			SvgElement parent = ele.getParent();
			if ( parent == null )
			{
				err.Log("Root node cannot be moved", ErrH._LogPriority.Info);
				err.LogEnd(false);

				return false;
			}

			if ( IsFirstChild(ele) )
			{
				err.Log("Element is already at the first position", ErrH._LogPriority.Info);
				err.LogEnd(false);

				return false;
			}

			SvgElement nxt = ele.getNext();
			SvgElement prv = ele.getPrevious();
			SvgElement prv2 = null;

			ele.setNext(null);
			ele.setPrevious(null);

			// fix Next
			if ( nxt != null )
			{
				nxt.setPrevious(prv);
			}

			// fix Previous
			if ( prv != null )
			{
				prv.setNext(nxt);
				prv2 = prv.getPrevious();
				prv.setPrevious(ele);

				// check if the Previous is the first child
				if ( IsFirstChild(prv) )
				{
					// if yes the moved element has to became the new first child
					if ( prv.getParent() != null )
					{
						prv.getParent().setChild(ele);
					}
				}
			}

			// fix Previous/Previous
			if ( prv2 != null )
			{
				prv2.setNext(ele);
			}

			// fix Element
			ele.setNext(prv);
			ele.setPrevious(prv2);

			err.Log("Element moved " + ele.ElementInfo(), ErrH._LogPriority.Info);
			err.LogEnd(true);

			return true;
		}

		/// <summary>
		/// It moves the element one level up in the tree hierarchy.
		/// </summary>
		/// <param name="ele">Element to be moved.</param>
		/// <returns>
		/// true if the operation succeeded.
		/// </returns>
		public bool ElementLevelUp(SvgElement ele)
		{
			ErrH err = new ErrH("SvgDoc", "ElementLevelUp");
			err.Log("Element to move " + ele.ElementInfo(), ErrH._LogPriority.Info);

			SvgElement parent = ele.getParent();
			if ( parent == null )
			{
				err.Log("Root node cannot be moved", ErrH._LogPriority.Info);
				err.LogEnd(false);

				return false;
			}

			if ( parent.getParent() == null )
			{
				err.Log("An element cannot be moved up to the root", ErrH._LogPriority.Info);
				err.LogEnd(false);

				return false;
			}
				
			SvgElement nxt = ele.getNext();
				
			// the first child of the parent became the next
			parent.setChild(nxt);

			if ( nxt != null )
			{
				nxt.setPrevious(null);
			}

			// get the last sibling of the parent
			SvgElement last = GetLastSibling(parent);
			if ( last != null )
			{
				last.setNext(ele);
			}

			ele.setParent(parent.getParent());
			ele.setPrevious(last);
			ele.setNext(null);

			return true;
		}

		/// <summary>
		/// It moves the element after its current next sibling.
		/// </summary>
		/// <param name="ele">Element to be moved.</param>
		/// <returns>
		/// true if the operation succeeded.
		/// </returns>
		public bool ElementPositionDown(SvgElement ele)
		{
			ErrH err = new ErrH("SvgDoc", "ElementPositionDown");
			err.Log("Element to move " + ele.ElementInfo(), ErrH._LogPriority.Info);

			SvgElement parent = ele.getParent();
			if ( parent == null )
			{
				err.Log("Root node cannot be moved", ErrH._LogPriority.Info);
				err.LogEnd(false);

				return false;
			}

			if ( IsLastSibling(ele) )
			{
				err.Log("Element is already at the last sibling position", ErrH._LogPriority.Info);
				err.LogEnd(false);

				return false;
			}
			
			SvgElement nxt = ele.getNext();
			SvgElement nxt2 = null;
			SvgElement prv = ele.getPrevious();
			
			// fix Next
			if ( nxt != null )
			{
				nxt.setPrevious(ele.getPrevious());
				nxt2 = nxt.getNext();
				nxt.setNext(ele);
			}

			// fix Previous
			if ( prv != null )
			{
				prv.setNext(nxt);
			}

			// fix Element
			if ( IsFirstChild(ele) )
			{
				parent.setChild(nxt);
			}

			ele.setPrevious(nxt);
			ele.setNext(nxt2);

			if ( nxt2 != null )
			{
				nxt2.setPrevious(ele);
			}
				
			err.Log("Element moved " + ele.ElementInfo(), ErrH._LogPriority.Info);
			err.LogEnd(true);

			return true;
		}
		// ---------- PUBLIC METHODS END

		// ---------- PRIVATE METHODS

		private bool DeleteElement(SvgElement ele, bool bDeleteFromParent)
		{
			ErrH err = new ErrH("SvgDoc", "DeleteElement");

			if ( ele == null )
			{
				err.LogEnd(false);

				return false;
			}

			SvgElement parent = ele.getParent();
			if ( parent == null )
			{
				// root node cannot be delete!
				err.Log("root node cannot be delete!", ErrH._LogPriority.Info);
				err.LogEnd(false);

				return false;
			}

			// set the Next reference of the previous
			if ( ele.getPrevious() != null )
			{
				ele.getPrevious().setNext(ele.getNext());
			}

			// set the Previous reference of the next
			if ( ele.getNext() != null )
			{
				ele.getNext().setPrevious(ele.getPrevious());
			}

			// check if the element is the first child
			// the bDeleteFromParent flag is used to avoid deleting
			// all parent-child relationship. This is used in the Cut
			// operation where the subtree can be pasted 
			if ( bDeleteFromParent )
			{
				if ( IsFirstChild(ele) )
				{
					// set the Child reference of the parent to the next
					ele.getParent().setChild(ele.getNext());
				}
			}

			// delete its children
			SvgElement child = ele.getChild();

			while ( child != null )
			{
				DeleteElement(child, false);
				child = child.getNext();
			}

			// delete the element from the colloection
			m_elements.Remove(ele.getInternalId());

			err.Log(ele.ElementInfo(), ErrH._LogPriority.Info);
			err.LogEnd(true);

			return true;
		}

		// returns true if the given elemebt is the first child 
		private bool IsFirstChild(SvgElement ele)
		{
			if ( ele.getParent() == null )
			{
				return false;
			}

			if ( ele.getParent().getChild() == null )
			{
				return false;
			}

			return (ele.getInternalId() == ele.getParent().getChild().getInternalId());
		}

		private bool IsLastSibling(SvgElement ele)
		{
			SvgElement last = GetLastSibling(ele);

			if ( last == null )
			{
				return false;
			}

			return (ele.getInternalId() == last.getInternalId());
		}

		private SvgElement GetLastSibling(SvgElement ele)
		{
			if ( ele == null )
			{
				return null;
			}

			SvgElement last = ele;
			while (last.getNext() != null)
			{
				last = last.getNext();
			}
			
			return last;
		}
		// ---------- PRIVATE METHODS END
	}
}
