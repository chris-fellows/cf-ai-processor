using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class AuditEventParameter
    {
        [MaxLength(50)]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// System Value Type Id
        /// </summary>
        [Required]
        [MaxLength(50)]
        //[ForeignKey("SystemValueType")]
        public string SystemValueTypeId { get; set; } = String.Empty;

        //[DeleteBehavior(DeleteBehavior.NoAction)]
        //public virtual SystemValueType? SystemValueType { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Value { get; set; } = String.Empty;

        //public SystemValue ToSystemValue()
        //{
        //    return new SystemValue()
        //    {
        //        TypeId = SystemValueTypeId,
        //        Value = Value
        //    };
        //}
    }
}
