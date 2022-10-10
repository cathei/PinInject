// PinInject, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using UnityEngine;

namespace Cathei.PinInject
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectNameSelectionAttribute : PropertyAttribute { }
}