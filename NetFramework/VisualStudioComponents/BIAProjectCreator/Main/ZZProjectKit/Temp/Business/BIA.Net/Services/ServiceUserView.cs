// <copyright file="ServiceUserView.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
#pragma warning disable SA1210 // Using directives must be ordered alphabetically by namespace
    using $companyName$.$saferootprojectname$.Model;
#pragma warning restore SA1210 // Using directives must be ordered alphabetically by namespace

    using BIA.Net.Common;
    using Business.DTO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Service to manipulate User View
    /// </summary>
    public class ServiceUserView : TServiceDTO<UserViewDTO, UserView, $saferootprojectname$DBContainer>
    {
        /// <summary>
        /// Method to delete an user view for an user
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <param name="viewId">Id of the view</param>
        public void DeleteUserView(int userId, int viewId)
        {
            // Retrieve the user view to delete for the user
            IQueryable<UserView> elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Delete);
            UserView viewToDelete = elementUserView.FirstOrDefault(x => x.UserId == userId && x.ViewId == viewId);

            // Delete the user view
            Repository.Delete(viewToDelete);

            // Save the Deletion for EF
            Repository.SaveChange();
        }

        /// <summary>
        /// Method to delete an user view for all user
        /// </summary>
        /// <param name="viewId">Id of the view</param>
        public void DeleteAllUserView(int viewId)
        {
            // Retrieve the user view to delete for the user
            IQueryable<UserView> elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Delete);
            List<UserView> userviewsToDelete = elementUserView.Where(x => x.ViewId == viewId).ToList();

            foreach (UserView userview in userviewsToDelete)
            {
                // Delete the user view
                Repository.Delete(userview);
            }

            // Save the Deletion for EF
            Repository.SaveChange();
        }

        /// <summary>
        /// Method to update the default user view for a view
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="viewId">Id of the view</param>
        /// <param name="active">Define if the default value is to set or to remove</param>
        public void UpdateUserDefaultView(int userId, int viewId, bool active)
        {
            // Retrieve the current view
            ViewDTO currentView = AllServicesDTO.Find<ViewDTO>(viewId);
            List<UserView> elementUserView = new List<UserView>();
            if (active)
            {
                if (currentView.ViewType == TypeOfView.Site)
                {
                    try
                    {
                        // Add relation between the site's view and the user to set in default this view
                        UserViewDTO newView = new UserViewDTO()
                        {
                            UserId = userId,
                            ViewId = currentView.Id,
                            IsDefault = true
                        };
                        Insert(newView);
                    }
                    catch (Exception ex)
                    {
                        TraceManager.Error($"Problem to {(active ? "associate" : "unlink")} of the site view {currentView.Id} to an user", ex);
                        throw ex;
                    }
                }

                // Retrieve the list of the user views to update
                elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Write).Where(x => x.UserId == userId && x.View.TableId == currentView.TableId && (x.IsDefault == true || x.ViewId == currentView.Id)).ToList();

                // Set all user views with a isdefault at false and set to true the appropriated view
                elementUserView.ForEach(x => x.IsDefault = false);
                elementUserView.Where(x => x.ViewId == currentView.Id).ToList().ForEach(x => x.IsDefault = true);
            }
            else
            {
                // Retrieve the list of the user views to update
                elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Write).Where(x => x.UserId == userId && x.ViewId == currentView.Id && x.IsDefault == true).ToList();

                // Set all user views with a isdefault at false and set to true the appropriated view
                elementUserView.ForEach(x => x.IsDefault = false);
            }

            // Parse the list to save the modification
            foreach (UserView item in elementUserView)
            {
                Repository.UpdateValues(item, new List<string> { "IsDefault" });
            }

            DeleteUserUnusedRelationSiteView(userId, currentView.TableId);

            // Save the Deletion for EF
            Repository.SaveChange();
        }

        /// <summary>
        /// Method to delete an user view for an user
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <param name="tableId">Id of the table</param>
        private void DeleteUserUnusedRelationSiteView(int userId, string tableId)
        {
            // Retrieve the user view to delete for the user
            IQueryable<UserView> elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Delete);
            List<UserView> viewToDelete = elementUserView.Where(x => x.UserId == userId && x.IsDefault == false && x.View.TableId == tableId && x.View.ViewType == (int)TypeOfView.Site).ToList();

            // Parse the list to delete association
            foreach (UserView item in viewToDelete)
            {
                Repository.Delete(item);
            }
        }
    }
}