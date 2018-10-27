using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Type_Binding.Register(app);
            GameFramework_GameFrameworkException_Binding.Register(app);
            GameFramework_Utility_Binding_Text_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Variable_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            UnityGameFramework_Runtime_Log_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Action_Binding.Register(app);
            Trinity_GameEntry_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneSuccessEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_EventComponent_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneFailureEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneUpdateEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneDependencyAssetEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_SoundComponent_Binding.Register(app);
            UnityGameFramework_Runtime_EntityComponent_Binding.Register(app);
            UnityGameFramework_Runtime_SceneComponent_Binding.Register(app);
            UnityGameFramework_Runtime_BaseComponent_Binding.Register(app);
            GameFramework_Variable_1_Int32_Binding.Register(app);
            UnityGameFramework_Runtime_DataTableComponent_Binding.Register(app);
            GameFramework_DataTable_IDataTable_1_DRScene_Binding.Register(app);
            System_Int32_Binding.Register(app);
            Trinity_DRScene_Binding.Register(app);
            Trinity_AssetUtility_Binding.Register(app);
            System_Action_Binding.Register(app);
            Trinity_SoundExtension_Binding.Register(app);
            System_Single_Binding.Register(app);
            UnityGameFramework_Runtime_ConfigComponent_Binding.Register(app);
            UnityGameFramework_Runtime_VarInt_Binding.Register(app);
            System_Collections_Generic_ICollection_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Threading_Monitor_Binding.Register(app);
            System_Collections_Generic_IEnumerable_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_IEnumerator_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_Activator_Binding.Register(app);
            System_Collections_Generic_IDictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_IReference_Binding.Register(app);
            GameFramework_IReference_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
