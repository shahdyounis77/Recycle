using Recycle.Data.Models;
using Recycle.Interfaces;
using Recycle.Data;
using Recycle.Dtos;

using Recycle.Helper;
using Microsoft.EntityFrameworkCore;

namespace Recycle.Repos
{
    public class TransactionRepo: ITransaction
    {
        private readonly Context _context;
       
        
        public TransactionRepo(Context context)
        {
            _context = context;
           
            
        }
        public IQueryable<Transaction> GetAllTransactions()
        {
            return _context.Transactions.AsQueryable().Include(t=>t.User).Include(t=>t.Machine);
        }

        public void AddTransaction(AddTransaction transaction,TypeOfItem itemType)
        {
            var otp = _context.Otps.Where(o => o.Id == transaction.OtpId).FirstOrDefault();
            
            
                var newtransaction = new Transaction
                {

                    UserId = otp.UserId,
                    MachineId = otp.MachineId,
                    ItemType = itemType,
                    Weight = transaction.Weight,
                    CoinsEarned = transaction.CoinsEarned,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Transactions.Add(newtransaction);

                _context.SaveChanges();
           
            
            
        }
    }
}
