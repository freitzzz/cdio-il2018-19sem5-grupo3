namespace core{
    /// <summary>
    /// Class that contains all application information (e.g. settings)
    /// </summary>
    public sealed class Application{
        /// <summary>
        /// ApplicationSettings with the current application settings
        /// </summary>
        private static readonly ApplicationSettings applicationSettings=new ApplicationSettings();

        /// <summary>
        /// Returns the current application settings
        /// </summary>
        /// <returns>ApplicationSettings with the current application settings</returns>
        public static ApplicationSettings settings(){return applicationSettings;}

        /// <summary>
        /// Hides default constructor
        /// </summary>
        private Application(){}
    }
}