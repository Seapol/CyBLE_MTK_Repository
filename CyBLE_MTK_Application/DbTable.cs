using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyBLE_MTK_Application
{
    public class DbTable
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public SFCS_DB_Helper SFCS_DB_Helper
        {
            get => default(SFCS_DB_Helper);
            set
            {
            }
        }

        private string[] colTitles;

        public string[] ColTitles
        {
            get { return colTitles; }
            set { colTitles = value; }
        }

    }
}