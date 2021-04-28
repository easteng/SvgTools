#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolPolygon.cs
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
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    using Draw;

    using SVGLib;

    /// <summary>
    /// Polygon tool
    /// </summary>
    public class ToolPolygon : ToolObject
    {
        #region Fields

        private const int MinDistance = 15*15;

        private int _lastX, _lastY;
        private DrawPolygon _newPolygon;

        #endregion Fields

        #region Constructors

        public ToolPolygon()
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
            // Create new polygon, add it to the list
            // and keep reference to it
            _newPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
            AddNewObject(drawArea, _newPolygon);
            _lastX = e.X;
            _lastY = e.Y;
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if ( e.Button != MouseButtons.Left )
                return;

            if ( _newPolygon == null )
                return;                 // precaution

            var point = new Point(e.X, e.Y);
            int distance = (e.X - _lastX)*(e.X - _lastX) + (e.Y - _lastY)*(e.Y - _lastY);
            try
            {
                if ( distance < MinDistance )
                {
                    // Distance between last two points is less than minimum -
                    // move last point
                    _newPolygon.MoveHandleTo(point, _newPolygon.HandleCount);
                }
                else
                {
                    // Add new point
                    _newPolygon.AddPoint(point);
                    _lastX = e.X;
                    _lastY = e.Y;
                }
                drawArea.Refresh();
            }
            catch(Exception ex)
            {
                ErrH.Log("ToolPolygon", "OnMouse", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            _newPolygon = null;
            IsComplete = true;

            base.OnMouseUp (drawArea, e);
        }

        #endregion Methods
    }
}