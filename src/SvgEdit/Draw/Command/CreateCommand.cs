#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       CreateCommand.cs
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

    class CreateCommand : ICommand
    {
        #region Fields

        private readonly ArrayList _graphicsList;
        private readonly DrawObject _shape;

        #endregion Fields

        #region Constructors

        public CreateCommand(DrawObject shape, ArrayList graphicsList)
        {
            _shape = shape;
            _graphicsList = graphicsList;
        }

        //Disable default constructor
        private CreateCommand()
        {
        }

        #endregion Constructors

        #region Methods

        public void Execute()
        {
            _graphicsList.Insert(0, _shape);
        }

        public void UnExecute()
        {
            _graphicsList.Remove(_shape);
        }

        #endregion Methods
    }
}