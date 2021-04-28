#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       PropertyChangeCommand.cs
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
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    class PropertyChangeCommand : ICommand
    {
        #region Fields

        private readonly ArrayList _itemsWhosePropertyChanged;
        private readonly Object _oldProperty;
        private readonly GridItem _propertyWhichChanged;

        private Object _newProperty;

        #endregion Fields

        #region Constructors

        public PropertyChangeCommand(ArrayList itemsWhosePropertyChanged,
            GridItem propertyWhichChanged,
            Object oldProperty)
        {
            _itemsWhosePropertyChanged = new ArrayList();
            _itemsWhosePropertyChanged.AddRange(itemsWhosePropertyChanged);

            _itemsWhosePropertyChanged = itemsWhosePropertyChanged;
            _propertyWhichChanged = propertyWhichChanged;
            _oldProperty = oldProperty;
        }

        //Disable default constructor
        private PropertyChangeCommand()
        {
        }

        #endregion Constructors

        #region Methods

        public void Execute()
        {
            try
            {
                for (int i = 0; i < _itemsWhosePropertyChanged.Count; i++)
                {
                    Type t = _itemsWhosePropertyChanged[i].GetType();
                    if (_propertyWhichChanged != null)
                    {
                        PropertyInfo pi = t.GetProperty(_propertyWhichChanged.Label);
                        pi.SetValue(_itemsWhosePropertyChanged[i], _newProperty, null);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UnExecute()
        {
            try
            {
                for (int i = 0; i < _itemsWhosePropertyChanged.Count; i++)
                {
                    Type t = _itemsWhosePropertyChanged[i].GetType();
                    if (_propertyWhichChanged != null)
                    {
                        PropertyInfo pi = t.GetProperty(_propertyWhichChanged.Label);
                        _newProperty = pi.GetValue(_itemsWhosePropertyChanged[i], null);
                        pi.SetValue(_itemsWhosePropertyChanged[i], _oldProperty, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Methods
    }
}