namespace SharpVectors.Dom.Svg
{
	/// <summary>
	/// Interface SvgFitToViewBox defines DOM attributes that apply to 
	/// elements which have XML attributes viewBox and preserveAspectRatio.
	/// </summary>
	public interface ISvgFitToViewBox
	{
		ISvgAnimatedRect ViewBox { get; }
		ISvgAnimatedPreserveAspectRatio PreserveAspectRatio { get; }
	}
}