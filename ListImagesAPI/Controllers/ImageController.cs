using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;


[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private readonly string _imagePath;
    private readonly ILogger<ImageController> _logger;

    public ImageController(ILogger<ImageController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _imagePath = configuration["ImagePath"] ?? string.Empty; // Hent miljøvariabel
    }
    [HttpGet ("version")]
    public IActionResult Version()
    {
        return Ok ("1,0,0");
    } 
    [HttpGet("listImages")]
    public IActionResult ListImages()
    {
        List<Uri> images = new List<Uri>();

        if (Directory.Exists(_imagePath))
        {
            string[] fileEntries = Directory.GetFiles(_imagePath);
            foreach (var file in fileEntries)
            {
                var imageURI = new Uri(file, UriKind.RelativeOrAbsolute);
                images.Add(imageURI);
            }
        }
        else
        {
            _logger.LogWarning("Image path not found: {ImagePath}", _imagePath);
            return NotFound("Image path not found.");
        }

        return Ok(images);
    }
}
