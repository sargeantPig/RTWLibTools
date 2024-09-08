namespace RTWLibPlus.helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using RTWLibPlus.interfaces;

public static class BaseHelper
{
    public static string[] ToArray(this List<IBaseObj> objects, Func<IBaseObj, string> sequence) => objects.Select(sequence).ToArray();

}
