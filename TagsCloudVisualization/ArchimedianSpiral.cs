using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;


namespace TagsCloudVisualization
{
	class ArchimedianSpiral : ISpiral
	{
		private readonly double angleShift;
		private readonly double size;
		private double currentAngle;
		private Point center;

		public ArchimedianSpiral(double angleShift = PI / 180, double size = 10, Point center = default(Point))
		{
			this.angleShift = angleShift;
			this.size = size;
			this.center = center;
		}

		public Point GetNextPoint()
		{
			var nextPoint = GetCartensianCoordinates(size * currentAngle, currentAngle);
			currentAngle += angleShift;
			nextPoint.Offset(center);
			return nextPoint;
		}

		public void Reset() // Не используется!
		{
			currentAngle = 0;
		}

		static Point GetCartensianCoordinates(double polarRadius, double polarAngle) // Опечатка в названии метода. Чтобы такого избегать, можно пользоваться различными плагинами, например для Resharper есть Respeller
		{
			return new Point(
				(int) (polarRadius * Cos(polarRadius)),
				(int) (polarRadius * Sin(polarAngle)));
		}
	}
}