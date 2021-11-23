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
    public class TransferController : Controller
    {

        private readonly IAccountsDao accountDao;
        private readonly IUserDao userDao;
        private readonly ITransferDao transferDao;

        public TransferController(IAccountsDao _accountsDao, IUserDao _userDao, ITransferDao _transferDao)
        {
            accountDao = _accountsDao;
            userDao = _userDao;
            transferDao = _transferDao;
        }



        [HttpGet("viewallusers")]
        public List<User> ViewUsersForTransfer()
        {
            return userDao.GetUsers();
        }



        [HttpGet("{id}/viewalltransfers")]
        public List<Transfers> ViewUserTransfers(int id)
        {
            return transferDao.ViewAllTransfers(id);
        }



        [HttpGet("{id}/viewtransfer")]
        public ActionResult GetTransferByID(int id)
        {
            Transfers tempTransfer = transferDao.FindTransferByID(id);
            if (tempTransfer == null)
            {
                return NotFound("Transfer not found");
            }
            return Ok(tempTransfer);
        }

        [HttpGet("{id}/viewpendingtransfers")]
        public List<Transfers> ViewPendingTransfers(int id)
        {
            return transferDao.ViewPendingTransfers(id);
        }



        [HttpPost("{id}/transfer")]
        public ActionResult<Transfers> CreateTransfer(Transfers transfer)
        {
            Transfers newTransfer = transferDao.CreateNewTransfer(transfer);
            return Created($"{newTransfer.TransferID}", newTransfer);
        }

        [HttpPut("{id}/approvetransfer")]
        public ActionResult ApproveTransfer(int id)
        {
            Transfers approveTransfer = transferDao.FindTransferByID(id);
            transferDao.ApproveTransfer(approveTransfer);
            if (approveTransfer == null)
            {
                return NotFound("Transfer not found");
            }
            transferDao.ApproveTransfer(approveTransfer);
            return Ok(approveTransfer);
        }
    }
    }

