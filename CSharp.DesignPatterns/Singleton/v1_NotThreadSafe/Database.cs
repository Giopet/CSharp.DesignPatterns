namespace CSharp.DesignPatterns.Singleton.v1_NotThreadSafe
{
    /// <summary>
    /// Antipattern.
    /// Multiple thread could create more than one instance of Singleton(Database).
    /// The more concurrent threads and the longer it takes to create the class,
    /// the more likely multiple instances would be created.
    /// This might cause a minor performance impact, or some substantial problems in the app.
    /// </summary>
    public sealed class Database
    {
        // Private static field holds the only instance.
        private static Database? _instance;

        // Static Property to control access to the private static field.
        public static Database Instance
        {
            // To ensure lazy instantiation, the instance is created only the first time it's requested.
            get 
            {
                // The simple singleton pattern it is not thread safe.
                // Multiple threads could each enter this block concurrently.
                Logger.Log("Database Instance called.");
                return _instance ??= new Database(); // Will only evaluate the right side of the expression if the left side is null.
            }
        }

        // Private, parameterless constructor. Makes the class impossible to be instantiated anywhere else.
        private Database()
        {
            Logger.Log("Database Constructor invoked.");
        }
    }
}
