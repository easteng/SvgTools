#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawEllipse.cs
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
    using System.Drawing;
    using System.Globalization;

    using SVGLib;

    /// <summary>
    /// Ellipse graphic object
    /// </summary>
    public sealed class DrawEllipse : DrawRectangle
    {
        #region Fields

        private const string Tag = "ellipse";

        #endregion Fields

        #region Constructors

        public DrawEllipse()
        {
            SetRectangleF(0, 0, 1, 1);
            Initialize();
        }

        public DrawEllipse(float x, float y, float width, float height)
        {
            RectangleF = new RectangleF(x, y, width, height);
            Initialize();
        }

        #endregion Constructors

        #region Methods

        public static DrawEllipse Create(SvgEllipse svg)
        {
            try
            {
                float cx = ParseSize(svg.CX,Dpi.X);
                float cy = ParseSize(svg.CY,Dpi.Y);
                float rx = ParseSize(svg.RX,Dpi.X);
                float ry = ParseSize(svg.RY,Dpi.Y);
                var dobj = new DrawEllipse(cx-rx,cy-ry,rx*2,ry*2);
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawEllipse", "CreateRectangle", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public override void Draw(Graphics g)
        {
            RectangleF r = GetNormalizedRectangle(RectangleF);
            if (Fill != Color.Empty)
            {
                Brush brush = new SolidBrush(Fill);
                g.FillEllipse(brush,r);
            }
            var pen = new Pen(Stroke, StrokeWidth);
            g.DrawEllipse(pen, r);
            pen.Dispose();
        }

        public override string GetXmlStr(SizeF scale)
        {
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            float cx = (RectangleF.X+RectangleF.Width/2)/scale.Width;
            float cy = (RectangleF.Y+RectangleF.Height/2)/scale.Height;
            float rx = (RectangleF.Width/2)/scale.Width;
            float ry = ((RectangleF.Height/2))/scale.Height;

            s += " cx = \""+cx.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " cy = \""+cy.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " rx = \""+rx.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " ry = \""+ry.ToString(CultureInfo.InvariantCulture)+"\"";
            s += " />" + "\r\n";
            return s;
        }

        #endregion Methods
    }
}