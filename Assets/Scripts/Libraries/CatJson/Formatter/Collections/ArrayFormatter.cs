using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// 数组类型的Json格式化器
    /// </summary>
    public class ArrayFormatter : BaseJsonFormatter<Array>
    {
        /// <inheritdoc />
        public override void ToJson(Array value, Type type, Type realType, int depth)
        {
            TextUtil.AppendLine("[");
            Type arrayType = type;
            if (!type.IsArray)
            {
                arrayType = realType;
            }
            Type elementType = TypeUtil.GetArrayOrListElementType(arrayType);
            for (int i = 0; i < value.Length; i++)
            {
                object element = value.GetValue(i);
                TextUtil.AppendTab(depth + 1);
                if (element == null)
                {
                    TextUtil.Append("null");
                }
                else
                {
                    JsonParser.InternalToJson(element,elementType,null,depth + 1);
                }
                if (i < value.Length-1)
                {
                    TextUtil.AppendLine(",");
                }
                 
            }
            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("]", depth);
        }

        /// <inheritdoc />
        public override Array ParseJson(Type type, Type realType)
        {
            List<object> list = new List<object>();
            Type arrayType = type;
            if (!type.IsArray)
            {
                arrayType = realType;
            }
            Type elementType = TypeUtil.GetArrayOrListElementType(arrayType);
            ParserHelper.ParseJsonArrayProcedure(list, elementType, (userdata1, userdata2) =>
            {
                object value = JsonParser.InternalParseJson((Type) userdata2);
                ((IList) userdata1).Add(value);
            });
            
            Array array = Array.CreateInstance(elementType, list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                object element = list[i];
                array.SetValue(element, i);
            }

            return array;
        }
    }
}