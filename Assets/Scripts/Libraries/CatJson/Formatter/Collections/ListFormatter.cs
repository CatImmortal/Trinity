using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// List类型的Json格式化器
    /// </summary>
    public class ListFormatter : BaseJsonFormatter<IList>
    {
        /// <inheritdoc />
        public override void ToJson(IList value, Type type, Type realType, int depth)
        {
            TextUtil.AppendLine("[");
            Type listType = type;
            if (!listType.IsGenericType)
            {
                listType = realType;
            }
            Type elementType = TypeUtil.GetArrayOrListElementType(listType);
            for (int i = 0; i < value.Count; i++)
            {
                object element = value[i];
                TextUtil.AppendTab(depth + 1);
                if (element == null)
                {
                    TextUtil.Append("null");
                }
                else
                {
                    JsonParser.InternalToJson(element,elementType,null,depth + 1);
                }
                if (i < value.Count - 1)
                {
                    TextUtil.AppendLine(",");
                }
                 
            }
            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("]", depth);
        }

        /// <inheritdoc />
        public override IList ParseJson(Type type, Type realType)
        {
            IList list = (IList)TypeUtil.CreateInstance(realType);
            
            Type listType = type;
            if (!listType.IsGenericType)
            {
                listType = realType;
            }
            Type elementType = TypeUtil.GetArrayOrListElementType(listType);
            
            ParserHelper.ParseJsonArrayProcedure(list, elementType, (userdata1, userdata2) =>
            {
                object value = JsonParser.InternalParseJson((Type) userdata2);
                ((IList) userdata1).Add(value);
            });

            return list;
        }
    }
}