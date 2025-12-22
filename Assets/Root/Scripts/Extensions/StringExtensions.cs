using System.IO;

namespace Root.Scripts.Extensions
{
    public static class StringExtensions
    {
        public static string SetValidName(this string value)
        {
            foreach (char character in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(character, '-');
            }
            
            return value;
        }
    }
}