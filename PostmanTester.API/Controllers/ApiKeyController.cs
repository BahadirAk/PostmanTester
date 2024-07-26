using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostmanTester.Application.Interfaces.Services;
using PostmanTester.Application.Models.Enums;
using PostmanTester.Application.Models.Requests.ApiKey;

namespace PostmanTester.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await _apiKeyService.GetListAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _apiKeyService.GetAsync(a => a.Id == id && a.Status == (byte)GeneralEnum.Active);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ApiKeyRequest apiKeyRequest)
        {
            var result = await _apiKeyService.AddAsync(apiKeyRequest);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ApiKeyRequest apiKeyRequest)
        {
            var result = await _apiKeyService.UpdateAsync(apiKeyRequest);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _apiKeyService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
