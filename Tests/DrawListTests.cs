using NUnit.Framework;
using System;
using DrawListBuddy;
using Microsoft.Xna.Framework;
using RenderBuddy;
using GameTimer;

namespace DrawList.Tests
{
	[TestFixture()]
	public class DrawListTest
	{
		#region init

		DrawListBuddy.DrawList dude;

		[SetUp]
		public void Setup()
		{
			dude = new DrawListBuddy.DrawList();
		}

		#endregion //init

		#region Defaults

		[Test()]
		public void DefaultStartAlpha()
		{
			Assert.AreEqual(255.0f, dude.StartAlpha);
		}

		[Test()]
		public void DefaultQuadList()
		{
			Assert.IsNotNull(dude.Quads);
		}

		[Test()]
		public void DefaultStartScale()
		{
			Assert.AreEqual(1.0f, dude.Scale);
		}

		[Test()]
		public void DefaultStartColor()
		{
			Assert.AreEqual(Color.White, dude.CurrentColor);
		}

		#endregion //Defaults

		#region Set

		[Test()]
		public void SetStartAlpha()
		{
			dude.Set(0.0f, Color.White, 1.0f);
			Assert.AreEqual(255.0f, dude.StartAlpha);
		}

		[Test()]
		public void SetStartAlpha1()
		{
			dude.Set(0.0f, new Color(Color.White, 1.0f), 1.0f);
			Assert.AreEqual(255.0f, dude.StartAlpha);
		}

		[Test()]
		public void SetStartAlpha2()
		{
			dude.Set(0.0f, new Color(Color.White, 0.5f), 1.0f);
			Assert.AreEqual(255 / 2, dude.StartAlpha);
		}

		[Test()]
		public void SetStartScale()
		{
			dude.Set(0.0f, Color.White, 1.0f);
			Assert.AreEqual(1.0f, dude.Scale);
		}

		[Test()]
		public void SetStartScale1()
		{
			dude.Set(0.0f, Color.White, 0.5f);
			Assert.AreEqual(0.5f, dude.Scale);
		}

		[Test()]
		public void SetStartColor()
		{
			dude.Set(0.0f, Color.White, 1.0f);
			Assert.AreEqual(Color.White, dude.CurrentColor);
		}

		[Test()]
		public void SetStartColor1()
		{
			dude.Set(0.0f, Color.Aqua, 1.0f);
			Assert.AreEqual(Color.Aqua, dude.CurrentColor);
		}

		[Test()]
		public void SetStartColor2()
		{
			Color StartColor = new Color(Color.White, 0.5f);
			dude.Set(0.0f, StartColor, 1.0f);
			Assert.AreEqual(StartColor, dude.CurrentColor);
		}

		#endregion //Set

		#region IsAlive tests

		[Test()]
		public void IsAliveDefault()
		{
			Assert.IsFalse(dude.IsAlive());
		}

		[Test()]
		public void IsAliveSetNoTime()
		{
			dude.Set(0.0f, Color.White, 1.0f);
			Assert.IsFalse(dude.IsAlive());
		}

		[Test()]
		public void IsAliveSetWithTime()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			Assert.IsTrue(dude.IsAlive());
		}

		[Test()]
		public void IsAliveUpdated()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 2.0f;
			time.TimeDelta = 0.5f;
			Assert.IsTrue(dude.Update(time));
		}

		[Test()]
		public void IsAliveUpdated1()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 2.0f;
			time.TimeDelta = 0.5f;
			dude.Update(time);
			Assert.IsTrue(dude.IsAlive());
		}

		[Test()]
		public void IsAliveNoTime()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 2.0f;
			time.TimeDelta = 1.0f;
			Assert.IsFalse(dude.Update(time));
		}

		[Test()]
		public void IsAliveNoTime1()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 2.0f;
			time.TimeDelta = 1.0f;
			dude.Update(time);
			Assert.IsFalse(dude.IsAlive());
		}

		[Test()]
		public void IsAliveWayOutOfTime()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 20.0f;
			time.TimeDelta = 10.0f;
			Assert.IsFalse(dude.Update(time));
		}

		[Test()]
		public void IsAliveWayOutOfTime1()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 20.0f;
			time.TimeDelta = 10.0f;
			dude.Update(time);
			Assert.IsFalse(dude.IsAlive());
		}

		#endregion //IsAlive tests

		#region Update tests

		[Test()]
		public void UpdateAlpha()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 2.0f;
			time.TimeDelta = 0.5f;
			dude.Update(time);
			Assert.AreEqual(127, dude.CurrentColor.A);
		}

		[Test()]
		public void UpdateAlphaAndDie()
		{
			dude.Set(1.0f, Color.White, 1.0f);
			GameClock time = new GameClock();
			time.CurrentTime = 4.0f;
			time.TimeDelta = 2.5f;
			dude.Update(time);
			Assert.AreEqual(0, dude.CurrentColor.A);
		}

		#endregion //Update tests
	}
}
