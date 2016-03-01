namespace Ubik.Domain.Core
{
    public class CommandResult : ICommandResult
    {
        protected CommandResult(bool success, string description)
        {
            Success = success;
            Description = description;
        }

        public virtual string Description
        {
            get; private set;
        }

        public virtual bool Success
        {
            get; private set;
        }

        public static ICommandResult Create(bool success, string description)
        {
            return new CommandResult(success, description);
        }
    }
}