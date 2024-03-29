﻿using System;
using System.Collections.Immutable;
using System.Linq;
using Rocket.Libraries.Delta.ProcessRunnerLogging;

namespace Rocket.Libraries.Delta.Running
{
    public interface IProcessResponseParser
    {
        string PeekLastResult { get; }

        bool ErrorIsOneOf(params string[] responseMarkers);
        bool ErrorIsLike (string responseMarker);
        bool ErrorIsNotLike (string responseMarker);
        bool Is (string responseMarker, ImmutableList<string> commandResults);

        bool IsNot (string responseMarker, ImmutableList<string> commandResults);
        bool OutputIs (string responseMarker);
        bool OutputIsNot (string responseMarker);
    }

    public class ProcessResponseParser : IProcessResponseParser
    {
        private readonly IProcessRunnerLoggerBuilder processRunnerLoggerBuilder;

        public ProcessResponseParser (
            IProcessRunnerLoggerBuilder processRunnerLoggerBuilder
        )
        {
            this.processRunnerLoggerBuilder = processRunnerLoggerBuilder;
        }

        public bool ErrorIsNotLike (string responseMarker)
        {
            return !ErrorIsLike (responseMarker);
        }

        public bool ErrorIsOneOf (params string[] responseMarkers)
        {
            foreach (var item in responseMarkers)
            {
                if (ErrorIsLike (item))
                {
                    return true;
                }
            }
            return false;

        }
        public bool ErrorIsLike (string responseMarker)
        {
            return Is (responseMarker, processRunnerLoggerBuilder.PeekErrors);
        }

        public bool OutputIs (string responseMarker)
        {
            return Is (responseMarker, processRunnerLoggerBuilder.PeekAll.SelectMany (x => x.Output).ToImmutableList ());
        }

        public bool OutputIsNot (string responseMarker)
        {
            return !OutputIs (responseMarker);
        }

        public bool Is (string responseMarker, ImmutableList<string> commandResults)
        {
            //
            if (commandResults != null)
            {
                for (var i = commandResults.Count - 1; i >= 0; i--)
                {
                    var item = commandResults[i];
                    var notEmpty = !string.IsNullOrEmpty (item);
                    var hasSufficientLength = item.Length >= responseMarker.Length;
                    var startsWithSearchString = item.Trim ().StartsWith (responseMarker, StringComparison.InvariantCultureIgnoreCase);
                    if (notEmpty && hasSufficientLength && startsWithSearchString)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string PeekLastResult
        {
            get
            {
                return processRunnerLoggerBuilder.PeekAll.LastOrDefault ()?.Output.LastOrDefault ();
            }
        }

        public bool IsNot (string responseMarker, ImmutableList<string> commandResults)
        {
            return !Is (responseMarker, commandResults);
        }
    }
}