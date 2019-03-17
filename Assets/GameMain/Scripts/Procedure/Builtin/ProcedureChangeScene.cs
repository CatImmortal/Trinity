using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Procedure;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Trinity
{
    public partial class ProcedureChangeScene : ProcedureBase
    {
        /// <summary>
        /// 是否切换场景完毕
        /// </summary>
        private bool m_IsChangeSceneComplete = false;

        /// <summary>
        /// 背景音乐ID
        /// </summary>
        private int m_BackgroundMusicId = 0;

        /// <summary>
        /// 要切换目标场景ID
        /// </summary>
        private int m_TargetSceneId = 0;

        /// <summary>
        /// 场景ID-流程切换方法的字典
        /// </summary>
        private Dictionary<int, Action> m_TargetProcedureChange = new Dictionary<int, Action>();

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            //TODO:在这里配置场景ID与要切换到的对应流程的方法
            m_TargetProcedureChange.Add((int)SceneId.TestScene, () => ChangeState<ProcedureTest>(procedureOwner));
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_IsChangeSceneComplete = false;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            //读取下一场景ID，根据ID读取场景表中的行数据
            m_TargetSceneId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(m_TargetSceneId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", m_TargetSceneId.ToString());
                return;
            }

            //根据行数据里的场景资源名加载场景
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);
            m_BackgroundMusicId = drScene.BackgroundMusicId;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_IsChangeSceneComplete)
            {
                return;
            }

            //根据切换到的目标场景ID进行对应的流程切换
            if (m_TargetProcedureChange.ContainsKey(m_TargetSceneId))
            {
                m_TargetProcedureChange[m_TargetSceneId]?.Invoke();
            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            //场景加载完毕后播放背景音乐
            if (m_BackgroundMusicId > 0)
            {
                GameEntry.Sound.PlayMusic(m_BackgroundMusicId);
            }

            m_IsChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }
    }
}
