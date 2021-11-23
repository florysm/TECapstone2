using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        public List<Transfers> ViewAllTransfers(int userId);

        public Transfers FindTransferByID(int transferId);
        public Transfers CreateNewTransfer(Transfers transfer);
        public List<Transfers> ViewPendingTransfers(int userId);
        public Transfers ApproveTransfer(Transfers transfer);
    }
}
