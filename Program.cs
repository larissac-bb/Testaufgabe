class Program
{
    public static int[] SplitEtappen(int[] etappen, int etappenzahl, int tagesanzahl)
    {
        // NOTE: would normally use etappen.Length instead of etappenzahl to reduce potential of errors
        // but the task specifies that etappenzahl is the number of elements in etappen so I go with that

        int[,] dp = new int[etappenzahl + 1, tagesanzahl + 1];
        int[,] splitPoints = new int[etappenzahl + 1, tagesanzahl + 1];

        // Initialize dp[i][1] as sum of weights up to i
        for (int i = 1; i <= etappenzahl; i++)
        {
            dp[i, 1] = dp[i - 1, 1] + etappen[i - 1];
        }

        // Initialize dp[1][j] as weight of first element
        for (int j = 1; j <= tagesanzahl; j++)
        {
            dp[1, j] = etappen[0];
        }

        // Fill dp using dynamic programming
        for (int i = 2; i <= etappenzahl; i++)
        {
            for (int j = 2; j <= tagesanzahl; j++)
            {
                dp[i, j] = int.MaxValue;

                for (int k = 1; k < i; k++)
                {
                    int temp = Math.Max(dp[k, j - 1], SumOfWeights(etappen, k, i - 1));
                    if (temp < dp[i, j])
                    {
                        dp[i, j] = temp;
                        splitPoints[i, j] = k; // Store split point
                    }
                }
            }
        }

        // Get ideal split for each section
        List<int> splits = new List<int>();

        int currSection = tagesanzahl;
        int lastIndex = etappenzahl;

        while (currSection > 1)
        {
            int splitIndex = splitPoints[lastIndex, currSection];
            splits.Add(SumOfWeights(etappen, splitIndex, lastIndex - 1));
            lastIndex = splitIndex;
            currSection--;
        }

        // Add remaining weights to the first section
        splits.Add(SumOfWeights(etappen, 0, lastIndex - 1));
        splits.Reverse();
        return splits.ToArray();
    }

    static int SumOfWeights(int[] etappen, int start, int end)
    {
        int sum = 0;
        for (int i = start; i <= end; i++)
        {
            sum += etappen[i];
        }
        return sum;
    }

    static void Main()
    {
        const string textFilePath = "./assets/input.txt";

        var lines = File.ReadLines(textFilePath);
        int etappenzahl = int.Parse(lines.First());
        int tagesanzahl = int.Parse(lines.Skip(1).First());
        int[] etappen = lines.Skip(2).Select(int.Parse).ToArray();

        int[] splits = SplitEtappen(etappen, etappenzahl, tagesanzahl);

        for (int i = 0; i < splits.Length; i++)
        {
            Console.WriteLine((i + 1) + ". Tag: " + splits[i] + " km");
        }
        Console.WriteLine("\nMaximum: " + splits.Max() + " km");
    }
}

