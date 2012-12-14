﻿using System;
using System.Web.Mvc;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class DefaultController : Controller {
    public ActionResult Index() {
      @ViewBag.FileList = Models.FileModel.FileListToTree(Context2.GetFiles("testuser0@example.com"));
      return View();
    }

    public ActionResult Editor() {
      Document documentToEdit = Models.FileModel.GetDocument(0);
      if (documentToEdit != null) {
        @ViewBag.DocumentTitle = documentToEdit.Title;
        @ViewBag.DocumentContent = documentToEdit.Content;
      } else {
        @ViewBag.DocumentTitle = "N/A";
        @ViewBag.DocumentContent = "Document not found or not a document";
      }
      return View();
    }

    public ActionResult Viewer() {
      FileInstance fileToView = Models.FileModel.GetFile(int.Parse(Request.Params["id"]));
      @ViewBag.Document = fileToView.ToString();
      return View();
    }

    public ActionResult History() {
      FileInstance fileToView = Models.FileModel.GetFile(0);
      try {
        var documentToView = (Document)fileToView;
        @ViewBag.Title = documentToView.Title;
      } catch (InvalidCastException) {
        @ViewBag.Title = "File: " + fileToView.File.name;
      }
      @ViewBag.DocumentHistory = fileToView.HistoryToString();
      return View();
    }
  }
}
