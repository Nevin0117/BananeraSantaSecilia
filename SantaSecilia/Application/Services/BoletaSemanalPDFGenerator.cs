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
            public required string Fecha { get; set; }
            public required string Actividad { get; set; }
            public required string Horas { get; set; }
            public required string Tarifa { get; set; }
            public required string Monto { get; set; }
        }

        // --- Estilos Reutilizables Actualizados con Color Coherente y Mayúsculas ---

        // Fondo y borde coherente para los cuadros principales
        private static string ColorFondoCoherente = "#E3F2FD"; // El azul cielo suave solicitado
        private static string ColorBordeCoherente = "#BBDEFB"; // Un azul ligeramente más oscuro para el borde sutil

        // Estilo para el encabezado de la tabla (Mejorado para coherencia visual)
        private static QuestPDF.Infrastructure.IContainer TableHeaderStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(ColorFondoCoherente) // Usamos el color coherente para el encabezado
                .Border(1)
                .BorderColor(ColorBordeCoherente) // Borde sutil coherente
                .Padding(12)            // Espaciado amplio para claridad
                .DefaultTextStyle(x => x.Bold().FontSize(10).FontColor(Colors.Black)); // Texto negro y negrita
        }

        // Estilo para las celdas de datos con líneas visibles
        private static QuestPDF.Infrastructure.IContainer TableCellStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor("#D6D6D6") // Gris sutil para líneas más visibles
                .Padding(12)            // Padding amplio para que los datos respiren
                .DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Black)); // Texto negro y legible
        }

        public static byte[] GenerarPDF(
            string idTrabajador,
            string cedulaTrabajador,
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
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black).FontFamily(Fonts.Verdana));

                    // --- Encabezado ---
                    page.Header()
                        .Column(column =>
                        {
                            // Logo y nombre de la finca
                            column.Item().Row(row =>
                            {
                                if (logoBytes != null && logoBytes.Length > 0)
                                {
                                    row.ConstantItem(75).Image(logoBytes);
                                    row.ConstantItem(15);
                                }

                                row.RelativeItem().AlignMiddle().Column(textColumn =>
                                {
                                    // AJUSTE: Nombre reducido y en color negro
                                    textColumn.Item().Text("BANANERA SANTA CECILIA")
                                        .FontSize(18) // Tamaño reducido
                                        .Bold()
                                        .FontColor(Colors.Black); // Color negro solicitado

                                    // AJUSTE: Descripción simplificada en español
                                    textColumn.Item().Text("OPERACIONES AGRÍCOLAS")
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Medium);

                                    // NUEVO: Título solicitado con color verde menta
                                    textColumn.Item().PaddingTop(2).Text("BOLETA SEMANAL DIARIA")
                                        .FontSize(11)
                                        .Bold()
                                        .FontColor("#A8E6CF"); // Color verde menta solicitado
                                });
                            });

                            column.Item().PaddingVertical(10);
                        });

                    // --- Contenido Principal ---
                    page.Content()
                        .Column(contentColumn =>
                        {
                            // --- Bloque de Información del Trabajador Estilizado (Mayúsculas y Color Coherente) ---
                            contentColumn.Item()
                                .PaddingBottom(20)
                                .Background(ColorFondoCoherente) // Color azul cielo suave coherente
                                .Border(1)
                                .BorderColor(ColorBordeCoherente) // Borde sutil coherente
                                .Padding(15)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3); // Nombre
                                        columns.RelativeColumn(2); // ID
                                        columns.RelativeColumn(2); // Periodo
                                    });

                                    // Mayúsculas y Español
                                    table.Header(header =>
                                    {
                                        header.Cell().PaddingBottom(5).Text("NOMBRE COLABORADOR").FontSize(9).Bold().FontColor(Colors.Black);
                                        header.Cell().PaddingBottom(5).Text("CÉDULA / ID").FontSize(9).Bold().FontColor(Colors.Black);
                                        header.Cell().PaddingBottom(5).Text("PERIODO DE PAGO").FontSize(9).Bold().FontColor(Colors.Black);
                                    });

                                    // Texto negro
                                    table.Cell().Text(nombreTrabajador).Bold().FontSize(11).FontColor(Colors.Black);
                                    table.Cell().Text(cedulaTrabajador).FontSize(11).FontColor(Colors.Black);
                                    table.Cell().Text(semana).FontSize(11).FontColor(Colors.Black);
                                });

                            // --- Tabla de Actividades ---
                            contentColumn.Item().Table(table =>
                            {
                                // Definir columnas exactas del diseño
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);    // Fecha
                                    columns.RelativeColumn(4);    // Actividad
                                    columns.RelativeColumn(1.5f); // Horas
                                    columns.RelativeColumn(2);    // Tarifa
                                    columns.RelativeColumn(2);    // Monto
                                });

                                // Encabezado de tabla con Color Coherente, Español, Mayúsculas y B/.
                                table.Header(header =>
                                {
                                    header.Cell().Element(TableHeaderStyle).Text("FECHA");
                                    header.Cell().Element(TableHeaderStyle).Text("ACTIVIDAD");
                                    header.Cell().Element(TableHeaderStyle).AlignCenter().Text("HORAS");
                                    header.Cell().Element(TableHeaderStyle).AlignCenter().Text("TARIFA (B/.)");
                                    header.Cell().Element(TableHeaderStyle).AlignCenter().Text("TOTAL (B/.)");
                                });

                                // Filas de datos con líneas visibles y texto negro
                                foreach (var registro in registros)
                                {
                                    table.Cell().Element(TableCellStyle).Text(registro.Fecha);
                                    table.Cell().Element(TableCellStyle).Text(registro.Actividad);
                                    table.Cell().Element(TableCellStyle).AlignCenter().Text(registro.Horas);
                                    table.Cell().Element(TableCellStyle).AlignCenter().Text(registro.Tarifa);
                                    table.Cell().Element(TableCellStyle).AlignCenter().Text(registro.Monto);
                                }
                            });

                            // --- Sección de Totales Estilizada ---
                            contentColumn.Item().PaddingTop(25).AlignRight().Width(280).Column(totalesCol =>
                            {
                                // Español y Mayúsculas
                                totalesCol.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingRight(10).Text("TOTAL SALARIO BRUTO").Bold();
                                    row.ConstantItem(100).AlignRight().Text(totalDevengado).Bold();
                                });

                                totalesCol.Item().PaddingTop(5).Row(row =>
                                {
                                    row.RelativeItem().PaddingRight(10).Text("DEDUCCIONES (N/A)").FontColor(Colors.Grey.Medium);
                                    row.ConstantItem(100).AlignRight().Text($"- {descuentos}").FontColor(Colors.Grey.Medium);
                                });

                                totalesCol.Item().PaddingVertical(8); // Pequeño espacio

                                // Cuadro NETO A PAGAR con Color Coherente, Español y Mayúsculas (Imagen 2)
                                totalesCol.Item()
                                    .Background(ColorFondoCoherente) // Color azul cielo suave coherente
                                    .Border(1)
                                    .BorderColor(ColorBordeCoherente) // Borde sutil coherente
                                    .Padding(12)
                                    .DefaultTextStyle(x => x.Bold().FontSize(12).FontColor(Colors.Black)) // Todo en negro y negrita
                                    .Row(row =>
                                    {
                                        row.RelativeItem().PaddingRight(10).Text("SALARIO NETO A PAGAR");
                                        row.ConstantItem(100).AlignRight().Text(totalAPagar).FontColor("#2C5F2D");
                                    });
                            });
                        });

                    // --- Pie de Página ---
                    page.Footer().Column(col =>
                    {
                        col.Item().PaddingTop(40).Row(row =>
                        {
                            row.RelativeItem().Column(signature =>
                            {
                                signature.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);
                                // Español y Mayúsculas
                                signature.Item().PaddingTop(5).AlignCenter().Text("FIRMA DEL TRABAJADOR").FontSize(9).FontColor(Colors.Black);
                            });

                            row.ConstantItem(60); // Espacio entre firmas

                            row.RelativeItem().Column(signature =>
                            {
                                signature.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);
                                // Español y Mayúsculas
                                signature.Item().PaddingTop(5).AlignCenter().Text("FIRMA AUTORIZADA").FontSize(9).FontColor(Colors.Black);
                            });
                        });

                    });
                });
            }).GeneratePdf();

            return pdf;
        }
    }
}