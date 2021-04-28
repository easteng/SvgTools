/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawObject.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

using SVGLib;
using System.ComponentModel;

namespace Draw 
{
	/// <summary>
	/// Base class for all draw objects
	/// </summary>
	public abstract class DrawObject : ICloneable
	{
		public static PointF Dpi;
        public static int ObjectId; //Initial value of static in is 0

		#region Properties

	    [Browsable(false)]
	    public bool HitOnCircumferance { get; set; }

	    [Browsable(false)]
	    public int Id { get; set; }

	    /// <summary>
	    /// Selection flag
	    /// </summary>
	    [Browsable(false)]
	    public bool Selected { get; set; }

	    /// <summary>
	    /// Color
	    /// </summary>
	    public Color Fill { get; set; }

	    /// <summary>
        /// Stroke
	    /// </summary>
	    public Color Stroke { get; set; }

	    /// <summary>
	    /// Pen width
	    /// </summary>
        [Browsable(false)]
	    protected float StrokeWidth { get; set; }

        public int Thick
        {
            get
            {
                return (int)(StrokeWidth / Zoom);
            }
            set
            {
                StrokeWidth = (int)(value * Zoom);
            }
        }

        public static float Zoom = 1;

	    /// <summary>
		/// Number of handles
		/// </summary>
        [Browsable(false)]
		public virtual int HandleCount
		{
			get
			{
				return 0;
			}
		}

	    /// <summary>
	    /// Last used color
	    /// </summary>
	    public static Color LastUsedColor { get; set; }

	    /// <summary>
	    /// Last used pen width
	    /// </summary>
	    public static float LastUsedPenWidth { get; set; }

	    public string Name { get; set; }

	    #endregion

		#region Virtual Functions

		/// <summary>
		/// Draw object
		/// </summary>
		/// <param name="g"></param>
		public virtual void Draw(Graphics g)
		{
		}

	    protected DrawObject()
        {
            Name = "";
            Fill = Color.Empty;
            Id = 0;
            SetId();
        }

	    static DrawObject()
	    {
	        LastUsedPenWidth = 1;
	        LastUsedColor = Color.Black;
	    }

	    private void SetId()
        {
            Id = ObjectId++;
        }

		/// <summary>
		/// Get handle point by 1-based number
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public virtual PointF GetHandle(int handleNumber)
		{
			return new PointF(0, 0);
		}

		/// <summary>
		/// Get handle rectangle by 1-based number
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public virtual RectangleF GetHandleRectangle(int handleNumber)
		{
			var point = GetHandle(handleNumber);

			return new RectangleF(point.X - 3, point.Y - 3, 7, 7);
		}

		/// <summary>
		/// Draw tracker for selected object
		/// </summary>
		/// <param name="g"></param>
		public virtual void DrawTracker(Graphics g)
		{
			if ( ! Selected )
				return;

			var brush = new SolidBrush(Color.Black);

			for ( int i = 1; i <= HandleCount; i++ )
			{
				try
				{
					g.FillRectangle(brush, GetHandleRectangle(i));
				} 
				catch
				{}
			}

			brush.Dispose();
		}

		/// <summary>
		/// Hit test.
		/// Return value: -1 - no hit
		///                0 - hit anywhere
		///                > 1 - handle number
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public virtual int HitTest(PointF point)
		{
			return -1;
		}


		/// <summary>
		/// Test whether point is inside of the object
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		protected virtual bool PointInObject(PointF point)
		{
			return false;
		}
        
		/// <summary>
		/// Get cursor for the handle
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public virtual Cursor GetHandleCursor(int handleNumber)
		{
			return Cursors.Default;
		}

        /// <summary>
		/// Get curesor for the border handle
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
        public virtual Cursor GetOutlineCursor(int handleNumber)
		{
            return Cursors.Cross;
		}

		/// <summary>
		/// Test whether object intersects with rectangle
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(RectangleF rectangle)
		{
			return false;
		}

		/// <summary>
		/// Move object
		/// </summary>
		/// <param name="deltaX"></param>
		/// <param name="deltaY"></param>
		public virtual void Move(float deltaX, float deltaY)
		{
		}

		/// <summary>
		/// Move handle to the point
		/// </summary>
		/// <param name="point"></param>
		/// <param name="handleNumber"></param>
		public virtual void MoveHandleTo(PointF point, int handleNumber)
		{
		}

        /// <summary>
        /// mouse click on a handle
        /// </summary>
        /// <param name="handle"></param>
        public virtual void MouseClickOnHandle(int handle)
        {
        }

