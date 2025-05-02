using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class PredictionRequest
    {
        [MaxLength(50)]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Input file, zip file containing the following:
        /// - CSV file(s).
        /// - CSV file configs (.json).
        /// - Prediction config
        /// </summary>
        [MaxLength(1000)]
        public string InputFile { get; set; } = String.Empty;
    }
}
