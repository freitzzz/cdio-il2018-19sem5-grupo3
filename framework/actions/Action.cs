namespace framework.actions{
    /// <summary>
    /// Functional Interface which represents an Action
    /// </summary>
    public interface Action{
        /// <summary>
        /// Executes an action
        /// </summary>
        /// <returns>boolean true if the action execution was succesful, false if not</returns>
        bool execute();
    }
}