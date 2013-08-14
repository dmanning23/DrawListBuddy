using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawListBuddy
{
	/// <summary>
	/// class for sorting the quads by layer
	/// </summary>
	class QuadSort : IComparer<Quad>
	{
		public int Compare(Quad quad1, Quad quad2)
		{
			return quad2.Layer.CompareTo(quad1.Layer);
		}
	}
}