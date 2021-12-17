using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[ApiController]
[Route("[controller]")]
public class InsuranceController : ControllerBase
{
    private readonly ILogger<InsuranceController> _logger;
    private readonly IApplicationService _applicationService;
    private readonly IAccountService _accountService;
    private readonly IInsuranceService _insuranceService;

    public InsuranceController(IApplicationService applicationService, 
        IInsuranceService insuranceService,
        IAccountService accountService,
        ILogger<InsuranceController> logger)
    {
        _applicationService = applicationService;
        _insuranceService = insuranceService;
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    [Route("get-operator/{operatorId:guid}")]
    public async Task<IActionResult> GetApplicationsAsync([FromRoute] Guid operatorId, CancellationToken cancellationToken)
    {
        var result = await _applicationService.GetApplicationsByOperatorId(operatorId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Route("approve/{applicationId:guid}")]
    public async Task<IActionResult> ApproveApplicationAsync([FromRoute] Guid applicationId, [FromBody] OperatorIdModel model, CancellationToken cancellationToken)
    {
        var result = await _applicationService.ApproveApplicationAsync(applicationId, model.OperatorId, cancellationToken);
        if (result)
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpPut]
    [Route("sign/{applicationId:guid}")]
    public async Task<IActionResult> SignInsuranceAsync([FromRoute] Guid applicationId, 
        [FromBody] OperatorIdModel model, CancellationToken cancellationToken)
    {
        var result =
            await _insuranceService.SignNewInsuranceContractAsync(applicationId, model.OperatorId, cancellationToken);
        if (result)
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateApplicationAsync([FromBody] NewApplicationModel model, 
        [FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var consumer = await this._accountService.GetConsumerAsync(username, cancellationToken);
        if (consumer == null)
        {
            return BadRequest("You are not a consumer");
        }

        model.ConsumerId = consumer.Id;
        return Ok(await _applicationService.CreateApplicationAsync(model, cancellationToken));
    }

    [HttpGet]
    [Route("agents")]
    public async Task<IActionResult> GetInsuranceAgentsAsync(CancellationToken cancellationToken)
    {
        return Ok(await _insuranceService.GetInsuranceAgentsAsync(cancellationToken));
    }

    [HttpGet]
    [Route("consumer-insurance")]
    public async Task<IActionResult> GetConsumerInsuranceAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var consumer = await _accountService.GetConsumerAsync(username, cancellationToken);
        if (consumer == null)
        {
            return BadRequest("You are not a consumer");
        }
        return Ok(await _insuranceService.GetConsumerInsuranceContract(consumer.Id, cancellationToken));
    }

    [HttpPut]
    [Route("stop-insurance/{insuranceId:guid}")]
    public async Task<IActionResult> StopInsuranceAsync([FromRoute] Guid insuranceId, CancellationToken cancellationToken)
    {
        var insurance = await _insuranceService.StopInsuranceAsync(insuranceId, cancellationToken);
        if (insurance == null)
        {
            return BadRequest("Application is not found");
        }
        return Ok(insurance);
    }

    [HttpGet]
    [Route("operator-applications")]
    public async Task<IActionResult> GetOperatorApplicationsAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var controller = await _accountService.GetOperatorAsync(username, cancellationToken);
        if (controller == null)
        {
            return BadRequest("You are not a operator");
        }
        return Ok(await _applicationService.GetApplicationsByOperatorId(controller.Id, cancellationToken));
    }

    [HttpGet]
    [Route("agent-applications")]
    public async Task<IActionResult> GetAgentApplicationsAsync([FromHeader(Name="X-User-Name")] string username, CancellationToken cancellationToken)
    {
        var agent = await _accountService.GetAgentAsync(username, cancellationToken);
        if (agent == null)
        {
            return BadRequest("You are not a agent");
        }
        return Ok(await _applicationService.GetApplicationsByAgentId(agent.Id, cancellationToken));
    }

    [HttpPut]
    [Route("reject-application/{applicationId:guid}")]
    public async Task<IActionResult> RejectApplication([FromRoute] Guid applicationId, CancellationToken cancellationToken)
    {
        var insurance = await _applicationService.RejectApplicationAsync(applicationId, cancellationToken);
        if (insurance == null)
        {
            return BadRequest("Application is not found");
        }
        return Ok(insurance);
    }
}