using PubSub;

public class ZeroGMessage : IMessage
{
    public bool Active;

    public ZeroGMessage(bool active)
    {
        this.Active = active;
    }
}

