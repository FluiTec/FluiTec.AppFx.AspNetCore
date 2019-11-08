using System.Linq;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin;
using FluiTec.AppFx.IdentityServer;
using FluiTec.AppFx.IdentityServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Controllers
{
    /// <summary>   A controller for handling identity admins. </summary>
    [Authorize(PolicyNames.AdministrativeAccess)]
    public class IdentityAdminController : Controller
    {
        #region Fields

        /// <summary>   The identity server data service. </summary>
        private readonly IIdentityServerDataService _identityServerDataService;

        #endregion

        #region Constructors

        public IdentityAdminController(IIdentityServerDataService identityServerDataService)
        {
            _identityServerDataService = identityServerDataService;
        }

        #endregion

        #region Clients

        /// <summary>(Restricted to PolicyNames.ClientsAccess) gets the manage clients.</summary>
        /// <value>The manage clients.</value>
        [Authorize(PolicyNames.ClientsAccess)]
        public IActionResult ManageClients()
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var clients = uow.ClientRepository.GetAll();
                return View(clients);
            }
        }

        /// <summary>(Restricted to PolicyNames.ClientsCreate) adds client.</summary>
        /// <returns>An IActionResult.</returns>
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsCreate)]
        public IActionResult AddClient()
        {
            return View(new ClientAddModel());
        }

        /// <summary>(An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClientsCreate)
        /// adds a client.</summary>
        /// <param name="model">    The model. </param>
        /// <returns>An IActionResult.</returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsCreate)]
        public IActionResult AddClient(ClientAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    uow.ClientRepository.Add(new ClientEntity
                    {
                        Name = model.Name,
                        ClientId = RandomStringCreator.GenerateRandomString(64),
                        Secret = RandomStringCreator.GenerateRandomString(64),
                        RedirectUri = model.RedirectUri,
                        PostLogoutUri = model.PostLogoutUri,
                        AllowOfflineAccess = model.AllowOfflineAccess,
                        GrantTypes = string.Join(',', model.GrantTypes)
                    });
                    uow.Commit();
                }
                return RedirectToAction(nameof(ManageClients));
            }

            return View(model);
        }

        /// <summary>(Restricted to PolicyNames.ClientsUpdate) manage client.</summary>
        /// <param name="clientId"> Identifier for the client. </param>
        /// <returns>An IActionResult.</returns>
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsUpdate)]
        public IActionResult ManageClient(int clientId)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var client = uow.ClientRepository.Get(clientId);
                var scopes = uow.ScopeRepository.GetAll();
                var clientScopes = uow.ClientScopeRepository.GetByClientId(client.Id).Select(cs => scopes.Single(s => s.Id == cs.ScopeId)).ToList();
                
                return View(new ClientEditModel
                {
                    Id = client.Id,
                    ClientId = client.ClientId,
                    ClientSecret = client.Secret,
                    AllowOfflineAccess = client.AllowOfflineAccess,
                    GrantTypes = client.GrantTypes.Split(",").ToList(),
                    Name = client.Name,
                    PostLogoutUri = client.PostLogoutUri,
                    RedirectUri = client.RedirectUri,
                    ClientScopes = clientScopes.Select(cs => new ClientScopeModel { Id = clientId, Name = cs.Name, DisplayName = cs.DisplayName }),
                    Scopes = scopes.Except(clientScopes).Select(cs => new ClientScopeModel { Id = clientId, Name = cs.Name, DisplayName = cs.DisplayName })
                });
            }
        }

        /// <summary>(An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClientsUpdate)
        /// manage client.</summary>
        /// <param name="model">    The model. </param>
        /// <returns>An IActionResult.</returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsUpdate)]
        public IActionResult ManageClient(ClientEditModel model)
        {
            model.Update();

            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var scopes = uow.ScopeRepository.GetAll();
                var clientScopes = uow.ClientScopeRepository.GetByClientId(model.Id).Select(cs => scopes.Single(s => s.Id == cs.ScopeId)).ToList();

                if (ModelState.IsValid)
                {
                    var existing = uow.ClientRepository.Get(model.Id);
                    existing.AllowOfflineAccess = model.AllowOfflineAccess;
                    existing.GrantTypes = string.Join(',', model.GrantTypes);
                    existing.Name = model.Name;
                    existing.PostLogoutUri = model.PostLogoutUri;
                    existing.RedirectUri = model.RedirectUri;
                    uow.ClientRepository.Update(existing);
                    uow.Commit();
                    model.UpdateSuccess();
                }
                model.ClientScopes = clientScopes.Select(cs => new ClientScopeModel { Id = model.Id, Name = cs.Name, DisplayName = cs.DisplayName });
                model.Scopes = scopes.Except(clientScopes).Select(cs => new ClientScopeModel { Id = model.Id, Name = cs.Name, DisplayName = cs.DisplayName });
            }

            return View(model);
        }

        /// <summary>(Restricted to PolicyNames.ClientsDelete) deletes the client described by
        /// clientId.</summary>
        /// <param name="clientId"> Identifier for the client. </param>
        /// <returns>An IActionResult.</returns>
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsDelete)]
        public IActionResult DeleteClient(int clientId)
        {
            return View(new ClientDeleteModel { Id = clientId });
        }

        /// <summary>(An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClientsDelete)
        /// deletes the client described by model.</summary>
        /// <param name="model">    The model. </param>
        /// <returns>An IActionResult.</returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsDelete)]
        public IActionResult DeleteClient(ClientDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var scopes = uow.ClientScopeRepository.GetByClientId(model.Id);
                    foreach(var scope in scopes)
                        uow.ClientScopeRepository.Delete(scope);

                    var claims = uow.ClientClaimRepository.GetAll().Where(c => c.Id == model.Id);
                    foreach(var claim in claims)
                        uow.ClientClaimRepository.Delete(claim);

                    uow.ClientRepository.Delete(model.Id);
                    uow.Commit();
                }
            }

            return RedirectToAction(nameof(ManageClients));
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesAccess) adds
        ///     a client scope.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsUpdate)]
        [Authorize(PolicyNames.ScopesAccess)]
        public IActionResult AddClientScope(ClientScopeModel model)
        {
            if (model.Id > 0)
            {
                // add user to role membership
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var client = uow.ClientRepository.Get(model.Id);
                    var scope = uow.ScopeRepository.GetByNames(new[] { model.Name }).SingleOrDefault();

                    if (client != null && scope != null)
                    {
                        uow.ClientScopeRepository.Add(new ClientScopeEntity { ScopeId = scope.Id, ClientId = client.Id });
                        uow.Commit();
                        return RedirectToAction(nameof(ManageClient), new { clientId = model.Id });
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesAccess)
        ///     removes the client scope described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsUpdate)]
        [Authorize(PolicyNames.ScopesAccess)]
        public IActionResult RemoveClientScope(ClientScopeModel model)
        {
            if (model.Id > 0)
            {
                // remove user from role membership
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var client = uow.ClientRepository.Get(model.Id);
                    var scope = uow.ScopeRepository.GetByNames(new[] { model.Name }).SingleOrDefault();

                    if (client != null && scope != null)
                    {
                        var scopeToRemove = uow.ClientScopeRepository.GetByClientId(client.Id).SingleOrDefault(cs => cs.ScopeId == scope.Id);
                        if (scopeToRemove != null)
                        {
                            uow.ClientScopeRepository.Delete(scopeToRemove);
                            uow.Commit();
                        }

                        return RedirectToAction(nameof(ManageClient), new { clientId = model.Id });
                    }
                }
            }

            return View("Error");
        }

        #endregion

        #region ClientClaims

        /// <summary>   (An Action that handles HTTP GET requests) manage Client claims. </summary>
        /// <param name="clientid">   The client-id. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ClientClaimsAccess)]
        public IActionResult ManageClientClaims(int clientid)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var claims = uow.ClientClaimRepository.GetAll().Where(c => c.ClientId == clientid);
                return View(new ClientClaimsModel
                {
                    ClientId = clientid,
                    ClaimEntries = claims.Select(c => new ClientClaimsModel.ClaimEntry { Type = c.ClaimType, Value = c.ClaimValue }).ToList()
                });
            }
        }

        /// <summary>   (An Action that handles HTTP GET requests) adds a Client claim. </summary>
        /// <param name="clientid">   The client-id. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ClientClaimsAccess)]
        [Authorize(PolicyNames.ClientClaimsUpdate)]
        public IActionResult AddClientClaim(int clientid)
        {
            return View(new ClientClaimAddModel { ClientId = clientid });
        }

        /// <summary>   Adds a Client claim. </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientClaimsAccess)]
        [Authorize(PolicyNames.ClientClaimsUpdate)]
        public IActionResult AddClientClaim(ClientClaimAddModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var existing = uow.ClientClaimRepository.GetAll().Where(c => c.ClientId == model.ClientId).SingleOrDefault(c => c.ClaimType == model.Type);
                    if (existing == null)
                    {
                        uow.ClientClaimRepository.Add(new ClientClaimEntity
                        {
                            ClientId = model.ClientId,
                            ClaimType = model.Type,
                            ClaimValue = model.Value
                        });
                    }
                    else
                    {
                        existing.ClaimValue = model.Value;
                        uow.ClientClaimRepository.Update(existing);
                    }

                    uow.Commit();
                    model.UpdateSuccess();
                    RedirectToAction(nameof(ManageClientClaims), new { Clientid = model.ClientId });
                }
            }

            return View(model);
        }

        /// <summary>   Edit Client claim. </summary>
        /// <param name="clientid">       The client-id. </param>
        /// <param name="claimType">    Type of the claim. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ClientClaimsAccess)]
        [Authorize(PolicyNames.ClientClaimsUpdate)]
        public IActionResult EditClientClaim(int clientid, string claimType)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var claim = uow.ClientClaimRepository.GetAll().Where(c => c.ClientId == clientid).SingleOrDefault(c => c.ClaimType == claimType);
                return View(new ClientClaimEditModel { ClientId = clientid, Type = claimType, Value = claim?.ClaimValue });
            }
        }

        /// <summary>   (An Action that handles HTTP POST requests) edit Client claim. </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientClaimsAccess)]
        [Authorize(PolicyNames.ClientClaimsUpdate)]
        public IActionResult EditClientClaim(ClientClaimEditModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var existing = uow.ClientClaimRepository.GetAll().Where(c => c.ClientId == model.ClientId).SingleOrDefault(c => c.ClaimType == model.Type);
                    if (existing == null)
                        return View("Error");

                    existing.ClaimValue = model.Value;
                    uow.ClientClaimRepository.Update(existing);

                    uow.Commit();
                    model.UpdateSuccess();
                }
            }

            return View(model);
        }

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsDelete)
        ///     deletes the Client claim.
        /// </summary>
        /// <param name="clientId">       Identifier for the Client. </param>
        /// <param name="claimType">    Type of the claim. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ClientClaimsAccess)]
        [Authorize(PolicyNames.ClientClaimsDelete)]
        public IActionResult DeleteClientClaim(int clientId, string claimType)
        {
            return View(new ClientClaimDeleteModel { ClientId = clientId, Type = claimType });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClaimsDelete)
        ///     deletes the Client claim described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientClaimsAccess)]
        [Authorize(PolicyNames.ClientClaimsDelete)]
        public IActionResult DeleteClientClaim(ClientClaimDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var claim = uow.ClientClaimRepository.GetAll().Where(c => c.ClientId == model.ClientId).SingleOrDefault(c => c.ClaimType == model.Type);
                    uow.ClientClaimRepository.Delete(claim);
                    uow.Commit();
                }

                return RedirectToAction(nameof(ManageClientClaims), new { Clientid = model.ClientId });
            }

            return View("Error");
        }

        #endregion

        #region Scopes

        /// <summary>(Restricted to PolicyNames.ScopesAccess) manage scopes.</summary>
        /// <returns>An IActionResult.</returns>
        [Authorize(PolicyNames.ScopesAccess)]
        public IActionResult ManageScopes()
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var scopes = uow.ScopeRepository.GetAll();
                return View(scopes);
            }
        }

        /// <summary>(Restricted to PolicyNames.ScopesCreate) adds scope.</summary>
        /// <returns>An IActionResult.</returns>
        [Authorize(PolicyNames.ScopesAccess)]
        [Authorize(PolicyNames.ScopesCreate)]
        public IActionResult AddScope()
        {
            return View(new ScopeAddModel { Emphasize = true, Required = true, ShowInDiscoveryDocument = true });
        }

        /// <summary>(An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesCreate)
        /// adds a scope.</summary>
        /// <param name="model">    The model. </param>
        /// <returns>An IActionResult.</returns>
        [HttpPost]
        [Authorize(PolicyNames.ScopesAccess)]
        [Authorize(PolicyNames.ScopesCreate)]
        public IActionResult AddScope(ScopeAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    uow.ScopeRepository.Add(new ScopeEntity
                    {
                        Name = model.Name,
                        DisplayName = model.DisplayName,
                        Description = model.Description,
                        Required = model.Required,
                        Emphasize = model.Emphasize,
                        ShowInDiscoveryDocument = model.ShowInDiscoveryDocument
                    });
                    uow.Commit();
                }

                return RedirectToAction(nameof(ManageScopes));
            }

            return View(model);
        }

        /// <summary>(Restricted to PolicyNames.ScopesUpdate) manage scope.</summary>
        /// <param name="scopeId">  Identifier for the scope. </param>
        /// <returns>An IActionResult.</returns>
        [Authorize(PolicyNames.ScopesAccess)]
        [Authorize(PolicyNames.ScopesUpdate)]
        public IActionResult ManageScope(int scopeId)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var scope = uow.ScopeRepository.Get(scopeId);
                return View(new ScopeEditModel
                {
                    Id = scope.Id,
                    Name = scope.Name,
                    DisplayName = scope.DisplayName,
                    Description = scope.Description,
                    Required = scope.Required,
                    Emphasize = scope.Emphasize,
                    ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument
                });
            }
        }

        /// <summary>(An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesUpdate)
        /// manage scope.</summary>
        /// <param name="model">    The model. </param>
        /// <returns>An IActionResult.</returns>
        [HttpPost]
        [Authorize(PolicyNames.ScopesAccess)]
        [Authorize(PolicyNames.ScopesUpdate)]
        public IActionResult ManageScope(ScopeEditModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var existing = uow.ScopeRepository.Get(model.Id);
                    existing.Name = model.Name;
                    existing.DisplayName = model.DisplayName;
                    existing.Description = model.Description;
                    existing.Required = model.Required;
                    existing.Emphasize = model.Emphasize;
                    existing.ShowInDiscoveryDocument = model.ShowInDiscoveryDocument;
                    uow.ScopeRepository.Update(existing);
                    uow.Commit();

                    model.UpdateSuccess();
                }
            }

            return View(model);
        }

        /// <summary>
        ///     (Restricted to PolicyNames.ScopesDelete) deletes the scope described by scopeId.
        /// </summary>
        /// <param name="scopeId">  Identifier for the scope. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.ScopesAccess)]
        [Authorize(PolicyNames.ScopesDelete)]
        public IActionResult DeleteScope(int scopeId)
        {
            return View(new ScopeDeleteModel { Id = scopeId });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesDelete)
        ///     deletes the scope described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ScopesAccess)]
        [Authorize(PolicyNames.ScopesDelete)]
        public IActionResult DeleteScope(ScopeDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var scope = uow.ScopeRepository.Get(model.Id);
                    uow.ScopeRepository.Delete(scope);
                    uow.Commit();
                }

                return RedirectToAction(nameof(ManageScopes));
            }

            return View(model);
        }

        #endregion

        #region ApiResources

        /// <summary>   (Restricted to PolicyNames.ApiResourcesAccess) manage API resources. </summary>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.ApiResourcesAccess)]
        public IActionResult ManageApiResources()
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var resources = uow.ApiResourceRepository.GetAll();
                return View(resources);
            }
        }
       
        /// <summary>   (Restricted to PolicyNames.ClientsCreate) adds API resouurce. </summary>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsCreate)]
        public IActionResult AddApiResource()
        {
            return View(new ResourceAddModel());
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClientsCreate)
        ///     adds an API resource.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ClientsAccess)]
        [Authorize(PolicyNames.ClientsCreate)]
        public IActionResult AddApiResource(ResourceAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    uow.ApiResourceRepository.Add(new ApiResourceEntity
                    {
                        Name = model.Name,
                        DisplayName = model.DisplayName,
                        Description = model.Description,
                        Enabled = model.Enabled
                    });
                    uow.Commit();
                }
                return RedirectToAction(nameof(ManageApiResources));
            }

            return View(model);
        }

        /// <summary>   (Restricted to PolicyNames.ApiResourcesUpdate) manage API resource. </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.ApiResourcesAccess)]
        [Authorize(PolicyNames.ApiResourcesUpdate)]
        public IActionResult ManageApiResource(int resourceId)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var resource = uow.ApiResourceRepository.Get(resourceId);
                var scopes = uow.ScopeRepository.GetAll();
                var resourceScopes = uow.ApiResourceScopeRepository.GetByApiIds(new [] {resource.Id}).Select(cs => scopes.Single(s => s.Id == cs.ScopeId)).ToList();

                return View(new ResourceEditModel
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    DisplayName = resource.DisplayName,
                    Description = resource.Description,
                    Enabled = resource.Enabled,
                    ResourceScopes = resourceScopes.Select(cs => new ResourceScopeModel { Id = resourceId, Name = cs.Name, DisplayName = cs.DisplayName }),
                    Scopes = scopes.Except(resourceScopes).Select(cs => new ResourceScopeModel { Id = resourceId, Name = cs.Name, DisplayName = cs.DisplayName })
                });
            }
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to
        ///     PolicyNames.ApiResourcesUpdate) manage API resource.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ApiResourcesAccess)]
        [Authorize(PolicyNames.ApiResourcesUpdate)]
        public IActionResult ManageApiResource(ResourceEditModel model)
        {
            model.Update();

            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var scopes = uow.ScopeRepository.GetAll();
                var resourceScopes = uow.ApiResourceScopeRepository.GetByApiIds(new[] { model.Id }).Select(cs => scopes.Single(s => s.Id == cs.ScopeId)).ToList();

                if (ModelState.IsValid)
                {
                    var existing = uow.ApiResourceRepository.Get(model.Id);
                    existing.Name = model.Name;
                    existing.DisplayName = model.DisplayName;
                    existing.Description = model.Description;
                    existing.Enabled = model.Enabled;
                    uow.ApiResourceRepository.Update(existing);
                    uow.Commit();
                    model.UpdateSuccess();
                }
                model.ResourceScopes = resourceScopes.Select(cs => new ResourceScopeModel { Id = model.Id, Name = cs.Name, DisplayName = cs.DisplayName });
                model.Scopes = scopes.Except(resourceScopes).Select(cs => new ResourceScopeModel { Id = model.Id, Name = cs.Name, DisplayName = cs.DisplayName });
            }

            return View(model);
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesAccess) adds
        ///     an API resource scope.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ApiResourcesAccess)]
        [Authorize(PolicyNames.ApiResourcesUpdate)]
        [Authorize(PolicyNames.ScopesAccess)]
        public IActionResult AddApiResourceScope(ResourceScopeModel model)
        {
            if (model.Id > 0)
            {
                // add user to role membership
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var resource = uow.ApiResourceRepository.Get(model.Id);
                    var scope = uow.ScopeRepository.GetByNames(new[] { model.Name }).SingleOrDefault();

                    if (resource != null && scope != null)
                    {
                        uow.ApiResourceScopeRepository.Add(new ApiResourceScopeEntity { ScopeId = scope.Id, ApiResourceId = resource.Id });
                        uow.Commit();
                        return RedirectToAction(nameof(ManageApiResource), new { resourceId = model.Id });
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesAccess)
        ///     removes the API resource scope described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ApiResourcesAccess)]
        [Authorize(PolicyNames.ApiResourcesUpdate)]
        [Authorize(PolicyNames.ScopesAccess)]
        public IActionResult RemoveApiResourceScope(ResourceScopeModel model)
        {
            if (model.Id > 0)
            {
                // remove user from role membership
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var resource = uow.ApiResourceRepository.Get(model.Id);
                    var scope = uow.ScopeRepository.GetByNames(new[] { model.Name }).SingleOrDefault();

                    if (resource != null && scope != null)
                    {
                        var scopeToRemove = uow.ApiResourceScopeRepository.GetByApiIds(new [] {resource.Id }).SingleOrDefault(cs => cs.ScopeId == scope.Id);
                        if (scopeToRemove != null)
                        {
                            uow.ApiResourceScopeRepository.Delete(scopeToRemove);
                            uow.Commit();
                        }

                        return RedirectToAction(nameof(ManageApiResource), new { resourceId = model.Id });
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        ///     (Restricted to PolicyNames.ApiResourcesDelete) deletes the API resource described by
        ///     resourceId.
        /// </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.ApiResourcesAccess)]
        [Authorize(PolicyNames.ApiResourcesDelete)]
        public IActionResult DeleteApiResource(int resourceId)
        {
            return View(new ResourceDeleteModel { Id = resourceId });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to
        ///     PolicyNames.ApiResourcesDelete) deletes the API resource described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ApiResourcesAccess)]
        [Authorize(PolicyNames.ApiResourcesDelete)]
        public IActionResult DeleteApiResource(ResourceDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var scopes = uow.ApiResourceScopeRepository.GetByApiIds(new[] {model.Id});
                    foreach(var scope in scopes)
                        uow.ApiResourceScopeRepository.Delete(scope);

                    var claims = uow.ApiResourceClaimRepository.GetByApiId(model.Id);
                    foreach(var claim in claims)
                        uow.ApiResourceClaimRepository.Delete(claim);

                    uow.ApiResourceRepository.Delete(model.Id);
                    uow.Commit();
                }
            }

            return RedirectToAction(nameof(ManageApiResources));
        }

        #endregion

        #region ApiResourceClaims

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsAccess)
        ///     manage API resource claims.
        /// </summary>
        /// <param name="resourceid">   The resourceid. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ApiResourceClaimsAccess)]
        public IActionResult ManageApiResourceClaims(int resourceid)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var claims = uow.ApiResourceClaimRepository.GetAll().Where(c => c.ApiResourceId == resourceid);
                return View(new ResourceClaimsModel
                {
                    ResourceId = resourceid,
                    ClaimEntries = claims.Select(c => new ResourceClaimsModel.ClaimEntry { Type = c.ClaimType }).ToList()
                });
            }
        }

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsUpdate) adds
        ///     an API resource claim.
        /// </summary>
        /// <param name="resourceid">   The resourceid. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ApiResourceClaimsAccess)]
        [Authorize(PolicyNames.ApiResourceClaimsUpdate)]
        public IActionResult AddApiResourceClaim(int resourceid)
        {
            return View(new ResourceClaimAddModel { ResourceId = resourceid });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClaimsUpdate) adds
        ///     an API resource claim.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ApiResourceClaimsAccess)]
        [Authorize(PolicyNames.ApiResourceClaimsUpdate)]
        public IActionResult AddApiResourceClaim(ResourceClaimAddModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var existing = uow.ApiResourceClaimRepository.GetAll().Where(c => c.ApiResourceId == model.ResourceId).SingleOrDefault(c => c.ClaimType == model.Type);
                    if (existing == null)
                    {
                        uow.ApiResourceClaimRepository.Add(new ApiResourceClaimEntity
                        {
                            ApiResourceId = model.ResourceId,
                            ClaimType = model.Type
                        });
                    }
                    else
                    {
                        uow.ApiResourceClaimRepository.Update(existing);
                    }

                    uow.Commit();
                    model.UpdateSuccess();
                    RedirectToAction(nameof(ManageApiResourceClaims), new { ApiResourceid = model.ResourceId });
                }
            }

            return View(model);
        }

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsUpdate) edit
        ///     API resource claim.
        /// </summary>
        /// <param name="resourceid">   The resourceid. </param>
        /// <param name="claimType">    Type of the claim. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ApiResourceClaimsAccess)]
        [Authorize(PolicyNames.ApiResourceClaimsUpdate)]
        public IActionResult EditApiResourceClaim(int resourceid, string claimType)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var claim = uow.ApiResourceClaimRepository.GetAll().Where(c => c.ApiResourceId == resourceid).SingleOrDefault(c => c.ClaimType == claimType);
                return View(new ResourceClaimEditModel { ResourceId = resourceid, Type = claimType });
            }
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClaimsUpdate) edit
        ///     API resource claim.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ApiResourceClaimsAccess)]
        [Authorize(PolicyNames.ApiResourceClaimsUpdate)]
        public IActionResult EditApiResourceClaim(ResourceClaimEditModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var existing = uow.ApiResourceClaimRepository.GetAll().Where(c => c.ApiResourceId == model.ResourceId).SingleOrDefault(c => c.ClaimType == model.Type);
                    if (existing == null)
                        return View("Error");

                    uow.ApiResourceClaimRepository.Update(existing);

                    uow.Commit();
                    model.UpdateSuccess();
                }
            }

            return View(model);
        }

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsDelete)
        ///     deletes the API resource claim.
        /// </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <param name="claimType">    Type of the claim. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.ApiResourceClaimsAccess)]
        [Authorize(PolicyNames.ApiResourceClaimsDelete)]
        public IActionResult DeleteApiResourceClaim(int resourceId, string claimType)
        {
            return View(new ResourceClaimDeleteModel { ResourceId = resourceId, Type = claimType });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClaimsDelete)
        ///     deletes the API resource claim described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.ApiResourceClaimsAccess)]
        [Authorize(PolicyNames.ApiResourceClaimsDelete)]
        public IActionResult DeleteApiResourceClaim(ResourceClaimDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var claim = uow.ApiResourceClaimRepository.GetAll().Where(c => c.ApiResourceId == model.ResourceId).SingleOrDefault(c => c.ClaimType == model.Type);
                    uow.ApiResourceClaimRepository.Delete(claim);
                    uow.Commit();
                }

                return RedirectToAction(nameof(ManageApiResourceClaims), new { ApiResourceid = model.ResourceId });
            }

            return View("Error");
        }

        #endregion

        #region IdentityResources

        /// <summary>   (Restricted to PolicyNames.IdentityResourcesAccess) manage Identity resources. </summary>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        public IActionResult ManageIdentityResources()
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var resources = uow.IdentityResourceRepository.GetAll();
                return View(resources);
            }
        }

        /// <summary>   (Restricted to PolicyNames.ClientsCreate) adds Identity resouurce. </summary>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesCreate)]
        public IActionResult AddIdentityResource()
        {
            return View(new IdentityResourceAddModel());
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClientsCreate)
        ///     adds an Identity resource.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesCreate)]
        public IActionResult AddIdentityResource(IdentityResourceAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    uow.IdentityResourceRepository.Add(new IdentityResourceEntity
                    {
                        Name = model.Name,
                        DisplayName = model.DisplayName,
                        Description = model.Description,
                        Enabled = model.Enabled,
                        Required = model.Required,
                        Emphasize = model.Emphasize,
                        ShowInDiscoveryDocument = model.ShowInDiscoveryDocument
                    });
                    uow.Commit();
                }
                return RedirectToAction(nameof(ManageIdentityResources));
            }

            return View(model);
        }

        /// <summary>   (Restricted to PolicyNames.IdentityResourcesUpdate) manage Identity resource. </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesUpdate)]
        public IActionResult ManageIdentityResource(int resourceId)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var resource = uow.IdentityResourceRepository.Get(resourceId);
                var scopes = uow.ScopeRepository.GetAll();
                var resourceScopes = uow.IdentityResourceScopeRepository.GetByIdentityIds(new[] { resource.Id }).Select(cs => scopes.Single(s => s.Id == cs.ScopeId)).ToList();

                return View(new IdentityResourceEditModel
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    DisplayName = resource.DisplayName,
                    Description = resource.Description,
                    Enabled = resource.Enabled,
                    Required = resource.Required,
                    Emphasize = resource.Emphasize,
                    ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                    ResourceScopes = resourceScopes.Select(cs => new ResourceScopeModel { Id = resourceId, Name = cs.Name, DisplayName = cs.DisplayName }),
                    Scopes = scopes.Except(resourceScopes).Select(cs => new ResourceScopeModel { Id = resourceId, Name = cs.Name, DisplayName = cs.DisplayName })
                });
            }
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to
        ///     PolicyNames.IdentityResourcesUpdate) manage Identity resource.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesUpdate)]
        public IActionResult ManageIdentityResource(IdentityResourceEditModel model)
        {
            model.Update();

            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var scopes = uow.ScopeRepository.GetAll();
                var resourceScopes = uow.IdentityResourceScopeRepository.GetByIdentityIds(new[] { model.Id }).Select(cs => scopes.Single(s => s.Id == cs.ScopeId)).ToList();

                if (ModelState.IsValid)
                {
                    var existing = uow.IdentityResourceRepository.Get(model.Id);
                    existing.Name = model.Name;
                    existing.DisplayName = model.DisplayName;
                    existing.Description = model.Description;
                    existing.Enabled = model.Enabled;
                    existing.Required = model.Required;
                    existing.Emphasize = model.Emphasize;
                    existing.ShowInDiscoveryDocument = model.ShowInDiscoveryDocument;
                    uow.IdentityResourceRepository.Update(existing);
                    uow.Commit();
                    model.UpdateSuccess();
                }
                model.ResourceScopes = resourceScopes.Select(cs => new ResourceScopeModel { Id = model.Id, Name = cs.Name, DisplayName = cs.DisplayName });
                model.Scopes = scopes.Except(resourceScopes).Select(cs => new ResourceScopeModel { Id = model.Id, Name = cs.Name, DisplayName = cs.DisplayName });
            }

            return View(model);
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesAccess) adds
        ///     an Identity resource scope.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesUpdate)]
        [Authorize(PolicyNames.ScopesAccess)]
        public IActionResult AddIdentityResourceScope(ResourceScopeModel model)
        {
            if (model.Id > 0)
            {
                // add user to role membership
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var resource = uow.IdentityResourceRepository.Get(model.Id);
                    var scope = uow.ScopeRepository.GetByNames(new[] { model.Name }).SingleOrDefault();

                    if (resource != null && scope != null)
                    {
                        uow.IdentityResourceScopeRepository.Add(new IdentityResourceScopeEntity { ScopeId = scope.Id, IdentityResourceId = resource.Id });
                        uow.Commit();
                        return RedirectToAction(nameof(ManageIdentityResource), new { resourceId = model.Id });
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ScopesAccess)
        ///     removes the Identity resource scope described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesUpdate)]
        [Authorize(PolicyNames.ScopesAccess)]
        public IActionResult RemoveIdentityResourceScope(ResourceScopeModel model)
        {
            if (model.Id > 0)
            {
                // remove user from role membership
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var resource = uow.IdentityResourceRepository.Get(model.Id);
                    var scope = uow.ScopeRepository.GetByNames(new[] { model.Name }).SingleOrDefault();

                    if (resource != null && scope != null)
                    {
                        var scopeToRemove = uow.IdentityResourceScopeRepository.GetByIdentityIds(new[] { resource.Id }).SingleOrDefault(cs => cs.ScopeId == scope.Id);
                        if (scopeToRemove != null)
                        {
                            uow.IdentityResourceScopeRepository.Delete(scopeToRemove);
                            uow.Commit();
                        }

                        return RedirectToAction(nameof(ManageIdentityResource), new { resourceId = model.Id });
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        ///     (Restricted to PolicyNames.IdentityResourcesDelete) deletes the Identity resource described by
        ///     resourceId.
        /// </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesDelete)]
        public IActionResult DeleteIdentityResource(int resourceId)
        {
            return View(new ResourceDeleteModel { Id = resourceId });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to
        ///     PolicyNames.IdentityResourcesDelete) deletes the Identity resource described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourcesAccess)]
        [Authorize(PolicyNames.IdentityResourcesDelete)]
        public IActionResult DeleteIdentityResource(ResourceDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var scopes = uow.IdentityResourceScopeRepository.GetByIdentityIds(new[] { model.Id });
                    foreach (var scope in scopes)
                        uow.IdentityResourceScopeRepository.Delete(scope);

                    var claims = uow.IdentityResourceClaimRepository.GetByIdentityId(model.Id);
                    foreach (var claim in claims)
                        uow.IdentityResourceClaimRepository.Delete(claim);

                    uow.IdentityResourceRepository.Delete(model.Id);
                    uow.Commit();
                }
            }

            return RedirectToAction(nameof(ManageIdentityResources));
        }

        #endregion

        #region IdentityResourceClaims

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsAccess)
        ///     manage Identity resource claims.
        /// </summary>
        /// <param name="resourceid">   The resourceid. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.IdentityResourceClaimsAccess)]
        public IActionResult ManageIdentityResourceClaims(int resourceid)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var claims = uow.IdentityResourceClaimRepository.GetAll().Where(c => c.IdentityResourceId == resourceid);
                return View(new ResourceClaimsModel
                {
                    ResourceId = resourceid,
                    ClaimEntries = claims.Select(c => new ResourceClaimsModel.ClaimEntry { Type = c.ClaimType }).ToList()
                });
            }
        }

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsUpdate) adds
        ///     an Identity resource claim.
        /// </summary>
        /// <param name="resourceid">   The resourceid. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.IdentityResourceClaimsAccess)]
        [Authorize(PolicyNames.IdentityResourceClaimsUpdate)]
        public IActionResult AddIdentityResourceClaim(int resourceid)
        {
            return View(new ResourceClaimAddModel { ResourceId = resourceid });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClaimsUpdate) adds
        ///     an Identity resource claim.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourceClaimsAccess)]
        [Authorize(PolicyNames.IdentityResourceClaimsUpdate)]
        public IActionResult AddIdentityResourceClaim(ResourceClaimAddModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var existing = uow.IdentityResourceClaimRepository.GetAll().Where(c => c.IdentityResourceId == model.ResourceId).SingleOrDefault(c => c.ClaimType == model.Type);
                    if (existing == null)
                    {
                        uow.IdentityResourceClaimRepository.Add(new IdentityResourceClaimEntity
                        {
                            IdentityResourceId = model.ResourceId,
                            ClaimType = model.Type
                        });
                    }
                    else
                    {
                        uow.IdentityResourceClaimRepository.Update(existing);
                    }

                    uow.Commit();
                    model.UpdateSuccess();
                    RedirectToAction(nameof(ManageIdentityResourceClaims), new { IdentityResourceid = model.ResourceId });
                }
            }

            return View(model);
        }

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsUpdate) edit
        ///     Identity resource claim.
        /// </summary>
        /// <param name="resourceid">   The resourceid. </param>
        /// <param name="claimType">    Type of the claim. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.IdentityResourceClaimsAccess)]
        [Authorize(PolicyNames.IdentityResourceClaimsUpdate)]
        public IActionResult EditIdentityResourceClaim(int resourceid, string claimType)
        {
            using (var uow = _identityServerDataService.StartUnitOfWork())
            {
                var claim = uow.IdentityResourceClaimRepository.GetAll().Where(c => c.IdentityResourceId == resourceid).SingleOrDefault(c => c.ClaimType == claimType);
                return View(new ResourceClaimEditModel { ResourceId = resourceid, Type = claimType });
            }
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClaimsUpdate) edit
        ///     Identity resource claim.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourceClaimsAccess)]
        [Authorize(PolicyNames.IdentityResourceClaimsUpdate)]
        public IActionResult EditIdentityResourceClaim(ResourceClaimEditModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var existing = uow.IdentityResourceClaimRepository.GetAll().Where(c => c.IdentityResourceId == model.ResourceId).SingleOrDefault(c => c.ClaimType == model.Type);
                    if (existing == null)
                        return View("Error");

                    uow.IdentityResourceClaimRepository.Update(existing);

                    uow.Commit();
                    model.UpdateSuccess();
                }
            }

            return View(model);
        }

        /// <summary>
        ///     (An Action that handles HTTP GET requests) (Restricted to PolicyNames.ClaimsDelete)
        ///     deletes the Identity resource claim.
        /// </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <param name="claimType">    Type of the claim. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpGet]
        [Authorize(PolicyNames.IdentityResourceClaimsAccess)]
        [Authorize(PolicyNames.IdentityResourceClaimsDelete)]
        public IActionResult DeleteIdentityResourceClaim(int resourceId, string claimType)
        {
            return View(new ResourceClaimDeleteModel { ResourceId = resourceId, Type = claimType });
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to PolicyNames.ClaimsDelete)
        ///     deletes the Identity resource claim described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.IdentityResourceClaimsAccess)]
        [Authorize(PolicyNames.IdentityResourceClaimsDelete)]
        public IActionResult DeleteIdentityResourceClaim(ResourceClaimDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _identityServerDataService.StartUnitOfWork())
                {
                    var claim = uow.IdentityResourceClaimRepository.GetAll().Where(c => c.IdentityResourceId == model.ResourceId).SingleOrDefault(c => c.ClaimType == model.Type);
                    uow.IdentityResourceClaimRepository.Delete(claim);
                    uow.Commit();
                }

                return RedirectToAction(nameof(ManageIdentityResourceClaims), new { IdentityResourceid = model.ResourceId });
            }

            return View("Error");
        }

        #endregion
    }
}