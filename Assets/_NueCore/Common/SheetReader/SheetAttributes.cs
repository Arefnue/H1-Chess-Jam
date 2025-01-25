using System;

namespace _NueCore.Common.SheetReader
{
	[AttributeUsage(AttributeTargets.Field)]
    public class SheetContentAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Field)]
	public class SheetPageAttribute : Attribute
	{
		public readonly string name;

		public SheetPageAttribute(string name)
		{
			this.name = name;
		}
	}
}
