namespace support.domain
{
    /// <summary>
    /// Abstract Class used for soft deletion.
    /// </summary>
    public abstract class Activatable
    {
        /// <summary>
        /// Activation status property.
        /// </summary>
        /// <value>Gets/sets the value of the activated property.</value>
        public bool activated { get; protected set; } = true;

        /// <summary>
        /// Activates the instance.
        /// </summary>
        public virtual bool activate()
        {
            if (activated)
            {
                return false;
            }

            activated = true;

            return true;
        }

        /// <summary>
        /// Deactivates the instance.
        /// </summary>
        public virtual bool deactivate()
        {
            if (!activated)
            {
                return false;
            }

            activated = false;

            return true;
        }
    }
}