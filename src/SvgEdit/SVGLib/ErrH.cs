// --------------------------------------------------------------------------------
// Name:     ErrH
//
// Author:   Maurizio Bigoloni <big71@fastwebnet.it>
//           See the ReleaseNote.txt file for copyright and license information.
//
// Remarks:
//
// --------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace SVGLib
{
	/// <summary>
	/// Summary description for ErrH.
	/// </summary>
	public class ErrH
	{
		public enum _LogMode
		{
			Trace,
			EventLog,
			MessageBox
		}

		public enum _LogPriority
		{
			Info,
			Warning,
			Error
		}

		/*private static _LogMode m_logmodeError = _LogMode.MessageBox;
		private static _LogMode m_logmodeWarning = _LogMode.Trace;
		private static _LogMode m_logmodeInfo = _LogMode.Trace;*/
 
		
		private string m_sClass;
		private string m_sMethod;

		public ErrH(string sClass, string sMethod)
		{
			m_sClass = sClass;
			m_sMethod = sMethod;

			Log("Enter Method", _LogPriority.Info);
		}

		~ErrH()
		{
			
		}

		public static void Log(string sClass, string sMethod, string sMessage, _LogPriority prio)
		{
			string sMessageToLog;
			bool bLogToEventLog = false;
			bool bLogToMessageBox = false;

			sMessageToLog = Priority(prio) + Where(sClass, sMethod) + sMessage + ".";

			Trace.WriteLine(sMessageToLog);

			if ( bLogToEventLog )
			{
				// todo
			}

			if ( bLogToMessageBox )
			{
				// todo
			}
		}

		public void Log(string sMessage, _LogPriority prio)
		{
			string sMessageToLog;
			bool bLogToEventLog = false;
			bool bLogToMessageBox = false;

			sMessageToLog = Priority(prio) + Where() + sMessage + ".";

			Trace.WriteLine(sMessageToLog);

			if ( bLogToEventLog )
			{
				// todo
			}

			if ( bLogToMessageBox )
			{
				// todo
			}
		}

		public void LogParameter(string sName, string sValue)
		{
			Log("Parameter " + sName + " = " + sValue, _LogPriority.Info);
		}

		public void LogEnd(bool bSuccess)
		{
			if ( bSuccess )
			{
				Log("Method Succeeded", _LogPriority.Info);
			}
			else
			{
				Log("Method Failed", _LogPriority.Info);
			}
		}

		public void LogException(Exception e)
		{
			Log("A exception was catched. Message:" + e.Message , _LogPriority.Error);
		}

		public void LogUnhandledException()
		{
			Log("An unhandled exception was catched", _LogPriority.Error);
		}

		private static string Priority(_LogPriority prio)
		{
			string sPriority = "";

			switch ( prio )
			{
				case _LogPriority.Info:
					sPriority = "INF ";
					break;

				case _LogPriority.Warning:
					sPriority = "WAR ";
					break;

				case _LogPriority.Error:
					sPriority = "ERR ";
					break;
			}

			return sPriority;
		}

		private static string Where(string sClass, string sMethod)
		{
			return "Class:" + sClass + ". Method:" + sMethod + ". ";
		}

		private string Where()
		{
			return "Class:" + m_sClass + ". Method:" + m_sMethod + ". ";
		}
	}
}
