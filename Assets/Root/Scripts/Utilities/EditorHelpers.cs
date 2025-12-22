namespace Root.Scripts.Utilities
{
    public static class EditorHelpers
    {
#if UNITY_EDITOR
        public static string SetAbsoluteLength(string data, int length)
        {
            if (data.Length < length)
            {
                int blankChars = length - data.Length;
                for (int i = 0; i < blankChars; i++)
                {
                    data += '0';
                }
            }
            else if (data.Length > length)
            {
                data = data.Substring(0, length);
            }

            return data;
        }
#endif
    }
}