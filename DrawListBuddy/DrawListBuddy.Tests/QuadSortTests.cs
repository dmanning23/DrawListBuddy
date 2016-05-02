using NUnit.Framework;
using System;
using DrawListBuddy;
using Microsoft.Xna.Framework;
using RenderBuddy;

namespace DrawListBuddy.Tests
{
	[TestFixture()]
	public class QuadSortTests
	{
		#region init

		DrawListBuddy.DrawList dude;

		[SetUp]
		public void Setup()
		{
			dude = new DrawListBuddy.DrawList();
		}

		#endregion //init

		#region Sort

		[Test()]
		public void NotSorted()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);

			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("first", first.Name);
			Assert.AreEqual("second", second.Name);
			Assert.AreEqual("third", third.Name);
		}

		[Test()]
		public void NotSorted1()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 2);
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 1);
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);

			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("first", first.Name);
			Assert.AreEqual("second", second.Name);
			Assert.AreEqual("third", third.Name);
		}

		[Test()]
		public void SameLayers()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);

			dude.Sort();
			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("first", first.Name);
			Assert.AreEqual("second", second.Name);
			Assert.AreEqual("third", third.Name);
		}

		[Test()]
		public void DifferentLayers()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 4);
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 2);
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 0);

			dude.Sort();
			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("third", first.Name);
			Assert.AreEqual("second", second.Name);
			Assert.AreEqual("first", third.Name);
		}

		[Test()]
		public void OutOfOrder()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, -2);
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 5);
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 10);

			dude.Sort();
			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("first", first.Name);
			Assert.AreEqual("second", second.Name);
			Assert.AreEqual("third", third.Name);
		}

		[Test()]
		public void OutOfOrder2()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, -20);
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 10);
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, -5);

			dude.Sort();
			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("first", first.Name);
			Assert.AreEqual("third", second.Name);
			Assert.AreEqual("second", third.Name);
		}

		[Test()]
		public void TwoSameLayer()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, -20); //drawn first because it is lowest number
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 10); //drawn second because layers match and added first
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 10);

			dude.Sort();
			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("first", first.Name);
			Assert.AreEqual("second", second.Name);
			Assert.AreEqual("third", third.Name);
		}

		[Test()]
		public void TwoSameLayer1()
		{
			dude.AddQuad(new TestTexture("first"), Vector2.Zero, Color.White, Color.White, 0.0f, false, -10);
			dude.AddQuad(new TestTexture("second"), Vector2.Zero, Color.White, Color.White, 0.0f, false, -10);
			dude.AddQuad(new TestTexture("third"), Vector2.Zero, Color.White, Color.White, 0.0f, false, 10);

			dude.Sort();
			TestTexture first = dude.Quads[0].Image as TestTexture;
			TestTexture second = dude.Quads[1].Image as TestTexture;
			TestTexture third = dude.Quads[2].Image as TestTexture;
			Assert.AreEqual("first", first.Name);
			Assert.AreEqual("second", second.Name);
			Assert.AreEqual("third", third.Name);
		}

		#endregion Sort
	}
}
