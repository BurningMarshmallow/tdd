using System.Drawing;

namespace TagsCloudVisualization
{
	interface ITagCloudLayouter
	{
		Rectangle PutNextRectangle(Size rectangleSize);
	}
}