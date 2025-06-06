﻿using Tensorflow.Checkpoint;

namespace CFAIProcessor.UI.Utilities
{
    public class ConfigUtilities
    {
        public static int MaxDataSetUploadSize = 10000000;

        public static int MaxImageSetUploadSize = 1000000000;

        public static int MaxUploadImageSize = 100000;

        /// <summary>
        /// Items per page on list pages (Audit Events, Issues etc0
        /// </summary>
        public static int ItemsPerListPage = 20;

        public static string ImageTempFilesRootFolder => Path.Combine(Path.GetTempPath(), "Images-Temp");
        public static string AuditEventTypeImageLocalFolder => "D:\\Data\\Dev\\C#\\cfai-processor\\CFAIProcessor.UI\\wwwroot\\images\\audit_event_types";

        public static string DataSetLocalFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Data Sets";

        public static string ImageClassifyModelsLocalFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Image Classify Models";

        public static string ImageSetLocalFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Image Sets";

        public static string TempLocalFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Temp";        

        public static string PredictionModelsLocalFolder = "D:\\Data\\Dev\\C#\\cf-ai-processor-local\\Prediction Models";

        public static string UserImageLocalFolder => "D:\\Data\\Dev\\C#\\cfai-processor\\CFAIProcessor.UI\\wwwroot\\images\\users";
    }
}
