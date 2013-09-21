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
		#region Member Variables

		private ITexture m_Image;
		private Vector2 m_position;
		private float m_fRotation;
		private bool m_bFlip;
		private Color m_PaletteSwapColor;

		#endregion //Member Variables

		#region Properties

		/// <summary>
		/// Get or set the layer to render at
		/// </summary>
		public int Layer { get; set; }

		/// <summary>
		/// The position of this wuad in the drawlist, quads added later have higher number
		/// If two quads have the same layer, the QuadSort uses this number to sort them out
		/// </summary>
		/// <value>The list position.</value>
		public int ListPosition { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// hello, standard constructor!
		/// </summary>
		/// <param name="UpperLeft">the upper left corner of the quad</param>
		/// <param name="UpperRight">the upper right corner of the quad</param>
		/// <param name="BottomLeft">the bottom left corner of the quad</param>
		/// <param name="BottomRight">the bottom right corner of the quad</param>
		/// <param name="iBmpID">the id of teh bitmap for this quad</param>
		/// <param name="iLayer">the layer to render the bitmpa at</param>
		public Quad(ITexture image, 
			Vector2 position, 
			float fRotation, 
			bool bFlip, 
			int iLayer,
			Color PaletteSwapColor,
			int iListPos)
		{
			Initialize(image, position, fRotation, bFlip, iLayer, PaletteSwapColor, iListPos);
		}

		public void Initialize(ITexture image, 
			Vector2 position, 
			float fRotation, 
			bool bFlip,
			int iLayer,
			Color PaletteSwapColor,
			int iListPos)
		{
			m_Image = image;
			m_position = position;
			m_fRotation = fRotation;
			m_bFlip = bFlip;
			Layer = iLayer;
			m_PaletteSwapColor = PaletteSwapColor;
			ListPosition = iListPos;
		}

		/// <summary>
		/// Render this textured quad
		/// </summary>
		/// <param name="MyRenderer">The renderer to draw this dude on</param>
		public void Render(Color DrawlistColor, IRenderer MyRenderer, float fScale)
		{
			if (m_PaletteSwapColor != Color.White)
			{
				//setup the color
				Vector3 vectColor = DrawlistColor.ToVector3() * m_PaletteSwapColor.ToVector3();
				Color FinalColor = new Color(vectColor);
				FinalColor.A = DrawlistColor.A; //use the alpha value from the drawlist color

				MyRenderer.Draw(
					m_Image,
					m_position,
					FinalColor,
					m_fRotation,
					m_bFlip,
					fScale);
			}
			else
			{
				//Send this guy to the renderer
				MyRenderer.Draw(
					m_Image,
					m_position,
					DrawlistColor,
					m_fRotation,
					m_bFlip,
					fScale);
			}
		}

		#endregion
	}
}