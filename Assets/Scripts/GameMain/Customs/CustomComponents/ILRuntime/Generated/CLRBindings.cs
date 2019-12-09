using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3> s_UnityEngine_Vector3_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion> s_UnityEngine_Quaternion_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2> s_UnityEngine_Vector2_Binding_Binder = null;

        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Text_StringBuilder_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Collections_IEnumerable_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Type_Binding.Register(app);
            Google_Protobuf_ByteString_Binding.Register(app);
            ETModel_ByteHelper_Binding.Register(app);
            System_Array_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Reflection_PropertyInfo_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Exception_Binding.Register(app);
            System_Collections_IDictionary_Binding.Register(app);
            ETModel_Log_Binding.Register(app);
            ETModel_IdGenerater_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            ETModel_MongoHelper_Binding.Register(app);
            System_Activator_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_ILTypeInstance_Binding.Register(app);
            ETModel_UnOrderMultiMap_2_Type_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_Int64_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            Trinity_GameEntry_Binding.Register(app);
            Trinity_ILRuntimeComponent_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding_Enumerator_Binding.Register(app);
            GameFramework_Utility_Binding_Json_Binding.Register(app);
            System_Collections_Generic_Queue_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Type_ILTypeInstance_Binding.Register(app);
            Google_Protobuf_ProtoPreconditions_Binding.Register(app);
            Google_Protobuf_CodedOutputStream_Binding.Register(app);
            Google_Protobuf_CodedInputStream_Binding.Register(app);
            Google_Protobuf_MessageParser_1_Google_Protobuf_IMessageAdaptor_Binding_Adaptor_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Google_Protobuf_IMessageAdaptor_Binding_Adaptor_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_String_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Int32_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Int64_Binding.Register(app);
            Google_Protobuf_FieldCodec_Binding.Register(app);
            System_NotImplementedException_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_UInt16_List_1_ILTypeInstance_Binding.Register(app);
            ETModel_Game_Binding.Register(app);
            ETModel_Entity_Binding.Register(app);
            ETModel_OpcodeTypeComponent_Binding.Register(app);
            ETModel_MessageProxy_Binding.Register(app);
            ETModel_MessageDispatcherComponent_Binding.Register(app);
            ETModel_MessageInfo_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_Queue_1_Object_Binding.Register(app);
            System_Collections_Generic_Queue_1_Object_Binding.Register(app);
            ETModel_DoubleMap_2_UInt16_Type_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_UInt16_Object_Binding.Register(app);
            ETModel_MessageAttribute_Binding.Register(app);
            ETModel_SessionCallbackComponent_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Action_1_Google_Protobuf_IMessageAdaptor_Binding_Adaptor_Binding.Register(app);
            ETModel_Session_Binding.Register(app);
            System_Action_1_Google_Protobuf_IMessageAdaptor_Binding_Adaptor_Binding.Register(app);
            ETModel_Component_Binding.Register(app);
            ETModel_NetworkComponent_Binding.Register(app);
            ETModel_IMessagePacker_Binding.Register(app);
            ETModel_OpcodeHelper_Binding.Register(app);
            ETModel_StringHelper_Binding.Register(app);
            ETModel_ETTaskCompletionSource_1_Google_Protobuf_IMessageAdaptor_Binding_Adaptor_Binding.Register(app);
            System_Threading_CancellationToken_Binding.Register(app);
            ETModel_ErrorCode_Binding.Register(app);
            ETModel_RpcException_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            GameFramework_GameFrameworkException_Binding.Register(app);
            Trinity_ETNetworkComponent_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            GameFramework_Utility_Binding_Text_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Variable_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            UnityGameFramework_Runtime_Log_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Action_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneSuccessEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_EventComponent_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneFailureEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneUpdateEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneDependencyAssetEventArgs_Binding.Register(app);
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
            UnityGameFramework_Runtime_VarInt_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncVoidMethodBuilder_Binding.Register(app);
            UnityEngine_Input_Binding.Register(app);
            ETModel_ETTask_1_Google_Protobuf_IMessageAdaptor_Binding_Adaptor_Binding.Register(app);
            ETModel_ETTask_1_Google_Protobuf_IMessageAdaptor_Binding_Adaptor_Binding_Awaiter_Binding.Register(app);
            System_Collections_Generic_ICollection_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_IEnumerable_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_IEnumerator_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_IDictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_LinkedList_1_EventHandler_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_LinkedList_1_EventHandler_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_LinkedListNode_1_EventHandler_1_ILTypeInstance_Binding.Register(app);
            System_EventHandler_1_ILTypeInstance_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector3));
            s_UnityEngine_Vector3_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Quaternion));
            s_UnityEngine_Quaternion_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector2));
            s_UnityEngine_Vector2_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2>;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            s_UnityEngine_Vector3_Binding_Binder = null;
            s_UnityEngine_Quaternion_Binding_Binder = null;
            s_UnityEngine_Vector2_Binding_Binder = null;
        }
    }
}
