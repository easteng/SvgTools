#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolText.cs
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
    using System.Drawing;

    /// <summary>
    /// Ellipse tool
    /// </summary>
    public class ToolText : ToolRectangle
    {
        #region Constructors

        public ToolText()
        {
            //Cursor = new Cursor(GetType(), "Text.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("SVGEditor2.Resources.Text.cur"));
            MinSize = new System.Drawing.Size(40, 20);
        }

        #endregion Constructors

        #region Methods

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawText(e.X, e.Y));
        }

        protected override void adjustForMinimumSize(DrawArea drawArea)
        {
            var objectAdded = (DrawText)drawArea.GraphicsList[0];
            Rectangle rect;

            rect = objectAdded.Rect;

            if (MinSize.Width > 0)
            {
                if (objectAdded.Rect.Width < MinSize.Width)
                {
                    rect.Width = (int)(MinSize.Width * DrawObject.Zoom);
                }
            }
            if (MinSize.Height > 0)
            {
                if (objectAdded.Rect.Height < MinSize.Height)
                {
                    rect.Height = (int)(MinSize.Height * DrawObject.Zoom);
                }
            }

            objectAdded.Rect = rect;
        }

        #endregion Methods
    }
}