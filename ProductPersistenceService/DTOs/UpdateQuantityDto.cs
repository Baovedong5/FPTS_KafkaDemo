﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPersistenceService.DTOs
{
    internal class UpdateQuantityDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
