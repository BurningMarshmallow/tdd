using System.Drawing;

namespace TagsCloudVisualization
{
	interface ITagCloudLayouter // to public
	{
		Rectangle PutNextRectangle(Size rectangleSize);
	}
}