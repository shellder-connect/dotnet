using Microsoft.AspNetCore.Mvc;

public class ProximidadeViewController : Controller 

{
    [HttpGet("Index")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Index()
    {
        return View();
    }
}