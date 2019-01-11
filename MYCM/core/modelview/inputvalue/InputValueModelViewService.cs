using core.domain;
using core.modelview.input;
using System;
using System.Collections.Generic;
using System.Text;

namespace core.modelview.inputvalue {
    public class InputValueModelViewService {
        public static InputValue toEntity(AddInputValueModelView inputValueMV) {
            Input input = Input.valueOf(inputValueMV.input.name, inputValueMV.input.range);
            InputValue iVal = new InputValue(input);
            iVal.value = inputValueMV.value;
            return iVal;
        }
        public static AddInputValueModelView fromEntity(InputValue inputValue) {
            AddInputValueModelView mv = new AddInputValueModelView();
            mv.input = new GetInputModelView();
            mv.input.name = inputValue.input.name;
            mv.input.range = inputValue.input.range;
            mv.value = inputValue.value;
            return mv;
        }
        public static Dictionary<Input, string> toDictionary(AddInputValuesModelView inputValuesMV) {
            Dictionary<Input, string> dictionary = new Dictionary<Input, string>();
            foreach (AddInputValueModelView inputValueMV in inputValuesMV) {
                dictionary.Add(Input.valueOf(inputValueMV.input.name, inputValueMV.input.range), inputValueMV.value);
            }
            return dictionary;
        }
        public static AddInputValuesModelView fromCollection(List<InputValue> inputValues) {
            AddInputValuesModelView addInputValues = new AddInputValuesModelView();
            foreach (InputValue inputValue in inputValues) {
                addInputValues.Add(fromEntity(inputValue));
            }
            return addInputValues;
        }
    }
}
