using Microsoft.AspNetCore.Identity;
using Recycle.Data;
using Recycle.Data.Models;
using Recycle.Interfaces;

namespace Recycle.Services
{
    public class UserServices
    {
        private readonly UserManager<User> userManager;
        private readonly ITransaction transactionRepo;
        public UserServices(UserManager<User> userManager,ITransaction transaction)
        {
            this.userManager = userManager;
            this.transactionRepo = transaction;
        }

        public async Task<double> Getcoins(string id ) {

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                
                var coins = transactionRepo.GetAllTransactions().Where(t => t.UserId == user.Id).Sum(c => c.CoinsEarned);
                user.Coins = coins;
                await userManager.UpdateAsync(user);
                return coins;
            }





        }
    }
}
