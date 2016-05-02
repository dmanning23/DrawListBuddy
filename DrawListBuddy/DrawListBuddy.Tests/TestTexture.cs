using RenderBuddy;

namespace DrawListBuddy.Tests
{
	/// <summary>
	/// This thing is a texture with a string. used only for testing
	/// </summary>
	public class TestTexture : TextureInfo
	{
		public string Name { get; set; }

		public new int Width
		{
			get
			{
				return 256;
			}
		}

		public new int Height
		{
			get
			{
				return 256;
			}
		}
		
		public TestTexture(string strName)
		{
			Name = strName;
		}

		public TestTexture()
		{
		}
	}
}