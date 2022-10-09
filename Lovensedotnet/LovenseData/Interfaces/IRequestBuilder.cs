using Data;
using Data.DTO;

namespace Data.Interfaces
{
    public interface IFluentRequestBuilder<T>
    {
        CommandDTO Request { get; set; }
        string DeveloperToken { set; }

        T SetToyOwner(string id);
        T SetDeveloperToken(string token);

        T SetCommandType(LovenseCommand command);
        T SetRuntime(int timeSec);
        T SetTargetToyID(string toyID);

        CommandDTO Build();
    }
}