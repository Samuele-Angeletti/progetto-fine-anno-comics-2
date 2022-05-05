
using PubSub;
public class OpenMenuMessage : IMessage
{
    public EMenu MenuType;
    public OpenMenuMessage(EMenu menuToOpen)
    {
        MenuType = menuToOpen;
    }
}
