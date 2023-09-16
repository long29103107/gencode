namespace gencode.service.Extensions;
public static class StringExtentions
{
    public static string ReplaceEmpty(this string line, string condition)
    {
        return line.Replace(condition, string.Empty);
    }

    public static List<string> SplitString(this string line, string condition)
    {
        var newLine = new string(line);
        var result = newLine.Split(condition)
                                        .Select(x => x.Trim())
                                        .ToList();

        if (result == null)
            return new List<string>();

        return result;
    }
}
