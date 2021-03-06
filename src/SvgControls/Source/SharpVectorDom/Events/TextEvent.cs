using System;

using SharpVectors.Dom.Views;

namespace SharpVectors.Dom.Events
{
	/// <summary>
	/// Summary description for TextEvent.
	/// </summary>
	public class TextEvent: UiEvent, ITextEvent
	{
		#region Private Fields
		
		private string _data;
		private IAbstractView _view;
		
		#endregion
		
		#region Constructors
		
		public TextEvent()
		{
		}
		
		public TextEvent(string eventType, bool bubbles, bool cancelable, 
            IAbstractView view, string data)
		{
			InitTextEvent(eventType, bubbles, cancelable, view, data);
		}
		
		public TextEvent(string namespaceUri, string eventType, bool bubbles,
			bool cancelable, IAbstractView view, string data)
		{
			InitTextEventNs(namespaceUri, eventType, bubbles, cancelable, view, data);
		}
		
		#endregion
		
		#region ITextEvent interface
		
		public string Data
		{
			get
			{
				return _data;
			}
		}
		
		public void InitTextEvent(string eventType, bool bubbles, bool cancelable, 
            IAbstractView view, string data)
		{
			InitEvent(eventType, bubbles, cancelable);
			
			_view = view;
			_data = data;
		}
		
		public void InitTextEventNs(string namespaceUri, string type, bool bubbles,
			bool cancelable, IAbstractView view, string data)
		{
			InitEventNs(namespaceUri, _eventType, bubbles, cancelable);
			
			_view = view;
			_data = data;
		}
		
		#endregion
	}
}
