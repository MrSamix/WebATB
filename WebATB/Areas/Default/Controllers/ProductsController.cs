using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebATB.Data;
using WebATB.Data.Entities;
using WebATB.Interfaces;
using WebATB.Models.Category;
using WebATB.Models.Helpers;
using WebATB.Models.Product;

namespace WebATB.Controllers;

public class ProductsController(AppATBDbContext dbContext,
    IMapper mapper, IImageService imageService) : Controller
{
    [Area("Default")]
    public IActionResult Index()
    {
        var model = dbContext.Products
            .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
            .ToList();

        return View(model);
    }

    [HttpGet]    
    public IActionResult Create()
    {
        ProductCreateModel model = new ProductCreateModel();
        model.Categories = dbContext.Categories
            .ProjectTo<SelectItemHelper>(mapper.ConfigurationProvider)
            .ToList();
        return View(model);
    }

    [HttpPost]
    public IActionResult Create(ProductCreateModel model)
    {
        if(!ModelState.IsValid)
        {
            model.Categories = dbContext.Categories
                .ProjectTo<SelectItemHelper>(mapper.ConfigurationProvider)
                .ToList();
            return View(model);
        }
        var entity = mapper.Map<ProductEntity>(model);
        dbContext.Products.Add(entity);
        dbContext.SaveChanges();
        if (model.Images != null && model.Images.Length > 0)
        {
            short priority = 1;
            foreach (var image in model.Images)
            {
                if (image.Length > 0)
                {
                    var imagePath = imageService.SaveImageAsync(image).Result;
                    var imageEntity = new ProductImageEntity
                    {
                        ProductId = entity.Id,
                        Priority = priority++,
                        Path = imagePath
                    };
                    dbContext.ProductImages.Add(imageEntity);

                }
            }
        }
        dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var entity = dbContext.Products.Find(id);
        if (entity == null) return NotFound();
        entity.IsDeleted = true;
        dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int id)
    {
        var model = dbContext.Products
            .ProjectTo<ProductUpdateModel>(mapper.ConfigurationProvider)
            .FirstOrDefault(p => p.Id == id);
        if (model == null) return NotFound();

        model.Categories = dbContext.Categories.ProjectTo<SelectItemHelper>(mapper.ConfigurationProvider).ToList();

        return View(model);
    }
    [HttpPost]
    public IActionResult Update(ProductUpdateModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = dbContext.Categories
                .ProjectTo<SelectItemHelper>(mapper.ConfigurationProvider)
                .ToList();
            return View(model);
        }
        var entity = mapper.Map<ProductEntity>(model);
        dbContext.Products.Update(entity);
        dbContext.SaveChanges();

        var images = dbContext.ProductImages
            .Where(pi => pi.ProductId == entity.Id)
            .ToList();
        if (model.Images != null && model.Images.Length > 0)
        {
            short priority = 1;
            foreach (var image in model.Images)
            {
                if (image.Length > 0)
                {
                    try
                    {
                        var imagePath = imageService.SaveImageAsync(image).Result;
                        var imageEntity = new ProductImageEntity
                        {
                            ProductId = entity.Id,
                            Priority = priority++,
                            Path = imagePath
                        };
                        dbContext.ProductImages.Add(imageEntity);
                    }
                    catch (System.AggregateException)
                    {
                        if (image.StartsWith("/images/"))
                        {
                            var imagePath = image[(image.IndexOf('_') + 1)..];
                            var imageObj = dbContext.ProductImages.FirstOrDefault(p => p.Path == imagePath && p.ProductId == model.Id);
                            if (imageObj != null)
                            {
                                images.Remove(imageObj);
                                imageObj.Priority = priority++;
                                dbContext.ProductImages.Update(imageObj);
                            }
                        }
                    }
                }
            }
        }
        if (images.Count > 0)
        {
            foreach (var img in images) // remove old photos from db and storage
            {
                imageService.DeleteImageAsync(img.Path).Wait();
                dbContext.ProductImages.Remove(img);
            }
        }
        dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
