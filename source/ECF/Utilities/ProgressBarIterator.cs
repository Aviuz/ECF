namespace ECF.Utilities;

public class ProgressBarIterator : ProgressBar
{
    public int TotalCount { get; private set; }
    public int CurrentCount { get; private set; }
    public int CountForOnePercent { get; private set; }

    public ProgressBarIterator(int totalCount)
    {
        TotalCount = totalCount;
        CurrentCount = 0;
        CountForOnePercent = GetPercentageStep(totalCount);
    }

    public void Increment()
    {
        if (++CurrentCount % CountForOnePercent != 0)
            return;

        if (TotalCount > 0)
            Progress = CurrentCount * 100 / TotalCount;
        else
            Progress = 100;
    }

    /// <summary>
    /// Returns how many rows need to be changed before 1 percentage progress
    /// </summary>
    /// <param name="totalCount">total number of records</param>
    private static int GetPercentageStep(int totalCount)
    {
        if (totalCount >= 100)
            return totalCount / 100;
        else
            return 1;
    }
}
