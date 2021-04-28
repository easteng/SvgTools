#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       BringToFrontCommand.cs
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

    struct State
    {
        #region Fields

        public DrawObject Obj;
        public int Zorder;

        #endregion Fields
    }

    class BringToFrontCommand : ICommand
    {
        #region Fields

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _objectsBroughtForward;
        private readonly ArrayList _zOrderBackup;

        #endregion Fields

        #region Constructors

        public BringToFrontCommand(
            ArrayList graphicsList,
            ArrayList shapesToBringForward)
        {
            _objectsBroughtForward = shapesToBringForward;
            _graphicsList = graphicsList;
            _zOrderBackup = new ArrayList();
        }

        //Disable default constructor
        private BringToFrontCommand()
        {
        }

        #endregion Constructors

        #region Methods

        public void Execute()
        {
            var tempList = new ArrayList();
            int n = _graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (int i = n - 1; i >= 0; i--)
            {
                if (_objectsBroughtForward.Contains(_graphicsList[i]))
                {
                    State oldState;

                    //Undo Redo -- Start
                    oldState.Obj = ((DrawObject)_graphicsList[i]);
                    oldState.Zorder = i;
                    _zOrderBackup.Add(oldState);
                    //Undo Redo -- End

                    tempList.Add(_graphicsList[i]);
                    _graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (int i = 0; i < n; i++)
            {
                _graphicsList.Insert(0, tempList[i]);
            }
        }

        public void UnExecute()
        {
            for (int i = 0; i < _objectsBroughtForward.Count; i++)
            {
                var state = (State)_zOrderBackup[i];
                _graphicsList.Remove(state.Obj);
                _graphicsList.Insert(state.Zorder, state.Obj);
            }
        }

        #endregion Methods
    }
}