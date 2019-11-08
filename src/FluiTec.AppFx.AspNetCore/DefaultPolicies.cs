using System.Collections.Generic;
using FluiTec.AppFx.Authorization.Activity;
using FluiTec.AppFx.Authorization.Activity.Requirements;
using FluiTec.AppFx.Identity.Entities;
using FluiTec.AppFx.IdentityServer.Entities;
using FluiTec.AppFx.Localization.Entities;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>A default policies.</summary>
    public class DefaultPolicies
    {
        /// <summary>Gets or sets the policies.</summary>
        /// <value>The policies.</value>
        public IReadOnlyList<DefaultPolicy> Policies { get; }

        /// <summary>Default constructor.</summary>
        public DefaultPolicies()
        {
            Policies = new List<DefaultPolicy>(new[]
            {
                AdministrativeAccess,
                UsersAccess, UsersCreate, UsersUpdate, UsersDelete,
                RolesAccess, RolesCreate, RolesUpdate, RolesDelete,
                ClaimsAccess, ClaimsCreate, ClaimsUpdate, ClaimsDelete,
                ClientsAccess, ClientsCreate, ClientsUpdate, ClientsDelete,
                ScopesAccess, ScopesCreate, ScopesUpdate, ScopesDelete,
                ClientClaimsAccess, ClientClaimsCreate, ClientClaimsUpdate, ClientClaimsDelete,
                ApiResourcesAccess, ApiResourcesCreate, ApiResourcesUpdate, ApiResourcesDelete,
                ApiResourceClaimsAccess, ApiResourceClaimsCreate, ApiResourceClaimsUpdate, ApiResourceClaimsDelete,
                IdentityResourcesAccess, IdentityResourcesCreate, IdentityResourcesUpdate, IdentityResourcesDelete,
                IdentityResourceClaimsAccess, IdentityResourceClaimsCreate, IdentityResourceClaimsUpdate, IdentityResourceClaimsDelete,
                LocalizationAccess, LocalizationCreate, LocalizationUpdate, LocalizationDelete
            }).AsReadOnly();
        }

        /// <summary>Gets the administrative access.</summary>
        /// <value>The administrative access.</value>
        public DefaultPolicy AdministrativeAccess { get; } = new DefaultPolicy(PolicyNames.AdministrativeAccess, new AdministrativeAccessRequirement(new[]
        {
            ResourceActivities.AccessRequirement(typeof(IdentityUserEntity)),
            ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)),
            ResourceActivities.AccessRequirement(typeof(ClientEntity))
        }));

        #region Users

        /// <summary>Gets the users access.</summary>
        /// <value>The users access.</value>
        public DefaultPolicy UsersAccess { get; } = new DefaultPolicy(PolicyNames.UsersAccess, ResourceActivities.AccessRequirement(typeof(IdentityUserEntity)));

        /// <summary>Gets the users create.</summary>
        /// <value>The users create.</value>
        public DefaultPolicy UsersCreate { get; } = new DefaultPolicy(PolicyNames.UsersCreate, ResourceActivities.CreateRequirement(typeof(IdentityUserEntity)));

        /// <summary>Gets the users update.</summary>
        /// <value>The users update.</value>
        public DefaultPolicy UsersUpdate { get; } = new DefaultPolicy(PolicyNames.UsersUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityUserEntity)));

        /// <summary>Gets the users delete.</summary>
        /// <value>The users delete.</value>
        public DefaultPolicy UsersDelete { get; } = new DefaultPolicy(PolicyNames.UsersDelete, ResourceActivities.DeleteRequirement(typeof(IdentityUserEntity)));

        #endregion

        #region Roles

        /// <summary>Gets the roles access.</summary>
        /// <value>The roles access.</value>
        public DefaultPolicy RolesAccess { get; } = new DefaultPolicy(PolicyNames.RolesAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the roles create.</summary>
        /// <value>The roles create.</value>
        public DefaultPolicy RolesCreate { get; } = new DefaultPolicy(PolicyNames.RolesCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the roles update.</summary>
        /// <value>The roles update.</value>
        public DefaultPolicy RolesUpdate { get; } = new DefaultPolicy(PolicyNames.RolesUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the roles delete.</summary>
        /// <value>The roles delete.</value>
        public DefaultPolicy RolesDelete { get; } = new DefaultPolicy(PolicyNames.RolesDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region Claims

        /// <summary>Gets the Claims access.</summary>
        /// <value>The Claims access.</value>
        public DefaultPolicy ClaimsAccess { get; } = new DefaultPolicy(PolicyNames.ClaimsAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Claims create.</summary>
        /// <value>The Claims create.</value>
        public DefaultPolicy ClaimsCreate { get; } = new DefaultPolicy(PolicyNames.ClaimsCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Claims update.</summary>
        /// <value>The Claims update.</value>
        public DefaultPolicy ClaimsUpdate { get; } = new DefaultPolicy(PolicyNames.ClaimsUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Claims delete.</summary>
        /// <value>The Claims delete.</value>
        public DefaultPolicy ClaimsDelete { get; } = new DefaultPolicy(PolicyNames.ClaimsDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region Clients

        /// <summary>Gets the Clients access.</summary>
        /// <value>The Clients access.</value>
        public DefaultPolicy ClientsAccess { get; } = new DefaultPolicy(PolicyNames.ClientsAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Clients create.</summary>
        /// <value>The Clients create.</value>
        public DefaultPolicy ClientsCreate { get; } = new DefaultPolicy(PolicyNames.ClientsCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Clients update.</summary>
        /// <value>The Clients update.</value>
        public DefaultPolicy ClientsUpdate { get; } = new DefaultPolicy(PolicyNames.ClientsUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Clients delete.</summary>
        /// <value>The Clients delete.</value>
        public DefaultPolicy ClientsDelete { get; } = new DefaultPolicy(PolicyNames.ClientsDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region Scopes

        /// <summary>Gets the Scopes access.</summary>
        /// <value>The Scopes access.</value>
        public DefaultPolicy ScopesAccess { get; } = new DefaultPolicy(PolicyNames.ScopesAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Scopes create.</summary>
        /// <value>The Scopes create.</value>
        public DefaultPolicy ScopesCreate { get; } = new DefaultPolicy(PolicyNames.ScopesCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Scopes update.</summary>
        /// <value>The Scopes update.</value>
        public DefaultPolicy ScopesUpdate { get; } = new DefaultPolicy(PolicyNames.ScopesUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the Scopes delete.</summary>
        /// <value>The Scopes delete.</value>
        public DefaultPolicy ScopesDelete { get; } = new DefaultPolicy(PolicyNames.ScopesDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region ClientClaims

        /// <summary>Gets the ClientClaims access.</summary>
        /// <value>The ClientClaims access.</value>
        public DefaultPolicy ClientClaimsAccess { get; } = new DefaultPolicy(PolicyNames.ClientClaimsAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ClientClaims create.</summary>
        /// <value>The ClientClaims create.</value>
        public DefaultPolicy ClientClaimsCreate { get; } = new DefaultPolicy(PolicyNames.ClientClaimsCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ClientClaims update.</summary>
        /// <value>The ClientClaims update.</value>
        public DefaultPolicy ClientClaimsUpdate { get; } = new DefaultPolicy(PolicyNames.ClientClaimsUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ClientClaims delete.</summary>
        /// <value>The ClientClaims delete.</value>
        public DefaultPolicy ClientClaimsDelete { get; } = new DefaultPolicy(PolicyNames.ClientClaimsDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region ApiResources

        /// <summary>Gets the ApiResources access.</summary>
        /// <value>The ApiResources access.</value>
        public DefaultPolicy ApiResourcesAccess { get; } = new DefaultPolicy(PolicyNames.ApiResourcesAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ApiResources create.</summary>
        /// <value>The ApiResources create.</value>
        public DefaultPolicy ApiResourcesCreate { get; } = new DefaultPolicy(PolicyNames.ApiResourcesCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ApiResources update.</summary>
        /// <value>The ApiResources update.</value>
        public DefaultPolicy ApiResourcesUpdate { get; } = new DefaultPolicy(PolicyNames.ApiResourcesUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ApiResources delete.</summary>
        /// <value>The ApiResources delete.</value>
        public DefaultPolicy ApiResourcesDelete { get; } = new DefaultPolicy(PolicyNames.ApiResourcesDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region ApiResourceClaims

        /// <summary>Gets the ApiResourceClaims access.</summary>
        /// <value>The ApiResourceClaims access.</value>
        public DefaultPolicy ApiResourceClaimsAccess { get; } = new DefaultPolicy(PolicyNames.ApiResourceClaimsAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ApiResourceClaims create.</summary>
        /// <value>The ApiResourceClaims create.</value>
        public DefaultPolicy ApiResourceClaimsCreate { get; } = new DefaultPolicy(PolicyNames.ApiResourceClaimsCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ApiResourceClaims update.</summary>
        /// <value>The ApiResourceClaims update.</value>
        public DefaultPolicy ApiResourceClaimsUpdate { get; } = new DefaultPolicy(PolicyNames.ApiResourceClaimsUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the ApiResourceClaims delete.</summary>
        /// <value>The ApiResourceClaims delete.</value>
        public DefaultPolicy ApiResourceClaimsDelete { get; } = new DefaultPolicy(PolicyNames.ApiResourceClaimsDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region IdentityResources

        /// <summary>Gets the IdentityResources access.</summary>
        /// <value>The IdentityResources access.</value>
        public DefaultPolicy IdentityResourcesAccess { get; } = new DefaultPolicy(PolicyNames.IdentityResourcesAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the IdentityResources create.</summary>
        /// <value>The IdentityResources create.</value>
        public DefaultPolicy IdentityResourcesCreate { get; } = new DefaultPolicy(PolicyNames.IdentityResourcesCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the IdentityResources update.</summary>
        /// <value>The IdentityResources update.</value>
        public DefaultPolicy IdentityResourcesUpdate { get; } = new DefaultPolicy(PolicyNames.IdentityResourcesUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the IdentityResources delete.</summary>
        /// <value>The IdentityResources delete.</value>
        public DefaultPolicy IdentityResourcesDelete { get; } = new DefaultPolicy(PolicyNames.IdentityResourcesDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region IdentityResourceClaims

        /// <summary>Gets the IdentityResourceClaims access.</summary>
        /// <value>The IdentityResourceClaims access.</value>
        public DefaultPolicy IdentityResourceClaimsAccess { get; } = new DefaultPolicy(PolicyNames.IdentityResourceClaimsAccess, ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the IdentityResourceClaims create.</summary>
        /// <value>The IdentityResourceClaims create.</value>
        public DefaultPolicy IdentityResourceClaimsCreate { get; } = new DefaultPolicy(PolicyNames.IdentityResourceClaimsCreate, ResourceActivities.CreateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the IdentityResourceClaims update.</summary>
        /// <value>The IdentityResourceClaims update.</value>
        public DefaultPolicy IdentityResourceClaimsUpdate { get; } = new DefaultPolicy(PolicyNames.IdentityResourceClaimsUpdate, ResourceActivities.UpdateRequirement(typeof(IdentityRoleEntity)));

        /// <summary>Gets the IdentityResourceClaims delete.</summary>
        /// <value>The IdentityResourceClaims delete.</value>
        public DefaultPolicy IdentityResourceClaimsDelete { get; } = new DefaultPolicy(PolicyNames.IdentityResourceClaimsDelete, ResourceActivities.DeleteRequirement(typeof(IdentityRoleEntity)));

        #endregion

        #region Localization

        /// <summary>Gets the Localization access.</summary>
        /// <value>The Localization access.</value>
        public DefaultPolicy LocalizationAccess { get; } = new DefaultPolicy(PolicyNames.LocalizationAccess, ResourceActivities.AccessRequirement(typeof(ResourceEntity)));

        /// <summary>Gets the Localization create.</summary>
        /// <value>The Localization create.</value>
        public DefaultPolicy LocalizationCreate { get; } = new DefaultPolicy(PolicyNames.LocalizationCreate, ResourceActivities.CreateRequirement(typeof(ResourceEntity)));

        /// <summary>Gets the Localization update.</summary>
        /// <value>The Localization update.</value>
        public DefaultPolicy LocalizationUpdate { get; } = new DefaultPolicy(PolicyNames.LocalizationUpdate, ResourceActivities.UpdateRequirement(typeof(ResourceEntity)));

        /// <summary>Gets the Localization delete.</summary>
        /// <value>The Localization delete.</value>
        public DefaultPolicy LocalizationDelete { get; } = new DefaultPolicy(PolicyNames.LocalizationDelete, ResourceActivities.DeleteRequirement(typeof(ResourceEntity)));

        #endregion
    }
}