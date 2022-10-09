using FluentAssertions;
using Data.Models;
using Data;
using Data.Builders;
using Data.DTO;
using static LovenseService.Validators.CommandStrengthValidator;

namespace Testing
{
    [TestClass]
    public class RequestBuilderTesting
    {
        private CommandDTO VibrateCommand;
        private CommandDTO PatternCommand;

        private Toy Toy = new()
        {
            Alias = "dummy",
            ID = "dummyID",
            Model = "lush",
            Owner = "owner",
            Status = 1
        };

        public RequestBuilderTesting()
        {
            VibrateCommand = new()
            {
                Token = "token",
                Username = Toy.Owner,
                TargetToyID = Toy.ID,
                Command = LovenseCommand.Function,
                Action = "Vibrate:5",
                Duration = 1,
                LoopInterval = 1,
                LoopLength = 1
            };
            PatternCommand = new()
            {
                Structure = "V:1;F:vr;S:1000#",
                Sequence = "1;2;3;4;5"
            };
        }
        [TestMethod]
        public void FunctionBuilder_ReturnsFunctionDTO_SameAsPrefabDTO()
        {
            FunctionRequestBuilder builder = new ();
            CommandDTO request = builder
                .SetAction(LovenseAction.Vibrate, 5)
                .SetLoopInterval(1)
                .SetLoopLength(1)
                .SetRuntime(1)
                .SetCommandType(LovenseCommand.Function)
                .SetTargetToyID(VibrateCommand.TargetToyID)
                .SetToyOwner(VibrateCommand.Username)
                .SetDeveloperToken(VibrateCommand.Token)
                .Build();

            VibrateCommand.Should().BeEquivalentTo(request);
        }
        [TestMethod]
        [ExpectedException(typeof(StrengthOutOfRangeException))]
        public void FunctionBuilder_BuildWithInvalidStrength_ShouldThrowError()
        {
            FunctionRequestBuilder builder = new();
            builder.SetAction(LovenseAction.Vibrate, 0);
            builder.SetAction(LovenseAction.Rotate, 21);
            builder.SetAction(LovenseAction.Pump, 5);
        }
        [TestMethod]
        public void PatternBuilder_ReturnsPattern_SameAsPrefabPattern()
        {
            PatternRequestBuilder builder = new();
            CommandDTO request = builder
                .DefineStructure(new[] { LovenseAction.Vibrate, LovenseAction.Rotate }, 1000)
                .AddPatternStep(1)
                .AddPatternStep(2)
                .AddPatternStep(3)
                .AddPatternStep(4)
                .AddPatternStep(5)
                .Build();
            PatternCommand.Should().BeEquivalentTo(request);
        }
    }
}