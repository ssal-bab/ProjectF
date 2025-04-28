using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ShibaInspector.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : Attribute
    {
        public string Label { get; }
        public ButtonAttribute(string label = null)
        {
            Label = label;
        }   
    }
}
