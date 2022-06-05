using IronstoneSettings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class Profile : IProfile
    {
        public Profile(string id, string org, string label)
        {
            Properties = new Dictionary<string, PropertyKeyValuePair[]>();

            this.Id = id;
            Label = label;
            Org = org;                            
        }

        public string Id { get; set; }        

        public string Label { get; set; }

        public string Org { get { return GetProperty("org"); } set { SetProperty("org", value); } }        

        public string Email { get { return GetProperty("email"); } set { SetProperty("email", value); } }

        public string Name { get { return GetProperty("name"); } set { SetProperty("name", value); } }

        public string Surname { get { return GetProperty("surname"); } set { SetProperty("surname", value); } }

        public Dictionary<string, PropertyKeyValuePair[]> Properties { get; set; }
        public MainSettings IronstoneSettings { get { return GetSettings(); } set { SetSettings(value); } }

        public static Profile FromDynamic(dynamic result)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string json = JsonSerializer.Serialize(result);
            Profile p = JsonSerializer.Deserialize<Profile>(json, options);            
            return p;
        }

        public class PropertyKeyValuePair
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }

        private string GetProperty(string id)
        {
            if(Properties.ContainsKey(id))
                return Properties[id][0].Value;

            return String.Empty;
        }

        private void SetProperty(string id, string newValue)
        {
            if (Properties.ContainsKey(id))
                Properties[id][0].Value = newValue;

            Properties[id] = new PropertyKeyValuePair[] { new PropertyKeyValuePair() { Id = id, Value = newValue } };
        }

        private MainSettings _cachedSettings;

        private MainSettings GetSettings()
        {            
            if (_cachedSettings != null)
                return _cachedSettings;

            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string settingData = GetProperty("ironstonesettings");

            if (string.IsNullOrEmpty(settingData))
            {                
                return new MainSettings();
            }
            else
            {
                Console.WriteLine("Decache called");
                _cachedSettings = JsonSerializer.Deserialize<MainSettings>(settingData, options);
                return _cachedSettings;
            }            
        }

        private void SetSettings(MainSettings settings)
        {            
            _cachedSettings = settings;
        }

        public string GetUpdateString()
        {
            return GetCreationString();
        }

        public string GetCreationString()
        {           
            StringBuilder build = new StringBuilder( $"g.addV('{Label}')");
            build.Append($".property('id','{Id}')");

            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string settings = JsonSerializer.Serialize(_cachedSettings, options);
            SetProperty("ironstonesettings", settings);

            foreach (var prop in Properties)
            {
                var key = prop.Key;
                var propEntry = prop.Value[0];
                build.Append($".property('{key}','{propEntry.Value}')");
            }

            return build.ToString();
            
        }
    }
}
