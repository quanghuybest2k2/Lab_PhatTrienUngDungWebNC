using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public enum SubscribeState
    {
        Subscribe,
        Unsubscribe,
        Banned
    }
    public class Subscriber : IEntity
    {
        public int Id { get; set; }
        public string SubscribeEmail { get; set; }
        public DateTime SubDated { get; set; }
        public DateTime? UnSubDated { get; set; }
        public string CancelReason { get; set; }
        public bool ForceLock { get; set; }
        public bool UnsubscribeVoluntary { get; set; }
        public string AdminNotes { get; set; }
    }
}
