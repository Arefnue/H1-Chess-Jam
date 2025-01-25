using System;
using System.Collections.Generic;
using _NueCore.Common.NueLogger;
using _NueCore.ManagerSystem.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.NComponentSystem
{
    public class NComponentManager : NManagerBase
    {
        #region Cache
        public static NComponentManager Instance { get; private set; }
        [ShowInInspector,ReadOnly]public List<NComponentBase> AllComponentsList { get; private set; } = new List<NComponentBase>();
        [ShowInInspector,ReadOnly]public List<NComponentBase> NormalUpdateComponentsList { get; private set; } = new List<NComponentBase>();
        [ShowInInspector,ReadOnly]public List<NComponentBase> FixedUpdateComponentsList { get; private set; } = new List<NComponentBase>();
        [ShowInInspector,ReadOnly]public List<NComponentBase> LateUpdateComponentsList { get; private set; } = new List<NComponentBase>();
        [ShowInInspector,ReadOnly]public List<NComponentBase> UnscaledUpdateComponentsList { get; private set; } = new List<NComponentBase>();
        

        #endregion

        #region Setup
        public override void NAwake()
        {
            Instance = InitSingleton<NComponentManager>();
            base.NAwake();
        }

        #endregion

        #region Methods
        public void AddNComponent(NComponentBase comp)
        {
            if (comp == null)
                return;
            AllComponentsList.Add(comp);
            switch (comp.UpdateType)
            {
                case UpdateTypeEnums.None:
                    break;
                case UpdateTypeEnums.Normal:
                    NormalUpdateComponentsList.Add(comp);
                    break;
                case UpdateTypeEnums.Fixed:
                    FixedUpdateComponentsList.Add(comp);
                    break;
                case UpdateTypeEnums.Late:
                    LateUpdateComponentsList.Add(comp);
                    break;
                case UpdateTypeEnums.Unscaled:
                    UnscaledUpdateComponentsList.Add(comp);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void RemoveNComponent(NComponentBase comp)
        {
            if (comp == null)
                return;
            AllComponentsList.Remove(comp);
            switch (comp.UpdateType)
            {
                case UpdateTypeEnums.None:
                    break;
                case UpdateTypeEnums.Normal:
                    NormalUpdateComponentsList.Remove(comp);
                    break;
                case UpdateTypeEnums.Fixed:
                    FixedUpdateComponentsList.Remove(comp);
                    break;
                case UpdateTypeEnums.Late:
                    LateUpdateComponentsList.Remove(comp);
                    break;
                case UpdateTypeEnums.Unscaled:
                    UnscaledUpdateComponentsList.Remove(comp);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Process

        private void Update()
        {
            foreach (var @base in NormalUpdateComponentsList)
            {
                if (@base == null)
                {
                    "<<NComponent>> Update Null".NLog(Color.red);
                    continue;
                }

                if (!@base.CanUpdateComponent())
                    continue;
                @base.TickComponent(Time.deltaTime);
            }
            
            foreach (var @base in UnscaledUpdateComponentsList)
            {
                if (@base == null)
                {
                    "<<NComponent>> Unscaled Null".NLog(Color.red);
                    continue;
                }
                
                if (!@base.CanUpdateComponent())
                    continue;
                @base.TickComponent(Time.unscaledDeltaTime);
            }
        }
        private void LateUpdate()
        {
            foreach (var @base in LateUpdateComponentsList)
            {
                if (@base == null)
                {
                    "<<NComponent>> Late Null".NLog(Color.red);
                    continue;
                }
                if (!@base.CanUpdateComponent())
                    continue;
                @base.TickComponent(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (var @base in FixedUpdateComponentsList)
            {
                if (@base == null)
                {
                    "<<NComponent>> Fixed Null".NLog(Color.red);
                    continue;
                }
                if (!@base.CanUpdateComponent())
                    continue;
                @base.TickComponent(Time.fixedDeltaTime);
            }
        }

       
        #endregion
    }
}