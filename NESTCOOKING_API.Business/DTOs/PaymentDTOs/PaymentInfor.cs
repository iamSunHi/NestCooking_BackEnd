using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.PaymentDTOs
{
    public class PaymentInfor
    {
        public string OrderType { get; set; } = null!;
        public double Amount { get; set; }
        public string OrderDescription { get; set; } = null!;
        public string Name { get; set; } = null!;

    }
}
