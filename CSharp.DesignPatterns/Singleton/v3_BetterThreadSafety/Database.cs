namespace CSharp.DesignPatterns.Singleton.v3_BetterThreadSafety
{
    /// <summary>
    /// Double check locking pattern.
    /// We are checking to see if the instance is null  twice, and then locking in between those checks.
    /// It will make sure that we only fetch the lock when the instance is null,
    /// so that will only happen when the application starts up or when the first request is made to reference this instance.
    /// Performance is better with this locking pattern.
    /// 
    /// Drowbacks:
    /// It's complex, easy to get wrong.
    /// It has some issues with the ECMA common language interface specification, CLI, that may be a concern.
    /// </summary>
    public sealed class Database
    {
        private static Database? _instance = null;

        private static readonly object _lock = new object();

        public static Database Instance
        {
            get
            {
                Logger.Log("Database Instance called.");

                if (_instance is null) // only get a lock if the instance is null
                {
                    lock (_lock)
                    {
                        return _instance ??= new Database();
                    }
                }

                return _instance;
            }
        }

        private Database()
        {
            Logger.Log("Database Constructor invoked.");
        }
    }
}
