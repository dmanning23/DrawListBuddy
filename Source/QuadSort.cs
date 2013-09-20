using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawListBuddy
{
	/// <summary>
	/// class for sorting the quads by layer
	/// </summary>
	class QuadSort<T> : IComparer<Quad<T>>
	{
		public int Compare(Quad<T> quad1, Quad<T> quad2)
		{
			if (quad2.Layer != quad1.Layer)
			{
				return quad2.Layer.CompareTo(quad1.Layer);
			}
			else
			{
				return quad2.ListPosition.CompareTo(quad1.ListPosition);
			}
		}
	}
}