/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       ResizeCommand.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

using System.Drawing;

namespace Draw.Command
{
    class ResizeCommand : ICommand
    {

        private readonly DrawObject _itemResized;
        private readonly PointF _oldPoint;
        private readonly PointF _newPoint;
        private readonly int _handle;

        //Disable default constructor
        private ResizeCommand()
        {
        }

        public ResizeCommand(DrawObject itemResized, PointF old, PointF newP, int handle)
        {
            _itemResized = itemResized;
            _oldPoint = new PointF(old.X, old.Y);
            _newPoint = new PointF(newP.X, newP.Y);
            _handle = handle;
        }

        #region ICommand Members

        public void Execute()
        {
            _itemResized.MoveHandleTo(_newPoint, _handle);
        }

        public void UnExecute()
        {
            _itemResized.MoveHandleTo(_oldPoint, _handle);
        }

        #endregion
    }
}