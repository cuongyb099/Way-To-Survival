using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace KatInventory
{
    public class ItemSoConverter : JsonConverter<ItemBaseSO>
    {
        public override void WriteJson(JsonWriter writer, ItemBaseSO value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteValue(value.ID);
        }

        public override ItemBaseSO ReadJson(JsonReader reader, Type objectType, ItemBaseSO existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            return ItemDataBase.Instance.SearchItem((string)reader.Value);
        }
    }
}
