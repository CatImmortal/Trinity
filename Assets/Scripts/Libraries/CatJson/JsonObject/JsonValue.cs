using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// Json值
    /// </summary>
    public class JsonValue
    {
        public ValueType Type;

        public bool Boolean;
        public double Number;
        public string Str;
        public JsonValue[] Array;
        public JsonObject Obj;

        #region 构造方法

        public JsonValue()
        {
            Type = ValueType.Null;
        }
        
        public JsonValue(bool b)
        {
            Type = ValueType.Boolean;
            Boolean = b;
        }
        public JsonValue(double d)
        {
            Type = ValueType.Number;
            Number = d;
        }
        public JsonValue(string s)
        {
            Type = ValueType.String;
            Str = s;
        }
        public JsonValue(JsonValue[] arr)
        {
            Type = ValueType.Array;
            Array = arr;
        }
        public JsonValue(JsonObject jo)
        {
            Type = ValueType.Object;
            Obj = jo;
        }

        #endregion
        
       
        
        public JsonValue this[int index]
        {
            get
            {
                if (Type != ValueType.Array)
                {
                    return default;
                }

                return Array[index];
            }
            set
            {
                if (Type != ValueType.Array)
                {
                    return;
                }

                Array[index] = value;
            }
        }

        public JsonValue this[string key]
        {
            get
            {
                if (Type != ValueType.Object)
                {
                    return default;
                }

                return Obj[key];
            }
            set
            {
                if (Type != ValueType.Object)
                {
                    return;
                }

                Obj[key] = value;
            }
        }

        #region 隐式类型转换

        public static implicit operator JsonValue(bool b)
        {
            JsonValue value = new JsonValue();
            value.Type = ValueType.Boolean;
            value.Boolean = b;
            return value;
        }
        
        public static implicit operator bool(JsonValue value)
        {
            if (value.Type != ValueType.Boolean)
            {
                throw new Exception("JsonValue转换bool失败");
            }
            return value.Boolean;
        }

        public static implicit operator double(JsonValue value)
        {
            if (value.Type != ValueType.Number)
            {
                throw new Exception("JsonValue转换double失败");
            }
            
            return value.Number;
        }
        
        public static implicit operator JsonValue(double d)
        {
            JsonValue value = new JsonValue();
            value.Type = ValueType.Number;
            value.Number = d;
            return value;
        }
        
        public static implicit operator JsonValue(string s)
        {
            JsonValue value = new JsonValue();
            value.Type = ValueType.String;
            value.Str = s;
            return value;
        }
        
        
        public static implicit operator string(JsonValue value)
        {
            if (value.Type != ValueType.String)
            {
                throw new Exception("JsonValue转换string失败");
            }
            return value.Str;
        }
        public static implicit operator JsonValue(JsonObject obj)
        {
            JsonValue value = new JsonValue();
            value.Type = ValueType.Object;
            value.Obj = obj;
            return value;
        }
        
        public static implicit operator JsonValue[](JsonValue value)
        {
            if (value.Type != ValueType.Array)
            {
                throw new Exception("JsonValue转换JsonValue[]失败");
            }

            return value.Array;
        }
        public static implicit operator JsonValue(JsonValue[] arr)
        {
            JsonValue value = new JsonValue();
            value.Type = ValueType.Array;
            value.Array = arr;
            return value;
        }
        
        public static implicit operator JsonObject(JsonValue value)
        {
            if (value.Type != ValueType.Object)
            {
                throw new Exception("JsonValue转换JsonObject失败");
            }

            return value.Obj;
        }

        #endregion
        
        
        public override string ToString()
        {
            string json = JsonParser.ToJson(this);
            return json;
        }
    }

}
