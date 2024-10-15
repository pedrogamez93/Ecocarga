using Ecocarga.Models;
using Ecocarga.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Diagnostics;

public class ElectrolinerasController : Controller
{
    private readonly ElectrolineraService _electrolineraService;
    private readonly UserActionService _userActionService; // Inyecta el servicio para registrar acciones

    public ElectrolinerasController(ElectrolineraService electrolineraService, UserActionService userActionService)
    {
        _electrolineraService = electrolineraService;
        _userActionService = userActionService; // Asigna el servicio de acciones
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var electrolineraResponse = await _electrolineraService.GetElectrolinerasAsync(page);

        var viewModel = new ElectrolineraListViewModel
        {
            Electrolineras = electrolineraResponse.Items,
            TotalPages = electrolineraResponse.TotalPages,
            CurrentPage = electrolineraResponse.CurrentPage
        };

        // Registrar la acción de visualizar la lista de electrolineras
        await _userActionService.LogActionAsync("Visualización de electrolineras", "El usuario ha visualizado la lista de electrolineras.");

        return View(viewModel);
    }

    public IActionResult Error(string message)
    {
        var errorModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            ErrorMessage = message ?? "Ocurrió un error inesperado. Por favor, intente nuevamente más tarde."
        };

        return View(errorModel);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadExcel()
    {
        try
        {
            var electrolineras = await _electrolineraService.GetElectrolinerasAsync();

            // Registrar la acción de descarga de archivo Excel
            await _userActionService.LogActionAsync("Descarga de Excel", "El usuario ha descargado el archivo Excel de electrolineras.");

            // Establece el contexto de la licencia
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Electrolineras");

                // Agregar encabezados de columnas
                worksheet.Cells[1, 1].Value = "Location ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Address";
                worksheet.Cells[1, 4].Value = "Commune";
                worksheet.Cells[1, 5].Value = "Region";
                worksheet.Cells[1, 6].Value = "Latitude";
                worksheet.Cells[1, 7].Value = "Longitude";
                worksheet.Cells[1, 8].Value = "EVSE ID";
                worksheet.Cells[1, 9].Value = "Status";
                worksheet.Cells[1, 10].Value = "Max Electric Power";

                // Agregar los datos de las electrolineras
                for (int i = 0; i < electrolineras.Items.Count; i++)
                {
                    var row = i + 2;
                    var electrolinera = electrolineras.Items[i];
                    worksheet.Cells[row, 1].Value = electrolinera.LocationId;
                    worksheet.Cells[row, 2].Value = electrolinera.Name;
                    worksheet.Cells[row, 3].Value = electrolinera.Address;
                    worksheet.Cells[row, 4].Value = electrolinera.Commune;
                    worksheet.Cells[row, 5].Value = electrolinera.Region;
                    worksheet.Cells[row, 6].Value = electrolinera.Coordinates.Latitude;
                    worksheet.Cells[row, 7].Value = electrolinera.Coordinates.Longitude;
                    worksheet.Cells[row, 8].Value = electrolinera.Evses.FirstOrDefault()?.EvseId;
                    worksheet.Cells[row, 9].Value = electrolinera.Evses.FirstOrDefault()?.Status;
                    worksheet.Cells[row, 10].Value = electrolinera.Evses.FirstOrDefault()?.MaxElectricPower;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream(package.GetAsByteArray());
                var fileName = "Electrolineras.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        catch (ApplicationException ex)
        {
            return RedirectToAction("Error", new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Si ocurre un error al generar el Excel
            return RedirectToAction("Error", new { message = "Ocurrió un error al generar el archivo Excel." });
        }
    }
}
