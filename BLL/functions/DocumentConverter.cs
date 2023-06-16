        using Leadtools; 
using Leadtools.Codecs; 
using Leadtools.Document.Writer; 
using Leadtools.Svg; 
//using LeadtoolsExamples.Common; 
using Leadtools.Document; 
using Leadtools.Caching; 
using Leadtools.Annotations.Engine; 
using Leadtools.Ocr; 
using Leadtools.Document.Converter; 
//using Leadtools.Annotations.Rendering;
namespace BLL.functions
{
    public class DocumentConverter
    {


        public void DocumentConverterExample()
        {
            /*using (DocumentConverter documentConverter = new DocumentConverter())
            {
                var inFile = Path.Combine(ImagesPath.Path, @"Leadtools.docx");
                var outFile = Path.Combine(ImagesPath.Path, @"output.pdf");
                var format = DocumentFormat.Pdf;
                var jobData = DocumentConverterJobs.CreateJobData(inFile, outFile, format);
                jobData.JobName = "conversion job";

                var documentWriter = new DocumentWriter();
                documentConverter.SetDocumentWriterInstance(documentWriter);

                var renderingEngine = new AnnWinFormsRenderingEngine();
                documentConverter.SetAnnRenderingEngineInstance(renderingEngine);

                var job = documentConverter.Jobs.CreateJob(jobData);
                documentConverter.Jobs.RunJob(job);

                if (job.Status == DocumentConverterJobStatus.Success)
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("{0} Errors", job.Status);
                    foreach (var error in job.Errors)
                    {
                        Console.WriteLine("  {0} at {1}: {2}", error.Operation, error.InputDocumentPageNumber, error.Error.Message);
                    }
                }
            }*/
        }
    }
}