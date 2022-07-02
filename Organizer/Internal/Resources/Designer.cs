using Organizer.Internal.Data;
using System.Collections.Generic;

namespace Organizer.Internal.Resources
{
    public class Designer
    {
        private static readonly int _toolBar = 0;
        private static readonly int _toolBarElements = 1;
        private static readonly int _main = 2;
        private static readonly int _text = 3;
        private static readonly int _downPanel = 4;
        private static readonly int _downPanelElements = 5;

        public enum Theme { Neon, Check }

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
        public int GetIdToolBarElementsColor () => _themes[_currentTheme][_toolBarElements];
        public int GetIdMainColor () => _themes[_currentTheme][_main];
        public int GetIdTextColor () => _themes[_currentTheme][_text];
        public int GetIdDownPanelColor () => _themes[_currentTheme][_downPanel];
        public int GetIdDownPanelElementsColor () => _themes[_currentTheme][_downPanelElements];


        private Dictionary<Theme, int[]> _themes = new Dictionary<Theme, int[]>()
        {
            { Theme.Check, new int[] 
                {
                    Resource.Color.check_tool_bar, Resource.Color.check_tool_bar_elements,
                    Resource.Color.check_main, Resource.Color.check_text,
                    Resource.Color.check_down_panel, Resource.Color.check_down_panel_elements
                }
            },
            { Theme.Neon, new int[]
                {
                    Resource.Color.neon_tool_bar, Resource.Color.neon_tool_bar_elements,
                    Resource.Color.neon_main, Resource.Color.neon_text,
                    Resource.Color.neon_down_panel, Resource.Color.neon_down_panel_elements
                }
            }
        };
    }
}