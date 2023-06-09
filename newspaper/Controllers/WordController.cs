using BLL.functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
// עבור תמונה
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;


namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public WordController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("FirstWord")]
        public IActionResult FirstWord()
        {
            string f = "C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\myFile1.docx";
            string[] cc = { "Title 1", "1. Hi my name is Yael and I leave in Rabi Tadok 10 Dira 1.", "Title 2", "2. Hi my name is Yael and I leave in Rabi Tadok 10 Dira 1.","Title 3", "3. Hi my name is Yael and I leave in Rabi Tadok 10 Dira 1." };

            _funcs.FirstWord(f, cc);
            //// Create a Word document with two columns
            //using (WordprocessingDocument document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            //{
            //    MainDocumentPart mainPart = document.AddMainDocumentPart();
            //    mainPart.Document = new Document();
            //    Body body = mainPart.Document.AppendChild(new Body());
            //    SectionProperties sectionProps = new SectionProperties();
            //    Columns columns = new Columns() { ColumnCount = 2 };
            //    sectionProps.Append(columns);
            //    body.Append(sectionProps);

            //    // Add paragraphs of text to each column
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Paragraph paragraph = new Paragraph();
            //        Run run = new Run(new Text("This is a paragraph of text."));
            //        paragraph.Append(run);

            //        if (i % 2 == 0)
            //        {
            //            body.AppendChild(new ParagraphProperties(new SectionColumnMark()));
            //        }
            //        body.AppendChild(paragraph);

            //    }
            //}
            return Ok("FirstWord is finish");
        }

        [HttpGet("InsertAPicture")]
        public IActionResult InsertAPicture()
        {
            string filePath = @"C:\Users\YAEL\OneDrive\שולחן העבודה\newfile.docx";
            string imagePath = @"C:\Users\YAEL\OneDrive\תמונות\f.jpg";
            //using (WordprocessingDocument wordprocessingDocument =
            //    WordprocessingDocument.Create(document, WordprocessingDocumentType.Document))
            //{
            //    MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

            //    mainPart = wordprocessingDocument.AddMainDocumentPart();

            //    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

            //    using (FileStream stream = new FileStream(fileName, FileMode.Create))
            //    {
            //        imagePart.FeedData(stream);
            //    }

            //    AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
            //}
            // Create a new Word document
            using (var document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                // Add a main document part
                var mainPart = document.AddMainDocumentPart();

                // Create a new document
                var documentElement = new Document();
                mainPart.Document = documentElement;

                // Create a body element
                var body = new Body();

                // Add a paragraph to the body
                var paragraph = new Paragraph();
                body.Append(paragraph);

                // Add a run to the paragraph
                var run = new Run();
                paragraph.Append(run);

                // Add a picture to the run
                var picture = new A.Blip();
                var embedId = "image1";
                var imagePart = mainPart.AddImagePart(ImagePartType.Jpeg, embedId);
                using (var stream = new FileStream(imagePath, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }
                picture.Embed = embedId;

                //var drawing = new A.Drawing();
                //var inline = new A.Inline();
                var drawing = new Drawing();
                var inline = new Inline();
                inline.Append(drawing);
                drawing.Append(picture);

                run.Append(inline);

                // Add the body to the document
                documentElement.Append(body);
            }
            return Ok("InsertAPicture is finish");
        }

        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body!.AppendChild(new Paragraph(new Run(element)));
        }
    }
}
