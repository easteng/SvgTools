/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolImage.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

using System.Windows.Forms;
using Draw;

namespace DrawTools
{
	/// <summary>
	/// Ellipse tool
	/// </summary>
	public class ToolImage : ToolRectangle
	{
		public ToolImage()
		{
            //Cursor = new Cursor(GetType(), "Bitmap.cur");
            Cursor = new Cursor(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("SVGEditor2.Resources.Text.cur"));
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawImage(e.X, e.Y));
        }

	}
}