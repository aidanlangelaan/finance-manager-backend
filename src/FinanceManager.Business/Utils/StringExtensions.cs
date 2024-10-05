namespace FinanceManager.Business.Utils;

public static class StringExtensions
{
    public static int LevenshteinDistance(this string source, string? target)
    {
        if (string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(target)) return target.Length;
        if (string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(source)) return source.Length;
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return 0;

        var sourceLength = source.Length;
        var targetLength = target.Length;

        var distance = new int[sourceLength + 1, targetLength + 1];

        // Initialize the first row
        for (var i = 0; i <= sourceLength; i++)
        {
            distance[i, 0] = i;
        }

        // Initialize the first column
        for (var j = 0; j <= targetLength; j++)
        {
            distance[0, j] = j;
        }

        for (int i = 1; i <= sourceLength; i++)
        {
            for (int j = 1; j <= targetLength; j++)
            {
                int cost = target[j - 1] == source[i - 1] ? 0 : 1;
                distance[i, j] = Math.Min(
                    Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                    distance[i - 1, j - 1] + cost);
            }
        }

        return distance[sourceLength, targetLength];
    }
    
    public static string NormalizeValue(this string input)
    {
        return new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
    }
}