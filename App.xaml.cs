using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
 
namespace blueshell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] Args;
        public static Dictionary<string, object> parsedArgs;
        void app_Startup(object sender, StartupEventArgs e)
        {
            // If no command line arguments were provided, don't process them if (e.Args.Length == 0) return;  
            if (e.Args.Length > 0)
            {
                Args = e.Args;
                parsedArgs = new Dictionary<string, object>();
                parsedArgs.Add("mute", false);
                parsedArgs.Add("src", Args[Args.Length-1]);
                parsedArgs.Add("opacity", 1f);
                parsedArgs.Add("duration", float.PositiveInfinity);
                parsedArgs.Add("loop", false);
                parsedArgs.Add("allScreens", false);
                for (int i = 0; i < Args.Length - 1; i++)
                {
                    switch (Args[i])
                    {
                        case "-m":
                            parsedArgs["mute"] = true;
                            break;
                        case "-o":
                            parsedArgs["opacity"] = float.Parse(Args[i + 1]);
                            i++; // skip next arg as we just read it
                            break;
                        case "-d":
                            parsedArgs["duration"] = float.Parse(Args[i + 1]);
                            i++; // skip next arg as we just read it
                            break;
                        case "-l":
                            parsedArgs["loop"] = true;
                            break;
                        case "-as":
                            parsedArgs["allScreens"] = true;
                            break;
                    }
                }
            }
        }
    }
}
