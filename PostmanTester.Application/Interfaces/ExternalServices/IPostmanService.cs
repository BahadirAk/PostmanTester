using PostmanTester.Application.Models.Responses.Postman;
using PostmanTester.Application.Models.Results;
using System.Net;
using Environment = PostmanTester.Application.Models.Responses.Postman.Environment;

namespace PostmanTester.Application.Interfaces.ExternalServices
{
    public interface IPostmanService
    {
        Task<IDictionary<string, HttpStatusCode>> RunTestAsync(CollectionDetail collectionDetail);

        #region Workspaces
        Task<IDataResult<List<Workspace>>> GetListWorkspaces();
        Task<IDataResult<Workspace>> GetWorkspace(string workspaceId);
        #endregion
        #region Collections
        Task<IDataResult<List<Collection>>> GetListCollections();
        Task<IDataResult<CollectionDetail>> GetCollection(string collectionId);
        #endregion
        #region Environments
        Task<IDataResult<List<Environment>>> GetListEnvironments();
        Task<IDataResult<Environment>> GetEnvironment(string environmentId);
        #endregion
    }
}
