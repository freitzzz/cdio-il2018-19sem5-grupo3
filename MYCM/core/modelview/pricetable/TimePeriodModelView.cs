using System.Runtime.Serialization;

namespace core.modelview.pricetable
{
    /// <summary>
    /// ModelView to represent a TimePeriod
    /// </summary>
    [DataContract]
    public class TimePeriodModelView
    {
        /// <summary>
        /// Time Period's starting date
        /// </summary>
        /// <value>Gets/Sets starting date</value>
        [DataMember(Name="startingDate")]
        public string startingDate {get;set;}

        /// <summary>
        /// Time Period's ending date
        /// </summary>
        /// <value>Gets/Sets ending date</value>
        [DataMember(Name="endingDate")]
        public string endingDate {get; set;}   
    }
}