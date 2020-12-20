﻿using RoomMate.Data.Context;
using RoomMate.Data.Repository;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RoomMateDbContext _context;
        public IUserRepository UsersRepository { get; }
        public IGenericRepository<Room> RoomsRepository { get; }
        public IGenericRepository<Equipment> EquipmentRepository { get;  }
        public IGenericRepository<Address> AddressRepository { get;  }
        public UnitOfWork(RoomMateDbContext context)
        {
            _context = context;
            UsersRepository = new UserRepository(_context);
            RoomsRepository = new GenericRepository<Room>(_context);
            EquipmentRepository = new GenericRepository<Equipment>(_context);
            AddressRepository = new GenericRepository<Address>(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}