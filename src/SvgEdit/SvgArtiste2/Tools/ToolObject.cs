#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ToolObject.cs
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
    using System.Windows.Forms;

    using Draw;
using System.Drawing;

    /// <summary>
    /// Base class for all tools which create new graphic object
    /// </summary>
    public abstract class ToolObject : Tool
    {
        #region Properties

        /// <summary>
        /// Tool cursor.
        /// </summary>
        protected Cursor Cursor
        {
            get; set;
        }

        /// <summary>
        /// The minimum size of the object
        /// </summary>
        protected Size MinSize { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            if(drawArea.GraphicsList[0] != null)
                drawArea.GraphicsList[0].Normalize();

            adjustForMinimumSize(drawArea);

            drawArea.Capture = false;
            IsComplete = true;
            drawArea.Refresh();
        }

        /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        [CLSCompliant(false)]
        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);

            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }

        protected virtual void adjustForMinimumSize(DrawArea drawArea)
        {
        }

        #endregion Methods
    }
}