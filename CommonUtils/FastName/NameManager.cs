using System;
using System.Collections.Generic;

namespace UGOAP.CommonUtils.FastName;

public partial class NameManager : Singleton<NameManager>
{
    Dictionary<UInt32, string> _nameIDs = new Dictionary<UInt32, string>();
    UInt32 _nextNameId = 1;
    static private object _nameIDsLock = new object();

    internal static uint CreateOrRetriveId(string inName)
    {
        if (Instance == null)
        {
            return 0;
        }
        return Instance.CreateOrRetrieveIdInternal(inName);
    }

    internal static string RetrieveNameFromId(uint nameId)
    {
        if (Instance == null)
        {
            return null;
        }

        return Instance.RetrieveNameFromIdInternal(nameId);
    }

    private UInt32 CreateOrRetrieveIdInternal(string inName)
    {
        lock (_nameIDsLock)
        {
            UInt32 foundNameId = 0;
            foreach (var kvp in _nameIDs)
            {
                if (kvp.Value == inName)
                {
                    foundNameId = kvp.Key;
                    break;
                }
            }

            // name ID not found - create a new entry
            if (foundNameId == 0)
            {
                foundNameId = _nextNameId;
                _nextNameId++;
                _nameIDs.Add(foundNameId, inName);
            }

            return foundNameId;
        }

    }

    private string RetrieveNameFromIdInternal(UInt32 nameId)
    {
        lock (_nameIDsLock)
        {
            string foundName = null;
            if (_nameIDs.TryGetValue(nameId, out foundName))
            {
                return foundName;
            }

            return null;
        }
    }
}