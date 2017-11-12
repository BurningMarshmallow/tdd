using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	static class Program // Здесь лучше проставить явно public
	{
		static void Main() // И здесь
		{
			var cloud1 = GetRandomCloudImage(100);
			var cloud2 = GetRandomCloudImage(200);;
			cloud1.Save("cloud1.jpg", ImageFormat.Jpeg);
			cloud2.Save("cloud2.jpg", ImageFormat.Jpeg);

		}

		public static IEnumerable<Rectangle> GetRandomCloud(int rectanglesAmount)
		{
			var rand = new Random();
			var layouter = new CircularCloudLayouter(new Point(750, 750));
			for (int i = 0; i < rectanglesAmount; i++)
			{
				yield return layouter.PutNextRectangle(rand.Next(100, 150), rand.Next(45, 65));
			}
		}

		public static Bitmap GetRandomCloudImage(int rectanglesAmount)
		{
			return Visualization.GetVisualisation(GetRandomCloud(rectanglesAmount)); // Теперь называется Visualization (в методе и .csproj надо тоже поправить)
		}
	}
}
