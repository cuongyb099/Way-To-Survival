using UnityEngine;
using System;
using System.Collections.Generic;
using KatLib.Utilities;

namespace KatLib.Observer
{
    public class Observer : MonoBehaviour, IObserver
    {
        protected readonly Dictionary<int, Delegate> observers = new();
        private static readonly Dictionary<Type, int> cacheTypeHash = new();
        
#if UNITY_EDITOR
        [SerializeField] public bool removeNamespace;
        public Dictionary<int, Delegate> Observers => observers;
        public  HashSet<(Enum EnumValue, Type Param)> EditorDebug = new ();
#endif
        
        public void Subscribe<T>(T enumValue, Action handler) where T: unmanaged, Enum
        {
            int id = EnumToHashID(enumValue);
            if (RegisterInternal(handler, id)) return;
            
#if UNITY_EDITOR
            EditorDebug.Add((enumValue, null));
#endif
        }

        private bool RegisterInternal(Action handler, int id)
        {
            if (observers.TryGetValue(id, out var existing))
            {
                if(existing is not Action action) return true;
                observers[id] = action + handler;
                return true;
            }

            observers[id] = handler;
            return false;
        }


        public void Subscribe<TValue, TEnum>(TEnum enumValue, Action<TValue> handler) where TEnum: unmanaged, Enum
        {
            int id = EnumToHashID(enumValue);
            if (RegisterParamInternal(handler, id)) return;

#if UNITY_EDITOR
            EditorDebug.Add((enumValue, typeof(TValue)));
#endif
        }

        private bool RegisterParamInternal<TValue>(Action<TValue> handler, int id)
        {
            if (observers.TryGetValue(id, out var existing))
            {
                if(existing is not Action<TValue> action) return true;
                observers[id] = action + handler;
                return true;
            }

            observers[id] = handler;
            return false;
        }

        public void Unsubscribe<T>(T enumValue, Action handler) where T : unmanaged, Enum
        {
            int id = EnumToHashID(enumValue);
            if(RemoveInternal(handler, id)) return;
            
#if UNITY_EDITOR
            EditorDebug.Remove((enumValue, null));
#endif
        }

        private bool RemoveInternal(Action handler, int id)
        {
            if (!observers.TryGetValue(id, out var existing)) return false;
            
            if (existing is not Action action) return true;
            
            var afterRemove = action - handler;
            if (afterRemove == null)
            {
                observers.Remove(id);
                return true;
            }
            
            observers[id] = afterRemove;
            return false;
        }

        public void Unsubscribe<TValue, TEnum>(TEnum enumValue, Action<TValue> handler) where TEnum: unmanaged, Enum
        {
            int id = EnumToHashID(enumValue);
            if(RemoveParamInternal(handler, id)) return;
#if UNITY_EDITOR
            EditorDebug.Remove((enumValue, typeof(TValue)));
#endif
        }

        private bool RemoveParamInternal<TValue>(Action<TValue> handler, int id) 
        {
            if (!observers.TryGetValue(id, out var existing)) return false;
            
            if(existing is not Action<TValue> action) return false;
            
            var afterRemove = action - handler;
            if (afterRemove == null)
            {
                observers.Remove(id);
                return true;
            }
            observers[id] = afterRemove;
            return false;
        }

        public void Publish<T>(T enumValue) where T : unmanaged, Enum
        {
            int id = EnumToHashID(enumValue);
            
            if (!observers.TryGetValue(id, out var handler)) return;
            
            (handler as Action)?.Invoke();
        }
        
        public void Publish<TValue, TEnum>(TEnum enumValue, TValue param) where TEnum : unmanaged, Enum
        {
            int hashID = EnumToHashID(enumValue);
           
            if (!observers.TryGetValue(hashID, out var handler)) return;
            
            (handler as Action<TValue>)?.Invoke(param);
        }

        
        private static int EnumToHashID<T>(T enumValue) where T : unmanaged, Enum
        {
            Type enumType = typeof(T);
            int intValue = KatUtilities.GenericEnumConvertInt(enumValue);

            return Hash(enumType, intValue);
        }

        private static int Hash(Type enumType, int intValue)
        {
            if(cacheTypeHash.TryGetValue(enumType, out int enumHashCode)){}
            else
            {
                enumHashCode = enumType.GetHashCode();
                cacheTypeHash[enumType] = enumHashCode;
            }

            return intValue ^ enumHashCode;
        }

        public void Clear()
        {
            observers.Clear();
#if UNITY_EDITOR
            EditorDebug.Clear();
#endif
        }

        public void Publish(GenericEnum enumValue)
        {
            if(enumValue?.EnumType == null) return;
            
            int id = Hash(enumValue.EnumType, enumValue.EnumValue);
            
            if (!observers.TryGetValue(id, out var handler)) return;
            
            (handler as Action)?.Invoke();
        }

        public void Publish<T>(GenericEnum enumValue, T param)
        {
            if(enumValue?.EnumType == null) return;
            
            int id = Hash(enumValue.EnumType, enumValue.EnumValue);
            
            if (!observers.TryGetValue(id, out var handler)) return;
            
            (handler as Action<T>)?.Invoke(param);
        }

        public void Subscribe(GenericEnum enumValue, Action handler)
        {
            if(enumValue?.EnumType == null) return;
            
            int id = Hash(enumValue.EnumType, enumValue.EnumValue);
            if (RegisterInternal(handler, id)) return;

#if UNITY_EDITOR
            var value = Enum.ToObject(enumValue.EnumType, enumValue.EnumValue) as Enum;
            EditorDebug.Add((value, null));
#endif
        }

        public void Subscribe<TValue>(GenericEnum enumValue, Action<TValue> handler)
        {
            if(enumValue?.EnumType == null) return;
            
            int id = Hash(enumValue.EnumType, enumValue.EnumValue);
            if (RegisterParamInternal(handler, id)) return;
            
#if UNITY_EDITOR
            var value = Enum.ToObject(enumValue.EnumType, enumValue.EnumValue) as Enum;
            EditorDebug.Add((value, typeof(TValue)));
#endif
        }

        public void Unsubscribe(GenericEnum enumValue, Action handler)
        {
            if(enumValue?.EnumType == null) return;
            
            int id = Hash(enumValue.EnumType, enumValue.EnumValue);
            if(RemoveInternal(handler, id)) return;
#if UNITY_EDITOR
            var value = Enum.ToObject(enumValue.EnumType, enumValue.EnumValue) as Enum;
            EditorDebug.Remove((value, null));
#endif
        }

        public void Unsubscribe<TValue>(GenericEnum enumValue, Action<TValue> handler)
        {
            if(enumValue?.EnumType == null) return;
            
            int id = Hash(enumValue.EnumType, enumValue.EnumValue);
            if(RemoveParamInternal(handler, id)) return;
            
#if UNITY_EDITOR
            var value = Enum.ToObject(enumValue.EnumType, enumValue.EnumValue) as Enum;
            EditorDebug.Remove((value, typeof(TValue)));
#endif
        }
    }
}