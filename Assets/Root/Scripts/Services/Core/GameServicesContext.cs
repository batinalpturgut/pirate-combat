using System;
using System.Collections.Generic;
using Root.Scripts.Services.Core.Abstractions;
using Root.Scripts.Services.Core.Attributes;
using UnityEngine;

namespace Root.Scripts.Services.Core
{
    [DefaultExecutionOrder(-9999)]
    public class GameServicesContext : AContext
    {
        [SerializeField]
        private AService[] serviceList;
        
        [SerializeField, HideInInspector]
        private Dictionary<AService, Type> serviceInterfaceDict = new Dictionary<AService, Type>();

        protected override bool IsPersistent => true;
        protected override void RegisterServices()
        {
            foreach (AService service in serviceList)
            {
                Register(service, service.GetType());

                if (serviceInterfaceDict.TryGetValue(service, out Type iface))
                {
                    Register(service, iface);
                }
            }
        }

        protected override void InitServices()
        {
            foreach (AService service in serviceList)
            {
                service.Initialize(this);
            }
        }
        
        private void OnValidate()
        {
            serviceInterfaceDict.Clear();

            foreach (AService service in serviceList)
            {
                Type[] interfaces = service.GetType().GetInterfaces();

                foreach (Type iface in interfaces)
                {
                    if (Attribute.IsDefined(iface, typeof(ServiceAPIAttribute)))
                    {
                        serviceInterfaceDict[service] = iface;
                    }
                }
            }
        }
    }
}