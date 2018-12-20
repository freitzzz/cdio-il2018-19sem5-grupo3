using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.utils;

namespace core.domain
{
    /// <summary>
    /// Class representing the 
    /// </summary>
    public class InputValue
    {
        /// <summary>
        /// Constant representing the message presented when the provided Input is not valid.
        /// </summary>
        private const string INVALID_INPUT = "The input is not valid.";

        /// <summary>
        /// InputValue's persistence identifier.
        /// </summary>
        /// <value>Gets/Protected sets the persistence identifier.</value>
        public long Id { get; protected set; }

        /// <summary>
        /// InputValue's value.
        /// </summary>
        /// <value>Gets/Sets the value.</value>
        public string value { get; set; }

        /// <summary>
        /// InputValue's Input.
        /// </summary>
        /// <value>Gets/Protected sets the Input.</value>
        public Input input { get; protected set; }

        /// <summary>
        /// Instance of ILazyLoader injected by the framework.
        /// </summary>
        /// <value>Gets/Sets the injected ILazyloader.</value>
        private ILazyLoader lazyLoader { get; set; }

        /// <summary>
        /// Constructor used for injecting an instance of ILazyLoader.
        /// </summary>
        /// <param name="lazyLoader">Instance of ILazyLoader being injected.</param>
        private InputValue(ILazyLoader lazyLoader)
        {
            this.lazyLoader = lazyLoader;
        }

        /// <summary>
        /// Creates an instance of InputValue referring to a given Input.
        /// </summary>
        /// <param name="input">Instance of Input to which the InputValue is referring.</param>
        public InputValue(Input input)
        {
            checkInput(input);
            this.input = input;
            //the input value is not initially set
            this.value = null;
        }

        /// <summary>
        /// Checks if the provided Input is valid.
        /// </summary>
        /// <param name="input">Instance of Input being checked.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Input is null.</exception>
        private void checkInput(Input input)
        {
            if (input == null) throw new ArgumentNullException(INVALID_INPUT);
        }

    }
}