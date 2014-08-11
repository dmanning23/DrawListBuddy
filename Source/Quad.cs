using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderBuddy;

namespace DrawListBuddy
{
	/// <summary>
	/// objects used to sort images in a draw list before they are actually rendered
	/// </summary>
	public class Quad
	{
		#region Properties

		public ITexture Image { get; private set; }

		public Vector2 Position { get; private set; }

		public float Rotation { get; private set; }

		public bool Flip { get; private set; }

		/// <summary>
		/// Get or set the layer to render at
		/// </summary>
		public int Layer { get; private set; }

		public Color PaletteSwapColor { get; private set; }

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
		/// <param name="paletteSwapColor">the color to tint this quad</param>
		/// <param name="fRotation">the amount to rotate this image</param>
		/// <param name="bFlip">whether or not this image is flipped</param>
		/// <param name="iLayer">the layer to render the bitmap at</param>
		public Quad(ITexture image, 
		            Vector2 position, 
		            Color paletteSwapColor,
		            float fRotation, 
		            bool bFlip, 
		            int iLayer,
		            int iListPos)
		{
			Initialize(image, position, paletteSwapColor, fRotation, bFlip, iLayer, iListPos);
		}

		/// <summary>
		/// Set all the items of this quad
		/// </summary>
		/// <param name="image">the id of teh bitmap for this quad</param>
		/// <param name="position">the position to render the upper left at</param>
		/// <param name="paletteSwapColor">the color to tint this quad</param>
		/// <param name="fRotation">the amount to rotate this image</param>
		/// <param name="bFlip">whether or not this image is flipped</param>
		/// <param name="iLayer">the layer to render the bitmap at</param>
		public void Initialize(ITexture image, 
		                       Vector2 position, 
		                       Color paletteSwapColor,
		                       float fRotation, 
		                       bool bFlip,
		                       int iLayer,
		                       int iListPos)
		{
			Image = image;
			Position = position;
			Rotation = fRotation;
			Flip = bFlip;
			Layer = iLayer;
			PaletteSwapColor = paletteSwapColor;
			ListPosition = iListPos;
		}

		/// <summary>
		/// Given a drawlist color and the palette swap color, get the final color to use
		/// </summary>
		/// <returns>The color to drwa the image</returns>
		/// <param name="DrawlistColor">Drawlist color.</param>
		public Color FinalColor(Color DrawlistColor)
		{
			if (PaletteSwapColor != Color.White)
			{
				//We need to mix the palette and drawlist colors
				Vector3 vectColor = DrawlistColor.ToVector3() * PaletteSwapColor.ToVector3();
				Color FinalColor = new Color(vectColor);
				FinalColor.A = DrawlistColor.A; //use the alpha value from the drawlist color
				return FinalColor;
			}
			else
			{
				return DrawlistColor;
			}
		}

		/// <summary>
		/// Render this textured quad
		/// </summary>
		/// <param name="MyRenderer">The renderer to draw this dude on</param>
		public void Render(Color DrawlistColor, IRenderer MyRenderer, float fScale)
		{
			MyRenderer.Draw(
				Image,
				Position,
				FinalColor(DrawlistColor),
				Rotation,
				Flip,
				fScale);
		}

		#endregion
	}
}