﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;

namespace Blog.Domain.Blogging
{
   /// <summary>
   /// It creates Image objects from source DataURLs and also generates new HTML that contains image file paths.
   /// </summary>
   public class ImageRenderer
   {
      public ImageRenderer(string postDirectory)
      {
         _imageUrls = new List<string>();
         _postDirectory = Assert.NotNull(postDirectory);
      }

      private readonly List<string> _imageUrls;
      private readonly string _postDirectory;

      public ImageRenderResult Render(string content)
      {
         var images = new List<Image>();
         var doc = new HtmlDocument();
         doc.LoadHtml(content);
         doc.DocumentNode.ForEachChild(node =>
         {
            if (node.Is("figure"))
            {
               var img = node.Child("img");
               if (Image.IsDataUrl(img.Attr("src")))
                  CreateNewImage(images, img);
               else
                  UpdateImage(images, img);
            }
         });

         var sw = new StringWriter();
         doc.Save(sw);
         return new ImageRenderResult(sw.ToString(), images);
      }

      private void CreateNewImage(List<Image> images, HtmlNode img)
      {
         var image = Image.Create(img, GenerateFilename(img), _postDirectory);
         images.Add(image);
         img.Attributes.RemoveAll();
         img.SetAttributeValue("src", image.RelativePath);
      }

      private void UpdateImage(List<Image> images, HtmlNode img)
      {
         var image = new Image(Path.GetFileName(img.Attr("src")), _postDirectory);
         img.SetAttributeValue("src", image.RelativePath);
         images.Add(image);
      }

      private string GenerateFilename(HtmlNode img)
      {
         var dataFilename = img.Attr("data-filename");
         if (string.IsNullOrEmpty(dataFilename))
            throw new InvalidOperationException("data-filename attribute is not set for <img>");

         while (_imageUrls.Contains(dataFilename))
            dataFilename = PostPath.Increment(dataFilename);

         _imageUrls.Add(dataFilename);
         return dataFilename;
      }
   }
}
