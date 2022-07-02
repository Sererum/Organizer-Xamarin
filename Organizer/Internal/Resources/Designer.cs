using Organizer.Internal.Data;
using System.Collections.Generic;

namespace Organizer.Internal.Resources
{
    public class Designer
    {
        private static readonly int _toolBar = 0;
        private static readonly int _toolBarElements = 1;
        private static readonly int _main = 2;
        private static readonly int _downPanel = 3;
        private static readonly int _downPanelElements = 4;

        public enum Theme { SunriseOcean }

        private Theme _currentTheme = (Theme) Server.Theme;
        public Theme CurrentTheme
        {
            set
            {
                Server.Theme = (int) value;
                _currentTheme = value;
            }
        }

        public int GetIdToolBarColor () => _themes[_currentTheme][_toolBar];
        public int GetIdToolBarElementsColor () => _themes[_currentTheme][_toolBarElements];
        public int GetIdMainColor () => _themes[_currentTheme][_main];
        public int GetIdDownPanelColor () => _themes[_currentTheme][_downPanel];
        public int GetIdDownPanelElementsColor () => _themes[_currentTheme][_downPanelElements];


        private Dictionary<Theme, int[]> _themes = new Dictionary<Theme, int[]>()
        {
            { Theme.SunriseOcean, new int[] {
                Resource.Color.sunrise_ocean_tool_bar, Resource.Color.sunrise_ocean_tool_bar_elements,
                Resource.Color.sunrise_ocean_main,
                Resource.Color.sunrise_ocean_down_panel, Resource.Color.sunrise_ocean_down_panel_elements
                }
            }

        };
    }
}