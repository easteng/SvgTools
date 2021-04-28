using SharpVectors.Dom.Events;

namespace SharpVectors.Dom.Svg
{
	/// <summary>
	/// The SvgEllipseElement interface corresponds to the 'ellipse' element. 
	/// </summary>
	public interface ISvgEllipseElement	: ISvgElement, ISvgTests, ISvgLangSpace,
		ISvgExternalResourcesRequired, ISvgStylable, ISvgTransformable, IEventTarget, ISvgElementVisitorTarget
    {
		ISvgAnimatedLength Cx{get;}

		ISvgAnimatedLength Cy{get;}

		ISvgAnimatedLength Rx{get;}
		
		ISvgAnimatedLength Ry{get;}
	}
}
