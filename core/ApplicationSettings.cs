using support.system;
using System.IO;
namespace core{
    /// <summary>
    /// Class that represents the application settings
    /// </summary>
    public sealed class ApplicationSettings{
        /// <summary>
        /// Constant that represents the name of the injector which will inject 
        /// the application settings
        /// </summary>
        private static readonly string SETTINGS_INJECTOR="application-settings";
        /// <summary>
        /// Constant that represents the name of the key which holds the value for 
        /// the application persistence context
        /// </summary>
        private static readonly string PERSISTENCE_CONTEXT_KEY="persistence.context";
        /// <summary>
        /// Current application settings holder
        /// </summary>
        private readonly Properties applicationSettings;
        /// <summary>
        /// Builds a new ApplicationSettings
        /// </summary>
        //Constructor is protected so settings can be refreshed
        internal ApplicationSettings(){
            this.applicationSettings=injectApplicationSettings();
        }

        /// <summary>
        /// Returns the application persistence context
        /// </summary>
        /// <returns>String with the current application persistence context</returns>
        public string getPersistenceContext(){return (string)applicationSettings.get(PERSISTENCE_CONTEXT_KEY);}

        /// <summary>
        /// Injects the application settings
        /// </summary>
        /// <returns>Properties with the injected application settings</returns>
        private Properties injectApplicationSettings(){
            return injectSettings(File.OpenRead(SETTINGS_INJECTOR));
        }

        /// <summary>
        /// Injects settings from an inputstream
        /// </summary>
        /// <param name="inputStream">Stream with the settings input stream</param>
        /// <returns>Properties with the injected settings</returns>
        private Properties injectSettings(Stream inputStream){
            Properties injectedApplicationSettings=new Properties();
            injectedApplicationSettings.load(inputStream);
            return injectedApplicationSettings;
        }
    }
}