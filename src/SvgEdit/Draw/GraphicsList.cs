#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       GraphicsList.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Draw
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using SVGLib;
    using Command;

    /// <summary>
    /// List of graphic objects
    /// </summary>
    [Serializable]
    public class GraphicsList// : ISerializable
    {
        #region Fields

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _inMemoryList;
        private readonly UndoRedo _undoRedo;

        private bool _isCut;

        #endregion Fields

        #region Constructors

        public GraphicsList()
        {
            _graphicsList = new ArrayList();
            _inMemoryList = new ArrayList();
            _undoRedo = new UndoRedo();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from GraphicsList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return _graphicsList.Count;
            }
        }

        /// <summary>
        /// Gets or Sets the description
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;
                foreach (DrawObject o in _graphicsList)
                {
                    if ( o.Selected )
                        n++;
                }
                return n;
            }
        }

        #endregion Properties

        #region Indexers

        public DrawObject this[int index]
        {
            get
            {
                if ( index < 0  ||  index >= _graphicsList.Count )
                    return null;

                return ((DrawObject)_graphicsList[index]);
            }
        }

        #endregion Indexers

        #region Methods

        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            _graphicsList.Insert(0, obj);
            var create = new CreateCommand(obj, _graphicsList);
            _undoRedo.AddCommand(create);
        }

        // **************   Read from SVG
        public void AddFromSvg(SvgElement ele)
        {
            while (ele != null)
            {
                DrawObject o = CreateDrawObject(ele);
                if (o != null)
                    Add(o);
                SvgElement child = ele.getChild();
                while (child != null)
                {
                    AddFromSvg(child);
                    child = child.getNext();
                }
                ele = ele.getNext();
            }
        }

        public bool AreItemsInMemory()
        {
            return (_inMemoryList.Count > 0);
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (_graphicsList.Count > 0);
            _graphicsList.Clear();
            return result;
        }

        /// <summary>
        /// Cut selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is cut
        /// </returns>
        public void CutSelection()
        {
            int i;
            int n = _graphicsList.Count;
            _inMemoryList.Clear();
            for (i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    _inMemoryList.Add(_graphicsList[i]);
                }
            }
            _isCut = true;

            var cmd = new CutCommand(_graphicsList, _inMemoryList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);
        }

        /// <summary>
        /// Copies selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is copied
        /// </returns>
        public bool CopySelection()
        {
            bool result = false;
            int n = _graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    _inMemoryList.Clear();
                    _inMemoryList.Add(_graphicsList[i]);
                    result = true;
                    _isCut = false;
                }
            }

            return result;
        }


        /// <summary>
        /// Paste selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is pasted
        /// </returns>
        public void PasteSelection()
        {
            int n = _inMemoryList.Count;

            UnselectAll();

            if (n > 0)
            {
                var tempList = new ArrayList();

                int i;
                for (i = n - 1; i >= 0; i--)
                {
                    tempList.Add(((DrawObject)_inMemoryList[i]).Clone());
                }

                if (_inMemoryList.Count > 0)
                {
                    var cmd = new PasteCommand(_graphicsList, tempList);
                    cmd.Execute();
                    _undoRedo.AddCommand(cmd);

                    //If the items are cut, we will not delete it
                    if (_isCut)
                        _inMemoryList.Clear();
                }
            }
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool DeleteSelection()
        {
            var cmd = new DeleteCommand(_graphicsList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);
            return true;
        }

        public void Draw(Graphics g)
        {
            int n = _graphicsList.Count;
            DrawObject o;

            // Enumerate list in reverse order
            // to get first object on the top
            for (int i = n - 1; i >= 0; i-- )
            //for (int i = 0; i < graphicsList.Count; i++ )
            {
                o = (DrawObject)_graphicsList[i];

                o.Draw(g);

                if ( o.Selected )
                {
                    o.DrawTracker(g);
                }
            }
        }

        public List<DrawObject> GetAllSelected()
        {
            var selectionList = new List<DrawObject>();
            foreach (DrawObject o in _graphicsList)
            {
                if (o.Selected)
                    selectionList.Add(o);
            }
            return selectionList;
        }

        public DrawObject GetFirstSelected()
        {
            foreach ( DrawObject o in _graphicsList )
            {
                if ( o.Selected )
                    return o;
            }
            return null;
        }

        public DrawObject GetSelectedObject(int index)
        {
            int n = -1;
            foreach (DrawObject o in _graphicsList)
            {
                if ( o.Selected )
                {
                    n++;

                    if ( n == index )
                        return o;
                }
            }
            return null;
        }

        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="scale"></param>
        //[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
        public string GetXmlString(SizeF scale)
        {
            string sXml = "";
            int n = _graphicsList.Count;
            for (int i = n - 1; i >= 0; i-- )
            {
                sXml += ((DrawObject)_graphicsList[i]).GetXmlStr(scale);
            }
            return sXml;
        }

        public bool IsAnythingSelected()
        {
            foreach (DrawObject o in _graphicsList)
                if (o.Selected)
                return true;

            return false;
        }

        public void Move(ArrayList movedItemsList, PointF delta)
        {
            var cmd = new MoveCommand(movedItemsList, delta);
            _undoRedo.AddCommand(cmd);
        }

        /// <summary>
        /// Move selected items to back (end of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToBack()
        {
            int n = _graphicsList.Count;
            var tempList = new ArrayList();

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    tempList.Add(_graphicsList[i]);
                }
            }

            var cmd = new SendToBackCommand(_graphicsList, tempList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);

            return true;
        }

        /// <summary>
        /// Move selected items to front (beginning of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToFront()
        {
            int n = _graphicsList.Count;
            var tempList = new ArrayList();

            for (int i = n - 1; i >= 0; i-- )
            {
                if ( ((DrawObject)_graphicsList[i]).Selected )
                {
                    tempList.Add(_graphicsList[i]);
                }
            }

            var cmd = new BringToFrontCommand(_graphicsList, tempList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);

            return true;
        }

        /// <summary>
        /// Property Changed
        /// </summary>
        /// <returns>
        /// void
        /// </returns>
        public void PropertyChanged(GridItem itemChanged, object oldVal)
        {
            int i;
            int n = _graphicsList.Count;
            var tempList = new ArrayList();

            for (i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    tempList.Add(_graphicsList[i]);
                }
            }

            var cmd = new PropertyChangeCommand(tempList,itemChanged, oldVal);

            _undoRedo.AddCommand(cmd);
        }

        public void Redo()
        {
            _undoRedo.Redo();
        }

        public void Resize(SizeF newscale,SizeF oldscale)
        {
            foreach (DrawObject o in _graphicsList)
                o.Resize(newscale,oldscale);
        }

        public void ResizeCommand(DrawObject obj, PointF old, PointF newP, int handle)
        {
            var cmd = new ResizeCommand(obj,old, newP, handle);
            _undoRedo.AddCommand(cmd);
        }

        public void SelectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = true;
            }
        }

        public void SelectInRectangle(RectangleF rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in _graphicsList)
            {
                if ( o.IntersectsWith(rectangle) )
                    o.Selected = true;
            }
        }

        // *************************************************
        public void Undo()
        {
            _undoRedo.Undo();
        }

        public void UnselectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = false;
            }
        }

        DrawObject CreateDrawObject(SvgElement svge)
        {
            DrawObject o = null;
            switch (svge.getElementType())
            {
                case SvgElement._SvgElementType.typeLine:
                    o = DrawLine.Create((SvgLine )svge);
                    break;
                case SvgElement._SvgElementType.typeRect:
                    o = DrawRectangle.Create((SvgRect )svge);
                    break;
                case SvgElement._SvgElementType.typeEllipse:
                    o = DrawEllipse.Create((SvgEllipse )svge);
                    break;
                case SvgElement._SvgElementType.typePolyline:
                    o = DrawPolygon.Create((SvgPolyline )svge);
                    break;
                case SvgElement._SvgElementType.typeImage:
                    o = DrawImage.Create((SvgImage )svge);
                    break;
                case SvgElement._SvgElementType.typeText:
                    o = DrawText.Create((SvgText )svge);
                    break;
                case SvgElement._SvgElementType.typeGroup:
                    o = CreateGroup((SvgGroup )svge);
                    break;
                case SvgElement._SvgElementType.typePath:
                    o = DrawPath.Create((SvgPath)svge);
                    break;
                case SvgElement._SvgElementType.typeDesc:
                    Description = ((SvgDesc)svge).Value;
                    break;
                default:
                    break;
            }
            return o;
        }

        DrawObject CreateGroup(SvgGroup svg)
        {
            DrawObject o = null;
                SvgElement child = svg.getChild();
                if (child != null)
                    AddFromSvg(child);
            return o;
        }

        #endregion Methods
    }
}