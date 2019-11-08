using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>A data Model for the edit client.</summary>
    [LocalizedModel]
    public class ClientEditModel : UpdateModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin.ClientEditModel";

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        /// <remarks>
        /// Cant be edited         
        /// </remarks>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int Id { get; set; }

        /// <summary>Gets or sets the identifier of the client.</summary>
        /// <value>The identifier of the client.</value>
        /// <remarks>
        /// Cant be edited         
        /// </remarks>
        [Display(Name = FullModelName + "ClientId", Description = "Id")]
        [DisplayTranslationForCulture("ClientId", "Id", "de")]
        public string ClientId { get; set; }

        /// <summary>Gets or sets the client secret.</summary>
        /// <value>The client secret.</value>
        /// <remarks>
        /// Cant be edited         
        /// </remarks>
        [Display(Name = FullModelName + "ClientSecret", Description = "Secret")]
        [DisplayTranslationForCulture("ClientSecret", "Geheimnis", "de")]
        public string ClientSecret { get; set; }

        [Display(Name = FullModelName + "Name", Description = "Name")]
        [DisplayTranslationForCulture("Name", "Name", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Name { get; set; }

        [Display(Name = FullModelName + "RedirectUri", Description = "Redirect uri")]
        [DisplayTranslationForCulture("RedirectUri", "Weiterleitungs-URL", "de")]
        public string RedirectUri { get; set; }

        [Display(Name = FullModelName + "PostLogoutUri", Description = "Postlogout uri")]
        [DisplayTranslationForCulture("PostLogoutUri", "Abmeldungs-URL", "de")]
        public string PostLogoutUri { get; set; }

        [Display(Name = FullModelName + "AllowOfflineAccess", Description = "Allow offline access")]
        [DisplayTranslationForCulture("AllowOfflineAccess", "Offline-Zugriff erlauben", "de")]
        public bool AllowOfflineAccess { get; set; }

        [Display(Name = FullModelName + "GrantTypes", Description = "Grant types")]
        [DisplayTranslationForCulture("GrantTypes", "Zugriffstypen", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public List<string> GrantTypes { get; set; }

        /// <summary>   Gets or sets the client scopes. </summary>
        /// <value> The client scopes. </value>
        public IEnumerable<ClientScopeModel> ClientScopes { get; set; }

        /// <summary>   Gets or sets the scopes. </summary>
        /// <value> The scopes. </value>
        public IEnumerable<ClientScopeModel> Scopes { get; set; }
    }
}