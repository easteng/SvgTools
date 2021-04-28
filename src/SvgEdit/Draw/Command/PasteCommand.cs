/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       PasteCommand.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

using System.Collections;

namespace Draw.Command
{
    class PasteCommand : ICommand
    {
        private readonly ArrayList _graphicsList;
        private readonly ArrayList _toBePasted;

        //Disable default constructor
        private PasteCommand()
        {
        }

        public PasteCommand(ArrayList graphicsList, ArrayList toBePasted)
        {
            _graphicsList = graphicsList;
            //_toBePasted = new ArrayList();
            //_toBePasted.AddRange(toBePasted);
            _toBePasted = toBePasted;
        }

        #region ICommand Members

        public void Execute()
        {
            int n = _toBePasted.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                var obj = (DrawObject)_toBePasted[i];
                obj.Move(10, 10);
                obj.Selected = true;
                _graphicsList.Insert(0,obj);
            }
        }

        public void UnExecute()
        {
            int n = _toBePasted.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                _graphicsList.Remove(_toBePasted[i]);
            }
        }

        #endregion
    }
}