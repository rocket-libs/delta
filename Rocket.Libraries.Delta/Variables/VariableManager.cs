using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Rocket.Libraries.Delta.EventStreaming;

namespace Rocket.Libraries.Delta.Variables
{
    public interface IVariableManager
    {
        string GetCommandParsedVariable(Guid projectId, string command);
        
        bool IsVariableSetRequest(string command);
        void SetVariable(Guid projectId, string name, string value);
    }

    public class VariableManager : IVariableManager
    {
        private const string PipeVariableKeyword = "pipe_variable";
        private readonly Dictionary<string, string> variables = new Dictionary<string, string>();
        private readonly IEventQueue eventQueue;

        public VariableManager(IEventQueue eventQueue)
        {
            this.eventQueue = eventQueue;
        }

        public bool IsVariableSetRequest(string command)
        {
            return !string.IsNullOrEmpty(command) && command == PipeVariableKeyword;
        }

        public string GetCommandParsedVariable(Guid projectId, string command)
        {
            if(string.IsNullOrEmpty(command) || !command.Contains("$"))
            {
                return command;
            }
            var pieces = command.Split('$');
            var parsedCommand = pieces[0];
            for(var i = 1; i < pieces.Length; i++)
            {
                var isVariable = i % 2 != 0;
                var value = isVariable ? GetVariable(pieces[i]) : pieces[i];
                parsedCommand += value;
            }
            eventQueue.EnqueueSingleAsync(projectId,$"Command with variable parsed to: '{parsedCommand}'");
            return parsedCommand;
        }

        public void SetVariable(Guid projectId, string name, string value)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new Exception("Variable name cannot be empty");
            }
            if (variables.ContainsKey(name))
            {
                throw new Exception($"Variable {name} already exists. Please use a different name.");
            }
            else
            {
                variables.Add(name, value);
                eventQueue.EnqueueSingleAsync(projectId, $"Variable {name} set to {value}");
            }
        }

        public string GetVariable(string name)
        {
            if (variables.ContainsKey(name))
            {
                return variables[name];
            }
            else
            {
                throw new Exception($"Variable {name} does not exist.");
            }
        }

    }
}