using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class AuditEvent
    {
        [MaxLength(50)]
        public string Id { get; set; } = String.Empty;

        [Required]
        [MaxLength(50)]
        //[ForeignKey("AuditEventType")]
        public string TypeId { get; set; } = String.Empty;     

        /// <summary>
        /// User who created audit event (Default to System)
        /// </summary>
        [Required]
        [MaxLength(50)]
        //[ForeignKey("User")]
        public string CreatedUserId { get; set; } = String.Empty;
        
        public DateTimeOffset CreatedDateTime { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public List<AuditEventParameter> Parameters { get; set; }
    }
}
