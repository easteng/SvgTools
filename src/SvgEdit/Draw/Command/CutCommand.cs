#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       CutCommand.cs
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

    class CutCommand : ICommand
    {
        #region Fields

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _itemsToBeCut;

        #endregion Fields

        #region Constructors

        public CutCommand(ArrayList graphicsList, ArrayList inMemory)
        {
            _graphicsList = graphicsList;
            _itemsToBeCut = new ArrayList();
            _itemsToBeCut.AddRange(inMemory);
        }

        //Disable default constructor
        private CutCommand()
        {
        }

        #endregion Constructors

        #region Methods

        public void Execute()
        {
            for (int i = _graphicsList.Count - 1; i >= 0; i--)
            {
                if (_itemsToBeCut.Contains(_graphicsList[i]))
                {
                    _graphicsList.RemoveAt(i);
                }
            }
        }

        public void UnExecute()
        {
            for (int i = 0; i < _itemsToBeCut.Count; i++)
            {
                _graphicsList.Add(_itemsToBeCut[i]);
            }
            _itemsToBeCut.Clear();
        }

        #endregion Methods
    }
}