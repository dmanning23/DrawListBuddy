using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameTimer;
using RenderBuddy;

namespace DrawListBuddy
{
	public class DrawList
	{
		#region Member Variables

		/// <summary>
		/// warehouse of quads
		/// </summary>
		private static Stack<Quad> g_listQuadWarehouse;

		/// <summary>
		/// The color to render with
		/// </summary>
		private Color m_CurrentColor;

		#endregion

		#region Properties

		/// <summary>
		/// the list of quads to draw
		/// </summary>
		public List<Quad> Quads { get; private set; }

		/// <summary>
		/// Get or set the m_CurrentColor member variable
		/// </summary>
		public Color CurrentColor
		{
			get { return m_CurrentColor; }
			set { m_CurrentColor = value; }
		}

		/// <summary>
		/// the alpha transparency to start with
		/// </summary>
		public float StartAlpha { get; set; }

		/// <summary>
		/// This is an optional timer for this drawlist
		/// </summary>
		public CountdownTimer Timer { get; private set; }

		/// <summary>
		/// The scale to render this dudes stuff at
		/// </summary>
		/// <value>The scale.</value>
		public float Scale { get; set; }

		#endregion

		#region Methods

		static DrawList()
		{
			g_listQuadWarehouse = new Stack<Quad>();
		}

		/// <summary>
		/// hello, standard constructor!
		/// </summary>
		public DrawList()
		{
			Timer = new CountdownTimer();
			Quads = new List<Quad>();
			m_CurrentColor = Color.White;
			StartAlpha = 255;
			Scale = 1.0f;
		}

		public void Set(float fStartTime, Color fStartColor, float fScale)
		{
			Flush();
			Timer.Start(fStartTime);
			m_CurrentColor = fStartColor;
			StartAlpha = fStartColor.A;
			Scale = fScale;
		}

		/// <summary>
		/// Add a quad to this draw list
		/// </summary>
		/// <param name="image">the id of teh bitmap for this quad</param>
		/// <param name="position">the position to render the upper left at</param>
		/// <param name="paletteSwapColor">the color to tint this quad</param>
		/// <param name="fRotation">the amount to rotate this image</param>
		/// <param name="bFlip">whether or not this image is flipped</param>
		/// <param name="iLayer">the layer to render the bitmap at</param>
		public void AddQuad(ITexture image, 
		                    Vector2 position, 
		                    Color paletteSwapColor, 
		                    float fRotation, 
		                    bool bFlip, 
		                    int iLayer)
		{
			//check if there is a quad in the warehouse
			if (g_listQuadWarehouse.Count > 0)
			{
				Quad myQuad = g_listQuadWarehouse.Pop();
				myQuad.Initialize(image, position, paletteSwapColor, fRotation, bFlip, iLayer, Quads.Count);
				Quads.Add(myQuad);
			}
			else
			{
				//otherwise order up a new one
				Quads.Add(new Quad(image, position, paletteSwapColor, fRotation, bFlip, iLayer, Quads.Count));
			}
		}

		/// <summary>
		/// Sort the quads in this drwalist
		/// </summary>
		public void Sort()
		{
			Quads.Sort(new QuadSort());
		}

		/// <summary>
		/// Render the draw list!
		/// </summary>
		/// <param name="MyRenderer">the renderer to sent it to</param>
		public void Render(IRenderer MyRenderer)
		{
			Sort();
			for (int i = 0; i < Quads.Count; i++)
			{
				Quads[i].Render(m_CurrentColor, MyRenderer, Scale);
			}
		}

		/// <summary>
		/// Flush out the quads in this guy's list
		/// </summary>
		public void Flush()
		{
			//push all the existing quads into the warehouse
			for (int i = 0; i < Quads.Count; i++)
			{
				if (g_listQuadWarehouse.Count < 100)
				{
					g_listQuadWarehouse.Push(Quads[i]);
				}
				else
				{
					break;
				}
			}

			Quads.Clear();
		}

		/// <summary>
		/// Update the drawlist.
		/// </summary>
		/// <param name="rClock">R clock.</param>
		/// <returns>bool: true if the drawlist is still alive, false if it is dead</returns>
		public bool Update(GameClock rClock)
		{
			Debug.Assert(rClock.TimeDelta >= 0.0f);

			Timer.Update(rClock);

			if (0.0f >= Timer.RemainingTime())
			{
				//This drawlist is dead!
				m_CurrentColor.A = 0;
				return false;
			}
			else
			{
				//update the alpha channel

				/*
				alpha channel algo
				255 * (current time / total time) = alpha channel
				*/

				//now that we have the current time delta, multiply the alpha by that
				float fCurAlpha = StartAlpha * Timer.Lerp();
				m_CurrentColor.A = (byte)fCurAlpha;
			}

			//This drawlist is still alive and needs to be updated again next frame
			return true;
		}

		#endregion
	}
}