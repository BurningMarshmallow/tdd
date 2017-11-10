using System;
using static System.Math;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public void Reset()
		{
			currentAngle = 0;
		}

		static Point GetCartensianCoordinates(double polarRadius, double polarAngle)
		{
			return new Point(
				(int) (polarRadius * Cos(polarRadius)),
				(int) (polarRadius * Sin(polarAngle)));
		}
	}
}