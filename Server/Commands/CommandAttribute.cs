using System;

namespace TACS_Server.Commands
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class CommandAttribute : Attribute
    {

        public string Command { get; }

        public CommandAttribute(string command)
        {
            Command = command;
        }
    }

    public sealed class IsAdminAttribute : Attribute
    {
        //public bool Value { get; }

        //public IsAdminAttribute()
        //{
        //    Value = true;
        //}
    }
}