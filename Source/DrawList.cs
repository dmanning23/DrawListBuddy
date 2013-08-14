using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using GameTimer;

namespace DrawListBuddy
{
	public class DrawList
	{
		#region Member Variables

		//This is an optional timer for this drawlist
		private CountdownTimer m_Timer;

		//warehouse of quads
		private static Stack<Quad> g_listQuadWarehouse;

		//the list of quads to draw
		private List<Quad> m_listQuads;

		//The color to render with
		private Color m_CurrentColor;

		//the alpha transparency to start with
		private float m_fStartAlpha;

		private float m_fScale;

		#endregion

		#region Properties

		/// <summary>
		/// Get or set the m_CurrentColor member variable
		/// </summary>
		public Color CurrentColor
		{
			get { return m_CurrentColor; }
			set { m_CurrentColor = value; }
		}

		/// <summary>
		/// Get or set the m_fAlpha member variable
		/// </summary>
		public float StartAlpha
		{
			get { return m_fStartAlpha; }
			set { m_fStartAlpha = value; }
		}

		/// <summary>
		/// Get or set the m_Timer member variable
		/// </summary>
		public CountdownTimer Timer
		{
			get { return m_Timer; }
			set { m_Timer = value; }
		}

		public float Scale
		{
			get { return m_fScale; }
			set { m_fScale = value; }
		}

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
			m_Timer = new CountdownTimer();
			m_listQuads = new List<Quad>();
			m_CurrentColor = Color.White;
			m_fStartAlpha = 255;
		}

		public void Set(float fStartTime, Color fStartColor, float fScale)
		{
			Flush();
			m_Timer.Start(fStartTime);
			m_CurrentColor = fStartColor;
			m_fStartAlpha = fStartColor.A;
			m_fScale = fScale;
		}

		/// <summary>
		/// Add a quad to this draw list
		/// </summary>
		/// <param name="UpperLeft">the upper left corner of the quad</param>
		/// <param name="UpperRight">the upper right corner of the quad</param>
		/// <param name="BottomLeft">the bottom left corner of the quad</param>
		/// <param name="BottomRight">the bottom right corner of the quad</param>
		/// <param name="iBmpID">the id of teh bitmap for this quad</param>
		/// <param name="iLayer">the layer to render the bitmap at</param>
		public void AddQuad(int iImageID, 
			Vector2 position, 
			Color color, 
			float fRotation, 
			bool bFlip, 
			int iLayer)
		{
			//check if there is a quad in the warehouse
			if (g_listQuadWarehouse.Count > 0)
			{
				Quad myQuad = g_listQuadWarehouse.Pop();
				myQuad.Initialize(iImageID, position, fRotation, bFlip, iLayer, color);
				m_listQuads.Add(myQuad);
			}
			else
			{
				//otherwise order up a new one
				m_listQuads.Add(new Quad(iImageID, position, fRotation, bFlip, iLayer, color));
			}
		}

		/// <summary>
		/// Render the draw list!
		/// </summary>
		/// <param name="MyRenderer">the renderer to sent it to</param>
		public void Render(IRenderer MyRenderer)
		{
			m_listQuads.Sort(new QuadSort());
			for (int i = 0; i < m_listQuads.Count; i++)
			{
				m_listQuads[i].Render(m_CurrentColor, MyRenderer, m_fScale);
			}
		}

		/// <summary>
		/// Flush out the quads in this guy's list
		/// </summary>
		public void Flush()
		{
			//push all the existing quads into the warehouse
			for (int i = 0; i < m_listQuads.Count; i++)
			{
				if (g_listQuadWarehouse.Count < 100)
				{
					g_listQuadWarehouse.Push(m_listQuads[i]);
				}
				else
				{
					break;
				}
			}

			m_listQuads.Clear();
		}

		public bool Update(GameClock rClock)
		{
			Debug.Assert(rClock.TimeDelta >= 0.0f);

			m_Timer.Update(rClock);

			if (0.0f >= m_Timer.RemainingTime())
			{
				return true;
			}
			else
			{
				//update the alpha channel

				/*
				alpha channel algo
				255 * (current time / total time) = alpha channel
				*/
				float fCurTime = m_Timer.RemainingTime();
				float fEndTime = m_Timer.CountdownLength;
				Debug.Assert(fCurTime <= fEndTime);
				float fCurDelta = fCurTime / fEndTime;

				//now that we have the current time delta, multiply the alpha by that
				float fCurAlpha = m_fStartAlpha * fCurDelta;

				//Debug.Assert(fCurAlpha <= 255.0f);
				//Debug.Assert(fCurAlpha > 0.0f);
				//Debug.Assert(fCurAlpha <= (float)CurrentColor.A);

				m_CurrentColor.A = (byte)fCurAlpha;
			}

			return false;
		}

		#endregion
	}
}