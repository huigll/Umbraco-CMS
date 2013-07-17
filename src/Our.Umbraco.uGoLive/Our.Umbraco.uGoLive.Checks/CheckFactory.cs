using System;
using System.Collections.Generic;
using umbraco.BusinessLogic.Utils;
namespace Our.Umbraco.uGoLive.Checks
{
	public class CheckFactory
	{
		public static readonly IList<ICheck> Checks;
		static CheckFactory()
		{
			CheckFactory.Checks = new List<ICheck>();
			CheckFactory.RegisterChecks();
		}
		private static void RegisterChecks()
		{
			List<Type> list = TypeFinder.FindClassesOfType<ICheck>(true);
			foreach (Type current in list)
			{
				ICheck check = Activator.CreateInstance(current) as ICheck;
				if (check != null)
				{
					CheckFactory.Checks.Add(check);
				}
			}
		}
	}
}
