using NUnit.Framework;
using System;
using DrawListBuddy;
using Microsoft.Xna.Framework;
using RenderBuddy;

namespace DrawListBuddy.Tests
{
	[TestFixture()]
	public class QuadTests
	{
		#region init

		DrawListBuddy.DrawList dude;

		[SetUp]
		public void Setup()
		{
			dude = new DrawListBuddy.DrawList();
		}

		#endregion //init

		#region AddQuad

		[Test()]
		public void QuadAdded()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Assert.AreEqual(1, dude.Quads.Count);
		}

		[Test()]
		public void QuadPosition()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(Vector2.Zero, myDude.Position);
		}

		[Test()]
		public void QuadPosition1()
		{
			Vector2 vect = new Vector2(10.0f, 20.0f);
			dude.AddQuad(new TextureInfo(), vect, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(vect, myDude.Position);
		}

		[Test()]
		public void QuadColor()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(Color.White, myDude.SecondaryColor);
		}

		[Test()]
		public void QuadColor1()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.Aqua, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(Color.Aqua, myDude.PrimaryColor);
		}

		[Test()]
		public void QuadColor2()
		{
			Color testdude = new Color(Color.AntiqueWhite, 0.5f);
			dude.AddQuad(new TextureInfo(), Vector2.Zero, testdude, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(testdude, myDude.PrimaryColor);
		}

		[Test()]
		public void QuadRotation()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(0.0f, myDude.Rotation);
		}

		[Test()]
		public void QuadRotation1()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.Aqua, Color.White, 50.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(50.0f, myDude.Rotation);
		}

		[Test()]
		public void QuadFlip()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(false, myDude.Flip);
		}

		[Test()]
		public void QuadFlip1()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.Aqua, Color.White, 0.0f, true, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(true, myDude.Flip);
		}

		[Test()]
		public void QuadLevel()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(0, myDude.Layer);
		}

		[Test()]
		public void QuadLevel1()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.Aqua, Color.White, 0.0f, false, 10, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(10, myDude.Layer);
		}

		[Test()]
		public void QuadTwoAdded()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Assert.AreEqual(2, dude.Quads.Count);
		}

		[Test()]
		public void QuadListPos()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(0, myDude.ListPosition);
		}

		[Test()]
		public void QuadListPos1()
		{
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			dude.AddQuad(new TextureInfo(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[1];
			Assert.AreEqual(1, myDude.ListPosition);
		}

		#endregion //AddQuad

		#region QuadColor

		[Test()]
		public void QuadColorBothDefault()
		{
			dude.AddQuad(new TestTexture(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(Color.White, myDude.FinalColor(Color.White, dude.BlendColor));
		}

		[Test()]
		public void QuadColorDefault()
		{
			dude.Set(0.0f, Color.Red, 1.0f);
			dude.AddQuad(new TestTexture(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(Color.Red, myDude.FinalColor(Color.White, dude.BlendColor));
		}

		[Test()]
		public void QuadColorDefault1()
		{
			Color myColor = new Color(Color.Red, 0.5f);
			dude.Set(0.0f, myColor, 1.0f);
			dude.AddQuad(new TestTexture(), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(myColor, myDude.FinalColor(dude.BlendColor, Color.White));
		}

		[Test()]
		public void UsesDrawlistAlpha()
		{
			Color myColor = new Color(Color.Red, 0.5f);
			dude.Set(0.0f, myColor, 1.0f);
			dude.AddQuad(new TestTexture(), Vector2.Zero, Color.Blue, Color.White, 0.0f, false, 0, 1f);
			Quad myDude = dude.Quads[0];
			Assert.AreEqual(myColor.A, myDude.FinalColor(dude.BlendColor, Color.White).A);
		}

		#endregion //QuadColor
	}
}
