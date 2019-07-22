using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace VSExtension.TeamExplorer.GitCreateBranch
{
  public class StringHandler : IMultiValueConverter
  {
    public static string StringInputChecker(string input)
    {
      input = input.ToLowerInvariant();
      input = input.Replace("&", "og");
      input = input.Replace("æ", "ae");
      input = input.Replace("ø", "o");
      input = input.Replace("å", "aa");
      input = Regex.Replace(input, @"[^a-z0-9._\-/ ]+", "");
      input = Regex.Replace(input, "-+", "-");
      input = Regex.Replace(input, "[ _]+", "_");
      input = input.Trim('-', '_');
      return input;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      return StringInputChecker(string.Format("tfs/{0}/{3}/{1} {2}", values));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
