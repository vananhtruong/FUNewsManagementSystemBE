﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CreateTransaction();
        Task Commit();
        Task Rollback();
        Task SaveChange();
    }
}
