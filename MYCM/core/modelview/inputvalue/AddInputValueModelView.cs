using core.modelview.input;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace core.modelview.inputvalue {
    [DataContract]
    public class AddInputValueModelView {
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public GetInputModelView input { get; set; }
    }
}
