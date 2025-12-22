using Root.Scripts.Services.Core.Abstractions;
using Root.Scripts.Utilities.Guards;
using UnityEngine;

namespace Root.Scripts.Services.Core
{
    public static class GameServices
    {
        private static readonly GameServicesContext Context;
        
        static GameServices()
        {
            Context = Object.FindObjectOfType<GameServicesContext>();
        }

        public static T Get<T>() where T : AService
        {
            Guard.Against.Null(Context, nameof(GameServicesContext));
            return Context.Resolve<T>();
        }
    }
}