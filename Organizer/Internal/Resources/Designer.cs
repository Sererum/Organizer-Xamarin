using Organizer.Internal.Data;
using System.Collections.Generic;

namespace Organizer.Internal.Resources
{
    public class Designer
    {
        private static readonly int _toolBar = 0;
        private static readonly int _elements = 1;
        private static readonly int _main = 2;
        private static readonly int _text = 3;
        private static readonly int _task = 4;
        private static readonly int _downPanel = 5;
        private static readonly int _downButtons = 6;

        public enum Theme { Main, Soft, Purple, MainDark, DeepWater, DarkPurple }

        private Theme _currentTheme = (Theme) Server.Theme;
        public Theme CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                Server.Theme = (int) value;
                _currentTheme = value;
            }
        }

        public int GetIdToolBarColor () => _themes[_currentTheme][_toolBar];
        public int GetIdElementsColor () => _themes[_currentTheme][_elements];
        public int GetIdMainColor () => _themes[_currentTheme][_main];
        public int GetIdTextColor () => _themes[_currentTheme][_text];
        public int GetIdTaskColor () => _themes[_currentTheme][_task];
        public int GetIdDownPanelColor () => _themes[_currentTheme][_downPanel];
        public int GetIdDownButtonsColor () => _themes[_currentTheme][_downButtons];


        private Dictionary<Theme, int[]> _themes = new Dictionary<Theme, int[]>()
        {
            { Theme.Main, new int[] 
                {
                    Resource.Color.main_tool_bar, Resource.Color.main_elements,
                    Resource.Color.main_main, Resource.Color.main_text, Resource.Color.main_task,
                    Resource.Color.main_down_panel, Resource.Color.main_down_buttons
                }
            },
            { Theme.Soft, new int[]
                {
                    Resource.Color.soft_tool_bar, Resource.Color.soft_elements,
                    Resource.Color.soft_main, Resource.Color.soft_text, Resource.Color.soft_task,
                    Resource.Color.soft_down_panel, Resource.Color.soft_down_buttons
                }
            },
            { Theme.Purple, new int[]
                {
                    Resource.Color.purple_tool_bar, Resource.Color.purple_elements,
                    Resource.Color.purple_main, Resource.Color.purple_text, Resource.Color.purple_task,
                    Resource.Color.purple_down_panel, Resource.Color.purple_down_buttons
                }
            },
            { Theme.MainDark, new int[]
                {
                    Resource.Color.main_dark_tool_bar, Resource.Color.main_dark_elements,
                    Resource.Color.main_dark_main, Resource.Color.main_dark_text, Resource.Color.main_dark_task,
                    Resource.Color.main_dark_down_panel, Resource.Color.main_dark_down_buttons
                }
            },
            { Theme.DeepWater, new int[]
                {
                    Resource.Color.deep_water_tool_bar, Resource.Color.deep_water_elements,
                    Resource.Color.deep_water_main, Resource.Color.deep_water_text, Resource.Color.deep_water_task,
                    Resource.Color.deep_water_down_panel, Resource.Color.deep_water_down_buttons
                }
            },
            { Theme.DarkPurple, new int[]
                {
                    Resource.Color.dark_purple_tool_bar, Resource.Color.dark_purple_elements,
                    Resource.Color.dark_purple_main, Resource.Color.dark_purple_text, Resource.Color.dark_purple_task,
                    Resource.Color.dark_purple_down_panel, Resource.Color.dark_purple_down_buttons
                }
            },
        };
    }
}