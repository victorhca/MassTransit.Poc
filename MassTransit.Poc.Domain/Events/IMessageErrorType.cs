﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Domain.Events
{
    public interface IMessageErrorType
    {
        int Id { get; }
    }
}
