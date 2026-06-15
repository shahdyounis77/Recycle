using Recycle.Data.Models;
using Recycle.Dtos;

namespace Recycle.Interfaces
{
    public interface ITransaction
    {
       IQueryable<Transaction> GetAllTransactions();

        void AddTransaction(AddTransaction transaction,TypeOfItem itemType);




        
    }
}
