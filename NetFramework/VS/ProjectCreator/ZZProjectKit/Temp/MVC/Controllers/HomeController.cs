// <copyright file="HomeController.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Controller for Home Pages
    /// </summary>
    /// <seealso cref="$safeprojectname$.Controllers.BaseController" />
    public class HomeController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>The index ActionResut</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Example for call the voice.
        /// </summary>
        /// <param name="word">message voice will say</param>
        /// <param name="language">voice language</param>
        /// <returns>the sound </returns>
        public ActionResult PathToSpeech(string word, string language)
        {
            return new RedirectResult("DMServiceSpeech/" + Url.Action("TextToMp3", "Voice") + "?word=" + "bonjour" + "&language=" + "FR");
        }
    }
}