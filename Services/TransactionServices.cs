using Recycle.Data.Models;
using Recycle.Dtos;
using Recycle.Interfaces;
using Recycle.Repos;

namespace Recycle.Services
{
    public class TransactionServices
    {
        private readonly ITransaction _transaction;
        private readonly OtpServices otpServices;
       
        public TransactionServices( ITransaction transaction, OtpServices services)
        {
            this._transaction = transaction;
            otpServices = services;
            
        }

        public void AddTransaction(AddTransaction transaction)
        {
            if (Enum.TryParse<TypeOfItem>(transaction.ItemType, out var itemType))
            {
                _transaction.AddTransaction(transaction, itemType);
            }
            else
            {
                throw new ArgumentException("Invalid item type");
            }

        }

        public List<ViewTransaction> GetTransactionByUserId(string userId) {
        
          var transactionn= _transaction.GetAllTransactions().Where(t=>t.UserId==userId).ToList();
            var transactions = new List<ViewTransaction>();
            foreach (var item in transactionn)
            {
                var view= new ViewTransaction
                {
                    ItemType = item.ItemType.ToString(),
                    Weight = item.Weight,
                    CoinsEarned = item.CoinsEarned,
                    CreatedAt = item.CreatedAt

                };
                transactions.Add(view);
            }
            return transactions;
        }

        public List<ViewTransactionForAdmin>GetViewTransactionForAdmins(int machineid) {
            var transactionn= _transaction.GetAllTransactions().Where(t=>t.MachineId==machineid).ToList();
            var transactions = new List<ViewTransactionForAdmin>();
            foreach (var item in transactionn)
            {
                var view= new ViewTransactionForAdmin
                {
                    UserName = item.User.UserName,
                    Email = item.User.Email,
                    MachineLocation = item.Machine.Location,
                    ItemType = item.ItemType.ToString(),
                    Weight = item.Weight,
                    CoinsEarned = item.CoinsEarned,
                    CreatedAt = item.CreatedAt
                };
                transactions.Add(view);
            }
            return transactions;
        }

        public void EndTransaction(int otpid) {

            var updatestatus = new UpdateStatusOfOtp
            {
                Status = StatusOtp.Finished
            };

            otpServices.UpdateOtp(updatestatus, otpid);
        
        }
        }
    }

