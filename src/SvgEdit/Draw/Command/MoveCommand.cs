#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       MoveCommand.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Draw.Command
{
    using System.Collections;
    using System.Drawing;

    class MoveCommand : ICommand
    {
        #region Fields

        private readonly ArrayList _graphicListMoved;
        private PointF _deltaMoved;

        #endregion Fields

        #region Constructors

            public MoveCommand(ArrayList itemsMoved, PointF delta)
            {
                _graphicListMoved = new ArrayList();
                _deltaMoved = new PointF();

                _graphicListMoved.AddRange(itemsMoved);
                _deltaMoved = delta;
            }

            //Disable default constructor
            private MoveCommand()
            {
            }

        #endregion Constructors

        #region Methods

            public void Execute()
            {
                for (int i = 0; i < _graphicListMoved.Count; i++)
                {
                    ((DrawObject)_graphicListMoved[i]).Move(_deltaMoved.X, _deltaMoved.Y);
                }
            }

            public void UnExecute()
            {
                for (int i = 0; i < _graphicListMoved.Count; i++)
                {
                    ((DrawObject)_graphicListMoved[i]).Move(-1 * _deltaMoved.X, -1 * _deltaMoved.Y);
                }
            }

        #endregion Methods
    }
}