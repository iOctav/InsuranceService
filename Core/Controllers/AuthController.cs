using System.Net;
using System.Text;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAccountService _accountService;

    public AuthController(IAccountService accountService, ILogger<AuthController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        return Ok(new StringFieldModel
        {
            Field = await _accountService.LoginAsync(model.Username, model.Password, cancellationToken)
        });
    }
    
    [HttpGet]
    [Route("get-role")]
    public async Task<IActionResult> GetRoleAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        return Ok(new RouteResultModel
        {
          RoutePath  = await _accountService.GetRoleAsync(username, cancellationToken)
        });
    }
    
    [HttpGet]
    [Route("get-consumer")]
    public async Task<IActionResult> GetConsumerAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var consumer = await _accountService.GetConsumerAsync(username, cancellationToken);
        if (consumer == null)
        {
            return BadRequest("You are not consumer");
        }
        return Ok(new ConsumerModel
        {
            FirstName = consumer.Firstname,
            Surname = consumer.Surname,
            Phone = consumer.Phone
        });
    }
    
    [HttpGet]
    [Route("get-operator")]
    public async Task<IActionResult> GetOperatorAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var controller = await _accountService.GetOperatorAsync(username, cancellationToken);
        if (controller == null)
        {
            return BadRequest("You are not operator");
        }
        return Ok(new PersonModel
        {
            Id = controller.Id,
            FirstName = controller.Firstname,
            Surname = controller.Surname
        });
    }
    
    [HttpGet]
    [Route("get-agent")]
    public async Task<IActionResult> GetAgentAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var agent = await _accountService.GetAgentAsync(username, cancellationToken);
        if (agent == null)
        {
            return BadRequest("You are not agent");
        }
        return Ok(new InsuranceAgentModel()
        {
            CompanyName = agent.CompanyName,
            Id = agent.Id,
            Tariff = agent.Tariff
        });
    }
}