using GameTimer;
using Microsoft.Xna.Framework;
using RenderBuddy;
using System.Collections.Generic;
using System.Diagnostics;

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

		private static readonly object _lock = new object();

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
			lock (_lock)
			{
				g_listQuadWarehouse = new Stack<Quad>();
			}
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
			Quad myQuad = null;

			lock (_lock)
			{
				//check if there is a quad in the warehouse
				if (g_listQuadWarehouse.Count > 0)
				{
					myQuad = g_listQuadWarehouse.Pop();
				}
			}

			if (myQuad == null)
			{
				//otherwise order up a new one
				myQuad = new Quad();
			}

			myQuad.Initialize(image, position, paletteSwapColor, fRotation, bFlip, iLayer, Quads.Count);
			Quads.Add(myQuad);
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
			lock (_lock)
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
			}

			Quads.Clear();
		}

		/// <summary>
		/// Determines whether this instance is alive.
		/// </summary>
		/// <returns><c>true</c> if this instance is alive; otherwise, <c>false</c>.</returns>
		public bool IsAlive()
		{
			return (0 < Timer.RemainingTime());
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

			//Check if this timer is still alive
			bool alive = IsAlive();
			if (alive)
			{
				//This drawlist is still alive and needs to update the alpha channel

				/*
				alpha channel algo
				255 * (current time / total time) = alpha channel
				*/

				float fCurAlpha = StartAlpha * Timer.Lerp();
				m_CurrentColor.A = (byte)fCurAlpha;
			}
			else
			{
				//This drawlist is dead!
				m_CurrentColor.A = 0;
			}

			return alive;
		}

		#endregion
	}
}