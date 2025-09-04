using System;
using KatLib.Utilities;
using UnityEngine;

namespace KatLib.Observer
{
    [Serializable]
    public class GenericEnum
    {
        [SerializeField] private string _enumTypeName;
        public int EnumValue;
        private Type enumType;
        public Type EnumType => enumType ??= Type.GetType(_enumTypeName);

        public GenericEnum(string enumTypeName, int enumValue)
        {
            this._enumTypeName = enumTypeName;
            this.EnumValue = enumValue;
        }

        public GenericEnum(Type enumType, int enumValue)
        {
            this.enumType = enumType;
            this.EnumValue = enumValue;
        }

        public GenericEnum(object enumValue)
        {
            var type = enumValue.GetType();
            if (!type.IsEnum)
            {
                Debug.LogError($"{type} is not an Enum");
            }
            enumType = type;
            EnumValue = (int)enumValue;
        }

        public int GetID()
        {
            return EnumValue ^ EnumType.GetHashCode();
        }
    }
}