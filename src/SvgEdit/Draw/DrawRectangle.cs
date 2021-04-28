#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawRectangle.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

#endregion Header

namespace Draw
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    using SVGLib;

    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    public class DrawRectangle : DrawObject
    {
        #region Fields

        private const string Tag = "rect";

        private RectangleF rectangle;

        #endregion Fields

        #region Constructors

        public DrawRectangle()
        {
            SetRectangleF(0, 0, 1,1);
            Initialize();
        }

        public DrawRectangle(float x, float y, float width, float height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Initialize();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }

        public Rectangle Rect
        {
            get
            {
                var rect= new Rectangle();
                rect.X = (int)(rectangle.X / Zoom) ;
                rect.Y = (int)(rectangle.Y / Zoom);
                rect.Width = (int)(rectangle.Width / Zoom);
                rect.Height = (int)(rectangle.Height / Zoom);
                return rect;
            }
            set
            {
                rectangle = value;
            }
        }

        protected float Height
        {
            get
            {
                return rectangle.Height;
            }
            set
            {
                rectangle.Height = value;
            }
        }

        protected RectangleF RectangleF
        {
            get
            {
                return rectangle;
            }
            set
            {
                rectangle = value;
            }
        }

        protected float Width
        {
            get
            {
                return rectangle.Width;
            }
            set
            {
                rectangle.Width = value;
            }
        }

        #endregion Properties

        #region Methods

        public static DrawRectangle Create(SvgRect svg)
        {
            try
            {
                var dobj = new DrawRectangle(ParseSize(svg.X,Dpi.X),
                    ParseSize(svg.Y,Dpi.Y),
                    ParseSize(svg.Width,Dpi.X),
                    ParseSize(svg.Height,Dpi.Y));
                dobj.SetStyleFromSvg(svg);
                dobj.Name = svg.ShapeName;

                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawRectangle", "CreateRectangle:", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public static RectangleF GetNormalizedRectangle(float x1, float y1, float x2, float y2)
        {
            if ( x2 < x1 )
            {
                float tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if ( y2 < y1 )
            {
                float tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        public static RectangleF GetNormalizedRectangle(PointF p1, PointF p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static RectangleF GetNormalizedRectangle(RectangleF r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }

        public static string GetRectangleXmlStr(Color stroke,bool isFill,Color fill,float strokewidth,RectangleF rect,SizeF scale, String shapeName)
        {
            string s = "<";
            s += Tag;
            s += GetStringStyle(stroke,fill,strokewidth,scale);//GetStrStyle(scale);
            s += GetRectStringXml(rect,scale, shapeName);
            s += " />" + "\r\n";
            return s;
        }

        public static string GetRectStringXml(RectangleF rect,SizeF scale, String shapeName)
        {
            string s = "";
            float x = rect.X/scale.Width;
            float y = rect.Y/scale.Height;
            float w = rect.Width/scale.Width;
            float h = rect.Height/scale.Height;

            s += " x = \""+x.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " y = \""+y.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " width = \""+w.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " height = \""+h.ToString(CultureInfo.InvariantCulture)+"\"";
            //Added by Ajay
            s += " ShapeName = \"" + shapeName + "\"";
            return s;
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {

            try
            {
                RectangleF r = GetNormalizedRectangle(RectangleF);
                if (Fill != Color.Empty)
                {
                    Brush brush = new SolidBrush(Fill);
                    g.FillRectangle(brush,r);
                }
                Pen pen = new Pen(Stroke, StrokeWidth);
                g.DrawRectangle(pen,r.X,r.Y,r.Width,r.Height);
                // 同时绘制文字上去
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                FontFamily fontFamily = new FontFamily("楷体");
                Font font1 = new Font(fontFamily, 20f, FontStyle.Bold, GraphicsUnit.Pixel);
                string txt = "床前明月光";
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(txt, font1, Brushes.Black, r, sf);
                pen.Dispose();
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawRectangle", "Draw", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        public override void Dump()
        {
            base.Dump ();

            Trace.WriteLine("rectangle.X = " + rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + rectangle.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override PointF GetHandle(int handleNumber)
        {
            float x, xCenter, yCenter;

            xCenter = rectangle.X + rectangle.Width/2;
            yCenter = rectangle.Y + rectangle.Height/2;
            x = rectangle.X;
            float y = rectangle.Y;

            switch ( handleNumber )
            {
                case 1:
                    x = rectangle.X;
                    y = rectangle.Y;
                    break;
                case 2:
                    x = xCenter;
                    y = rectangle.Y;
                    break;
                case 3:
                    x = rectangle.Right;
                    y = rectangle.Y;
                    break;
                case 4:
                    x = rectangle.Right;
                    y = yCenter;
                    break;
                case 5:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = rectangle.Bottom;
                    break;
                case 7:
                    x = rectangle.X;
                    y = rectangle.Bottom;
                    break;
                case 8:
                    x = rectangle.X;
                    y = yCenter;
                    break;
            }
            return new PointF(x, y);
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Cursors.Default;
            }
        }

        public override string GetXmlStr(SizeF scale)
        {
            //  <rect x="1" y="1" width="1198" height="398"
            //		style="fill:none; stroke:blue"/>

            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            s += GetRectStringXml(RectangleF,scale, Name);
            s += " />" + "\r\n";
            return s;
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(PointF point)
        {
            if ( Selected )
            {
                for ( int i = 1; i <= HandleCount; i++ )
                {
                    if ( GetHandleRectangle(i).Contains(point) )
                        return i;
                }
            }

            if ( PointInObject(point) )
                return 0;

            return -1;
        }

        public override bool IntersectsWith(RectangleF rect)
        {
            try
            {
                return RectangleF.IntersectsWith(rect);
            }
            catch(Exception ex)
            {
                ErrH.Log("DrawRectangle", "Intersect", ex.ToString(), ErrH._LogPriority.Info);
                return false;
            }
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(float deltaX, float deltaY)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(PointF point, int handleNumber)
        {
            float left = RectangleF.Left;
            float top = RectangleF.Top;
            float right = RectangleF.Right;
            float bottom = RectangleF.Bottom;

            switch ( handleNumber )
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
                    right = point.X;
                    top = point.Y;
                    break;
                case 4:
                    right = point.X;
                    break;
                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;
                case 6:
                    bottom = point.Y;
                    break;
                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 8:
                    left = point.X;
                    break;
            }

            SetRectangleF(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }

        public override void Resize(SizeF newscale,SizeF oldscale)
        {
            PointF p = RecalcPoint(RectangleF.Location, newscale,oldscale);
            var ps = new PointF(RectangleF.Width, RectangleF.Height);
            ps = RecalcPoint(ps,newscale,oldscale);
            RectangleF = new RectangleF(p.X,p.Y,ps.X,ps.Y);
            RecalcStrokeWidth(newscale,oldscale);
        }

        protected override bool PointInObject(PointF point)
        {
            return rectangle.Contains(point);
        }

        protected void SetRectangleF(float x, float y, float width, float height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
        }

        #endregion Methods
    }
}