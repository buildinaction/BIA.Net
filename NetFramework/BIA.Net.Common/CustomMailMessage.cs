using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Common
{
    public class CustomMailMessage : MailMessage
    {
        public int ParentObjectId { get; set; }

        public bool IsSuccess { get; set; }
    }
}
