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
		private static readonly Stack<Quad> _quadWarehouse;

		private static readonly object _lock = new object();

		/// <summary>
		/// The color to blend with the final render
		/// </summary>
		private Color _blendColor;

		#endregion

		#region Properties

		/// <summary>
		/// the list of quads to draw
		/// </summary>
		public List<Quad> Quads { get; private set; }

		/// <summary>
		/// Get or set the m_CurrentColor member variable
		/// </summary>
		public Color BlendColor
		{
			get { return _blendColor; }
			set { _blendColor = value; }
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
				_quadWarehouse = new Stack<Quad>();
			}
		}

		/// <summary>
		/// hello, standard constructor!
		/// </summary>
		public DrawList()
		{
			Timer = new CountdownTimer();
			Quads = new List<Quad>();
			BlendColor = Color.White;
			StartAlpha = 255;
			Scale = 1.0f;
		}

		public void Set(float startTime, Color startColor, float scale)
		{
			Flush();
			Timer.Start(startTime);
			BlendColor = startColor;
			StartAlpha = startColor.A;
			Scale = scale;
		}

		/// <summary>
		/// Add a quad to this draw list
		/// </summary>
		/// <param name="image">the id of teh bitmap for this quad</param>
		/// <param name="position">the position to render the upper left at</param>
		/// <param name="primaryColor">the color to tint this quad</param>
		/// <param name="secondaryColor"></param>
		/// <param name="rotation">the amount to rotate this image</param>
		/// <param name="flip">whether or not this image is flipped</param>
		/// <param name="layer">the layer to render the bitmap at</param>
		public void AddQuad(TextureInfo image, 
			Vector2 position,
			Color primaryColor,
			Color secondaryColor,
			float rotation,
			bool flip,
			int layer,
			float scale)
		{
			Quad quad = null;

			lock (_lock)
			{
				//check if there is a quad in the warehouse
				if (_quadWarehouse.Count > 0)
				{
					quad = _quadWarehouse.Pop();
				}
			}

			if (quad == null)
			{
				//otherwise order up a new one
				quad = new Quad();
			}

			quad.Initialize(image, position, primaryColor, secondaryColor, rotation, flip, layer, Quads.Count, scale);
			Quads.Add(quad);
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
		/// <param name="renderer">the renderer to sent it to</param>
		public void Render(IRenderer renderer)
		{
			Sort();
			for (int i = 0; i < Quads.Count; i++)
			{
				Quads[i].Render(BlendColor, renderer, Scale);
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
					if (_quadWarehouse.Count < 100)
					{
						_quadWarehouse.Push(Quads[i]);
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
		/// <param name="time">A clock with the current time.</param>
		/// <returns>bool: true if the drawlist is still alive, false if it is dead</returns>
		public bool Update(GameClock time)
		{
			Debug.Assert(time.TimeDelta >= 0.0f);

			Timer.Update(time);

			//Check if this timer is still alive
			bool alive = IsAlive();
			if (alive)
			{
				//This drawlist is still alive and needs to update the alpha channel

				/*
				alpha channel algo
				255 * (current time / total time) = alpha channel
				*/

				float alpha = StartAlpha * Timer.Lerp();
				_blendColor.A = (byte)alpha;
			}
			else
			{
				//This drawlist is dead!
				_blendColor.A = 0;
			}

			return alive;
		}

		#endregion
	}
}