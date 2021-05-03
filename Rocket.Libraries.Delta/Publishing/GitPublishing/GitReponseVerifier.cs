using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delta.Publishing.GitPublishing
{
    public interface IGitReponseVerifier
    {
        bool Is(string responseMarker, string[] commandResults);

        bool IsNot(string responseMarker, string[] commandResults);
    }

    public class GitReponseVerifier : IGitReponseVerifier
    {
        public bool Is(string responseMarker, string[] commandResults)
        {
            //
            if (commandResults != null)
            {
                foreach (var item in commandResults)
                {
                    var notEmpty = !string.IsNullOrEmpty(item);
                    var hasSufficientLength = item.Length >= responseMarker.Length;
                    var startsWithSearchString = item.Trim().StartsWith(responseMarker, StringComparison.InvariantCultureIgnoreCase);
                    if (notEmpty && hasSufficientLength && startsWithSearchString)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsNot(string responseMarker, string[] commandResults)
        {
            return !Is(responseMarker, commandResults);
        }
    }
}