using System.Drawing;

/*  --------------------------------------------------------------------------------------------------------------
 *  I Software Innovations
 *  --------------------------------------------------------------------------------------------------------------
 *  SVG Artieste 2.0
 *  --------------------------------------------------------------------------------------------------------------
 *  File     :       DrawPolygon.cs
 *  Author   :       ajaysbritto@yahoo.com
 *  Date     :       20/03/2010
 *  --------------------------------------------------------------------------------------------------------------
 *  Change Log
 *  --------------------------------------------------------------------------------------------------------------
 *  Author	Comments
 */

namespace Draw
{
    public class PathCommands
    {
        public PointF P;
        public char Pc;

        public PathCommands(PointF p, char pc)
        {
            P = p;
            Pc = pc;
        }
    }
}