using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[ApiController]
[Route("[controller]")]
public class CompensationController : ControllerBase
{

    private readonly ILogger<CompensationController> _logger;
    private readonly ICompensationService _compensationService;
    private readonly IAccountService _accountService;

    public CompensationController(
        ICompensationService compensationService,
        ILogger<CompensationController> logger, IAccountService accountService)
    {
        _compensationService = compensationService;
        _logger = logger;
        _accountService = accountService;
    }

    [HttpGet]
    [Route("{compensationId:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid compensationId, 
        CancellationToken cancellationToken)
    {
        return Ok(await _compensationService.GetCompensationAsync(compensationId, cancellationToken));
    }

    [HttpGet]
    [Route("reject/{compensationId:guid}")]
    public async Task<IActionResult> RejectAsync([FromRoute] Guid compensationId, 
        CancellationToken cancellationToken)
    {
        return Ok(await _compensationService.RejectCompensationAsync(compensationId, cancellationToken));
    }

    [HttpGet]
    [Route("/consumer/{consumerId:guid}")]
    public async Task<IActionResult> GetAllAsync([FromRoute] Guid consumerId, 
        CancellationToken cancellationToken)
    {
        return Ok(await _compensationService.GetCompensationsAsync(consumerId, cancellationToken));
    }

    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> New([FromBody] NewCompensationModel model, 
        [FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var consumer = await _accountService.GetConsumerAsync(username, cancellationToken);
        if (consumer == null)
        {
            return BadRequest("You are not a consumer");
        }

        model.ConsumerId = consumer.Id;
        return Ok(await _compensationService.CreateCompensationAsync(model, cancellationToken));
    }

    [HttpGet]
    [Route("consumer-compensations")]
    public async Task<IActionResult> GetConsumerCompensationsAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var consumer = await _accountService.GetConsumerAsync(username, cancellationToken);
        if (consumer == null)
        {
            return BadRequest("You are not a consumer");
        }
        return Ok((await _compensationService.GetCompensationsAsync(cancellationToken))
            .Where(x => x.ConsumerId.Equals(consumer.Id)));
    }

    [HttpGet]
    [Route("operator-compensations")]
    public async Task<IActionResult> GetOperatorCompensationsAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var controller = await _accountService.GetOperatorAsync(username, cancellationToken);
        if (controller == null)
        {
            return BadRequest("You are not a operator");
        }
        return Ok((await _compensationService.GetCompensationsAsync(cancellationToken))
            .Where(x => x.Status.Equals(RequestState.Open)));
    }

    [HttpGet]
    [Route("agent-compensations")]
    public async Task<IActionResult> GetAgentCompensationsAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var agent = await _accountService.GetAgentAsync(username, cancellationToken);
        if (agent == null)
        {
            return BadRequest("You are not a agent");
        }
        return Ok((await _compensationService.GetCompensationsAsync(cancellationToken))
            .Where(x => x.Status.Equals(RequestState.Processing)));
    }

    [HttpPost]
    [Route("approve/{compensationId:guid}")]
    public async Task<IActionResult> ApproveApplicationAsync([FromRoute] Guid compensationId,
        [FromBody] CompensationVerdictModel model, [FromHeader(Name="X-User-Name")] string username,
        CancellationToken cancellationToken)
    {
        var controller = await _accountService.GetOperatorAsync(username, cancellationToken);
        if (controller == null)
        {
            return BadRequest("You are not a operator");
        }
        model.CompensationId = compensationId;
        model.OperatorId = controller.Id;
        var result = await _compensationService.ApproveCompensationAsync(model, cancellationToken);
        if (result != null)
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpPost]
    [Route("sign/{compensationId:guid}")]
    public async Task<IActionResult> SignCompensationAsync([FromRoute] Guid compensationId,
        [FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var agent = await _accountService.GetAgentAsync(username, cancellationToken);
        if (agent == null)
        {
            return BadRequest("You are not a operator");
        }
        var result = await _compensationService.SignCompensationAsync(compensationId, cancellationToken);
        if (result != null)
        {
            return NoContent();
        }
        return BadRequest();
    }
}