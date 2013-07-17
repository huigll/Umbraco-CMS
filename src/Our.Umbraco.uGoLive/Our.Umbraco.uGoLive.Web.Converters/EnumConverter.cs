using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
namespace Our.Umbraco.uGoLive.Web.Converters
{
	public class EnumConverter<T> : JavaScriptConverter
	{
		private static readonly Type[] _supportedTypes = new Type[]
		{
			typeof(T)
		};
		public override IEnumerable<Type> SupportedTypes
		{
			get
			{
				return EnumConverter<T>._supportedTypes;
			}
		}
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			object result;
			if (type == typeof(T))
			{
				result = Enum.Parse(typeof(T), serializer.ConvertToType<string>(dictionary["Value"]));
			}
			else
			{
				result = null;
			}
			return result;
		}
		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			IDictionary<string, object> result;
			if (obj is T)
			{
				result = new Dictionary<string, object>
				{

					{
						"Value",
						obj.ToString()
					}
				};
			}
			else
			{
				result = new Dictionary<string, object>();
			}
			return result;
		}
	}
}
