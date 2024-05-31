using System;

namespace UGOAP.CommonUtils.FastName;

public class FastName : IComparable<FastName>
{
    private static readonly FastName None = new();
    private readonly uint _nameId;

    // Invalid NameID
    private FastName()
    {
        _nameId = 0;
    }

    public FastName(string inName)
    {
        _nameId = NameManager.CreateOrRetriveId(inName);
    }

    public override string ToString()
    {
        return this == None ? "None" : NameManager.RetrieveNameFromId(_nameId);
    }

    public int CompareTo(FastName other)
    {
        return _nameId.CompareTo(other._nameId);
    }

    public override bool Equals(object obj)
    {
        if (obj is FastName fastName)
        {
            return _nameId == fastName._nameId;
        }
        return false;
    }

    public static implicit operator bool(FastName self)
    {
        return self != None;
    }

    public override int GetHashCode()
    {
        return _nameId.GetHashCode();
    }

    public static bool operator ==(FastName left, FastName right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }

    public static bool operator !=(FastName left, FastName right)
    {
        return !(left == right);
    }

    public static bool operator <(FastName left, FastName right)
    {
        return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
    }

    public static bool operator <=(FastName left, FastName right)
    {
        return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
    }

    public static bool operator >(FastName left, FastName right)
    {
        return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
    }

    public static bool operator >=(FastName left, FastName right)
    {
        return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}