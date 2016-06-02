using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    class SettingsViewModel : IObserver<KeyValuePair<string, string>>
    {
        private string Id;
        private readonly IPersonalSettings _personalSettings;

        public SettingsViewModel(IPersonalSettings personalSettings, string id)
        {
            Id = id;
            _personalSettings = personalSettings;
            personalSettings.SubscribeSetting("MountedItems-29eff31a-8bd2-40a2-bdac-c020db132c8b").Subscribe(this);
            personalSettings.Dispose();
        }

        public void OnNext(KeyValuePair<string, string> value)
        {
            if (value.Key == "MountedItems-29eff31a-8bd2-40a2-bdac-c020db132c8b")
            {
                var index = value.Value.IndexOf(Id);
                if (index == -1)
                {                  
                    _personalSettings.ChangeSettingValue(value.Key, value.Value + ";" + Id);
                }
            }
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnCompleted()
        {
            
        }
    }
}
