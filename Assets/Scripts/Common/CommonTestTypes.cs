// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using Cathei.PinInject;

public interface IBindWithInterface
{
    int Value { get; }
}

public class BindWithInterface : IBindWithInterface
{
    public int Value { get; }

    public BindWithInterface(int value)
    {
        Value = value;
    }
}

public class BindWithNew
{
    public string Value { get; }

    public BindWithNew()
    {
        Value = "Default Value";
    }

    public BindWithNew(string value)
    {
        Value = value;
    }
}

public class FlatInjectableClass
{
    [Inject]
    public IBindWithInterface bindedField;

    [Inject]
    public BindWithNew BindedProperty { get; private set; }
}

