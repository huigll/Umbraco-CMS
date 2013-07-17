using System;
namespace Our.Umbraco.uGoLive.Attribution
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class CheckAttribute : Attribute
	{
		public Guid Id
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			private set;
		}
		public string Description
		{
			get;
			private set;
		}
		public string Group
		{
			get;
			private set;
		}
		public CheckAttribute(string id, string name, string description) : this(id, name, description, "Core")
		{
		}
		public CheckAttribute(string id, string name, string description, string group)
		{
			this.Id = new Guid(id);
			this.Name = name;
			this.Description = description;
			this.Group = group;
		}
	}
}
