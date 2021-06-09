using System.Collections.Generic;

namespace kontur_project
{
    public static class MessageManager
    {
        public static bool IsMessageCorrect(string text, ICollection<string> map = null)
        {
            if (map != null)
                return map.Contains(text);

            return text != null;
        }
    }
}
