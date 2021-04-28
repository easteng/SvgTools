#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DeleteCommand.cs
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

    class DeleteCommand : ICommand
    {
        #region Fields

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _graphicsListDeleted;

        #endregion Fields

        #region Constructors

        public DeleteCommand(ArrayList graphicsList)
        {
            _graphicsList = graphicsList;
            _graphicsListDeleted = new ArrayList();
        }

        //Disable default constructor
        private DeleteCommand()
        {
        }

        #endregion Constructors

        #region Methods

        public void Execute()
        {
            int n = _graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    State obj;
                    obj.Obj = (DrawObject)_graphicsList[i];
                    obj.Zorder = i;
                    _graphicsListDeleted.Add(obj);
                    _graphicsList.RemoveAt(i);
                }
            }
        }

        public void UnExecute()
        {
            for (int i = 0; i < _graphicsListDeleted.Count; i++)
            {
                //put back whatever is deleted
                var obj = (State)_graphicsListDeleted[i];
                _graphicsList.Insert(obj.Zorder, obj.Obj);
            }
        }

        #endregion Methods
    }
}