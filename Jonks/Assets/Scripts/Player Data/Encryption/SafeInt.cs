using System;
using System.Text.Json.Serialization;

public struct SafeInt
{
    private int value;
    private int salt;

    public SafeInt(int value)
    {
        salt = new Random().Next(int.MinValue / 4, int.MaxValue / 4);
        this.value = value ^ salt;
    }


    public SafeInt(SafeFloat safeIntValue)
    {
        salt = new Random().Next(int.MinValue / 4, int.MaxValue / 4);
        this.value = (int)((float)safeIntValue) ^ salt;
    }


    public override bool Equals(object obj)
    {
        return (int)this == (int)obj;
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


    public override string ToString()
    {
        return ((int)this).ToString();
    }


    public static implicit operator int(SafeInt safeInt)
    {
        return safeInt.value ^ safeInt.salt;
    }


    public static implicit operator SafeInt(int normalInt)
    {
        return new SafeInt(normalInt);
    }
}
