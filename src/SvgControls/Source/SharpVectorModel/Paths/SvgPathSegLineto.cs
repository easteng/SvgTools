using System;

namespace SharpVectors.Dom.Svg
{
    public abstract class SvgPathSegLineto : SvgPathSeg
    {
        protected SvgPathSegLineto(SvgPathSegType type) : base(type)
        {
        }

        public override SvgPathType PathType
        {
            get {
                return SvgPathType.LineTo;
            }
        }

        public abstract override SvgPointF AbsXY { get; }

        public override double StartAngle
        {
            get {
                SvgPointF prevPoint = this.GetPrevPoint();
                SvgPointF curPoint  = this.AbsXY;

                double dx = curPoint.X - prevPoint.X;
                double dy = curPoint.Y - prevPoint.Y;

                double a = (Math.Atan2(dy, dx) * 180 / Math.PI);
                a += 270;
                a %= 360;
                return a;
            }
        }

        public override double EndAngle
        {
            get {
                double a = this.StartAngle;
                a += 180;
                a %= 360;
                return a;
            }
        }

        public override double Length
        {
            get {
                SvgPointF prevPoint = this.GetPrevPoint();
                SvgPointF thisPoint = this.AbsXY;

                double dx = thisPoint.X - prevPoint.X;
                double dy = thisPoint.Y - prevPoint.Y;

                return Math.Sqrt(dx * dx + dy * dy);
            }
        }

        private SvgPointF GetPrevPoint()
        {
            SvgPathSeg prevSeg = this.PreviousSeg;
            SvgPointF prevPoint;
            if (prevSeg == null)
            {
                prevPoint = new SvgPointF(0, 0);
            }
            else
            {
                prevPoint = prevSeg.AbsXY;
            }
            return prevPoint;
        }
    }
}
