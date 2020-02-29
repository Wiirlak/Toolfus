using System;
using System.Configuration;

namespace Toolfus
{
    public class Keybind
    {
        public char Up;
        public char Down;
        public char Left;
        public char Right;
        public char Follow;

        public Keybind()
        {
            if (ConfigurationManager.AppSettings["default"] == "true")
            {
                Up = '8';
                Down = '5';
                Left = '4';
                Right = '6';
                Follow = '2';
            }
            else
            {
                Up = ConfigurationManager.AppSettings.GetValues("KeyUp")[0][0];
                Down = ConfigurationManager.AppSettings.GetValues("KeyDown")[0][0];
                Left = ConfigurationManager.AppSettings.GetValues("KeyLeft")[0][0];
                Right = ConfigurationManager.AppSettings.GetValues("KeyRight")[0][0];
                Follow = ConfigurationManager.AppSettings.GetValues("KeyFollow")[0][0];
            }
        }

        public void updateKey(string configName, string newValue)
        {
            
        }
    }
}