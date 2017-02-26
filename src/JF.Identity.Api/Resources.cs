
namespace Identity.Api
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Identity.Api.Resources", typeof(Resources).GetTypeInfo().Assembly);

		internal static string DefaultError => GetString(nameof(DefaultError));
		internal static string EntityNotFound (params string[] p0) => string.Format(CultureInfo.CurrentCulture, GetString(nameof(EntityNotFound)), p0);
		internal static string ModelNotValid => GetString(nameof(ModelNotValid));
		internal static string SignUpNotOpen => GetString(nameof(SignUpNotOpen));
   
        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}
