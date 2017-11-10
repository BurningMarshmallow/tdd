using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
	[TestFixture]
	class CircularCloudLayouter_Should
	{
		private CircularCloudLayouter layouter;
		private List<Rectangle> rectangles;

		[SetUp]
		public void SetUp()
		{
			layouter = CreateLayouter(50, 50);
		}

		[TestCase(-5, 0)]
		[TestCase(0, -5)]
		[TestCase(-5, -5)]
		public void LayouterShouldThrow_WhenCenterOutOfBorders(int x, int y)
		{
			Action act = () => CreateLayouter(x, y);
			act.ShouldThrow<ArgumentOutOfRangeException>();
		}

		[TestCase(0, 5)]
		[TestCase(5, 0)]
		[TestCase(0, 0)]
		public void LayouterShoudThrow_WhenPuttingEmptyRectangle(int width, int heigth)
		{
			Action act = () => layouter.PutNextRectangle(width, heigth);
			act.ShouldThrow<ArgumentException>();
		}

		[Test]
		public void LayouterShould_PutFirstRectangleAtCenter()
		{
			var rectangle = layouter.PutNextRectangle(3, 3);

			rectangle.Location.Should().Be(layouter.Center);
		}

		[Test]
		public void LayouterShould_AvoidRectangleIntersection()
		{
			var rectangles = new[]
			{
				layouter.PutNextRectangle(3, 3),
				layouter.PutNextRectangle(3, 3),
				layouter.PutNextRectangle(6, 2),
				layouter.PutNextRectangle(1, 1),
			};
			AreIntersectedRectangles(rectangles).Should().BeFalse();
		}

		[Test]
		public void LayouterShould_PutRectanglesInDenseCloud()
		{
			int width = 200;
			int height = 50;
			int repeatCount = 100;
			var sizes = Enumerable.Repeat(new Size(width, height), repeatCount).ToArray();

			var rectangles = sizes.Select(size => layouter.PutNextRectangle(size));

			(rectangles.Max(r => r.X) - rectangles.Min(r => r.X)).Should().BeLessThan(width * (repeatCount));
			(rectangles.Max(r => r.Y) - rectangles.Min(r => r.Y)).Should().BeLessThan(height * (repeatCount));
		}

		[Test]
		public void LayouterShouldNot_ReturnRectanglesWithNegativeCoordinates()
		{
			var layouter = CreateLayouter(0, 0);
			var sizes = Enumerable.Repeat(new Size(40, 40), 100);

			var rectangles = sizes.Select(size => layouter.PutNextRectangle(size));

			rectangles.Should().NotContain(rectangle => rectangle.X < 0 || rectangle.Y < 0);
		}

		[TearDown]
		public void TearDown()
		{
			if (TestContext.CurrentContext.Result.Outcome.Equals(ResultState.Failure))
			{
				var path = Path.Combine(Environment.CurrentDirectory, "Log", TestContext.CurrentContext.Test.Name + ".jpg");
				var image = Visualisation.GetVisualisation(layouter.Rectangles);
				image.Save(path, ImageFormat.Jpeg);
				Console.WriteLine($"Tag cloud visualization saved to file {path}");
			}
		}



		static CircularCloudLayouter CreateLayouter(int x, int y)
		{
			return new CircularCloudLayouter(new Point(x, y));
		}

		static bool AreIntersectedRectangles(IEnumerable<Rectangle> rectangles)
		{
			return (
				from rectangle1 in rectangles
				from rectangle2 in rectangles
				where rectangle1 != rectangle2
				where rectangle1.IntersectsWith(rectangle2)
				select true).Any();
		}
	}
}