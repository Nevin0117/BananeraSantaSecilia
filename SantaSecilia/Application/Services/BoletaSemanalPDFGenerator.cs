using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

namespace SantaSecilia.Application.Services
{
    public class BoletaSemanalPDFGenerator
    {
        // Modelo para cada registro de actividad diaria
        public class RegistroActividad
        {
            public string Fecha { get; set; }
            public string Actividad { get; set; }
            public string Horas { get; set; }
            public string Tarifa { get; set; }
            public string Monto { get; set; }
        }

        public static byte[] GenerarPDF(
            string nombreTrabajador,
            string semana,
            List<RegistroActividad> registros,
            string totalDevengado,
            string descuentos,
            string totalAPagar,
            byte[] logoBytes)
        {
            // Configurar licencia
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Configuración de página
                    page.Size(PageSizes.Letter);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black));

                    // Encabezado
                    page.Header()
                        .Column(column =>
                        {
                            // Logo y nombre en horizontal
                            column.Item().Row(row =>
                            {
                                // Logo a la izquierda
                                if (logoBytes != null && logoBytes.Length > 0)
                                {
                                    row.ConstantItem(90).Image(logoBytes);
                                    row.ConstantItem(15);
                                }

                                // Nombre de la finca
                                row.RelativeItem().AlignMiddle().Column(textColumn =>
                                {
                                    textColumn.Item().Text("Bananera Santa Cecilia")
                                        .FontSize(22)
                                        .Bold()
                                        .FontColor("#2C5F2D");

                                    textColumn.Item().Text("Boleta Semanal")
                                        .FontSize(16)
                                        .FontColor("#555555");
                                });
                            });

                            column.Item().PaddingVertical(5);

                            // Información del trabajador y semana
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"Trabajador: {nombreTrabajador}")
                                        .FontSize(12)
                                        .Bold();
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"Semana: {semana}")
                                        .FontSize(11)
                                        .FontColor("#555555")
                                        .AlignRight();

                                    col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                        .FontSize(11)
                                        .FontColor("#555555")
                                        .AlignRight();
                                });
                            });

                            column.Item().PaddingVertical(10);
                            column.Item().LineHorizontal(1).LineColor("#E0E0E0");
                            column.Item().PaddingVertical(5);
                        });

                    // Contenido (tabla)
                    page.Content()
                        .Column(contentColumn =>
                        {
                            contentColumn.Item().Table(table =>
                            {
                                // Definir columnas
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1.5f);  // Fecha
                                    columns.RelativeColumn(3);     // Actividad
                                    columns.RelativeColumn(1.5f);  // Horas
                                    columns.RelativeColumn(1.5f);  // Tarifa
                                    columns.RelativeColumn(2);     // Monto
                                });

                                // Encabezado de tabla
                                table.Header(header =>
                                {
                                    header.Cell().Background("#80BFFF")
                                        .Padding(8).AlignCenter().Text("Fecha").FontSize(12).Bold();
                                    header.Cell().Background("#80BFFF")
                                        .Padding(8).AlignLeft().Text("Actividad").FontSize(12).Bold();
                                    header.Cell().Background("#80BFFF")
                                        .Padding(8).AlignCenter().Text("Horas").FontSize(12).Bold();
                                    header.Cell().Background("#80BFFF")
                                        .Padding(8).AlignCenter().Text("Tarifa").FontSize(12).Bold();
                                    header.Cell().Background("#80BFFF")
                                        .Padding(8).AlignCenter().Text("Monto").FontSize(12).Bold();
                                });

                                // Filas de datos
                                foreach (var registro in registros)
                                {
                                    table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                        .Padding(8).AlignCenter().Text(registro.Fecha).FontSize(11);
                                    table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                        .Padding(8).AlignLeft().Text(registro.Actividad).FontSize(11);
                                    table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                        .Padding(8).AlignCenter().Text(registro.Horas).FontSize(11);
                                    table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                        .Padding(8).AlignCenter().Text(registro.Tarifa).FontSize(11);
                                    table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                        .Padding(8).AlignCenter().Text(registro.Monto).FontSize(11);
                                }
                            });

                            // Sección de totales
                            contentColumn.Item().PaddingTop(15);
                            contentColumn.Item().AlignRight().Width(300).Column(totalesColumn =>
                            {
                                // Total Devengado
                                totalesColumn.Item().Row(row =>
                                {
                                    row.RelativeItem().Text("Total Devengado:")
                                        .FontSize(11);
                                    row.ConstantItem(100).Text(totalDevengado)
                                        .FontSize(11)
                                        .AlignRight();
                                });

                                totalesColumn.Item().PaddingVertical(3);

                                // Descuentos
                                totalesColumn.Item().Row(row =>
                                {
                                    row.RelativeItem().Text("Descuentos:")
                                        .FontSize(11);
                                    row.ConstantItem(100).Text(descuentos)
                                        .FontSize(11)
                                        .AlignRight();
                                });

                                totalesColumn.Item().PaddingVertical(5);
                                totalesColumn.Item().LineHorizontal(1).LineColor("#E0E0E0");
                                totalesColumn.Item().PaddingVertical(5);

                                // Total a Pagar
                                totalesColumn.Item().Background("#80BFFF")
                                    .Padding(8)
                                    .Row(row =>
                                    {
                                        row.RelativeItem().Text("Total a pagar:")
                                            .FontSize(12)
                                            .Bold();
                                        row.ConstantItem(100).Text(totalAPagar)
                                            .FontSize(12)
                                            .Bold()
                                            .AlignRight();
                                    });
                            });
                        });

                    // Pie de página
                    page.Footer()
                        .AlignCenter()
                        .DefaultTextStyle(x => x.FontSize(9).FontColor("#999999"))
                        .Text(txt =>
                        {
                            txt.Span("Página ");
                            txt.CurrentPageNumber();
                            txt.Span(" de ");
                            txt.TotalPages();
                        });
                });
            }).GeneratePdf();

            return pdf;
        }
    }
}
