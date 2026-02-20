using System;

[Serializable]
public class BaseMessage
{
    public string action;
}

[Serializable]
public class EndTurnMessage : BaseMessage
{
    public ulong clientId;
}
