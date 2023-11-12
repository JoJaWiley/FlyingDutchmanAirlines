namespace FlyingDutchmanAirlines.ControllerLayer.JsonData;

public class BookingData
{
    private string _firstName;

    public string FirstName
    {
        get => _firstName; 
        set => _firstName = ValidateName(value, nameof(FirstName));
    }

    private string _lastName;

    public string LastName
    {
        get => _lastName; 
        set => _lastName = ValidateName(value, nameof(LastName));
    }

    private string ValidateName(string name, string propertyName) =>
        string.IsNullOrEmpty(name)
            ? throw new InvalidOperationException("could not set " + propertyName)
            : name;
}