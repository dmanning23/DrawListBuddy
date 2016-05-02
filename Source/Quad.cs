using Microsoft.Xna.Framework;
using RenderBuddy;

namespace DrawListBuddy
{
	/// <summary>
	/// objects used to sort images in a draw list before they are actually rendered
	/// </summary>
	public class Quad
	{
		#region Properties

		public TextureInfo Image { get; private set; }

		public Vector2 Position { get; private set; }

		public float Rotation { get; private set; }

		public bool Flip { get; private set; }

		/// <summary>
		/// Get or set the layer to render at
		/// </summary>
		public int Layer { get; private set; }

		public Color PrimaryColor { get; private set; }

		public Color SecondaryColor { get; private set; }

		/// <summary>
		/// The position of this wuad in the drawlist, quads added later have higher number
		/// If two quads have the same layer, the QuadSort uses this number to sort them out
		/// </summary>
		/// <value>The list position.</value>
		public int ListPosition { get; private set; }

		#endregion //Properties

		#region Methods

		public Quad()
		{
		}

		/// <summary>
		/// hello, standard constructor!
		/// </summary>
		/// <param name="image">the id of teh bitmap for this quad</param>
		/// <param name="position">the position to render the upper left at</param>
		/// <param name="primaryColor">the color to tint this quad</param>
		/// <param name="secondaryColor"></param>
		/// <param name="rotation">the amount to rotate this image</param>
		/// <param name="flip">whether or not this image is flipped</param>
		/// <param name="layer">the layer to render the bitmap at</param>
		/// <param name="listPos"></param>
		public Quad(TextureInfo image,
			Vector2 position, 
			Color primaryColor,
			Color secondaryColor,
			float rotation, 
			bool flip, 
			int layer,
			int listPos)
		{
			Initialize(image, position, primaryColor, secondaryColor, rotation, flip, layer, listPos);
		}

		/// <summary>
		/// Set all the items of this quad
		/// </summary>
		/// <param name="image">the id of teh bitmap for this quad</param>
		/// <param name="position">the position to render the upper left at</param>
		/// <param name="primaryColor">the color to tint this quad</param>
		/// <param name="secondaryColor"></param>
		/// <param name="rotation">the amount to rotate this image</param>
		/// <param name="flip">whether or not this image is flipped</param>
		/// <param name="layer">the layer to render the bitmap at</param>
		/// <param name="listPos"></param>
		public void Initialize(TextureInfo image, 
			Vector2 position,
			Color primaryColor,
			Color secondaryColor,
			float rotation,
			bool flip,
			int layer,
			int listPos)
		{
			Image = image;
			Position = position;
			Rotation = rotation;
			Flip = flip;
			Layer = layer;
			PrimaryColor = primaryColor;
			SecondaryColor = secondaryColor;
			ListPosition = listPos;
		}

		/// <summary>
		/// Given a drawlist color and the palette swap color, get the final color to use
		/// </summary>
		/// <returns>The color to drwa the image</returns>
		/// <param name="drawlistColor">Drawlist color.</param>
		/// <param name="combineColor">the color to mix with the drawlist color</param>
		public Color FinalColor(Color drawlistColor, Color combineColor)
		{
			if (combineColor != Color.White)
			{
				//We need to mix the palette and drawlist colors
				Vector3 vectColor = drawlistColor.ToVector3() * combineColor.ToVector3();
				Color finalColor = new Color(vectColor);
				finalColor.A = drawlistColor.A; //use the alpha value from the drawlist color
				return finalColor;
			}
			else
			{
				return drawlistColor;
			}
		}

		/// <summary>
		/// Render this textured quad
		/// </summary>
		public void Render(Color drawlistColor, IRenderer renderer, float scale)
		{
			renderer.Draw(
				Image,
				Position,
				FinalColor(drawlistColor, PrimaryColor),
				FinalColor(drawlistColor, SecondaryColor),
				Rotation,
				Flip,
				scale);
		}

		#endregion
	}
}