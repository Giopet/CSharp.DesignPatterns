namespace CSharp.DesignPatterns.Singleton.v2_SimpleThreadSafety
{
    /// <summary>
    /// Thread safe singleton by adding locking.
    /// The multiple threads will sychronize and enter the code of checking the instance one by one.
    /// 
    /// This approach works, but it has negative performance impacts 
    /// because every use of the Instance property will now incur the overhead of this lock.
    /// 
    /// Note:
    /// If you use locks,be sure to use a dedicated private instance variable,
    /// not a shared or static value that could be used by another lock elsewhere in the application.
    /// Lock instances should be paired one for one with their lock statements.
    /// </summary>
    public sealed class Database
    {
        private static Database? _instance = null;

        // New instance of an object to be used as a lock variable.
        private static readonly object _lock = new object();

        public static Database Instance
        {
            get
            {
                Logger.Log("Database Instance called.");

                // Lock instance before performing the check to see if our singleton database instance is null.
                // Ensuring we never have two threads creating the instance at the same time.
                lock (_lock) // This lock is used on every reference to Singleton Database.
                {
                    return _instance ??= new Database();
                }
            }
        }

        private Database()
        {
            Logger.Log("Database Constructor invoked.");
        }
    }
}
