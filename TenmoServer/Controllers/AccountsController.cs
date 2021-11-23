using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsDao accountDao;
        

        public AccountsController(IAccountsDao _accountsDao)
        {
            accountDao = _accountsDao;
        }
        


        [HttpGet("{id}")]
        public ActionResult<Accounts> GetAccountByID(int id)
        { 
            Accounts newAccount = accountDao.GetAccount(id);
            if(newAccount == null)
            {
                return NotFound("Account doesn't exist");
            }
            return newAccount;
        }



        [HttpGet("{id}/balance")]
        public ActionResult GetBalanceByID(int id)
        {
            Accounts tempAccount = accountDao.GetAccount(id);
            if(tempAccount == null)
            {
                return NotFound("Account doesn't exist");
            }
            return Ok(tempAccount);
        }


        

    }
}
