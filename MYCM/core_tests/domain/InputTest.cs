//TODO: FIX ME
// using core.domain;
// using core.dto;
// using System;
// using Xunit;

// namespace core_tests.domain {
//     public class InputTest {
//         /// <summary>
//         /// Ensures Input constructor throws ArgumentException when name is null
//         /// </summary>
//         [Fact]
//         public void ensureConstructorThrowsArgumentExceptionWhenArgumentIsNull() {
//             Console.WriteLine("ensureConstructorThrowsArgumentExceptionWhenArgumentIsNull");
//             Action invalidNullInputNameCreation = () => Input.valueOf(null);
//             //Since the input was created with a null name then it should throw an ArgumentException
//             Assert.Throws<ArgumentException>(invalidNullInputNameCreation);
//         }
//         /// <summary>
//         /// Ensures Input constructor throws ArgumentException when name is empty string
//         /// </summary>
//         [Fact]
//         public void ensureConstructorThrowsArgumentExceptionWhenArgumentIsEmpty() {
//             Console.WriteLine("ensureConstructorThrowsArgumentExceptionWhenArgumentIsEmpty");
//             Action invalidNullInputNameCreation = () => new Input("");
//             //Since the input was created with name as an empty string then it should throw an ArgumentException
//             Assert.Throws<ArgumentException>(invalidNullInputNameCreation);
//         }
//         /// <summary>
//         /// Ensure Input is  created successfully with a name
//         /// </summary>
//         [Fact]
//         public void ensureInputNameConstructorIsSuccessful() {
//             Console.WriteLine("ensureInputNameConstructorIsSuccessful");
//             string name = "der alte würfelt nicht";
//             Input input = new Input(name);
//             Assert.Equal(name, input.name);
//         }
//         /// <summary>
//         /// Ensure toDTO succeeds
//         /// </summary>
//         [Fact]
//         public void ensureToDTOSucceeds() {
//             Console.WriteLine("ensureToDTOSucceeds");
//             Input input = new Input("Altair");
//             input.value = "Vega";
//             InputDTO dto = input.toDTO();
//             Assert.Equal(input.name, dto.name);
//             Assert.Equal(input.value, dto.value);
//         }
//     }
// }
