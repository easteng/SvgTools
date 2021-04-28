#region Header

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawText.cs
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
    /// text graphic object
    /// </summary>
    public class DrawText : DrawRectangle
    {
        #region Fields

        public static Font LastFontText = new Font("Microsoft Sans Serif",12);
        public static string LastInputText = "";
        public static StringFormat LastStringFormat = new StringFormat();
        public StringFormat TextAnchor;
        private const string Tag = "text";

        #endregion Fields

        #region Constructors

        public DrawText()
        {
            Font = new Font("Microsoft Sans Serif",9 * Zoom);
            Text = "";
            SetRectangleF(0, 0, 1, 1);
            Initialize();
            TextAnchor = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
        }

        public DrawText(float x, float y)
        {
            Font = new Font(LastFontText.FontFamily, LastFontText.Size * Zoom);
            Text = LastInputText;
            TextAnchor = new StringFormat(DrawText.LastStringFormat) {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            RectangleF = new RectangleF(x* Zoom , y * Zoom, 0, 0);
            Initialize();
        }

        #endregion Constructors

        #region Properties

        public Font Font { get; set; }

        public string Text
        {
            get; set;
        }

        public float Y
        {
            get
            {
                return RectangleF.Y;
            }
            set
            {
                RectangleF = new RectangleF(RectangleF.X,value,RectangleF.Width,RectangleF.Height);
            }
        }

        #endregion Properties

        #region Methods

        public static RectangleF CalcSize(Graphics g,string txt,Font fnt,float x,float y,StringFormat fmt)
        {
            SizeF rectNeed = g.MeasureString(txt, fnt);
            var rect = new RectangleF(x,y,rectNeed.Width,rectNeed.Height);
            if (fmt.Alignment == StringAlignment.Center)
                rect.X -= rect.Width/2;
            else if (fmt.Alignment == StringAlignment.Far)
                rect.X -= rect.Width;
            return rect;
        }

        public static DrawText Create(SvgText svg)
        {
            if (string.IsNullOrEmpty(svg.Value))
                return null;
            try
            {
                var dobj = new DrawText(ParseSize(svg.X,Dpi.X),
                    ParseSize(svg.Y,Dpi.Y)) {Text = svg.Value};
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                ErrH.Log("DrawText", "DrawText", ex.ToString(), ErrH._LogPriority.Info);
                return null;
            }
        }

        public  string GetXmlText(RectangleF rect,Color color,Font font,string txt,SizeF scale,StringFormat anchor)
        {
            //</text>
            Console.WriteLine(font);

            var s = "<";
            s += "rect";
            s += GetStringStyle(color, Fill, 1, scale);
            s += GetRectStringXml(rect, scale, "");
            s += " />" + "\r\n";

            s += "<";
            s += Tag;
            string sc = " style = \"fill:"+Color2String(color)+
                "; font-family:"+font.FontFamily.Name;
            if (font.Bold)
                sc += "; font-weight:bold";
            if (font.Italic)
                sc += "; font-style:italic";
            float fs = font.Size/scale.Height;
            sc += "; font-size:"+fs.ToString(CultureInfo.InvariantCulture)+"pt";
            if (anchor.Alignment != StringAlignment.Near)
            {
                string sa = "";
                switch (anchor.Alignment)
                {
                    case StringAlignment.Center:
                        sa = "middle";
                        break;
                    case StringAlignment.Far:
                        sa = "end";
                        break;
                }
                if (sa.Length>0)
                    sc += "; text-anchor:"+sa;
            }
            sc += "\"";
            s += sc;
            RectangleF crect = rect;
            if (anchor.Alignment == StringAlignment.Center)
            {
                crect.X += crect.Width/2;
            }
            else if (anchor.Alignment == StringAlignment.Far)
            {
                crect.X += crect.Width;
            }
            crect.Y += font.Height;
            s += GetRectStringXml(crect, scale, "");
            s += " >";
            s += txt;
            s += "</"+Tag+">";
            s += "\r\n";

            return s;
        }
        private SizeF ReactSizeF;
        public override void Draw(Graphics g)
        {
            if (RectangleF.Width == 0 || RectangleF.Height == 0)
                RectangleF = CalcSize(g,Text,Font,RectangleF.X,RectangleF.Y,TextAnchor);

            if (Fill != Color.Empty)
            {
                Brush brush1 = new SolidBrush(Fill);
                g.FillRectangle(brush1, RectangleF);
            }
            Pen pen = new Pen(Stroke, StrokeWidth);
            g.DrawRectangle(pen, RectangleF.X, RectangleF.Y, RectangleF.Width, RectangleF.Height);
            ReactSizeF = new SizeF(RectangleF.Width, RectangleF.Height);
            Brush brush = new SolidBrush(Stroke);
            try
            {
                g.DrawString(Text,Font,brush,RectangleF,TextAnchor);
            }
            catch(Exception ex)
            {
                ErrH.Log("DrawText", "Draw", ex.ToString(), ErrH._LogPriority.Info);
            }
        }

        public override string GetXmlStr(SizeF scale)
        {
            return GetXmlText(RectangleF,Stroke,Font,Text,scale,TextAnchor);
        }

        public override void Resize(SizeF newscale,SizeF oldscale)
        {
            base.Resize(newscale,oldscale);
            float newfw = RecalcFloat(Font.Size, newscale.Width,oldscale.Width);
            Font = new Font(Font.FontFamily.Name,newfw,Font.Style);
        }

        [CLSCompliant(false)]
        public bool SetStyleFromSvg(SvgText svg)
        {
            try
            {
                float x = ParseSize(svg.X,Dpi.X);
                float y = ParseSize(svg.Y,Dpi.Y);
                float w = ParseSize(svg.Width,Dpi.X);
                float h = ParseSize(svg.Height,Dpi.Y);
                Text = svg.Value;
                //font
                Stroke = svg.Fill;
                string family = svg.FontFamily;
                float size = ParseSize(svg.FontSize,Dpi.X);
                int fs = 0;
                if (svg.FontWeight.IndexOf("bold")>=0)
                    fs = 1;
                if (svg.FontStyle.IndexOf("italic")>=0)
                    fs = fs|2;
                Font = new Font(family,size,(FontStyle )fs);
                //				y -= font.Size;
                y -= Font.Height;
                RectangleF = new RectangleF(x,y,w,h);
                if (svg.TextAnchor.Length > 0)
                {
                    switch (svg.TextAnchor)
                    {
                        case "start":
                            TextAnchor.Alignment = StringAlignment.Near;
                            break;
                        case "end":
                            TextAnchor.Alignment = StringAlignment.Far;
                            RectangleF = new RectangleF(x-w,y,w,h);
                            break;
                        case "middle":
                            TextAnchor.Alignment = StringAlignment.Center;
                            RectangleF = new RectangleF(x-w/2+30,y+10,w+60,h+60);
                            break;
                    }
                }
                return true;
            }
            catch
            {
                ErrH.Log("DrawText", "SetStyleFromSvg", "SetStyleFromSvg", ErrH._LogPriority.Info);
                return false;
            }
        }

        #endregion Methods
    }
}