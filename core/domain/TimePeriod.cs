using System;
using NodaTime;
using support.domain.ddd;

namespace core.domain
{
    /// <summary>
    /// Represents a Time Period (starting date and ending date)
    /// </summary>
    public class TimePeriod : ValueObject
    {
        /// <summary>
        /// Constant that represents the message that occurs if the starting date is null
        /// </summary>
        private const string NULL_STARTING_DATE = "The starting date can't be null";

        /// <summary>
        /// Constant that represents the message that occurs if the ending date is null
        /// </summary>
        private const string NULL_ENDING_DATE = "The ending date can't be null";

        /// <summary>
        /// Constant that represents the message that occurs if the starting date is the same as the ending date
        /// </summary>
        private const string STARTING_DATE_SAME_AS_ENDING_DATE = "The starting date can't be the same as the ending date";

        /// <summary>
        /// Constant that represents the message that occurs if the starting date is later than the ending date
        /// </summary>
        private const string STARTING_DATE_LATER_THAN_ENDING_DATE = "The starting date can't be later than the ending date";

        /// <summary>
        /// PID of the TimePeriod
        /// </summary>
        /// <value>Gets/Sets the Identifier</value>
        public long Id {get; set;}

        /// <summary>
        /// Starting date of the time period
        /// </summary>
        /// <value>Gets/Sets the LocalDateTime</value>
        public LocalDateTime startingDate { get; internal set; }

        /// <summary>
        /// Ending date of the time period
        /// </summary>
        /// <value>Gets/Sets the LocalDateTime</value>
        public LocalDateTime endingDate { get; internal set; }

        /// <summary>
        /// Builds a TimePeriod with the value of a starting date and an ending date
        /// </summary>
        /// <param name="startingDate">starting date of the time period</param>
        /// <param name="endingDate">ending date of the time period</param>
        /// <returns>Finite TimePeriod</returns>
        public static TimePeriod valueOf(LocalDateTime startingDate, LocalDateTime endingDate)
        {
            return new TimePeriod(startingDate, endingDate);
        }

        /// <summary>
        /// Builds an infinite TimePeriod with the value of a starting date
        /// </summary>
        /// <param name="startingDate">starting date of the time period</param>
        /// <returns>Infinite TimePeriod</returns>
        public static TimePeriod valueOf(LocalDateTime startingDate)
        {
            return new TimePeriod(startingDate);
        }

        /// <summary>
        /// Empty constructor for ORM
        /// </summary>
        protected TimePeriod() { }

        /// <summary>
        /// Builds a TimePeriod with a starting date and an ending date
        /// </summary>
        /// <param name="startingDate">starting date of the Time Period</param>
        /// <param name="endingDate">ending date of the Time Period</param>
        private TimePeriod(LocalDateTime startingDate, LocalDateTime endingDate)
        {
            checkLocalDateTimes(startingDate, endingDate);
            this.startingDate = startingDate;
            this.endingDate = endingDate;
        }

        /// <summary>
        /// Builds an infinite TimePeriod (only with a starting date)
        /// </summary>
        /// <param name="startingDate">starting date of the time period</param>
        private TimePeriod(LocalDateTime startingDate)
        {
            this.startingDate = startingDate;
            this.endingDate = LocalDate.MaxIsoValue.At(LocalTime.MaxValue);
        }

        /// <summary>
        /// Checks if starting date and ending date LocalDateTimes are valid
        /// </summary>
        /// <param name="startingDate">starting date of the time period</param>
        /// <param name="endingDate">ending date of the time period</param>
        private void checkLocalDateTimes(LocalDateTime startingDate, LocalDateTime endingDate)
        {
            if (startingDate.CompareTo(endingDate) == 0)
            {
                throw new ArgumentException(STARTING_DATE_SAME_AS_ENDING_DATE);
            }

            if (startingDate.CompareTo(endingDate) > 0)
            {
                throw new ArgumentException(STARTING_DATE_LATER_THAN_ENDING_DATE);
            }
        }

        public override int GetHashCode()
        {
            return startingDate.GetHashCode() + endingDate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            TimePeriod other = (TimePeriod)obj;

            return this.startingDate.Equals(other.startingDate)
                    && this.endingDate.Equals(other.endingDate);
        }

        public override string ToString()
        {
            return String.Format("Starting Date:{0}\nEnding Date:{1}",
                    startingDate.ToString(), endingDate.ToString());
        }
    }
}