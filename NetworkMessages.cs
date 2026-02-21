using System;

[Serializable]
public class BaseMessage
{
    public string action;
}
[Serializable]
public class TimerMessage : BaseMessage
{
    public int timeLeft;
}
[Serializable]
public class TurnMessage : BaseMessage
{
    public string turnInfo;
}

[Serializable]
public class EndTurnMessage : BaseMessage
{
    public ulong clientId;
}
