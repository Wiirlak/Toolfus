using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;

namespace Toolfus
{
    public class Keybind
    {
        public char Up;
        public char Down;
        public char Left;
        public char Right;
        public char Follow;
        public char Switch;

        public Keybind()
        {
            // SetDefault();
            GetConfig();
        }

        public void GetConfig()
        {
            try
            {
                Up = ConfigurationManager.AppSettings.GetValues("KeyUp")[0][0];
                Down = ConfigurationManager.AppSettings.GetValues("KeyDown")[0][0];
                Left = ConfigurationManager.AppSettings.GetValues("KeyLeft")[0][0];
                Right = ConfigurationManager.AppSettings.GetValues("KeyRight")[0][0];
                Follow = ConfigurationManager.AppSettings.GetValues("KeyFollow")[0][0];
                Switch = ConfigurationManager.AppSettings.GetValues("KeySwitch")[0][0];
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e);
                try
                {
                    SetDefault();
                }
                catch (Exception fatal)
                {
                    Environment.Exit(-1);  
                }
            }
        }

        public bool InMoveKeyList(char a)
        {
            char[] keys = {Up,Down,Left,Right} ;
            return ((IList) keys).Contains(a);
        }

        public void SetDefault()
        {
            Edit("KeyUp",     "8");
            Edit("KeyDown",   "5");
            Edit("KeyLeft",   "4");
            Edit("KeyRight",  "6");
            Edit("KeyFollow", "2");
            Edit("KeySwitch", "9");
        }

        public void Edit(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                
                if (settings[key] == null) settings.Add(key, value);
                else settings[key].Value = value;
                
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
            GetConfig();
        }
    }
}