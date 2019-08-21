//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace Trinity.Hotfix
{
    /// <summary>
    /// 有限状态机事件响应函数。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="sender">事件源。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void FsmEventHandler(IFsm fsm, object sender, object userData);
}
