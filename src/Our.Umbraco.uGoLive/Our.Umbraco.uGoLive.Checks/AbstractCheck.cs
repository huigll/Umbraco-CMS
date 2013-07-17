using Our.Umbraco.uGoLive.Attribution;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Our.Umbraco.uGoLive.Checks
{
	public abstract class AbstractCheck : ICheck
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
		public virtual bool CanRectify
		{
			get
			{
				return false;
			}
		}
		protected AbstractCheck()
		{
			IEnumerable<CheckAttribute> source = base.GetType().GetCustomAttributes(typeof(CheckAttribute), true).OfType<CheckAttribute>();
			if (!source.Any<CheckAttribute>())
			{
				throw new InvalidOperationException(string.Format("The Check of type {0} is missing the {1} attribute", base.GetType().FullName, typeof(CheckAttribute).FullName));
			}
			CheckAttribute checkAttribute = source.First<CheckAttribute>();
			this.Id = checkAttribute.Id;
			this.Name = checkAttribute.Name;
			this.Description = checkAttribute.Description;
			this.Group = checkAttribute.Group;
		}
		public abstract CheckResult Check();
		public virtual RectifyResult Rectify()
		{
			throw new ApplicationException("Check does not support automatic rectifying.");
		}
	}
}
