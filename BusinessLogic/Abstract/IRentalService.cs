﻿using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Abstract
{
    public interface IRentalService
    {
        IDataResult<List<Rental>> GetAll();
        IDataResult<Rental> GetById(int id);
        IResult Add(Rental rental);
        IResult Delete(Rental rental);
        IResult Update(Rental rental);
        public IDataResult<List<RentalDetailDto>> GetRentalDetails();
        IResult RentalCarControl(int carId);
        public  IResult CheckFindexPuanIsEnough(int carId, int customerId);
        
        }
}
