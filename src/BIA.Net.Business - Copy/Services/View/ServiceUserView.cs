// <copyright file="ServiceUserView.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
    using BIA.Net.Business.Services;
    using Business.DTO;
    using Model;
    using System.Collections.Generic;
    using System.Linq;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Common;

    /// <summary>
    /// Service to manipulate User View
    /// </summary>
    public class ServiceUserView : TServiceDTO<UserViewDTO, UserView, ZZProjectNameZZDBContainer>
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
        /// Method to delete an user view for an user
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <param name="viewId">Id of the view</param>
        public void DeleteSiteViewToUser(int userId, int viewId)
        {
            // Retrieve the user view to delete for the user
            IQueryable<UserView> elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Delete);
            List<UserView> viewToDelete = elementUserView.Where(x => x.UserId == userId && x.ViewId != viewId && x.View.ViewType == (int)Constants.ViewType.Site).ToList();

            // Parse the list to delete association
            foreach (UserView item in viewToDelete)
            {
                Repository.Delete(item);
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
            List<UserView> elementUserView = new List<UserView>();
            if (active)
            {
                // Retrieve the list of the user views to update
                elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Write).Where(x => x.UserId == userId).ToList();

                // Set all user views with a isdefault at false and set to true the appropriated view
                elementUserView.ForEach(x => x.IsDefault = false);
                elementUserView.Where(x => x.ViewId == viewId).ToList().ForEach(x => x.IsDefault = true);
            }
            else
            {
                // Retrieve the list of the user views to update
                elementUserView = Repository.GetStandardQuery(BIA.Net.Model.DAL.AccessMode.Write).Where(x => x.UserId == userId && x.ViewId == viewId && x.IsDefault == true).ToList();

                // Set all user views with a isdefault at false and set to true the appropriated view
                elementUserView.ForEach(x => x.IsDefault = false);
            }

            // Parse the list to save the modification
            foreach (UserView item in elementUserView)
            {
                Repository.Update(item);
            }

            // Save the Deletion for EF
            Repository.SaveChange();
        }
    }
}