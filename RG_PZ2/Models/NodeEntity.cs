﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RG_PZ2.Models
{
    class NodeEntity : PowerEntity
    {
        public NodeEntity()
        {
            
        }
        public override void SetDefaultColor()
        {
            shape.Fill = Brushes.DeepSkyBlue;
        }


    }
}
