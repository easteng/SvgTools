#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolLine.cs
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
    /// Line tool
    /// </summary>
    public class ToolLine : ToolObject
    {
        #region Constructors

        public ToolLine()
        {
            //Cursor = new Cursor(GetType(), "Line.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("SVGEditor2.Resources.Line.cur"));
        }

        #endregion Constructors

        #region Methods

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawLine(e.X, e.Y, e.X + 1, e.Y + 1));
            IsComplete = true;
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            if ( e.Button == MouseButtons.Left )
            {
                var point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 2);
                drawArea.Refresh();
            }
        }

        #endregion Methods
    }
}