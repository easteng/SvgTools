namespace SharpVectors.Dom.Svg
{
	/// <summary>
	/// The SvgPathSegCurvetoQuadraticSmoothRel interface corresponds to a 
    /// "relative smooth quadratic curveto" (t) path data command. 
	/// </summary>
	public interface ISvgPathSegCurvetoQuadraticSmoothRel : ISvgPathSeg
	{
		double X{get;set;}
		double Y{get;set;}
	}
}