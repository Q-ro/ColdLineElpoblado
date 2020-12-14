
public static class Util
{
    public static int wrap(int value, int min, int max)
    {
        if (value < min)
        {
            value = max;
        }
        if (value > max)
        {
            value = min;
        }

        return value;
    }

}
