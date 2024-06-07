namespace FuStudy_Service.Service;

using System.Globalization;
using System.Text.RegularExpressions;

public static class PluralizationService
{
    private static readonly Regex EndWithES = new Regex("es$", RegexOptions.Compiled);
    private static readonly Regex EndWithS = new Regex("s$", RegexOptions.Compiled);

    public static string Pluralize(string name)
    {
        if (EndWithES.IsMatch(name) || EndWithS.IsMatch(name))
        {
            return name;
        }
        return name + "s";
    }
}
