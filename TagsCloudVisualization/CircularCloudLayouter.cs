using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // Не используется, можно спокойно убрать (в других файлах аналогично)

namespace TagsCloudVisualization
{
	class CircularCloudLayouter : ITagCloudLayouter // to public
	{
		public Point Center { get; }
		public List<Rectangle> Rectangles { get; }
		private int widthSum; // Не используется!
		private ISpiral spiral; // Можно сделать readonly, так как сама спираль не пересоздаётся

		public CircularCloudLayouter(Point center)
		{
			if (center.X < 0 || center.Y < 0)
				throw new ArgumentOutOfRangeException("Center coordinates should be non-negative numbers");
			Center = center;
			Rectangles = new List<Rectangle>();
			spiral = new ArchimedianSpiral(size: 1, center: center); // Лучше поставить angleShift последним аргументом,
																	 // тогда тут можно будет использовать значения без указания аргументов
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
				throw new ArgumentException("Size must not be empty");

			var newRectangleLocation = spiral.GetNextPoint();
			while (!RectangleCanBePlacedAt(newRectangleLocation, rectangleSize))
				newRectangleLocation = spiral.GetNextPoint();

			var rectangle = new Rectangle(newRectangleLocation, rectangleSize);
			Rectangles.Add(rectangle);
			return rectangle;
		}

		public Rectangle PutNextRectangle(int width, int heigth)
		{
			return PutNextRectangle(new Size(width, heigth));
		}

		private bool RectangleCanBePlacedAt(Point p, Size size) // В начале методов обычно идёт глагол => CanRectangleBePlacedAt
		{
			return (p.X >= 0 && p.Y >= 0 && !IsRectangleAt(p, size));
		}

		private bool IsRectangleAt(Point p, Size size)
		{
			var rectToPlace = new Rectangle(p, size); // Можно не экономить на буквах, и назвать rectangleToPlace :)
			return Rectangles.Any(r => r.IntersectsWith(rectToPlace));
		}
	}
}
