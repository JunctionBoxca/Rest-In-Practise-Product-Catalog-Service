﻿using System.Reflection;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class PrivateField
    {
        public static T GetValue<T>(string fieldName, object o)
        {
            FieldInfo fieldInfo = o.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);
            return (T) fieldInfo.GetValue(o);
        }
    }
}