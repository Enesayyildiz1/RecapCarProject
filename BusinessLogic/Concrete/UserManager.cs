﻿using BusinessLogic.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Aspects.Autofac.Validation;
using BusinessLogic.Constants;
using Entities.Dtos;
using BusinessLogic.ValidationRules.FluentValidation;
using Core.Utilities.Security.Hashing;
using Core.Aspects.Autofac.Transcation;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Caching;

namespace BusinessLogic.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        //[SecuredOperation("admin,user.admin")]
        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult(Messages.UserDeleted);
        }

        [CacheRemoveAspect("IUserManager.Get")]
        //[SecuredOperation("admin,user.admin")]
        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult(Messages.UserAdded);
        }

        [CacheRemoveAspect("IUserManager.Get")]
        //[SecuredOperation("admin,user.admin")]
        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdated);
        }

        [PerformanceScopeAspect(5)]
        [CacheAspect]
        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll());
        }

        public IDataResult<User> GetByUserId(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == id));
        }

        [PerformanceScopeAspect(5)]
        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IDataResult<OperationClaim> GetClaimById(int userId)
        {
            return new SuccessDataResult<OperationClaim>(_userDal.GetClaimById(userId));
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        [TransactionScopeAspect]
        public IResult TransactionalTest(User user)
        {
            _userDal.Add(user);
            _userDal.Update(user);
            return null;
        }


        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Email == email));
        }

        public IResult EditProfile(UserUpdateDto user)
        {
            byte[] passwordHash;
            byte[] passwordSalt;

            HashingHelper.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            var userInfo = new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };

            _userDal.Update(userInfo);
            return new SuccessResult(Messages.UserUpdated);
        }
    }
}
