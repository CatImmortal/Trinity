using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity.Hotfix
{
    public class ProcedureHotfixTest : ProcedureBase
    {
        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了热更新测试流程");
        }
    }
}

