#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolPath.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace DrawTools
{
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    using Draw;

    /// <summary>
    /// Polygon tool
    /// </summary>
    public class ToolPath : ToolObject
    {
        #region Fields

        private DrawPath _newPath;
        bool _startPathDraw = true;

        #endregion Fields

        #region Constructors

        public ToolPath()
        {
            //Cursor = new Cursor(GetType(), "Pencil.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("SVGEditor2.Resources.Pencil.cur"));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Left nouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolActionCompleted();
                return;
            }

            // Create new polygon, add it to the list
            // and keep reference to it
            if (_startPathDraw)
            {
                _newPath = new DrawPath(e.X, e.Y);
                AddNewObject(drawArea, _newPath);
                _startPathDraw = false;
                IsComplete = false;
            }
            else
            {
                _newPath.AddPoint(e.Location);
            }
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            if (e.Button == MouseButtons.Left)
            {
                var point = new Point(e.X, e.Y);
                _newPath.MoveHandleTo(point, _newPath.HandleCount);
                drawArea.Refresh();
            }
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
        }

        public override void ToolActionCompleted()
        {
            if(_newPath != null)
            _newPath.CloseFigure();
            _startPathDraw = true;
            IsComplete = true;
            _newPath = null;
        }

        #endregion Methods
    }
}