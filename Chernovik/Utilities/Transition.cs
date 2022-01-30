using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chernovik.Entities;
using System.Windows.Controls;

namespace Chernovik.Utilities
{
    internal class Transition
    {
        public static Frame mainFrame { get; set; }
        private static DraftEntities _context {  get; set; }
        public static DraftEntities Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new DraftEntities();
                }
                return _context;
            }
        }
    }
}
