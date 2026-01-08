using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class Otp
    {
        [Key]
        public int OtpId {  get; set; }
        public int UserId {  get; set; }
        public User User { get; set; }
        public string Code {  get; set; }
        public DateTime ExpiresAt {  get; set; }
        public bool IsUsed {  get; set; }
    }
}
