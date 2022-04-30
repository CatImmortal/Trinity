using System;

namespace CatJson
{
    /// <summary>
    /// Json解析器辅助类
    /// </summary>
    public static class ParserHelper
    {
        /// <summary>
        /// 解析Json对象的通用流程
        /// </summary>
        public static void ParseJsonObjectProcedure(object userdata1,object userdata2,bool isIntKey,Action<object,object,bool,RangeString> action)
        {
            //跳过 {
            JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while ( JsonParser.Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                RangeString key =  JsonParser.Lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                JsonParser.Lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                action(userdata1,userdata2,isIntKey,key);

                //此时value已经被提取了
                
                //有逗号就跳过逗号
                if ( JsonParser.Lexer.LookNextTokenType() == TokenType.Comma)
                {
                     JsonParser.Lexer.GetNextTokenByType(TokenType.Comma);

                    if ( JsonParser.Lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new Exception("Json对象不能以逗号结尾");
                    }
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }

            }

            //跳过 }
             JsonParser.Lexer.GetNextTokenByType(TokenType.RightBrace);
        }

        /// <summary>
        /// 解析Json数组的通用流程
        /// </summary>
        public static void ParseJsonArrayProcedure(object userdata1,object userdata2, Action<object,object> action)
        {
            //跳过[
             JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBracket);

            while ( JsonParser.Lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                action(userdata1,userdata2);

                //此时value已经被提取了
                
                //有逗号就跳过
                if ( JsonParser.Lexer.LookNextTokenType() == TokenType.Comma)
                {
                     JsonParser.Lexer.GetNextTokenByType(TokenType.Comma);

                    if ( JsonParser.Lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new Exception("数组不能以逗号结尾");
                    }
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }
            }

            //跳过]
             JsonParser.Lexer.GetNextTokenByType(TokenType.RightBracket);
        }
        
        /// <summary>
        /// 尝试从多态序列化的Json文本中读取真实类型
        /// </summary>
        public static bool TryParseRealType(Type type, out Type realType)
        {
            realType = null;
            
            if (JsonParser.Lexer.LookNextTokenType() == TokenType.LeftBrace)
            {
                int curIndex = JsonParser.Lexer.GetCurIndex(); //记下当前lexer的index，是在{后的第一个字符上
                JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBrace); // {
                
                RangeString rs = JsonParser.Lexer.GetNextToken(out TokenType tokenType);
                if (tokenType == TokenType.String && rs.Equals(new RangeString(PolymorphicFormatter.RealTypeKey))) //"<>RealType"
                {
                    //是被多态序列化的 获取真实类型
                    JsonParser.Lexer.GetNextTokenByType(TokenType.Colon); // :
                    
                    rs = JsonParser.Lexer.GetNextTokenByType(TokenType.String); //RealType Value
                    realType = TypeUtil.GetRealType(type, rs.ToString());  //获取真实类型

                    return true;
                }
               
                //不是被多态序列化的
                //回退到前一个{的位置上，并将缓存置空，因为被look过所以需要-1
                JsonParser.Lexer.SetCurIndex(curIndex - 1);
                return false;


            }

            return false;

        }
    }
}