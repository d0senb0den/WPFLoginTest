using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFLoginTest
{
    public class UserCircle
    {
        public string Name { get; set; }
        public Color Background { get; set; }

        UserCircle[] userCircles;
        
        public UserCircle(string name, Color background)
        {
            Name = name;
            Background = background;
        }

    }
}
