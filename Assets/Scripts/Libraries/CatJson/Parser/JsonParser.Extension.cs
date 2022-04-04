using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
namespace CatJson
{
    public static partial class JsonParser
    {
        static JsonParser()
        {

            //添加自定义扩展解析方法
            //主要是在json值不能直接解析为默认的对应类型时
            //比如"2016/5/9 13:09:55"可能需要被解析为DateTime而不是string
            //同时可以辅助生成的代码


            //解析DateTime
            ExtensionParseFuncDict.Add(typeof(DateTime), () =>
            {
                RangeString rs = Lexer.GetNextTokenByType(TokenType.String);
                return DateTime.Parse(rs.ToString());
            });


            //添加自定义扩展转换Json文本方法
            ExtensionToJsonFuncDict.Add(typeof(DateTime), (value) =>
            {
                TextUtil.Append("\"");
                TextUtil.Append(value.ToString());
                TextUtil.Append("\"");
            });


            //添加需要忽略的非自定义类的字段/属性
            IgnoreSet.Add(typeof(Quaternion), new HashSet<string>()
            {
                nameof(Quaternion.eulerAngles),
            }
            );

            IgnoreSet.Add(typeof(Bounds), new HashSet<string>()
            {
                nameof(Bounds.extents),
                nameof(Bounds.min),
                nameof(Bounds.max),
            }
            );

            IgnoreSet.Add(typeof(Rect), new HashSet<string>()
            {
                nameof(Rect.position),
                nameof(Rect.center),
                nameof(Rect.min),
                nameof(Rect.max),
                nameof(Rect.size),
                nameof(Rect.yMin),
                nameof(Rect.yMax),
                nameof(Rect.xMin),
                nameof(Rect.xMax),
            }
            );

            IgnoreSet.Add(typeof(Keyframe), new HashSet<string>()
            {
                nameof(Keyframe.inWeight),
                nameof(Keyframe.outWeight),
                nameof(Keyframe.weightedMode),
                "tangentMode",
            }
            );

            IgnoreSet.Add(typeof(AnimationCurve), new HashSet<string>()
            {
                nameof(AnimationCurve.preWrapMode),
                nameof(AnimationCurve.postWrapMode),
            }
            );
        }
    }

}
