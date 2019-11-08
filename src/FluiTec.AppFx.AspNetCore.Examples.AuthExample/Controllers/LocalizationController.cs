using System;
using System.Collections.Generic;
using System.Linq;
using FluiTec.DbLocalizationProvider;
using FluiTec.DbLocalizationProvider.Cache;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization;
using FluiTec.AppFx.Localization;
using FluiTec.AppFx.Localization.Entities;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Controllers
{
    /// <summary>   A controller for handling localizations. </summary>
    [Authorize(PolicyNames.AdministrativeAccess)]
    public class LocalizationController : Controller
    {
        #region Fields

        /// <summary>   The localization data service. </summary>
        private readonly ILocalizationDataService _localizationDataService;

        /// <summary>   The localizer. </summary>
        private readonly IStringLocalizer<Resources.Controllers.LocalizationResource> _localizer;

        #endregion

        /// <summary>   Constructor. </summary>
        /// <param name="localizationDataService">  The localization data service. </param>
        /// <param name="localizer">                The localizer. </param>
        public LocalizationController(ILocalizationDataService localizationDataService, IStringLocalizer<Resources.Controllers.LocalizationResource> localizer)
        {
            _localizationDataService = localizationDataService;
            _localizer = localizer;
        }

        /// <summary>   Gets the index. </summary>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        public IActionResult Index(int pageNumber = 1)
        {
            var rGroups = new List<ResourceGroupModel>();
            using (var uow = _localizationDataService.StartUnitOfWork())
            {
                var resources = uow.ResourceRepository.GetAll().ToList();

                var ungroupable = resources.Where(r => r.ResourceKey != null && !r.ResourceKey.Contains(".")).OrderBy(r => r.ResourceKey).ToList();
                var groupable = resources.Except(ungroupable);
                var groups = groupable.GroupBy(r =>
                    r.ResourceKey.Substring(0, r.ResourceKey.LastIndexOf(".", StringComparison.Ordinal))).OrderBy(g => g.Key);

                rGroups.Add(new ResourceGroupModel { Name = _localizer.GetString(r => r.Ungrouped), Entries = ungroupable.Count});
                rGroups.AddRange(groups.Select(g => new ResourceGroupModel { Name = g.Key, Entries = g.Count() }));
            }

            var page = PageableModel<ResourceGroupModel>.PageExisting(rGroups, pageNumber).InitRoute(this, nameof(Index));

            return View(page);
        }

        /// <summary>   Groups. </summary>
        /// <param name="groupKey">     (Optional) The group key. </param>
        /// <param name="pageNumber">   (Optional) The page number. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        public IActionResult Group(string groupKey = "", int pageNumber = 1)
        {
            if (groupKey == _localizer.GetString(r => r.Ungrouped)) groupKey = string.Empty;
            var displayResources = new List<ResourceEntity>();
            using (var uow = _localizationDataService.StartUnitOfWork())
            {
                var resources = uow.ResourceRepository.GetAll().ToList();

                var ungroupable = resources.Where(r => r.ResourceKey != null && !r.ResourceKey.Contains(".")).OrderBy(r => r.ResourceKey).ToList();
                var groupable = resources.Except(ungroupable);
                var groups = groupable.GroupBy(r =>
                    r.ResourceKey.Substring(0, r.ResourceKey.LastIndexOf(".", StringComparison.Ordinal))).OrderBy(g => g.Key);

                if (groupKey == string.Empty)
                {
                    displayResources.AddRange(ungroupable);
                    var page = PageableModel<ResourceEntity>.PageExisting(displayResources, pageNumber)
                        .InitRoute(this, nameof(Group));
                    return View(page);
                }

                var group = groups.SingleOrDefault(g => g.Key == groupKey);
                if (group == null) return RedirectToAction(nameof(Index));
                {
                    displayResources.AddRange(group);
                    var page = PageableModel<ResourceEntity>.PageExisting(displayResources, pageNumber)
                        .InitRoute(this, nameof(Group));
                    return View(page);
                }

            }
        }

        /// <summary>   Adds resource. </summary>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationCreate)]
        public IActionResult AddResource()
        {
            var model = new ResourceAddModel
            {
                Author = User.GetDisplayName()
            };
            return View(model);
        }

        /// <summary>   (An Action that handles HTTP POST requests) adds a resource. </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationCreate)]
        public IActionResult AddResource(ResourceAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _localizationDataService.StartUnitOfWork())
                {
                    var resource = new ResourceEntity
                    {
                        ResourceKey = model.Name,
                        Author = model.Author,
                        FromCode = false,
                        IsHidden = false,
                        IsModified = false,
                        ModificationDate = DateTime.Now
                    };
                    uow.ResourceRepository.Add(resource);
                    uow.Commit();
                    return RedirectToAction(nameof(EditResource), new { resourceId = resource.Id });
                }          
            }

            return View();
        }

        /// <summary>   Edit resource. </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationUpdate)]
        public IActionResult EditResource(int resourceId)
        {
            using (var uow = _localizationDataService.StartUnitOfWork())
            {
                var resource = uow.ResourceRepository.Get(resourceId);
                return View(new ResourceEditModel
                {
                    Id = resourceId,
                    Author = resource.Author,
                    Name = resource.ResourceKey
                });
            }
        }

        /// <summary>   (An Action that handles HTTP POST requests) edit resource. </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationUpdate)]
        public IActionResult EditResource(ResourceEditModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _localizationDataService.StartUnitOfWork())
                {
                    var existing = uow.ResourceRepository.Get(model.Id);
                    existing.ResourceKey = model.Name;
                    existing.Author = model.Author;
                    uow.ResourceRepository.Update(existing);
                    uow.Commit();
                    model.UpdateSuccess();
                }
            }

            return View(model);
        }

        /// <summary>   Deletes the resource described by resourceId. </summary>
        /// <param name="resourceId">   Identifier for the resource. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationDelete)]
        public IActionResult DeleteResource(int resourceId)
        {
            return View(new ResourceDeleteModel {Id = resourceId});
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) deletes the resource described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationDelete)]
        public IActionResult DeleteResource(ResourceDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _localizationDataService.StartUnitOfWork())
                {
                    var existing = uow.ResourceRepository.Get(model.Id);
                    if (existing != null)
                    {
                        foreach(var translation in uow.TranslationRepository.ByResource(existing))
                            uow.TranslationRepository.Delete(translation);

                        uow.ResourceRepository.Delete(existing);
                        uow.Commit();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View("Error");
        }

        /// <summary>   Translations. </summary>
        /// <param name="resourceKey">  The resource key. </param>
        /// <param name="pageNumber">   (Optional) The page number. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        public IActionResult Translations(string resourceKey, int pageNumber = 1)
        {
            using (var uow = _localizationDataService.StartUnitOfWork())
            {
                var resource = uow.ResourceRepository.GetByKey(resourceKey);
                if (resource == null) return RedirectToAction(nameof(Index));
                var translations = uow.TranslationRepository.ByResource(resource);
                var page = PageableModel<TranslationEntity>.PageExisting(translations, pageNumber)
                    .InitRoute(this, nameof(Translations));
                return View(page);
            }
        }

        /// <summary>   Translations. </summary>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationUpdate)]
        public IActionResult EditTranslation(int id)
        {
            using (var uow = _localizationDataService.StartUnitOfWork())
            {
                var translation = uow.TranslationRepository.Get(id);
                if (translation == null) return RedirectToAction(nameof(Index));

                return View(new TranslationEditModel {Id = translation.Id, Value = translation.Value});
            }
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) translations the given model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationUpdate)]
        public IActionResult EditTranslation(TranslationEditModel model)
        {
            model.Update();
            if (ModelState.IsValid)
            {
                using (var uow = _localizationDataService.StartUnitOfWork())
                {
                    var existing = uow.TranslationRepository.Get(model.Id);
                    if (existing == null) return RedirectToAction(nameof(Index));

                    var resource = uow.ResourceRepository.Get(existing.ResourceId);
                    resource.Author = User.GetDisplayName();
                    resource.FromCode = false;
                    uow.ResourceRepository.Update(resource);

                    existing.Value = model.Value;
                    uow.TranslationRepository.Update(existing);

                    uow.Commit();
                    model.UpdateSuccess();
                    new ClearCache.Command().Execute();
                }
            }

            return View(model);
        }

        /// <summary>   Adds a translation. </summary>
        /// <param name="resourceKey">  The resource key. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationCreate)]
        public IActionResult AddTranslation(string resourceKey)
        {
            using (var uow = _localizationDataService.StartUnitOfWork())
            {
                var resource = uow.ResourceRepository.GetByKey(resourceKey);
                if (resource == null) return RedirectToAction(nameof(Index));
                return View(new TranslationAddModel { ResourceId = resource.Id });
            }
        }

        /// <summary>   Adds a translation. </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationCreate)]
        public IActionResult AddTranslation(TranslationAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _localizationDataService.StartUnitOfWork())
                {
                    var resource = uow.ResourceRepository.Get(model.ResourceId);
                    if (resource == null) return RedirectToAction(nameof(Index));

                    var translation = uow.TranslationRepository.ByResource(resource).SingleOrDefault(t => t.Language == model.Language);
                    if (translation != null) return RedirectToAction(nameof(EditTranslation), new {id = translation.Id});

                    var newTranslation = new TranslationEntity
                    {
                        ResourceId = model.ResourceId,
                        Language = model.Language,
                        Value = model.Value
                    };
                    uow.TranslationRepository.Add(newTranslation);
                    uow.Commit();
                }
            }

            return View(model);
        }

        /// <summary>   Deletes the translation described by translationId. </summary>
        /// <param name="translationId">    Identifier for the translation. </param>
        /// <returns>   An IActionResult. </returns>
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationDelete)]
        public IActionResult DeleteTranslation(int translationId)
        {
            return View(new TranslationDeleteModel {Id = translationId});
        }

        /// <summary>
        ///     (An Action that handles HTTP POST requests) (Restricted to
        ///     PolicyNames.LocalizationDelete) deletes the translation described by model.
        /// </summary>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        [HttpPost]
        [Authorize(PolicyNames.LocalizationAccess)]
        [Authorize(PolicyNames.LocalizationDelete)]
        public IActionResult DeleteTranslation(TranslationDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _localizationDataService.StartUnitOfWork())
                {
                    var existing = uow.TranslationRepository.Get(model.Id);
                    if (existing != null)
                    {
                        uow.TranslationRepository.Delete(existing);
                        uow.Commit();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View("Error");
        }
    }
}