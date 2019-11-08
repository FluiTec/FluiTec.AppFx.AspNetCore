using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluiTec.DbLocalizationProvider.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin
{
    /// <summary>   A data Model for the role edit. </summary>
    [LocalizedModel]
    public class RoleEditModel : UpdateModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin.RoleAddModel";

        /// <summary>   Gets or sets the identifier. </summary>
        /// <value> The identifier. </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int Id { get; set; }

        /// <summary>   Gets or sets the name. </summary>
        /// <value> The name. </value>
        [Display(Name = FullModelName + "Name", Description = "Name")]
        [DisplayTranslationForCulture("Name", "Name", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Name { get; set; }

        /// <summary>   Gets or sets the description. </summary>
        /// <value> The description. </value>
        [Display(Name = FullModelName + "Description", Description = "Name")]
        [DisplayTranslationForCulture("Description", "Beschreibung", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Description { get; set; }

        public List<ActivityRightGroup> Rights { get; set; }
    }

    public class ActivityRightGroup
    {
        /// <summary>   Gets or sets the name. </summary>
        /// <value> The name. </value>
        public string Name { get; set; }

        /// <summary>   Gets or sets the name of the display. </summary>
        /// <value> The name of the display. </value>
        public string DisplayName { get; set; }

        /// <summary>   Gets or sets the rights. </summary>
        /// <value> The rights. </value>
        public List<ActivityRight> Rights { get; set; }
    }

    public class ActivityRight
    {
        /// <summary>   Gets or sets the identifier of the activity. </summary>
        /// <value> The identifier of the activity. </value>
        public int ActivityId { get; set; }

        /// <summary>   Gets or sets the type of the activity. </summary>
        /// <value> The type of the activity. </value>
        public string ActivityType { get; set; }

        /// <summary>   Gets or sets the value. </summary>
        /// <value> The value. </value>
        /// <remarks>
        /// 0 = NULL,
        /// 1 = DENY, 
        /// 2 = ALLOW         
        /// </remarks>
        public int Value { get; set; }
    }
}