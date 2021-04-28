#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolEllipse.cs
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
    using System.Reflection;
    using System.Windows.Forms;

    using Draw;

    /// <summary>
    /// Ellipse tool
    /// </summary>
    public class ToolEllipse : ToolRectangle
    {
        #region Constructors

        public ToolEllipse()
        {
            //Cursor = new Cursor(GetType(), "Ellipse.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("SVGEditor2.Resources.Ellipse.cur"));
        }

        #endregion Constructors

        #region Methods

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawEllipse(e.X, e.Y, 1, 1));
        }

        #endregion Methods
    }
}