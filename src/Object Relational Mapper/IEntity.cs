﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Object_Relational_Mapper
{
    public interface IEntity<G>
    {
        G Id { get; set; }
    }
}
