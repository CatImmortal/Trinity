using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity
{
    public class ProcedureTest : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了测试流程");

            UIForm uiForm = await GameEntry.UI.AwaitOpenUIForm(UIFormId.TestForm);
            Log.Info("打开了测试界面");
        }

    }
}

