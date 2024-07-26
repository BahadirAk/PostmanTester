using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Models.Responses.Postman;

namespace PostmanTester.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostmanController : ControllerBase
    {
        private readonly IPostmanService _postmanService;

        public PostmanController(IPostmanService postmanService)
        {
            _postmanService = postmanService;
        }

        [AllowAnonymous]
        [HttpPost("test")]
        public async Task<IActionResult> RunTest(CollectionDetail collectionDetail)
        {
            var result = await _postmanService.RunTestAsync(collectionDetail);
            return Ok(result);
        }

        #region Workspaces
        [HttpGet]
        [Route("workspaces")]
        public async Task<IActionResult> GetListWorkspaces()
        {
            var result = await _postmanService.GetListWorkspaces();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("workspaces/{workspaceId}")]
        public async Task<IActionResult> GetWorkspace(string workspaceId)
        {
            var result = await _postmanService.GetWorkspace(workspaceId);
            return StatusCode(result.StatusCode, result);
        }
        #endregion

        #region Collections
        [HttpGet]
        [Route("collections")]
        public async Task<IActionResult> GetListCollections()
        {
            var result = await _postmanService.GetListCollections();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("collections/{collectionId}")]
        public async Task<IActionResult> GetCollection(string collectionId)
        {
            var result = await _postmanService.GetCollection(collectionId);
            return StatusCode(result.StatusCode, result);
        }
        #endregion

        #region Environments
        [HttpGet]
        [Route("environments")]
        public async Task<IActionResult> GetListEnvironments()
        {
            var result = await _postmanService.GetListEnvironments();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("environments/{environmentId}")]
        public async Task<IActionResult> GetEnvironment(string environmentId)
        {
            var result = await _postmanService.GetEnvironment(environmentId);
            return StatusCode(result.StatusCode, result);
        }
        #endregion
    }
}
