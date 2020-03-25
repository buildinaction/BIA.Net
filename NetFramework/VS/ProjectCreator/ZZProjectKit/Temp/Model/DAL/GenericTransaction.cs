// <copyright file="GenericTransaction.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.DAL
{
    using BIA.Net.Model.DAL;
    using System;

    /// <summary>
    /// Generic transaction constructor.
    /// </summary>
    public class GenericTransaction : TGenericTransaction<$saferootprojectname$DBContainer>
    {
        /// <summary>
        /// Method to commit the transaction
        /// </summary>
        /// <param name="rootModeInTransaction">Define if the root made is true or false</param>
        public static void EndTransaction(bool rootModeInTransaction)
        {
            if (DelegateSuccesStatic == null)
            {
                DelegateSuccesStatic = FinalizeTransaction;
            }

            BaseEndTransaction(rootModeInTransaction);
        }

        /// <summary>
        /// Method to finalize the action after the transaction to do any action
        /// </summary>
        /// <param name="rootModeInTransaction">Define if the root made is true or false</param>
        public static void FinalizeTransaction(bool rootModeInTransaction)
        {
            if (!rootModeInTransaction)
            {
                /* do any action */
            }
        }
    }
}