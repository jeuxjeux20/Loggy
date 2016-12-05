namespace Loggy
{
    internal class AsciiBar
    {
        public static string DrawProgressBar(uint percent)
        {
            char[] progress = new char[22];
            progress[0] = '[';
            progress[21] = ']';
            if (percent > 100)
                percent = 100;
            float charsToFill = percent / (float)5;
            byte lel = 1;
            for (int i = 0; i < charsToFill; i++)
            {
                progress[lel] = '#';
                lel++;
            }
            for (int i = 0; i < progress.Length; i++)
            {
                if (progress[i] != '[' && progress[i] != ']' && progress[i] != '#')
                {
                    progress[i] = '-';
                }
            }
            return new string(progress);
        }
    }
}
