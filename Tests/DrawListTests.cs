using NUnit.Framework;
using System;
using DrawListBuddy;
using Microsoft.Xna.Framework;
using RenderBuddy;

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
	}
}
