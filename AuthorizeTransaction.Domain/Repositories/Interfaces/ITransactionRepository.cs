﻿using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces.Base;


namespace AuthorizeTransaction.Domain.Repositories.Interfaces
{

    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
    }
}
