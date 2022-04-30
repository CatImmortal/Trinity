using System;

namespace CatJson
{
    /// <summary>
    /// 处理多态序列化/反序列化的Json格式化器
    /// </summary>
    public class PolymorphicFormatter : IJsonFormatter
    {
        /// <summary>
        /// 真实类型key
        /// </summary>
        public const string RealTypeKey = "<>RealType";

        /// <summary>
        /// 对象Json文本key
        /// </summary>
        private const string objectKey = "<>Object";
        
        /// <inheritdoc />
        public void ToJson(object value, Type type, Type realType, int depth)
        {
            TextUtil.AppendLine("{");
                
            //写入真实类型
            TextUtil.Append("\"", depth);
            TextUtil.Append(RealTypeKey);
            TextUtil.Append("\"");
            TextUtil.Append(":");
            TextUtil.Append(GetRealTypeJsonValue(realType));
                
            TextUtil.AppendLine(",");
                
            //写入对象的json文本
            TextUtil.Append("\"", depth);
            TextUtil.Append(objectKey);
            TextUtil.Append("\"");
            TextUtil.Append(":");
            JsonParser.InternalToJson(value,type,realType,depth + 1,false);
                
            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("}", depth);
        }

        /// <inheritdoc />
        public object ParseJson(Type type, Type realType)
        {
           
            //{
            //"<>RealType":"xxxx"
            //在进入此方法前，已经将这之前的部分提取掉了
            
            //接下来只需要提取下面这部分就行
            //","
            //"<>Object":xxxx
            //}
            
            //跳过,
            JsonParser.Lexer.GetNextTokenByType(TokenType.Comma);
            
            //跳过"<>Object"
            JsonParser.Lexer.GetNextTokenByType(TokenType.String);
            
            //跳过 :
            JsonParser.Lexer.GetNextTokenByType(TokenType.Colon);
            
            //读取被多态序列化的对象的Json文本
            object obj = JsonParser.InternalParseJson(type,realType,false);
            
            //跳过}
            JsonParser.Lexer.GetNextTokenByType(TokenType.RightBrace);

            
            
            return obj;
        }
        
        /// <summary>
        /// 获取用于多态序列化的真实类型的json value字符串
        /// </summary>
        private static string GetRealTypeJsonValue(Type realType)
        {
#if FUCK_LUA
            if (realType is ILRuntime.Reflection.ILRuntimeType ilrtType)
            {
                 return $"\"{ilrtType.FullName}\"";
            }
#endif
            return $"\"{realType.FullName},{realType.Assembly.GetName().Name}\"";
        }
    }
}