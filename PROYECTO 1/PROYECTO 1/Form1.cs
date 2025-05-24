using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using A = DocumentFormat.OpenXml.Drawing;
using System.Linq;

namespace PROYECTO_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Método para consultar la API de Cohere con tu API Key
        public async Task<string> ConsultarAIAsync(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer UxX0bgRvEC5PLbHPWia681XbY56Q7aevQ76pO7un");
                client.DefaultRequestHeaders.Add("Cohere-Version", "2022-12-06");

                var requestBody = new
                {
                    model = "command",
                    prompt = prompt,
                    max_tokens = 300,
                    temperature = 0.8
                };

                string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.cohere.ai/v1/generate", content);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error de la API Cohere: " + json, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                if (result.generations == null || result.generations.Count == 0 || result.generations[0].text == null)
                {
                    MessageBox.Show("La respuesta de la API Cohere no contiene resultados válidos:\n" + json, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }

                return result.generations[0].text.ToString();
            }
        }

        // Método para guardar en SQL Server
        public void GuardarInvestigacion(string prompt, string resultado)
        {
            string connectionString = "Data Source=PC-GAMING-RAAS\\SQLEXPRESS;Initial Catalog=PROYECTO1;Integrated Security=True";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Investigaciones (Prompt, Resultado) VALUES (@prompt, @resultado)", conn);
                cmd.Parameters.AddWithValue("@prompt", prompt);
                cmd.Parameters.AddWithValue("@resultado", resultado);
                cmd.ExecuteNonQuery();
            }
        }

        // Método para generar archivo Word usando Open XML SDK y soportando saltos de línea
        public void GenerarWord(string contenido, string ruta)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(ruta, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Soporta saltos de línea: cada línea será un párrafo
                foreach (var linea in contenido.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    Paragraph para = body.AppendChild(new Paragraph());
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(linea));
                }
            }
        }

        // Método para generar archivo PowerPoint (estructura robusta y válida)
        public void GenerarPowerPoint(string contenido, string ruta)
        {
            using (PresentationDocument presentationDocument = PresentationDocument.Create(ruta, PresentationDocumentType.Presentation))
            {
                // Crear la parte de la presentación
                PresentationPart presentationPart = presentationDocument.AddPresentationPart();
                presentationPart.Presentation = new Presentation();

                // SlideMaster y SlideLayout con estructura mínima válida
                SlideMasterPart slideMasterPart = presentationPart.AddNewPart<SlideMasterPart>();
                slideMasterPart.SlideMaster = new SlideMaster(
                    new CommonSlideData(new ShapeTree(
                        new NonVisualGroupShapeProperties(
                            new NonVisualDrawingProperties() { Id = 1, Name = "" },
                            new NonVisualGroupShapeDrawingProperties(),
                            new ApplicationNonVisualDrawingProperties()
                        ),
                        new GroupShapeProperties()
                    )),
                    new SlideLayoutIdList(
                        new SlideLayoutId() { Id = 1U, RelationshipId = "rId1" }
                    ),
                    new TextStyles()
                );

                SlideLayoutPart slideLayoutPart = slideMasterPart.AddNewPart<SlideLayoutPart>("rId1");
                slideLayoutPart.SlideLayout = new SlideLayout(
                    new CommonSlideData(new ShapeTree(
                        new NonVisualGroupShapeProperties(
                            new NonVisualDrawingProperties() { Id = 1, Name = "" },
                            new NonVisualGroupShapeDrawingProperties(),
                            new ApplicationNonVisualDrawingProperties()
                        ),
                        new GroupShapeProperties()
                    )),
                    new SlideMasterIdList()
                );

                // Crear la diapositiva
                SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();
                slidePart.Slide = new Slide(new CommonSlideData(new ShapeTree(
                    new NonVisualGroupShapeProperties(
                        new NonVisualDrawingProperties() { Id = 1, Name = "" },
                        new NonVisualGroupShapeDrawingProperties(),
                        new ApplicationNonVisualDrawingProperties()
                    ),
                    new GroupShapeProperties()
                )));

                ShapeTree shapeTree = slidePart.Slide.CommonSlideData.ShapeTree;

                // Título
                Shape titleShape = new Shape(
                    new NonVisualShapeProperties(
                        new NonVisualDrawingProperties() { Id = 2, Name = "Title" },
                        new NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                        new ApplicationNonVisualDrawingProperties(new PlaceholderShape() { Type = PlaceholderValues.Title })
                    ),
                    new ShapeProperties(),
                    new TextBody(
                        new A.BodyProperties(),
                        new A.ListStyle(),
                        new A.Paragraph(new A.Run(new A.Text("Informe de Investigación")))
                    )
                );
                shapeTree.Append(titleShape);

                // Cuerpo del contenido
                var contentTextBody = new TextBody(
                    new A.BodyProperties(),
                    new A.ListStyle()
                );
                foreach (var linea in contenido.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    contentTextBody.Append(new A.Paragraph(new A.Run(new A.Text(linea))));
                }

                Shape contentShape = new Shape(
                    new NonVisualShapeProperties(
                        new NonVisualDrawingProperties() { Id = 3, Name = "Content" },
                        new NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                        new ApplicationNonVisualDrawingProperties(new PlaceholderShape() { Type = PlaceholderValues.Body })
                    ),
                    new ShapeProperties(),
                    contentTextBody
                );
                shapeTree.Append(contentShape);

                // Relacionar la diapositiva con el SlideLayout
                slidePart.AddPart(slideLayoutPart);

                // Agregar la diapositiva a la presentación
                presentationPart.Presentation.SlideIdList = new SlideIdList();
                uint slideId = 256U;
                SlideId newSlideId = new SlideId() { Id = slideId, RelationshipId = presentationPart.GetIdOfPart(slidePart) };
                presentationPart.Presentation.SlideIdList.Append(newSlideId);

                // Agregar SlideMasterIdList
                presentationPart.Presentation.SlideMasterIdList = new SlideMasterIdList(
                    new SlideMasterId() { Id = 1U, RelationshipId = presentationPart.GetIdOfPart(slideMasterPart) }
                );

                presentationPart.Presentation.Save();
            }
        }

        // Método para crear carpeta y guardar archivos
        public void CrearCarpetaYGuardar(string contenido)
        {
            string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InvestigacionAI");
            Directory.CreateDirectory(carpeta);

            string rutaWord = Path.Combine(carpeta, "Informe.docx");
            string rutaPpt = Path.Combine(carpeta, "Presentacion.pptx");

            GenerarWord(contenido, rutaWord);
            GenerarPowerPoint(contenido, rutaPpt);
        }

        // Botón INVESTIGAR: consulta la AI y muestra el resultado para revisión/edición
        private async void btnInvestigar_Click(object sender, EventArgs e)
        {
            lblEstado.Text = "Investigando...";
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;

            string prompt = "Responde en español: " + txtPrompt.Text;
            string resultado = await ConsultarAIAsync(prompt);

            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 0;

            if (!string.IsNullOrWhiteSpace(resultado))
            {
                txtResultados.Text = resultado;
                lblEstado.Text = "Revisa y edita el resultado antes de generar el informe.";
            }
            else
            {
                lblEstado.Text = "No se obtuvo resultado de la investigación.";
            }
        }

        // Botón GENERAR INFORME: guarda, exporta y muestra estado
        private void btnGenerarInforme_Click(object sender, EventArgs e)
        {
            lblEstado.Text = "Generando informe...";
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;

            string prompt = txtPrompt.Text;
            string resultado = txtResultados.Text;

            if (string.IsNullOrWhiteSpace(prompt) || string.IsNullOrWhiteSpace(resultado))
            {
                MessageBox.Show("Debe haber un tema y un resultado para generar el informe.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblEstado.Text = "Faltan datos para generar el informe.";
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                return;
            }

            GuardarInvestigacion(prompt, resultado);
            CrearCarpetaYGuardar(resultado);

            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 100;
            lblEstado.Text = "¡Informe generado y archivos guardados!";
            MessageBox.Show("¡Investigación guardada y archivos generados!");
        }

        private void txtPrompt_TextChanged(object sender, EventArgs e)
        {
            // Puedes agregar validaciones en tiempo real si lo deseas
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblEstado.Text = "Esperando acción...";
            progressBar.Value = 0;
        }
    }
}
