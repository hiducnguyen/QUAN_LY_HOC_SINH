﻿using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private IUnitOfWork _unitOfWork;
        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Delete(object o)
        {
            _unitOfWork.Session.Delete(o);
        }

        public object Save(object o)
        {
            return _unitOfWork.Session.Save(o);
        }

        public void Update(object o)
        {
            _unitOfWork.Session.Update(o);
        }
    }
}
