using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class PredictionResponse
    {
        [MaxLength(50)]
        public string Id { get; set; } = String.Empty;

        [MaxLength(500)]
        public string ErrorMessage { get; set; } = String.Empty;
    }
}
