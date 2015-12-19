using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace Common.Container.Management
{
    public class AdditionalServiceContainer : ServiceContainer
    {
        private List<Type> serviceTypes;
        private static object syncObj = new object();
        private static AdditionalServiceContainer instance = new AdditionalServiceContainer();

        private AdditionalServiceContainer()
        {
            this.serviceTypes = new List<Type>();
        }

        public static void AddService(IAdditionalService serviceInstance)
        {
            if (instance.serviceTypes == null) return;
            Type serviceType = serviceInstance.GetType();
            if (!instance.serviceTypes.Contains(serviceType))
            {
                lock (syncObj)
                {
                    if (!instance.serviceTypes.Contains(serviceType))
                    {
                        instance.AddService(serviceType, serviceInstance);
                        instance.serviceTypes.Add(serviceType);
                        return;
                    }
                }
            }
            throw new ArgumentException("duplicated service :"+ serviceType.Name);
        }

        public static T GetService<T>() where T : IAdditionalService, new()
        {
            if (instance.serviceTypes == null) return default(T);
            Type serviceType = typeof(T);
            if (!instance.serviceTypes.Contains(serviceType))
            {
                lock (syncObj)
                {
                    if (!instance.serviceTypes.Contains(serviceType))
                    {
                        instance.AddService(serviceType, new T());
                        instance.serviceTypes.Add(serviceType);
                    }
                }
            }
            return (T)instance.GetService(serviceType);
        }

        public static bool ContainsService<T>() where T : IAdditionalService
        {
            if (instance.serviceTypes == null) 
                return false;
            Type serviceType = typeof(T);
            return instance.serviceTypes.Contains(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Type serviceType in serviceTypes)
                {
                    IAdditionalService service = (IAdditionalService)this.GetService(serviceType);
                    this.RemoveService(serviceType);
                    service.Dispose();
                    service = null;
                }
                serviceTypes.Clear();
                serviceTypes = null;
            }
            base.Dispose(disposing);
        }
    }
}