		/// <summary>
		/// Dump (for debugging)
		/// </summary>
		public virtual void Dump()
		{
			Trace.WriteLine("");
			Trace.WriteLine(GetType().Name);
			Trace.WriteLine("Selected = " + Selected.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Normalize object.
		/// Call this function in the end of object resizing.
		/// </summary>
		public virtual void Normalize()
		{
		}

        /// <summary>
        /// Normalize object.
        /// </summary>
        public virtual void MouseClickOnBorder()
        {

        }

		/// <summary>
		/// Save object to serialization stream
		/// </summary>
        /// <param name="writer"></param>
        /// <param name="scale"></param>
		public virtual void SaveToXml(XmlTextWriter writer,double scale)
		{
		}

		/// <summary>
		/// Load object from serialization stream
		/// </summary>
        /// <param name="reader"></param>
		public virtual void LoadFromXml(XmlTextReader reader)
		{
		}

		#endregion

		#region Other functions

		/// <summary>
		/// Initialization
		/// </summary>
		public virtual void Initialize()
		{
			Stroke = LastUsedColor;
			StrokeWidth = LastUsedPenWidth * Zoom;
		}

		public static string Color2String(Color c)
		{
			if ( c.IsNamedColor )
			{
				return c.Name;
			}

		    byte[] bytes = BitConverter.GetBytes(c.ToArgb());

		    string sColor = "#";
		    sColor += BitConverter.ToString(bytes, 2, 1);
		    sColor += BitConverter.ToString(bytes, 1, 1);
		    sColor += BitConverter.ToString(bytes, 0, 1);

		    return sColor;
		}

		public virtual string GetXmlStr(SizeF scale)
		{
			return "";
		}

		public string GetStrStyle(SizeF scale)
		{
			return GetStringStyle(Stroke,Fill,StrokeWidth,scale);
		}

		public static string GetStringStyle(Color color,Color fill,float strokewidth,SizeF scale)
		{
			float strokeWidth = strokewidth/scale.Width;
		    string sfill = fill != Color.Empty ? Color2String(fill) : "none";
			string sc = " style = \"fill:"+sfill+"; stroke:"+Color2String(color)+"; stroke-width:"+strokeWidth.ToString(CultureInfo.InvariantCulture)+"\"";
			return sc;
		}

		public virtual void Resize(SizeF newscale,SizeF oldscale) 
		{
		}

		public static PointF RecalcPoint(PointF pp, SizeF newscale,SizeF oldscale)
		{
			PointF p = pp;
			p.X = p.X/oldscale.Width;
			p.Y = p.Y/oldscale.Height;
			p.X = p.X*newscale.Width;
			p.Y = p.Y*newscale.Height;
			return p;
		}

		public static float RecalcFloat(float val, float newscale1,float oldscale1)
		{
			val = val/oldscale1;
			val = val*newscale1;
			return val;
		}

		public void RecalcStrokeWidth(SizeF newscale,SizeF oldscale)
		{
			StrokeWidth = RecalcFloat(StrokeWidth, newscale.Width,oldscale.Width);
		}

		public void SetStyleFromSvg(SvgBasicShape svg)
		{
			Stroke = svg.Stroke;
			StrokeWidth = ParseSize(svg.StrokeWidth,Dpi.X);
			Fill = svg.Fill != Color.Transparent ? svg.Fill : Color.Empty;
		}

		public static float ParseSize(string str, float dpi)
		{
			float koef = 1;
		    int ind = str.IndexOf("pt");
			if (ind == -1)
				ind = str.IndexOf("px");
			if (ind == -1)
				ind = str.IndexOf("pc");
			if (ind == -1)
			{
				ind = str.IndexOf("cm");
				if (ind > 0)
				{
					koef = dpi/2.54f;
				}
			}
			if (ind == -1)
			{
				ind = str.IndexOf("mm");
				if (ind > 0)
				{
					koef = dpi/25.4f;
				}
			}
			if (ind == -1)
			{
				ind = str.IndexOf("in");
				if (ind > 0)
				{
					koef = dpi;
				}
			}
			if (ind > 0 )
				str = str.Substring(0,ind);
			str = RemoveAlphas(str);
			try
			{
				float res = float.Parse(str,CultureInfo.InvariantCulture);
				if (koef != 1.1)
					res *= koef;
				return res;
			} 
			catch (Exception ex)
			{
				ErrH.Log("ParseFloat()", "DrawObject", ex.ToString(), ErrH._LogPriority.Info);
				return 0;
			}
		}

		static string RemoveAlphas(string str)
		{
			string s = str.Trim();
			string res = "";
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] < '0' || s[i] > '9')
					if (s[i] != '.')
						continue;
				res += s[i];
			}
			return res;
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}