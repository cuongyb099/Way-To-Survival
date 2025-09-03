using System;

namespace KatLib.Observer
{
    public interface IObserver
    {
        public void Subscribe<T>(T enumValue, Action handler) where T : unmanaged, Enum;
        public void Subscribe<TValue, TEnum>(TEnum enumValue, Action<TValue> handler) where TEnum : unmanaged, Enum;
        public void Unsubscribe<T>(T enumValue, Action handler) where T : unmanaged, Enum;
        public void Unsubscribe<TValue, TEnum>(TEnum enumValue, Action<TValue> handler) where TEnum: unmanaged, Enum;
        public void Publish<T>(T enumValue) where T : unmanaged, Enum;
        public void Publish<TValue, TEnum>(TEnum enumValue, TValue param) where TEnum : unmanaged, Enum;
        public void Clear();
        
        //Generic Enum
        public void Publish(GenericEnum enumValue);
        public void Publish<T>(GenericEnum enumValue, T param);
        public void Subscribe(GenericEnum enumValue, Action handler);
        public void Subscribe<TValue>(GenericEnum enumValue, Action<TValue> handler);
        public void Unsubscribe(GenericEnum enumValue, Action handler);
        public void Unsubscribe<TValue>(GenericEnum enumValue, Action<TValue> handler);
    }
}