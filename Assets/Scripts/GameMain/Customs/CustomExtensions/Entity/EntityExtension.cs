using GameFramework;
using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace Trinity
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static EntityLogic GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (EntityLogic)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, EntityLogic entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, EntityLogic entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

       
        /// <summary>
        /// 显示热更新层实体
        /// </summary>
        public static void ShowHotfixEntity(this EntityComponent entityComponent,int priority, HotfixEntityData data)
        {
            entityComponent.ShowEntity(typeof(HotfixEntityLogic),priority, data);
        }
        
        /// <summary>
        /// 显示实体
        /// </summary>
        public static void ShowEntity(this EntityComponent entityComponent, Type logicType, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }
            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), drEntity.EntityGroupName, priority, data);
        }

        /// <summary>
        /// 生成实体序列ID
        /// </summary>
        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
