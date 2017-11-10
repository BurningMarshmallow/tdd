using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
	class CircularCloudLayouter : ITagCloudLayouter
	{
		public Point Center { get; }
		private List<Rectangle> rectangles;
		private int widthSum;
		private ISpiral spiral;

		public CircularCloudLayouter(Point center)
		{
			if (center.X < 0 || center.Y < 0)
				throw new ArgumentOutOfRangeException("Center coordinates should be non-negative numbers");
			Center = center;
			rectangles = new List<Rectangle>();
			spiral = new ArchimedianSpiral(size: 1, center: center);
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
				throw new ArgumentException("Size must not be empty");

			var newRectangleLocation = spiral.GetNextPoint();
			while (!RectangleCanBePlacedAt(newRectangleLocation, rectangleSize))
				newRectangleLocation = spiral.GetNextPoint();

			var rectangle = new Rectangle(newRectangleLocation, rectangleSize);
			rectangles.Add(rectangle);
			return rectangle;
		}

		public Rectangle PutNextRectangle(int width, int heigth)
		{
			return PutNextRectangle(new Size(width, heigth));
		}

		private bool RectangleCanBePlacedAt(Point p, Size size)
		{
			return (p.X >= 0 && p.Y >= 0 && !IsRectangleAt(p, size));
		}

		private bool IsRectangleAt(Point p, Size size)
		{
			var rectToPlace = new Rectangle(p, size);
			return rectangles.Any(r => r.IntersectsWith(rectToPlace));
		}
	}
}
